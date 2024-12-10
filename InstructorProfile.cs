using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Driving_Management_System
{
    public partial class InstructorProfile : Form
    {
        public InstructorProfile()
        {
            InitializeComponent();
        }
        SqlConnection cn = new SqlConnection("data source=localhost; database=Instructor; Integrated Security=True;");

        private void InstructorProfile_Load(object sender, EventArgs e)
        {
            DisplayLastUserId();
            LoadUserProfileImage();
            FormBorderStyle = FormBorderStyle.None;

        }
        private void DisplayLastUserId()
        {
            string sql = "SELECT FirstName, LastName, Phone, Email, Certification, Salary FROM Instructor WHERE Username = @Username";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@Username", CurrentUser.Username); // Using CurrentUser.Username.

            try
            {
                cn.Open();
                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    if (read.Read())
                    {
                        // Handle possible DBNull values for each field
                        label1.Text = $"First Name: {(read["FirstName"] != DBNull.Value ? read["FirstName"] : "Not yet entered")}";
                        label2.Text = $"Last Name: {(read["LastName"] != DBNull.Value ? read["LastName"] : "Not yet entered")}";
                        label3.Text = $"Phone: {(read["Phone"] != DBNull.Value ? read["Phone"] : "Not yet entered")}";
                        label4.Text = $"Email: {(read["Email"] != DBNull.Value ? read["Email"] : "Not yet entered")}";
                        label5.Text = $"Certification: {(read["Certification"] != DBNull.Value ? read["Certification"] : "Not yet entered")}";
                    }
                    else
                    {
                        // If no record is found, display "Not found"
                        label1.Text = "First Name: Not found";
                        label2.Text = "Last Name: Not found";
                        label3.Text = "Phone: Not found";
                        label4.Text = "Email: Not found";
                        label5.Text = "Certification: Not found";
                        MessageBox.Show("Instructor not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog
            {
                Filter = "Choose Image (*.JPG;*.PNG;*.GIF)|*.jpg;*.png;*.gif"
            };

            if (opf.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(opf.FileName);
                SaveImageToDatabase(opf.FileName);
            }
        }

        private void LoadUserProfileImage()
        {
            string sql = "SELECT ProfileImage FROM Instructor WHERE Username = @Username";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@Username", CurrentUser.Username);

            try
            {
                cn.Open();
                byte[] imageBytes = cmd.ExecuteScalar() as byte[];

                if (imageBytes != null && imageBytes.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pictureBox1.Image = null; // No image found.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }
        private void SaveImageToDatabase(string imagePath)
        {
            byte[] imageBytes;
            using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    imageBytes = br.ReadBytes((int)fs.Length);
                }
            }

            string sql = "UPDATE Instructor SET ProfileImage = @ProfileImage WHERE Username = @Username";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@ProfileImage", imageBytes);
            cmd.Parameters.AddWithValue("@Username", CurrentUser.Username); // Using the logged-in user's username.

            try
            {
                cn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                MessageBox.Show(rowsAffected > 0 ? "Image saved successfully!" : "Image save failed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }
    }
}
