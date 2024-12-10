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
    public partial class SalaryInstructor : Form
    {
        public SalaryInstructor()
        {
            InitializeComponent();
        }

        private void SetSalary_Click(object sender, EventArgs e)//set the salary-insert button
        {
            if (InstructorIDCbox.SelectedItem != null && !string.IsNullOrWhiteSpace(SalaryAmount.Text))
            {
                string selectedInstructorID = InstructorIDCbox.SelectedItem.ToString();
                decimal salary;

                if (decimal.TryParse(SalaryAmount.Text, out salary))
                {
                    using (SqlConnection conn = new SqlConnection(@"Data Source=localhost;Initial Catalog=Instructor;Integrated Security=True"))
                    {
                        conn.Open();
                        string query = "UPDATE Instructor SET Salary = @Salary WHERE InstructorID = @InstructorID";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Salary", salary);
                            cmd.Parameters.AddWithValue("@InstructorID", selectedInstructorID);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Salary updated successfully.");
                                loadInstructor();                            
                            }
                            else
                            {
                                MessageBox.Show("Failed to update salary.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid salary amount.");
                }
            }
            else
            {
                MessageBox.Show("Please select an Instructor ID and enter a salary.");
            }
        }

        private void UpdateSalary_Click(object sender, EventArgs e) //button to update salary
        {
            if (InstructorIDCbox.SelectedItem != null && !string.IsNullOrWhiteSpace(SalaryAmount.Text))
            {
                string selectedInstructorID = InstructorIDCbox.SelectedItem.ToString();
                decimal salary;

                if (decimal.TryParse(SalaryAmount.Text, out salary))
                {
                    using (SqlConnection conn = new SqlConnection(@"Data Source=localhost;Initial Catalog=Instructor;Integrated Security=True"))
                    {
                        conn.Open();
                        string query = "UPDATE Instructor SET Salary = @Salary WHERE InstructorID = @InstructorID";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Salary", salary);
                            cmd.Parameters.AddWithValue("@InstructorID", selectedInstructorID);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Salary updated successfully.");
                                loadInstructor();
                            }
                            else
                            {
                                MessageBox.Show("Update failed. Instructor not found.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid numeric salary.");
                }
            }
            else
            {
                MessageBox.Show("Please select an Instructor ID and enter a salary.");
            }
        }

        private void SalaryAmount_TextChanged(object sender, EventArgs e)//This a textbox
        {

        }

        private void InstructorIDCbox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1Salary_CellContentClick(object sender, DataGridViewCellEventArgs e) //View the  and instructorID And Firstname and Lastname ,salary
        {

        }
        private void LoadInstructors()
        {
            InstructorIDCbox.Items.Clear();

            using (SqlConnection conn = new SqlConnection(@"Data Source=localhost;Initial Catalog=Instructor;Integrated Security=True"))
            {
                conn.Open();
                string query = "SELECT InstructorID FROM Instructor";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        InstructorIDCbox.Items.Add(reader["InstructorID"].ToString());
                    }
                }
            }
            loadInstructor();
        }

        private void SalaryInstructor_Load(object sender, EventArgs e)
        {
            LoadInstructors();
            loadInstructor();
            dataGridView1Salary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void button1_Click(object sender, EventArgs e) //Button search with the combobox
        {
            string selectedInstructorID = InstructorIDCbox.Text.Trim(); // Handles both typed and selected values

            using (SqlConnection conn = new SqlConnection(@"Data Source=localhost;Initial Catalog=Instructor;Integrated Security=True"))
            {
                conn.Open();
                string query = "SELECT * FROM Instructor WHERE InstructorID = @InstructorID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@InstructorID", selectedInstructorID);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        SalaryAmount.Text = reader["Salary"].ToString();
                        loadInstructor();

                    }
                    else
                    {
                        MessageBox.Show("Instructor not found.");
                    }
                }
            }

        }

        private void loadInstructor()
        {
            string query;
            bool filterByID = !string.IsNullOrWhiteSpace(InstructorIDCbox.Text); // Check if there's input

            if (filterByID)
            {
                query = "SELECT InstructorID, FirstName, LastName, Salary FROM Instructor WHERE InstructorID = @InstructorID";
            }
            else
            {
                query = "SELECT InstructorID, FirstName, LastName, Salary FROM Instructor"; // Load all
            }

            using (SqlConnection conn = new SqlConnection(@"Data Source=localhost;Initial Catalog=Instructor;Integrated Security=True"))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (filterByID)
                    {
                        cmd.Parameters.AddWithValue("@InstructorID", InstructorIDCbox.Text.Trim());
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1Salary.DataSource = dataTable;
                    }
                }
            }
        }

    }
}
