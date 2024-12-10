using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Driving_Management_System
{
    public partial class ApplicationFormInstructor : Form
    {
        public ApplicationFormInstructor()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection("data source=localhost; database=Instructor; Integrated Security=True;");


        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void ApplicationFormInstructor_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if the instructor has already submitted
                using (SqlConnection connCheck = new SqlConnection("data source=localhost; database=Instructor; Integrated Security=True;"))
                {
                    connCheck.Open();
                    string checkQuery = "SELECT HasSubmittedForm FROM Instructor WHERE Username = @Username";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, connCheck);
                    checkCmd.Parameters.AddWithValue("@Username", CurrentUser.Username);

                    var result = checkCmd.ExecuteScalar();
                    if (result != DBNull.Value && Convert.ToInt32(result) == 1)
                    {
                        MessageBox.Show("You have already submitted the form.", "Submission Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Proceed with form submission if not submitted
                using (SqlCommand cmd = new SqlCommand("UPDATE Instructor SET FirstName = @FirstName, LastName = @LastName, Phone = @Phone, Email = @Email, " +
                                         "Certification = @Certification, HireDate = @HireDate, LicenseImage = @LicenseImage WHERE Username = @Username AND Password = @Password", conn))
                {
                    conn.Open();

                    // Add parameter values for text inputs
                    cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
                    cmd.Parameters.AddWithValue("@LastName", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Phone", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Email", textBox4.Text);
                    cmd.Parameters.AddWithValue("@Certification", textBox5.Text);
                    cmd.Parameters.AddWithValue("@HireDate", dateTimePicker1.Value);

                    // Add image parameter (handling for null)
                    byte[] imageBytes = pictureBox1.Image != null ? GetImageBytes(pictureBox1.Image) : null;
                    cmd.Parameters.AddWithValue("@LicenseImage", imageBytes ?? (object)DBNull.Value);

                    // Add parameters for Username and Password from CurrentUser
                    cmd.Parameters.AddWithValue("@Username", CurrentUser.Username); // Assuming CurrentUser has a Username
                    cmd.Parameters.AddWithValue("@Password", CurrentUser.Password); // Assuming CurrentUser has a Password

                    // Execute the update command
                    cmd.ExecuteNonQuery();
                }


                // Update HasSubmittedForm to 1
                using (SqlConnection connUpdate = new SqlConnection("data source=localhost; database=Instructor; Integrated Security=True;"))
                {
                    connUpdate.Open();
                    string updateQuery = "UPDATE Instructor SET HasSubmittedForm = 1 WHERE Username = @Username";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, connUpdate);
                    updateCmd.Parameters.AddWithValue("@Username", CurrentUser.Username);
                    updateCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Form submitted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
                // Show PDF form
                InstructorPDF frm = new InstructorPDF();
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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

        private void button1_Click(object sender, EventArgs e)
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTimePicker1.Value;
            DateTime today = DateTime.Today;
            DateTime minDate = today.AddYears(-10); // Define a minimum date of 10 years ago

            // Check if the selected date is in the future or too far in the past
            if (selectedDate > today)
            {
                // Show MessageBox for future date
                dateTimePicker1.BackColor = Color.Red;
            }
            else if (selectedDate < minDate)
            {
                // Show MessageBox for past date (more than 10 years ago)
                dateTimePicker1.BackColor = Color.Red;
            }
            else
            {
                // Show MessageBox for valid date
                dateTimePicker1.BackColor = Color.White;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^[A-Za-z]+([ A-Za-z]+)*$";
            if (!Regex.IsMatch(textBox1.Text, pattern))
            {
                textBox1.BackColor = System.Drawing.Color.Red; // Incorrect input
            }
            else
            {
                textBox1.BackColor = System.Drawing.Color.White; // Correct input
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^[A-Za-z]+([ A-Za-z]+)*$";
            if (!Regex.IsMatch(textBox2.Text, pattern))
            {
                textBox2.BackColor = System.Drawing.Color.Red; // Incorrect input
            }
            else
            {
                textBox2.BackColor = System.Drawing.Color.White; // Correct input
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

    }
}

