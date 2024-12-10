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
    public partial class Grading : Form
    {
        public Grading()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e) //Searchtextbox
        {
            SearchLesson();
            Schedule();
        }

        private void button2_Click(object sender, EventArgs e) //searchbutton
        {
            SearchLesson();
            Schedule();
        }

        private void Grading_Load(object sender, EventArgs e)
        {
            Schedule();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            FormBorderStyle = FormBorderStyle.None;



        }
        private void Schedule()
        {
            try
            {
                // Define the query to fetch specific columns
                string query = @"SELECT StudentID, StudentName, LessonType, Duration, LessonStatus, Username
                         FROM Lesson
                         WHERE IUsername = @IUsername";

                using (SqlConnection connStudent = new SqlConnection("data source=localhost; database=Sche; Integrated Security=True;"))
                {
                    // Ensure the connection is open
                    if (connStudent.State == ConnectionState.Closed)
                        connStudent.Open();

                    // Create a DataTable to hold the results
                    DataTable dt = new DataTable("Lessons");

                    // Define the SQL command to execute the query
                    using (SqlCommand cmd = new SqlCommand(query, connStudent))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@IUsername", CurrentUser.Username);

                        // Execute the query and fill the DataTable
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }

                    // Bind the DataTable to the DataGridView
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching schedule: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SearchLesson()
        {
            try
            {
                // Define the query to fetch specific columns based on StudentID
                string query = @"SELECT StudentID, StudentName, LessonType, Duration, LessonStatus, Username
                         FROM Lesson
                         WHERE StudentID LIKE @StudentID AND IUsername = @IUsername";

                using (SqlConnection connStudent = new SqlConnection("data source=localhost; database=Sche; Integrated Security=True;"))
                {
                    // Ensure the connection is open
                    if (connStudent.State == ConnectionState.Closed)
                        connStudent.Open();

                    // Create a DataTable to hold the results
                    DataTable dt = new DataTable("Lessons");

                    // Define the SQL command
                    using (SqlCommand cmd = new SqlCommand(query, connStudent))
                    {
                        // Add parameters for StudentID and IUsername
                        cmd.Parameters.AddWithValue("@StudentID", "%" + textBox2.Text + "%");  // Using LIKE for partial match
                        cmd.Parameters.AddWithValue("@IUsername", CurrentUser.Username);
                        // Execute the query and fill the DataTable
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }

                    // Bind the DataTable to the DataGridView
                    dataGridView1.DataSource = dt;

                    // Set the LessonStatus in comboBox1
                    if (dt.Rows.Count > 0)
                    {
                        // Get the LessonStatus of the first row (or relevant row based on other conditions)
                        string lessonStatus = dt.Rows[0]["LessonStatus"].ToString();
                        string username = dt.Rows[0]["Username"].ToString();
                        string lessontype = dt.Rows[0]["LessonType"].ToString();

                        // Set the LessonStatus in comboBox1
                        comboBox1.SelectedItem = lessonStatus;
                        label1.Text = username;
                        label2.Text = lessontype;
                        // Assuming the status is available in the ComboBox
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching lessons: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //display the lesson status in this which is update the lesson status
        {
           
        }

        private void button1_Click(object sender, EventArgs e)//button to update
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a lesson status.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter the StudentID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedLessonStatus = comboBox1.SelectedItem.ToString();
            string studentID = textBox2.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection("data source=localhost; database=Sche; Integrated Security=True;"))
                {
                    string updateQuery = @"UPDATE Lesson 
                                   SET LessonStatus = @LessonStatus
                                   WHERE StudentID = @StudentID";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@LessonStatus", selectedLessonStatus);
                        cmd.Parameters.AddWithValue("@StudentID", studentID);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Lesson status updated successfully.");
                            Schedule(); // Assuming this updates your DataGridView or relevant UI
                        }
                        else
                        {
                            MessageBox.Show("No record found for the student with the given StudentID.");
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating lesson status: " + ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // If error occurs, stop further execution
            }

            // Then, insert the grade status into the new table
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Please select a grade status (Pass/Fail).", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ensure StudentID and Username are valid
            if (string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(label1.Text))
            {
                MessageBox.Show("StudentID and Username cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Establish a connection to the StudentGrades database
                using (SqlConnection conn = new SqlConnection("data source=localhost; database=Instructor; Integrated Security=True;"))
                {
                    // Create an SQL query to insert the grade status
                    string insertQuery = @"INSERT INTO StudentGrades (StudentID, Username, Grade, LessonType)
                                   VALUES (@StudentID, @Username, @Grade, @LessonType)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Grade", comboBox2.SelectedItem);
                        cmd.Parameters.AddWithValue("@StudentID", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Username", label1.Text);
                        cmd.Parameters.AddWithValue("@LessonType", label2.Text);
                        // Open the connection
                        conn.Open();

                        // Execute the insert query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Show a message based on whether any rows were inserted
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Grade status inserted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Failed to insert grade status.");
                        }

                        // Close the connection
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting grade status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}