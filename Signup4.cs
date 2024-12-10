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
    public partial class Signup4 : Form
    {
        public Signup4()
        {
            InitializeComponent();
        }
        SqlConnection connInstructor = new SqlConnection("data source=localhost; database=Instructor; Integrated Security=True;");


        private void Signup4_Load(object sender, EventArgs e)
        {
       
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboBox1.Text == "Student")
            {
                if (textBox2.Text == textBox3.Text)
                {
                    // Check if the username already exists
                    using (SqlConnection connStudent = new SqlConnection(@"Data Source=localhost;Initial Catalog=StudentInfo;Integrated Security=True"))
                    {
                        connStudent.Open();
                        string checkQuery = "SELECT COUNT(*) FROM Students WHERE Username = @Username";

                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, connStudent))
                        {
                            checkCmd.Parameters.AddWithValue("@Username", textBox1.Text);
                            int usernameCount = (int)checkCmd.ExecuteScalar();

                            if (usernameCount > 0)
                            {
                                // If the username already exists, show an error message
                                MessageBox.Show("Username is already taken.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return; // Exit method if username is already taken
                            }
                        }

                        // If the username is not taken, proceed with account creation
                        SqlCommand cmd = new SqlCommand(
                            "INSERT INTO Students (Username, Password, HasSubmittedForm) VALUES (@Username, @Password, @HasSubmittedForm)", connStudent);

                        cmd.Parameters.AddWithValue("@Username", textBox1.Text);
                        cmd.Parameters.AddWithValue("@Password", textBox2.Text);
                        cmd.Parameters.AddWithValue("@HasSubmittedForm", false); // Default value when creating an account

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Account created Successfully.");

                        connStudent.Close();
                        Login1 frm = new Login1();
                        frm.Show();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Please double check the password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else if (comboBox1.Text == "Instructor")
            {
                if (textBox2.Text == textBox3.Text)
                {
                    try
                    {
                        using (SqlConnection connInstructor = new SqlConnection(@"Data Source=localhost;Initial Catalog=Instructor;Integrated Security=True"))
                        {
                            connInstructor.Open();
                            string checkQuery = "SELECT COUNT(*) FROM Instructor WHERE Username = @Username";

                            using (SqlCommand checkCmd = new SqlCommand(checkQuery, connInstructor))
                            {
                                checkCmd.Parameters.AddWithValue("@Username", textBox1.Text);
                                int usernameCount = (int)checkCmd.ExecuteScalar();

                                if (usernameCount > 0)
                                {
                                    // If the username already exists, show an error message
                                    MessageBox.Show("Username is already taken.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return; // Exit method if username is already taken
                                }
                            }

                            SqlCommand cmd = new SqlCommand(
                            "INSERT INTO Instructor (Username, Password, HasSubmittedForm) VALUES (@Username, @Password, @HasSubmittedForm)", connInstructor);

                            cmd.Parameters.AddWithValue("@Username", textBox1.Text);
                            cmd.Parameters.AddWithValue("@Password", textBox2.Text);
                            cmd.Parameters.AddWithValue("@HasSubmittedForm", false); // Default to not submitted


                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Instructor account created successfully.");

                            Login1 frm = new Login1(); // Assuming `Login1` is the login form
                            frm.Show();
                            this.Hide();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error creating account: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (connInstructor.State == ConnectionState.Open)
                            connInstructor.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Passwords do not match. Please double-check the password.");
                }
            }
            else 
            {

                MessageBox.Show("Please select if you're student or instructor.");
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {

                textBox3.PasswordChar = '\0';// Assigning the password character
            }
            else
            {

                textBox3.PasswordChar = '*';


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Login1 frm = new Login1();
            frm.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
;        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$";
            if (!Regex.IsMatch(textBox2.Text, pattern))
            {
                textBox2.BackColor = System.Drawing.Color.Red;
                return;// Incorrect input
            }
            else
            {
                textBox2.BackColor = System.Drawing.Color.White; // Correct input
            }
        }
    }
}
