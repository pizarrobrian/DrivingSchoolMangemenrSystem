using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Driving_Management_System
{
    public partial class StudentPDF : Form
    {
        public StudentPDF()
        {
            InitializeComponent();
        }

        SqlConnection cn = new SqlConnection("data source=localhost; database=StudentInfo; Integrated Security=True;");

        private void label1_Click(object sender, EventArgs e) { }

        private void DisplayLastUserId()
        {
            string sql = "SELECT TOP 1 StudentID, FirstName, LastName, DateOfBirth, Phone, Email, Address, Balance FROM Students ORDER BY StudentID DESC";
            SqlCommand cmd = new SqlCommand(sql, cn);

            try
            {
                cn.Open();
                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    if (read.Read())
                    {
                        label1.Text = $"Driver's ID: {read["StudentID"]}";
                        label2.Text = $"Full Name: {read["FirstName"]} {read["LastName"]}";
                        label3.Text = $"Date of Birth: {read["DateOfBirth"]}";
                        label4.Text = $"Phone: {read["Phone"]}";
                        label5.Text = $"Email: {read["Email"]}";
                        label6.Text = $"Address: {read["Address"]}";
                        label7.Text = $"Balance : {read["Balance"]}";
                    }
                    else
                    {
                        label1.Text = "No users found";
                        label2.Text = "";
                        label3.Text = "";
                        label4.Text = "";
                        label5.Text = "";
                        label6.Text = "";
                        label7.Text = "";
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

        private void USERID_Load(object sender, EventArgs e)
        {
            DisplayLastUserId();
            this.FormBorderStyle = FormBorderStyle.None;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnDownloadPDF1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files|*.pdf",
                Title = "Save PDF File",
                FileName = "DriverDetails.pdf"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4, 20, 20, 30, 30);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        // Fonts
                        Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK);
                        Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.BLUE);
                        Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK);

                        // Title
                        Paragraph title = new Paragraph("Driver Information", titleFont)
                        {
                            Alignment = Element.ALIGN_CENTER,
                            SpacingAfter = 20
                        };
                        pdfDoc.Add(title);

                        // Separator Line
                        LineSeparator lineSeparator = new LineSeparator(1f, 100f, BaseColor.GRAY, Element.ALIGN_CENTER, -2);
                        pdfDoc.Add(new Chunk(lineSeparator));
                        pdfDoc.Add(new Paragraph("\n"));

                        // Driver ID Section
                        Paragraph driverID = new Paragraph(label1.Text, headerFont);
                        driverID.SpacingAfter = 10;
                        pdfDoc.Add(driverID);

                        // Name Section
                        Paragraph name = new Paragraph(label2.Text, normalFont);
                        name.SpacingAfter = 10;
                        pdfDoc.Add(name);

                        // Add the text for DateOfBirth
                        Paragraph dateOfBirth = new Paragraph(label3.Text, normalFont);
                        dateOfBirth.SpacingAfter = 10;
                        pdfDoc.Add(dateOfBirth);

                        // Add the text for Phone
                        Paragraph phone = new Paragraph(label4.Text, normalFont);
                        phone.SpacingAfter = 10;
                        pdfDoc.Add(phone);

                        // Add the text for Email
                        Paragraph email = new Paragraph(label5.Text, normalFont);
                        email.SpacingAfter = 10;
                        pdfDoc.Add(email);

                        // Add the text for Address
                        Paragraph address = new Paragraph(label6.Text, normalFont);
                        address.SpacingAfter = 10;
                        pdfDoc.Add(address);

                        Paragraph balance = new Paragraph(label7.Text, normalFont);
                        balance.SpacingAfter = 10;
                        pdfDoc.Add(balance);

                        // Footer
                        Paragraph footer = new Paragraph("Generated by Driving Management System", normalFont)
                        {
                            Alignment = Element.ALIGN_CENTER,
                            SpacingBefore = 20
                        };
                        pdfDoc.Add(footer);

                        pdfDoc.Close();
                        writer.Close();
                    }

                    MessageBox.Show("PDF downloaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                this.Close();
            }
        }
    }
}
