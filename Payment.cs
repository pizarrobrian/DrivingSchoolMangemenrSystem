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
    public partial class Payment : Form
    {
        public Payment()
        {
            InitializeComponent();
        }
        SqlConnection cn = new SqlConnection("data source=localhost; database=Payments; Integrated Security=True;");
        //SqlConnection conStudents = new SqlConnection("data source=localhost; database=master; Integrated Security=True;");

        private void button1_Click(object sender, EventArgs e)
        {
            // Check for required fields
            if (string.IsNullOrWhiteSpace(DriverID.Text) || string.IsNullOrWhiteSpace(FullName.Text) || string.IsNullOrWhiteSpace(Amount.Text) || string.IsNullOrWhiteSpace(Contact.Text) || string.IsNullOrWhiteSpace(PMethod.Text) || string.IsNullOrWhiteSpace(Reference.Text))
            {
                MessageBox.Show("Please fill all the needed.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Parse the payment amount from Amount.Text
            decimal amountPaid;
            if (!decimal.TryParse(Amount.Text, out amountPaid))
            {
                MessageBox.Show("Invalid amount entered.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string proofOfPayment = pictureBox1.ImageLocation; // Get the file path from the ImageLocation property
            if (string.IsNullOrEmpty(proofOfPayment) && PMethod.Text == "Gcash")
            {
                MessageBox.Show("Please select a Proof of Payment image.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
                try
                {
                    // Open Payments database connection
                    cn.Open();

                    // 1. Insert the payment details into the Payments table
                    SqlCommand cmdInsertPayment = new SqlCommand("INSERT INTO Payments (StudentID, FullName, Amount, ContactNo, PaymentMethod, ReferenceID, PaymentDate, ProofOfPayment) VALUES (@StudentID, @FullName, @Amount, @ContactNo, @PaymentMethod, @ReferenceID, GETDATE(), @ProofOfPayment)", cn);
                    cmdInsertPayment.Parameters.AddWithValue("@StudentID", DriverID.Text);
                    cmdInsertPayment.Parameters.AddWithValue("@FullName", FullName.Text);
                    cmdInsertPayment.Parameters.AddWithValue("@Amount", amountPaid);
                    cmdInsertPayment.Parameters.AddWithValue("@ContactNo", Contact.Text);
                    cmdInsertPayment.Parameters.AddWithValue("@PaymentMethod", PMethod.Text);

                if (PMethod.Text == "Cash")
                {
                    cmdInsertPayment.Parameters.AddWithValue("@ReferenceID", "Cash");
                    cmdInsertPayment.Parameters.AddWithValue("@ProofOfPayment", DBNull.Value);  // Insert NULL for cash payments
                }
                else
                {
                    cmdInsertPayment.Parameters.AddWithValue("@ReferenceID", Reference.Text);
                    cmdInsertPayment.Parameters.AddWithValue("@ProofOfPayment", pictureBox1.ImageLocation ?? (object)DBNull.Value);
                }
                cmdInsertPayment.ExecuteNonQuery();

                    MessageBox.Show("Payment details saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    cn.Close();
                Receipt frm = new Receipt();
                frm.Show();
                // Close Payments connection after insertion
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Ensure the connection is closed
                    if (cn.State == ConnectionState.Open)
                        cn.Close();
                }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PMethod.SelectedItem.ToString() == "Cash")
            {
                label4.Visible = false;
                Reference.Visible = false;
                Reference.Text = "Cash";  // Set default value to avoid errors

                pictureBox1.Visible = false;
                button2.Visible = false;

                pictureBox1.ImageLocation = null;  // Reset the image path to avoid inserting a file path
            }
            else
            {
                label4.Visible = true;
                Reference.Visible = true;

                pictureBox1.Visible = true;
                button2.Visible = true;
            }

        }

        private void Payment_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;



        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp"; // Set file types filter

            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the file path of the selected image
                string filePath = openFileDialog.FileName;

                // Optionally, display the image in a PictureBox
                pictureBox1.Image = Image.FromFile(filePath);

                // Store the file path directly (or use it in SQL query)
                pictureBox1.ImageLocation = filePath; // Set the ImageLocation property to the file path
            }
        }

        private void DriverID_TextChanged(object sender, EventArgs e)
        {
            string reference = @"^[0-9]+$";
            if (!Regex.IsMatch(DriverID.Text, reference))
            {
                DriverID.BackColor = System.Drawing.Color.Red; // Incorrect input
            }
            else
            {
                DriverID.BackColor = System.Drawing.Color.White; // Correct input
            }
        }

        private void Amount_TextChanged(object sender, EventArgs e)
        {
            string reference = @"^[0-9]+$";
            if (!Regex.IsMatch(Amount.Text, reference))
            {
                Amount.BackColor = System.Drawing.Color.Red; // Incorrect input
            }
            else
            {
                Amount.BackColor = System.Drawing.Color.White; // Correct input
            }
        }

        private void FullName_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^[A-Za-z]+([ A-Za-z]+)*$";
            if (!Regex.IsMatch(FullName.Text, pattern))
            {
                FullName.BackColor = System.Drawing.Color.Red; // Incorrect input
            }
            else
            {
                FullName.BackColor = System.Drawing.Color.White; // Correct input
            }
        }

        private void Reference_TextChanged(object sender, EventArgs e)
        {
            string reference = @"^[0-9]+$";
            if (!Regex.IsMatch(Reference.Text, reference))
            {
                Reference.BackColor = System.Drawing.Color.Red; // Incorrect input
            }
            else
            {
                Reference.BackColor = System.Drawing.Color.White; // Correct input
            }

        }

        private void Contact_TextChanged(object sender, EventArgs e)
        {
            string phonePattern = @"^09[0-9]{9}$";
            if (!Regex.IsMatch(Contact.Text, phonePattern))
            {
                Contact.BackColor = System.Drawing.Color.Red; // Incorrect input
            }
            else
            {
                Contact.BackColor = System.Drawing.Color.White; // Correct input
            }
        }
    }
}
