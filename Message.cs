using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Driving_Management_System
{
    public partial class Message : Form
    {
        public Message()
        {
            InitializeComponent();
        }

        private void Message_Load(object sender, EventArgs e)
        {
            LoadStudents();
            LoadInstructor();
            this.FormBorderStyle = FormBorderStyle.None;

        }

        private void LoadStudents()
        {
            comboBox1.Items.Clear();
            label1.Text = ""; // Clear label before adding

            using (SqlConnection conn = new SqlConnection(@"Data Source=localhost;Initial Catalog=StudentInfo;Integrated Security=True"))
            {
                conn.Open();
                string query = "SELECT Username FROM Students";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["Username"].ToString());
                    }
                }
            }
        }


        private void LoadInstructor()
        {
            label2.Text = ""; // Clear label before adding

            comboBox2.Items.Clear();
            using (SqlConnection connInstructor = new SqlConnection(@"Data Source=localhost;Initial Catalog=Instructor;Integrated Security=True"))
            {
                connInstructor.Open();
                string query = "SELECT Username FROM Instructor"; // Removed WHERE clause
                using (SqlCommand cmd = new SqlCommand(query, connInstructor))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox2.Items.Add(reader["Username"].ToString());
                    }
                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
           
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Please enter a message.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (checkBox3.Checked) // All students
                {
                    AllStudent();
                }
                if (checkBox4.Checked) // All students
                {
                    AllInstructor();
                }
            else
                {
                    if (checkBox1.Checked && comboBox1.SelectedItem == null)
                    {
                        MessageBox.Show("Please select a student recipient.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (checkBox2.Checked && comboBox2.SelectedItem == null)
                    {
                        MessageBox.Show("Please select an instructor recipient.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (checkBox1.Checked)
                    {
                        Student();
                    }

                    if (checkBox2.Checked)
                    {
                        Instructor();
                    }
                }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Student()
        {
            string student = comboBox1.SelectedItem.ToString();
            string message = textBox1.Text.Trim();

            using (SqlConnection conn = new SqlConnection(@"Data Source=XIANVILLE;Initial Catalog=adminDSM;Integrated Security=True"))
            {
                conn.Open();
                string query = "INSERT INTO Notifications (RecipientUsername, Message, SenderUsername) VALUES (@Recipient, @Message, @SenderUsername)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SenderUsername", CurrentUser.Username);  // Add sender's username to the query
                    cmd.Parameters.AddWithValue("@Recipient", student);
                    cmd.Parameters.AddWithValue("@Message", message);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Message sent to student.");
        }

        private void Instructor()
        {
            string instructor = comboBox2.SelectedItem.ToString();
            string message = textBox1.Text.Trim();

            using (SqlConnection conn = new SqlConnection(@"Data Source=XIANVILLE;Initial Catalog=adminDSM;Integrated Security=True"))
            {
                conn.Open();
                string query = "INSERT INTO Notifications (RecipientUsername, Message, SenderUsername) VALUES (@Recipient, @Message, @SenderUsername)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SenderUsername", CurrentUser.Username);  // Add sender's username to the query
                    cmd.Parameters.AddWithValue("@Recipient", instructor);
                    cmd.Parameters.AddWithValue("@Message", message);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Message sent to instructor.");
        }



        private void AllStudent()
        {
            string message = textBox1.Text.Trim();

            using (SqlConnection conn = new SqlConnection(@"Data Source=localhost;Initial Catalog=StudentInfo;Integrated Security=True"))
            {
                conn.Open();  // Ensure the connection is open

                string query = "SELECT Username FROM Students"; // Adjust table and column names
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string student = reader["Username"].ToString();
                        // Use CurrentUser.Username as the sender
                        SendMessageToRecipient(student, message, CurrentUser.Username);
                    }
                }
                MessageBox.Show("Message sent to.");
            }
        }

        private void AllInstructor()
        {
            string message = textBox1.Text.Trim();

            using (SqlConnection conn = new SqlConnection(@"Data Source=localhost;Initial Catalog=Instructor;Integrated Security=True"))
            {
                conn.Open();  // Ensure the connection is open

                string query = "SELECT Username FROM Instructor";  // Adjust table and column names
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string instructor = reader["Username"].ToString();
                        // Use CurrentUser.Username as the sender
                        SendMessageToRecipient(instructor, message, CurrentUser.Username);
                    }
                }
                MessageBox.Show("Message sent.");

            }
        }



        private void SendMessageToRecipient(string recipient, string message, string senderUsername)
        {
            try
            {
                using (SqlConnection conn1 = new SqlConnection(@"Data Source=XIANVILLE;Initial Catalog=adminDSM;Integrated Security=True"))
                {
                    conn1.Open();

                    string insertQuery = "INSERT INTO Notifications (RecipientUsername, Message, SenderUsername) VALUES (@Recipient, @Message, @SenderUsername)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn1))
                    {
                        cmd.Parameters.AddWithValue("@Recipient", recipient);
                        cmd.Parameters.AddWithValue("@Message", message);
                        cmd.Parameters.AddWithValue("@SenderUsername", senderUsername);  // Add sender's username to the query
                        cmd.ExecuteNonQuery(); // Execute the insert command
                    }
                }

            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"General Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedUsername = comboBox1.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedUsername))
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source=localhost;Initial Catalog=StudentInfo;Integrated Security=True"))
                {
                    conn.Open();
                    string query = "SELECT StudentID FROM Students WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", selectedUsername);
                        object studentID = cmd.ExecuteScalar();
                        if (studentID != null)
                        {
                            label1.Text = $"StudentID: {studentID.ToString()}";
                        }
                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedUsername = comboBox2.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedUsername))
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source=localhost;Initial Catalog=Instructor;Integrated Security=True"))
                {
                    conn.Open();
                    string query = "SELECT InstructorID FROM Instructor WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", selectedUsername);
                        object InstructorID = cmd.ExecuteScalar();
                        if (InstructorID != null)
                        {
                            label2.Text = $"InstructorID: {InstructorID.ToString()}";
                        }
                    }
                }
            }
        }
    }

}

