using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;
using iText.Layout.Properties;

namespace Driving_Management_System
{
    public partial class InstructorPDF : Form
    {
        public InstructorPDF()
        {
            InitializeComponent();
        }

        SqlConnection cn = new SqlConnection("data source=localhost; database=Instructor; Integrated Security=True;");

        private void InstructorPDF_Load(object sender, EventArgs e)
        {
            DisplayLastUserId();
            FormBorderStyle = FormBorderStyle.None;

        }

        private void DisplayLastUserId()
        {
            string sql = "SELECT TOP 1 InstructorID, FirstName, LastName, Phone, Email, Certification, HireDate, LicenseImage FROM Instructor ORDER BY InstructorID DESC";
            SqlCommand cmd = new SqlCommand(sql, cn);

            try
            {
                cn.Open();
                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    if (read.Read())
                    {
                        label1.Text = $"Instructor ID: {read["InstructorID"]}";
                        label2.Text = $"Full Name: {read["FirstName"]} {read["LastName"]}";
                        label3.Text = $"Phone: {read["Phone"]}";
                        label4.Text = $"Email: {read["Email"]}";
                        label5.Text = $"Certification: {read["Certification"]}";
                        label6.Text = $"Hire Date: {read["HireDate"]}";

                        // Check if the photo is not null or empty
                        if (read["LicenseImage"] != DBNull.Value)
                        {
                            byte[] photoData = (byte[])read["LicenseImage"];
                            using (MemoryStream ms = new MemoryStream(photoData))
                            {
                                pictureBox1.Image = System.Drawing.Image.FromStream(ms);
                            }
                        }
                        else
                        {
                            pictureBox1.Image = null; // Or a default image if no photo
                        }
                    }
                    else
                    {
                        label1.Text = "No users found";
                        label2.Text = "";
                        label3.Text = "";
                        label4.Text = "";
                        label5.Text = "";
                        label6.Text = "";
                        pictureBox1.Image = null;
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

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files|*.pdf",
                Title = "Save PDF File",
                FileName = "InstructorDetails.pdf"
            };

            // Show SaveFileDialog
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string savePath = saveFileDialog.FileName;

                // Create PDF and save it to the selected path
                try
                {
                    // Create a PdfWriter to write the document to the selected file path
                    using (PdfWriter writer = new PdfWriter(savePath))
                    {
                        // Create a PdfDocument and pass the writer to it
                        using (PdfDocument pdf = new PdfDocument(writer))
                        {
                            // Create a Document to add elements
                            Document document = new Document(pdf);

                            // Add a title to the PDF
                            document.Add(new Paragraph("Instructor Details")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(18));
                               

                            document.Add(new Paragraph("\n"));

                            // Add instructor details to the PDF in a structured format
                            document.Add(new Paragraph($"Instructor ID: {label1.Text}").SetFontSize(12));
                            document.Add(new Paragraph($"Full Name: {label2.Text}").SetFontSize(12));
                            document.Add(new Paragraph($"Phone: {label3.Text}").SetFontSize(12));
                            document.Add(new Paragraph($"Email: {label4.Text}").SetFontSize(12));
                            document.Add(new Paragraph($"Certification: {label5.Text}").SetFontSize(12));
                            document.Add(new Paragraph($"Hire Date: {label6.Text}").SetFontSize(12));

                            document.Add(new Paragraph("\n"));

                            // Add picture to PDF (if available)
                            if (pictureBox1.Image != null)
                            {
                                // Convert the Image to a byte array
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                    byte[] imageBytes = ms.ToArray();

                                    // Create iTextSharp Image from byte array
                                    iText.Layout.Element.Image pdfImage = new iText.Layout.Element.Image(ImageDataFactory.Create(imageBytes))
                                        .SetWidth(100) // Set width of the image
                                        .SetHeight(100) // Set height of the image
                                        .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                                    // Add the image to the PDF document
                                    document.Add(pdfImage);
                                }
                            }
                            else
                            {
                                document.Add(new Paragraph("No image available").SetFontSize(12));
                            }

                            // Close the document to save the file
                            document.Close();
                            this.Close();
                        }
                    }
                    MessageBox.Show("PDF Saved Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
