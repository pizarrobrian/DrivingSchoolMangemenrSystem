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
    public partial class SstudentDS : Form
    {
        public SstudentDS()
        {
            InitializeComponent();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        

        public void loadform(object Form)
        {
            if (this.mainpanel.Controls.Count > 0)
            {
                this.mainpanel.Controls.RemoveAt(0);
            }
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.mainpanel.Controls.Add(f);
            this.mainpanel.Tag = f;
            f.Show();
           


        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadform(new ApplicationFormStudent());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadform(new Payment());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadform(new StudentSchedule());
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            panel2.BorderStyle = BorderStyle.FixedSingle;  // Simple black border

        }

        private void button5_Click(object sender, EventArgs e)
        {
            loadform(new UserProfile());
            

        }

        private void button6_Click(object sender, EventArgs e)
        {
            loadform(new Message());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Reset static properties
            CurrentUser.Username = null;
            CurrentUser.IsAdmin = false;

            // Optionally, navigate back to the login form
            Login1 loginForm = new Login1();
            loginForm.Show();

            // Close the current form
            this.Close();
        }

        private void SstudentDS_Load(object sender, EventArgs e)
        {
            label1.Text = $"Welcome, {CurrentUser.Username}";
            this.FormBorderStyle = FormBorderStyle.None;
            ShowDashboardAdmin();


        }
        private void ShowDashboardAdmin()
        {
            Form formToShow = new ApplicationFormStudent            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };

            mainpanel.SuspendLayout();
            mainpanel.Controls.Clear();
            mainpanel.Controls.Add(formToShow);
            mainpanel.AutoScroll = false;
            formToShow.Show();
            mainpanel.ResumeLayout();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.BorderStyle = BorderStyle.FixedSingle;  // Simple black border

        }

        private void button8_Click(object sender, EventArgs e)
        {
            loadform(new StudentNotification());

        }

        private void button4_Click(object sender, EventArgs e)
        {
            loadform(new StudentBalance());
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            loadform(new StudentGrade());
        }
    }
}
