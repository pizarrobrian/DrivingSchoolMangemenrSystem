using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Driving_Management_System
{
    public partial class InstructorDS : Form
    {
        public InstructorDS()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadform(new ApplicationFormInstructor());
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
        private void mainpanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void InstructorDS_Load(object sender, EventArgs e)
        {
            label1.Text = $"Welcome, {CurrentUser.Username}";
            ShowDashboardAdmin();
            this.FormBorderStyle = FormBorderStyle.None;

        }
        private void ShowDashboardAdmin()
        {
            Form formToShow = new ApplicationFormInstructor
            {
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

        private void button2_Click(object sender, EventArgs e)
        {
            loadform(new InstructorSchedule());

        }

        private void button4_Click(object sender, EventArgs e)
        {
            loadform(new InstructorProfile());

        }

        private void button5_Click(object sender, EventArgs e)
        {
            CurrentUser.Username = null;
            CurrentUser.IsAdmin = false;

            // Optionally, navigate back to the login form
            Login1 loginForm = new Login1();
            loginForm.Show();

            // Close the current form
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            loadform(new StudentNotification());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadform(new Salary());

        }

        private void button7_Click(object sender, EventArgs e)
        {
            loadform(new Grading());
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            panel2.BorderStyle = BorderStyle.FixedSingle;  // Simple black border

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.BorderStyle = BorderStyle.FixedSingle;  // Simple black border

        }
    }
}
