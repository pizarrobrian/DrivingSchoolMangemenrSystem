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

namespace Driving_Management_System
{
    public partial class Schedule : Form
    {
        public Schedule()
        {
            InitializeComponent();
        }
        SqlConnection connSc = new SqlConnection("data source=localhost; database=Sche; Integrated Security=True;");
        SqlConnection connStu = new SqlConnection("data source=localhost; database=StudentInfo; Integrated Security=True;");
        SqlConnection connIns = new SqlConnection("data source=localhost; database=Instructor; Integrated Security=True;");
        SqlConnection connVeh = new SqlConnection("data source=localhost; database=Vehicle; Integrated Security=True;");
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void LoadStudentNames()
        {
            try
            {
                connStu.Open();
                SqlCommand comm = new SqlCommand("SELECT StudentID, FirstName, LastName, Username FROM [Students]", connStu);
                SqlDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    string fullName = $"{reader["FirstName"]} {reader["LastName"]}";
                    string studentID = reader["StudentID"].ToString();
                    string username = reader["Username"].ToString();

                    comboBox1.Items.Add(new KeyValuePair<string, string>(studentID, $"{fullName} ({username})"));
                }

                comboBox1.DisplayMember = "Key"; // Full name with username
                comboBox1.ValueMember = "Value";     // Student ID
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student names: " + ex.Message);
            }
            finally
            {
                connStu.Close();
            }

        }
        private void LoadInstructorNames()
        {
            try
            {
                connIns.Open();
                SqlCommand comm = new SqlCommand("SELECT InstructorID, FirstName, LastName, Username FROM [Instructor]", connIns);
                SqlDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    string fullName = $"{reader["FirstName"]} {reader["LastName"]}";
                    string instructorID = reader["InstructorID"].ToString();
                    string username = reader["Username"].ToString();

                    // Add full name with username in parentheses
                    comboBox2.Items.Add(new KeyValuePair<string, string>(instructorID, $"{fullName} ({username})"));
                }

                comboBox2.DisplayMember = "Key"; // Full name with username
                comboBox2.ValueMember = "Value";     // InstructorID
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading instructor names: " + ex.Message);
            }
            finally
            {
                connIns.Close();
            }
        }
        private void LoadVehicle()
        {
            try
            {
                connVeh.Open();
                SqlCommand comm = new SqlCommand("SELECT VehicleID, Model, LicensePLate FROM [Vehicles]", connVeh);
                SqlDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    string fullName = $"{reader["Model"]} {reader["LicensePLate"]}";
                    string InstructorID = reader["VehicleID"].ToString();
                    comboBox3.Items.Add(new KeyValuePair<string, string>(InstructorID, fullName));

                }
                comboBox3.DisplayMember = "Key";
                comboBox3.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student names: " + ex.Message);
            }
            finally
            {
                connVeh.Close();
            }
        }
        private void Schedule_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            LoadVehicle();
            LoadStudentNames();
            LoadInstructorNames();
            display();
            LoadLessonID();
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";
            label11.Text = "";
            label12.Text = "";

        }

  
        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("data source=localhost; database=Sche; Integrated Security=True;"))
            {
                conn.Open();

                string query = "INSERT INTO Lesson (StudentID, InstructorID, VehicleID, LessonDate, LessonStatus, LessonType, Duration, StudentName, InstructorName, Vehicle, Username, IUsername) " +
                               "VALUES (@StudentID, @InstructorID, @VehicleID, @LessonDate, @LessonStatus, @LessonType, @Duration, @StudentName, @InstructorName, @Vehicle, @Username, @IUsername)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@InstructorID", comboBox2.Text);
                    cmd.Parameters.AddWithValue("@VehicleID", comboBox3.Text);
                    cmd.Parameters.AddWithValue("@LessonDate", dateTimePicker1.Value.Date); // Remove time if required
                    cmd.Parameters.AddWithValue("@LessonStatus", comboBox5.Text);
                    cmd.Parameters.AddWithValue("@LessonType", comboBox4.Text);
                    cmd.Parameters.AddWithValue("@Duration", textBox1.Text);
                    cmd.Parameters.AddWithValue("@StudentName", label7.Text);
                    cmd.Parameters.AddWithValue("@InstructorName", label8.Text);
                    cmd.Parameters.AddWithValue("@Vehicle", label9.Text);
                    cmd.Parameters.AddWithValue("@Username", label11.Text); // Adding Username from label11
                    cmd.Parameters.AddWithValue("@IUsername", label12.Text);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Data Inserted Successfully.");
            }

            display();
        }


        private void display()
        {
            try
            {
                connSc.Open();
                string load = "SELECT * FROM Lesson";
                SqlDataAdapter sda = new SqlDataAdapter(load, connSc);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
                connSc.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in loading the data table: " + ex);
            }
            finally
            {
                connSc.Close();
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

                if (comboBox1.SelectedItem != null)
                {
                    var selectedItem = (KeyValuePair<string, string>)comboBox1.SelectedItem;

                    // Extract the full name and username
                    string fullNameWithUsername = selectedItem.Value; // "FullName (username)"
                    int startIndex = fullNameWithUsername.IndexOf("(");
                    int endIndex = fullNameWithUsername.IndexOf(")");

                    // Extract the full name (without username) and set it to label7
                    string fullName = startIndex != -1 ? fullNameWithUsername.Substring(0, startIndex).Trim() : fullNameWithUsername;
                    label7.Text = fullName;

                    // Extract the username and set it to label11
                    if (startIndex != -1 && endIndex != -1)
                    {
                        string username = fullNameWithUsername.Substring(startIndex + 1, endIndex - startIndex - 1); // Extracts 'username'
                        label11.Text = username; // Set the username to label11
                    }
                }
       


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
            {
            if (comboBox2.SelectedItem != null)
            {
                var selectedItem = (KeyValuePair<string, string>)comboBox2.SelectedItem;

                // Extract the full name and username
                string fullNameWithUsername = selectedItem.Value; // "FullName (username)"
                int startIndex = fullNameWithUsername.IndexOf("(");
                int endIndex = fullNameWithUsername.IndexOf(")");

                // Extract the full name (without username) and set it to label7
                string fullName = startIndex != -1 ? fullNameWithUsername.Substring(0, startIndex).Trim() : fullNameWithUsername;
                label8.Text = fullName;

                // Extract the username and set it to label11
                if (startIndex != -1 && endIndex != -1)
                {
                    string username = fullNameWithUsername.Substring(startIndex + 1, endIndex - startIndex - 1); // Extracts 'username'
                    label12.Text = username; // Set the username to label11
                }
            }



        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem != null)
            {
                var selectedItem = (KeyValuePair<string, string>)comboBox3.SelectedItem;
                label9.Text = selectedItem.Value;  // Full Name
                string InstructorID = selectedItem.Key;

            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                connSc.Open();
                SqlCommand comm = new SqlCommand("SELECT * FROM Lesson WHERE LessonID = @LessonID", connSc);
                comm.Parameters.AddWithValue("@LessonID", comboBox7.Text);

                SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    comboBox1.Text = reader.IsDBNull(5) ? "" : reader.GetValue(5).ToString();
                    comboBox2.Text = reader.IsDBNull(7) ? "" : reader.GetValue(7).ToString();
                    comboBox3.Text = reader.IsDBNull(9) ? "" : reader.GetValue(9).ToString();

                    // Properly parse the DateTime value (column index 1)
                    if (!reader.IsDBNull(1))
                    {
                        dateTimePicker1.Value = Convert.ToDateTime(reader.GetValue(1));
                    }

                    comboBox4.Text = reader.IsDBNull(2) ? "" : reader.GetValue(2).ToString();
                    textBox1.Text = reader.IsDBNull(3) ? "" : reader.GetValue(3).ToString();
                    comboBox5.Text = reader.IsDBNull(4) ? "" : reader.GetValue(4).ToString();

                    label7.Text = reader.IsDBNull(6) ? "" : reader.GetValue(6).ToString();
                    label8.Text = reader.IsDBNull(8) ? "" : reader.GetValue(8).ToString();
                    label9.Text = reader.IsDBNull(10) ? "" : reader.GetValue(10).ToString();
                    label11.Text = reader.IsDBNull(11) ? "" : reader.GetValue(11).ToString();
                    label12.Text = reader.IsDBNull(12) ? "" : reader.GetValue(12).ToString();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in searching: " + ex.Message);
            }
            finally
            {
                connSc.Close();
            }
            display();
        }


        private void LoadLessonID()
        {
            try
            {
                connSc.Open();
                SqlCommand comm = new SqlCommand("SELECT LessonID FROM [Lesson]", connSc);
                SqlDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    comboBox7.Items.Add(reader["LessonID"].ToString());
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Lesson: " + ex.Message);
            }
            finally
            {
                connSc.Close();
            }
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            string duration = textBox1.Text;
            try
            {
                connSc.Open();

                // Complete SQL update query including all fields.
                string query = "UPDATE Lesson SET " +
                               "StudentID = '" + comboBox1.Text + "', " +
                               "InstructorID = '" + comboBox2.Text + "', " +
                               "VehicleID = '" + comboBox3.Text + "', " +
                               "LessonDate = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "', " +
                               "LessonStatus = '" + comboBox5.Text + "', " +
                               "LessonType = '" + comboBox4.Text + "', " +
                               "Duration = '" + duration + "', " +
                               "StudentName = '" + label7.Text + "', " +
                               "InstructorName = '" + label8.Text + "', " +
                               "Vehicle = '" + label9.Text + "' " +
                               "WHERE LessonID = '" + comboBox7.Text + "'";

                SqlDataAdapter sda = new SqlDataAdapter(query, connSc);
                sda.SelectCommand.ExecuteNonQuery();

                MessageBox.Show("Updated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connSc.Close();
            }
            display();

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
