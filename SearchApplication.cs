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
    public partial class SearchApplication : Form
    {
        public SearchApplication()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection("data source=localhost; database=StudentInfo; Integrated Security=True;");

        private void button1_Click(object sender, EventArgs e)
        {
            //string s_name = comboBox1.Text;  // Assuming you search by first name
            
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("SELECT * FROM Students WHERE StudentID = '" + comboBox1.Text + "'AND FirstName + ' ' + LastName = '" + comboBox2.Text + "'", conn);


                SqlDataReader reader;
                reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    textBox1.Text = reader.GetValue(3).ToString();
                    textBox2.Text = reader.GetValue(4).ToString();
                    dateTimePicker1.Value = Convert.ToDateTime(reader.GetValue(5));
                    textBox4.Text = reader.GetValue(6).ToString();
                    textBox5.Text = reader.GetValue(7).ToString();
                    textBox6.Text = reader.GetValue(8).ToString();

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in searching: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        // This method will load all student names into the ComboBox when the form loads
        private void LoadStudentNames()
        {
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("SELECT StudentID, FirstName, LastName FROM [Students]", conn);
                SqlDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["StudentID"].ToString());
                    string fullName = $"{reader["FirstName"]} {reader["LastName"]}";
                    comboBox2.Items.Add(fullName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student names: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }


        private void display() {

            try
            {
                conn.Open();
                string load = "SELECT * FROM Students";
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

        private void SearchApplication_Load(object sender, EventArgs e)
        {
            display() ;
            LoadStudentNames();
            FormBorderStyle = FormBorderStyle.None;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Please fill all the needed.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ensure the connection is opened
            conn.Open();

            // Use parameterized queries to prevent SQL injection
            string query = "INSERT INTO Students (Username, Password, FirstName, LastName, DateOfBirth, Phone, Email, Address) " +
                           "VALUES (@Username, @Password, @FirstName, @LastName, @DateOfBirth, @Phone, @Email, @Address)";

            // Create a SqlCommand object and set the query and connection
            SqlCommand cmd = new SqlCommand(query, conn);

            // Add parameters to the command to prevent SQL injection
            cmd.Parameters.AddWithValue("@Username", textBox7.Text); // Username from textBox7
            cmd.Parameters.AddWithValue("@Password", textBox8.Text); // Password from textBox8
            cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
            cmd.Parameters.AddWithValue("@LastName", textBox2.Text);
            cmd.Parameters.AddWithValue("@DateOfBirth", dateTimePicker1.Value); // Ensure Date is properly formatted
            cmd.Parameters.AddWithValue("@Phone", textBox4.Text);
            cmd.Parameters.AddWithValue("@Email", textBox5.Text);
            cmd.Parameters.AddWithValue("@Address", textBox6.Text);

            // Execute the query
            cmd.ExecuteNonQuery();

            // Show a success message
            MessageBox.Show("Data Inserted Successfully.");

            // Close the connection
            conn.Close();


            clear();
            display();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                // Split the full name into first and last names
                string[] nameParts = comboBox2.Text.Split(' ');
                if (nameParts.Length < 2)
                {
                    MessageBox.Show("Invalid name format. Please ensure the full name includes both first and last names.");
                    conn.Close();
                    return;
                }

                string firstName = nameParts[0];
                string lastName = nameParts[1];

                SqlCommand del = new SqlCommand("DELETE FROM Students WHERE StudentID = '" + comboBox1.Text + "' AND FirstName = '" + firstName + "' AND LastName = '" + lastName + "'", conn);
                del.ExecuteNonQuery();

                MessageBox.Show("Successfully deleted");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            clear();
            display();
        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Update_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
               // SqlCommand cmd = new SqlCommand("insert into Students values('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "' , '" + textBox4.Text + "' , '" + textBox5.Text + "' , '" + textBox6.Text + "')", conn);

                string query = "UPDATE Students SET Phone = '" + textBox4.Text + "', Email = '" + textBox5.Text + "' , Address = '" + textBox6.Text + "' WHERE FirstName = '" + textBox1.Text + "'";
                SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                sda.SelectCommand.ExecuteNonQuery();
                MessageBox.Show("Updated successfully");
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

            display();
            clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void REF_Click(object sender, EventArgs e)
        {

        }

        private void clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            comboBox1.SelectedIndex = -1; // Reset the selection to null/empty
            comboBox2.SelectedIndex = -1; // Reset the selection to null/empty

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            clear();
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
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(textBox5.Text, pattern))
            {
                textBox5.BackColor = System.Drawing.Color.Red; // Incorrect input
                return;
            }
            else
            {
                textBox5.BackColor = System.Drawing.Color.White; // Correct input
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^[A-Za-z]+([ A-Za-z]+)*$";
            if (!Regex.IsMatch(textBox6.Text, pattern))
            {
                textBox6.BackColor = System.Drawing.Color.Red; // Incorrect input
                return;
            }
            else
            {
                textBox6.BackColor = System.Drawing.Color.White; // Correct input
            }
        }
    }
}

