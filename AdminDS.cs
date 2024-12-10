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
    public partial class AdminDS : Form
    {
        public AdminDS()
        {
            InitializeComponent();
            this.Load += new EventHandler(AdminDS_Load);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            loadform(new SearchApplication());
            /*
            SearchApplication frm = new SearchApplication();
            frm.Show();
            this.Hide(); */
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadform(new DashboardAdmin());
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
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

        private void mainpanel_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadform(new Transacrtion());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            loadform(new Instructor());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            loadform(new Schedule());
        }

        private void AdminDS_Load(object sender, EventArgs e)
        {
            ShowDashboardAdmin();
            this.FormBorderStyle = FormBorderStyle.None;
            label1.Text = "Welcome, " + CurrentUser.Username;
        }
        private void ShowDashboardAdmin()
        {
            Form formToShow = new DashboardAdmin
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


        private void button6_Click(object sender, EventArgs e)
        {
            loadform(new Vehicle());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            CurrentUser.Username = null;
            CurrentUser.IsAdmin = false;

            Login1 loginForm = new Login1();
            loginForm.Show();

            // Close the current form
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            loadform(new Message());

        }

        private void button9_Click(object sender, EventArgs e)
        {
            loadform(new SalaryInstructor());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.BorderStyle = BorderStyle.FixedSingle;  // Simple black border

        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {
            panel2.BorderStyle = BorderStyle.FixedSingle;  // Simple black border

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            

        }

        private void button10_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
