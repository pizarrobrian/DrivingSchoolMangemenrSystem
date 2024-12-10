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
    public partial class StudentBalance : Form
    {
        public StudentBalance()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void GetInstructorSalary()
        {
            string salary = "";

            using (SqlConnection conn = new SqlConnection(@"Data Source=localhost;Initial Catalog=StudentInfo;Integrated Security=True"))
            {
                conn.Open();

                // SQL query to get the instructor's salary based on the username
                string query = "SELECT Balance FROM Students WHERE Username = @Username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", CurrentUser.Username); // Use CurrentUser.Username as the parameter

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        // Get the salary from the query result
                        salary = reader["Balance"].ToString();
                    }
                }
            }

            // Set the label's text to the salary
            if (!string.IsNullOrEmpty(salary))
            {
                label1.Text = $"Balance: {salary}";  // Assuming you have a label named labelSalary
            }
            else
            {
                label1.Text = "Balance not found.";  // Handle case where no salary is found
            }
        }

        private void StudentBalance_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None; 
            GetInstructorSalary();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
