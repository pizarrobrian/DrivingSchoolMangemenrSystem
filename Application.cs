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
using System.Text.RegularExpressions;

namespace Driving_Management_System
{
    public partial class ApplicationS2 : Form
    {
        public ApplicationS2()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("data source=localhost; database=master; Integrated Security=True;");
        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Please fill all the needed.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!SubmitApplication())
            {
                return;
            }
            eml();
            birth();
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Studentss values('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "' , '" + textBox4.Text + "' , '" + textBox5.Text + "' , '" + textBox6.Text + "')",con);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Data Inserted Successfully.\n Proceed to payment");
            con.Close();
            StudentPDF frm = new StudentPDF();
            frm.Show();
        }
       
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Regex pattern to match MM/DD/YYYY format
            string pattern = @"^(0[1-9]|1[0-2])\/(0[1-9]|[12][0-9]|3[01])\/\d{4}$";
            string input = textBox3.Text;

            if (Regex.IsMatch(input, pattern) && IsValidDate(input))
            {
                // Input is valid
                textBox3.BackColor = Color.LightGreen; // Example feedback
            }
            else
            {
                // Input is invalid
                textBox3.BackColor = Color.LightCoral; // Example feedback
            }
        }

        // Optional method to further validate the date (e.g., check for valid day/month combinations)
        private bool IsValidDate(string input)
        {
            try
            {
                // Try to parse the date to ensure it's a valid calendar date
                DateTime parsedDate = DateTime.ParseExact(input, "MM/dd/yyyy", null);

                // You can also check if the user is above a certain age, e.g., at least 18 years old
                int age = DateTime.Now.Year - parsedDate.Year;
                if (DateTime.Now.Month < parsedDate.Month || (DateTime.Now.Month == parsedDate.Month && DateTime.Now.Day < parsedDate.Day))
                {
                    age--;
                }

                // Example: Ensure the user is at least 18 years old
                if (age < 18)
                {
                    textBox3.BackColor = Color.LightCoral; // Feedback if the user is too young
                    return false;
                }

                return true; // Valid date and age
            }
            catch
            {
                return false; // Invalid date
            }
        }


        private void textBox4_TextChanged(object sender, EventArgs e)
        {

            string pattern = @"^(09\d{9})|(\+639\d{9})$";
            string input = textBox4.Text;

            if (Regex.IsMatch(input, pattern) &&
                (input.Length == 11 || input.Length == 13))
            {
                // Input is valid
                textBox4.BackColor = Color.LightGreen; // Example feedback
            }
            else
            {
                // Input is invalid
                textBox4.BackColor = Color.LightCoral; // Example feedback
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void eml() {


            
               string emailPattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
               string email = textBox5.Text;

               // Email validation for Gmail
               if (Regex.IsMatch(email, emailPattern))
               {
                   MessageBox.Show("Valid Gmail address.");
               }
               else
               {
                   MessageBox.Show("Invalid Gmail address. Please ensure the email ends with @gmail.com.");
               }
               


        }

        private void birth() {

            string dob = textBox3.Text; // Assuming TextBox for DOB input

            // Regex check for MM/DD/YYYY format
            string dobPattern = @"^(0[1-9]|1[0-2])/(0[1-9]|[12][0-9]|3[01])/([0-9]{4})$";

            if (Regex.IsMatch(dob, dobPattern))
            {
                DateTime parsedDate;
                if (DateTime.TryParse(dob, out parsedDate) && parsedDate <= DateTime.Now)
                {
                    MessageBox.Show("Valid date of birth.");
                }
                else
                {
                    MessageBox.Show("Invalid date of birth. Please ensure it's a real date and not in the future.");
                }
            }
            else
            {
                MessageBox.Show("Invalid date of birth format. Please use MM/DD/YYYY.");
            }


        }
        private bool SubmitApplication()
        {
            SqlConnection connApplication = new SqlConnection("data source=localhost; database=master; Integrated Security=True;");


            try
            {
                connApplication.Open();

                // Check if the user has already submitted the application
                string checkQuery = "SELECT HasSubmittedForm FROM SignupStudent WHERE username = @username";
                SqlCommand checkCmd = new SqlCommand(checkQuery, connApplication);
                checkCmd.Parameters.AddWithValue("@username", CurrentUser.Username);

                object result = checkCmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    bool hasSubmitted = Convert.ToBoolean(result);

                    if (hasSubmitted)
                    {
                        MessageBox.Show("You have already submitted your application and cannot submit again.", "Submission Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false; // Stop further processing
                    }
                }

                // Update query to mark the application as submitted
                string updateQuery = "UPDATE SignupStudent SET HasSubmittedForm = 1 WHERE username = @username";
                SqlCommand cmd = new SqlCommand(updateQuery, connApplication);
                cmd.Parameters.AddWithValue("@username", CurrentUser.Username);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Application submitted successfully.", "Submission", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true; // Allow further processing
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during application submission: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Stop further processing on error
            }
            finally
            {
                if (connApplication.State == ConnectionState.Open)
                {
                    connApplication.Close();
                }
            }
        }


    }
}
