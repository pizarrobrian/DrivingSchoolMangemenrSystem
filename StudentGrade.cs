using MaterialSkin;
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
using MaterialSkin.Controls;


namespace Driving_Management_System
{
    public partial class StudentGrade : Form
    {
        public StudentGrade()
        {
            InitializeComponent();
        }

        private void StudentGrade_Load(object sender, EventArgs e)
        {
            RetrieveGrade();
            this.FormBorderStyle = FormBorderStyle.None;
            


        }

        private void RetrieveGrade()
        {
            try
            {
                // Define the query to fetch LessonType and Grade for the current user
                string query = @"SELECT LessonType, Grade 
                         FROM StudentGrades 
                         WHERE Username = @Username";

                using (SqlConnection conn = new SqlConnection("data source=localhost; database=Instructor; Integrated Security=True;"))
                {
                    // Ensure the connection is open
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    // Create a DataTable to hold the results
                    DataTable dt = new DataTable();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters for Username and StudentID
                        cmd.Parameters.AddWithValue("@Username", CurrentUser.Username); // Assuming CurrentUser.Username is set properly

                        // Execute the query and fill the DataTable
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }

                    // Check if there are results, then populate the textboxes
                    if (dt.Rows.Count > 0)
                    {
                        // Display the LessonType and Grade in the respective textboxes
                        textBox3.Text = dt.Rows[0]["LessonType"].ToString();
                        label1.Text = dt.Rows[0]["Grade"].ToString();
                    }
                    else
                    {
                        label1.Text = "No grade yet";
                        textBox3.Clear();
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving grade: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
