using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Driving_Management_System
{
    public partial class ApplicationFormStudent : Form
    {
        public ApplicationFormStudent()
        {
            InitializeComponent();
        }

   
        SqlConnection connStudentss = new SqlConnection("data source=localhost; database=StudentInfo; Integrated Security=True;");

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure that the Username and Password are not null or empty before proceeding
                if (string.IsNullOrEmpty(CurrentUser.Username) || string.IsNullOrEmpty(CurrentUser.Password))
                {
                    MessageBox.Show("Username and Password are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Open the connection once for all database operations
                connStudentss.Open();

                // Check if the user has already submitted the form
                string checkQuery = "SELECT HasSubmittedForm FROM Students WHERE Username = @Username";
                SqlCommand checkCmd = new SqlCommand(checkQuery, connStudentss);
                checkCmd.Parameters.AddWithValue("@Username", CurrentUser.Username);

                var result = checkCmd.ExecuteScalar();

                // If HasSubmittedForm is 1 (true), inform the user they cannot submit again
                if (result != DBNull.Value && Convert.ToInt32(result) == 1)
                {
                    MessageBox.Show("You have already submitted the form.", "Submission Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Insert data into Students table if the form has not been submitted
                string updateQuery1 = "UPDATE Students SET " +
                                 "Password = @Password, FirstName = @FirstName, LastName = @LastName, " +
                                 "DateOfBirth = @DateOfBirth, Phone = @Phone, Email = @Email, Address = @Address " +
                                 "WHERE Username = @Username";
                SqlCommand insertCmd = new SqlCommand(updateQuery1, connStudentss);

                // Add parameters to prevent SQL injection
                insertCmd.Parameters.AddWithValue("@Username", CurrentUser.Username); // Ensure Username is added here
                insertCmd.Parameters.AddWithValue("@Password", CurrentUser.Password); // Ensure Password is added here
                insertCmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
                insertCmd.Parameters.AddWithValue("@LastName", textBox2.Text);
                insertCmd.Parameters.AddWithValue("@DateOfBirth", dateTimePicker1.Value);
                insertCmd.Parameters.AddWithValue("@Phone", textBox4.Text);
                insertCmd.Parameters.AddWithValue("@Email", textBox5.Text);
                insertCmd.Parameters.AddWithValue("@Address", textBox6.Text);

                insertCmd.ExecuteNonQuery();

                // Update HasSubmittedForm to 1 to mark the form as submitted
                string updateQuery = "UPDATE Students SET HasSubmittedForm = 1 WHERE Username = @Username";
                SqlCommand updateCmd = new SqlCommand(updateQuery, connStudentss);
                updateCmd.Parameters.AddWithValue("@Username", CurrentUser.Username);

                updateCmd.ExecuteNonQuery();

                // Close the connection after all operations are complete
                connStudentss.Close();

                // Display success message
                MessageBox.Show("Submitted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Optionally, show the PDF form
                StudentPDF frm = new StudentPDF();
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the connection is closed in case of an exception
                if (connStudentss.State == System.Data.ConnectionState.Open)
                {
                    connStudentss.Close();
                }
            }
        }




        private void ApplicationFormStudent_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;// You can add any necessary form load logic here
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string firstNamePattern = @"^[A-Z][a-zA-Z]*$";
            if (!Regex.IsMatch(textBox1.Text, firstNamePattern))
            {
                textBox1.BackColor = Color.LightCoral;
                return;
            }
            else
            {
                textBox1.BackColor = Color.White;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string lastNamePattern = @"^[A-Z][a-zA-Z]*$";
            if (!Regex.IsMatch(textBox2.Text, lastNamePattern))
            {
                textBox2.BackColor = Color.LightCoral;
                return;
            }
            else
            {
                textBox2.BackColor = Color.White;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string phonePattern = @"^09[0-9]{9}$";
            if (!Regex.IsMatch(textBox4.Text, phonePattern))
            {
                textBox4.BackColor = Color.LightCoral;
                return;
            }
            else
            {
                textBox4.BackColor = Color.White;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(textBox5.Text, emailPattern))
            {
                textBox5.BackColor = Color.LightCoral;
                return;
            }
            else
            {
                textBox5.BackColor = Color.White;
            }

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string addressPattern = @"^[a-zA-Z0-9\s,.'-]+$";
            if (!Regex.IsMatch(textBox6.Text, addressPattern))
            {
                textBox6.BackColor = Color.LightCoral;
                return;
            }
            else
            {
                textBox6.BackColor = Color.White;
            }

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            DateTime today = DateTime.Today;
            int age = today.Year - dateTimePicker1.Value.Year;

            // Adjust if the birthday hasn't occurred this year
            if (dateTimePicker1.Value.Date > today.AddYears(-age))
            {
                age--;
            }

            // Check if the applicant's age is valid
            if (age < 18 || age > 99)
            {
                // Show MessageBox if age is invalid
                MessageBox.Show("Age is invalid. Must be between 18 and 99.", "Invalid Age", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Change background color to LightGreen for invalid age
                dateTimePicker1.BackColor = Color.LightGreen;
            }
            else
            {
                // Reset the background color if age is valid
                dateTimePicker1.BackColor = Color.White;
            }
        }
    }

}
