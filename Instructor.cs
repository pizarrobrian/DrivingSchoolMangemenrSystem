using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace Driving_Management_System
{
    public partial class Instructor : Form
    {
        public Instructor()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection("data source=localhost; database=Instructor; Integrated Security=True;");

        private void display()
        {
            try
            {
                conn.Open();
                string load = "SELECT * FROM Instructor";
                SqlDataAdapter sda = new SqlDataAdapter(load, conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in loading the data table: " + ex);
            }
            finally
            {
                conn.Close();
            }
            
        }
        private void Instructor_Load(object sender, EventArgs e)
        {
            display();
            LoadNames();
            this.FormBorderStyle = FormBorderStyle.None;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Please fill all the needed.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string insertQuery = "INSERT INTO Instructor (Username, Password, FirstName, LastName, Phone, Email, Certification, HireDate, LicenseImage) " +
          "VALUES (@Username, @Password, @FirstName, @LastName, @Phone, @Email, @Certification, @HireDate, @LicenseImage)";

            using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
            {
                conn.Open();
                // Add parameter values for text inputs
                cmd.Parameters.AddWithValue("@Username", textBox7.Text);
                cmd.Parameters.AddWithValue("@Password", textBox8.Text);
                cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
                cmd.Parameters.AddWithValue("@LastName", textBox2.Text);
                cmd.Parameters.AddWithValue("@Phone", textBox3.Text);
                cmd.Parameters.AddWithValue("@Email", textBox4.Text);
                cmd.Parameters.AddWithValue("@Certification", textBox5.Text);
                cmd.Parameters.AddWithValue("@HireDate", DateTime.Parse(textBox6.Text)); // Assuming the date is in a valid format

                // Add parameter for image (License)
                byte[] imageBytes = GetImageBytes(pictureBox1.Image);
                cmd.Parameters.AddWithValue("@LicenseImage", imageBytes ?? (object)DBNull.Value); // Handle null image

                // Execute the command
                int rowsAffected = cmd.ExecuteNonQuery();
                MessageBox.Show($"{rowsAffected} row(s) inserted.");
            }
            conn.Close();
            clear();
            display();
        }
        private byte[] GetImageBytes(Image image)
        {
            if (image == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // Save in PNG format
                return ms.ToArray();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                SqlCommand del = new SqlCommand("DELETE FROM  Instructor WHERE InstructorID = '" + comboBox1.Text + "' AND FirstName = '" + comboBox2.Text + "'", conn);
                del.ExecuteNonQuery();
                MessageBox.Show("Successfully deleted");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
            finally
            {
                conn.Close();
            }
            clear();
            display();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string updateQuery = @"
            UPDATE Instructor 
            SET 
                Password = @Password,
                FirstName = @FirstName,
                LastName = @LastName,
                Phone = @Phone,
                Email = @Email,
                Certification = @Certification,
                HireDate = @HireDate
            WHERE Username = @Username"; // Use Username as identifier

                using (SqlConnection conn = new SqlConnection("data source=localhost; database=Instructor; Integrated Security=True;"))
                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", textBox7.Text); // Assuming Username is used to find the record
                    cmd.Parameters.AddWithValue("@Password", textBox8.Text);
                    cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
                    cmd.Parameters.AddWithValue("@LastName", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Phone", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Email", textBox4.Text);
                    cmd.Parameters.AddWithValue("@Certification", textBox5.Text);

                    if (DateTime.TryParse(textBox6.Text, out DateTime hireDate))
                        cmd.Parameters.AddWithValue("@HireDate", hireDate);
                    else
                    {
                        MessageBox.Show("Invalid Hire Date format.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

     
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    MessageBox.Show(rowsAffected > 0 ? "Updated successfully" : "No records updated.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                clear();
                display();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                // Split the full name (comboBox2) into first and last names
                string[] nameParts = comboBox2.Text.Split(' ');

                if (nameParts.Length == 2) // Make sure we have both first and last names
                {
                    string firstName = nameParts[0];
                    string lastName = nameParts[1];

                    SqlCommand comm = new SqlCommand("SELECT * FROM Instructor WHERE InstructorID = @InstructorID AND FirstName = @FirstName AND LastName = @LastName", conn);
                    comm.Parameters.AddWithValue("@InstructorID", comboBox1.Text);
                    comm.Parameters.AddWithValue("@FirstName", firstName);
                    comm.Parameters.AddWithValue("@LastName", lastName);

                    SqlDataReader reader = comm.ExecuteReader();

                    while (reader.Read())
                    {
                        // Populate textboxes with values from the reader

                        textBox7.Text = reader.GetValue(1).ToString(); // Assuming textBox7 is for Username
                        textBox8.Text = reader.GetValue(2).ToString(); // 
                        textBox1.Text = reader.GetValue(3).ToString();
                        textBox2.Text = reader.GetValue(4).ToString();
                        textBox3.Text = reader.GetValue(5).ToString();
                        textBox4.Text = reader.GetValue(6).ToString();
                        textBox5.Text = reader.GetValue(7).ToString();
                        textBox6.Text = reader.GetValue(8).ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter both first and last names in the format 'FirstName LastName'.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in searching: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

        }


        private void LoadNames()
        {
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("SELECT InstructorID, FirstName, LastName FROM [Instructor]", conn); // Modify query to include LastName
                SqlDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["InstructorID"].ToString()); // Add InstructorID to comboBox1
                    comboBox2.Items.Add($"{reader["FirstName"]} {reader["LastName"]}"); // Add FirstName and LastName to comboBox2
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading instructor names: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }
        private void clear() {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            pictureBox1.Image = null;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openFileDialog.Title = "Select an Image for the License";

                // If the user selects a file, load the image into the PictureBox
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Load the selected image into the pictureBox
                        pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading image: " + ex.Message);
                    }
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^[A-Za-z]+([ A-Za-z]+)*$";
            if (!Regex.IsMatch(textBox2.Text, pattern))
            {
                textBox2.BackColor = System.Drawing.Color.Red; // Incorrect input
                return;
            }
            else
            {
                textBox2.BackColor = System.Drawing.Color.White; // Correct input
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^[A-Za-z]+([ A-Za-z]+)*$";
            if (!Regex.IsMatch(textBox7.Text, pattern))
            {
                textBox7.BackColor = System.Drawing.Color.Red; // Incorrect input
                return;
            }
            else
            {
                textBox7.BackColor = System.Drawing.Color.White; // Correct input
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^[A-Za-z]+([ A-Za-z]+)*$";
            if (!Regex.IsMatch(textBox1.Text, pattern))
            {
                textBox1.BackColor = System.Drawing.Color.Red; // Incorrect input
                return;
            }
            else
            {
                textBox1.BackColor = System.Drawing.Color.White; // Correct input
            }

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$";
            if (!Regex.IsMatch(textBox8.Text, pattern))
            {
                textBox8.BackColor = System.Drawing.Color.Red;
                return;// Incorrect input
            }
            else
            {
                textBox8.BackColor = System.Drawing.Color.White; // Correct input
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string phonePattern = @"^09[0-9]{9}$";
            if (!Regex.IsMatch(textBox3.Text, phonePattern))
            {
                textBox3.BackColor = System.Drawing.Color.Red; // Incorrect input
            }
            else
            {
                textBox3.BackColor = System.Drawing.Color.White; // Correct input
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(textBox4.Text, pattern))
            {
                textBox4.BackColor = System.Drawing.Color.Red; // Incorrect input
            }
            else
            {
                textBox4.BackColor = System.Drawing.Color.White; // Correct input
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string licenseType = textBox5.Text.Trim();

            // Check if the license type is valid and change the color accordingly
            if (IsValidLicense(licenseType))
            {
                textBox5.BackColor = Color.White;

            }
            else
            {
                textBox5.BackColor = Color.Red;

            }
        }

        public static bool IsValidLicense(string licenseType)
        {
            string pattern = @"^(Professional Driver’s License|Conductor’s License |PDL|CD)$";

            // Use Regex to check if the license type matches the pattern
            return Regex.IsMatch(licenseType, pattern, RegexOptions.IgnoreCase);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
