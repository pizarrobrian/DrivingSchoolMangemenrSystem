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
    public partial class DashboardAdmin : Form
    {
        public DashboardAdmin()
        {
            InitializeComponent();
        }
        SqlConnection cn = new SqlConnection("data source=localhost; database=StudentInfo; Integrated Security=True;");
        SqlConnection con = new SqlConnection("data source=localhost; database=Instructor; Integrated Security=True;");
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void DashboardAdmin_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            DisplayLastUserId();
            DisplayInstructorUserId();
        }



        private void DisplayLastUserId()
        {
            string sql = "SELECT COUNT(*) AS TotalUsers FROM Students";
            SqlCommand cmd = new SqlCommand(sql, cn);

            try
            {
                cn.Open();
                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    if (read.Read())
                    {
                        // Retrieve the total number of users
                        label2.Text = read["TotalUsers"].ToString(); // Display total users in label
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }
        private void DisplayInstructorUserId()
        {
            string sql = "SELECT COUNT(*) AS TotalUsers FROM Instructor";
            SqlCommand cmd = new SqlCommand(sql, con);

            try
            {
                con.Open();
                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    if (read.Read())
                    {
                        // Retrieve the total number of users
                        label3.Text = read["TotalUsers"].ToString(); // Display total users in label
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
    }
}
