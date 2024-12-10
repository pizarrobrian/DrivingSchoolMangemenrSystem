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
    public partial class Login1 : Form
    {
        public Login1()
        {
            InitializeComponent();
        }

        // Connection for admin login (admin database)
        SqlConnection connAdmin = new SqlConnection(@"Data Source=XIANVILLE;Initial Catalog=adminDSM;Integrated Security=True");

        // Connection for student login (student database)
        SqlConnection connStudent = new SqlConnection("data source=localhost; database=StudentInfo; Integrated Security=True;");

        // Connection for instructor login (instructor database)
        SqlConnection connInstructor = new SqlConnection("data source=localhost; database=Instructor; Integrated Security=True;");
        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }

       


        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string user_password = textBox2.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(user_password))
            {
                MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Queries for login
                String queryAdmin = "SELECT * FROM Login_new WHERE username = @username AND password = @password";
                String queryStudent = "SELECT * FROM Students WHERE username = @username AND password = @password";
                String queryInstructor = "SELECT * FROM Instructor WHERE username = @username AND password = @password";

                SqlCommand cmd;
                SqlConnection activeConnection;

                bool isAdminLogin = username.Equals("admin", StringComparison.OrdinalIgnoreCase);

                if (isAdminLogin)
                {
                    cmd = new SqlCommand(queryAdmin, connAdmin);
                    activeConnection = connAdmin;
                }
                else
                {
                    // Check for instructor login first, assuming instructors may have priority.
                    cmd = new SqlCommand(queryInstructor, connInstructor);
                    activeConnection = connInstructor;
                    activeConnection.Open();

                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", user_password);

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dtable = new DataTable();
                    sda.Fill(dtable);

                    if (dtable.Rows.Count > 0)
                    {
                        // Instructor login successful
                        CurrentUser.Username = username;
                        CurrentUser.Password = user_password;
                        CurrentUser.IsAdmin = false; // Not admin
                        InstructorDS frm = new InstructorDS(); // New form for instructors
                        frm.Show();
                        this.Hide();
                        return;
                    }
                    else
                    {
                        // If not an instructor, proceed to student login
                        cmd = new SqlCommand(queryStudent, connStudent);
                        activeConnection = connStudent;
                    }
                }

                activeConnection.Open();
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", user_password);

                SqlDataAdapter sdaFinal = new SqlDataAdapter(cmd);
                DataTable dtableFinal = new DataTable();
                sdaFinal.Fill(dtableFinal);

                if (dtableFinal.Rows.Count > 0)
                {
                    CurrentUser.Username = username;
                    CurrentUser.Password = user_password;
                    CurrentUser.IsAdmin = isAdminLogin;

                    if (isAdminLogin)
                    {
                        AdminDS frm = new AdminDS();
                        frm.Show();
                    }
                    else
                    {
                        SstudentDS frm = new SstudentDS();
                        frm.Show();
                    }

                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid login details", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox1.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred while logging in: {ex.Message}");
            }
            finally
            {
                if (connAdmin.State == ConnectionState.Open)
                    connAdmin.Close();
                if (connStudent.State == ConnectionState.Open)
                    connStudent.Close();
                if (connInstructor.State == ConnectionState.Open)
                    connInstructor.Close();
            }

        }


        private void button3_Click(object sender, EventArgs e)
        {
            // Clear input fields and set focus back to username
            textBox1.Clear();
            textBox2.Clear();
            textBox1.Focus();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Open the signup form
            Signup4 frm = new Signup4();
            frm.Show();
            this.Hide();
        }

        


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {

                textBox2.PasswordChar = '\0';// Assigning the password character
            }
            else {

                textBox2.PasswordChar = '*';


            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
