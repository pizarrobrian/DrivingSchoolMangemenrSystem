using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
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
    public partial class Receipt : Form
    {
        public Receipt()
        {
            InitializeComponent();
        }
        SqlConnection cn = new SqlConnection("data source=localhost; database=Payments; Integrated Security=True;");

        private void Receipt_Load(object sender, EventArgs e)
        {
            DisplayLastPayment();
        }
        private void DisplayLastPayment()
        {
            string sql = "SELECT TOP 1 PaymentID, StudentID, FullName, Amount, ContactNo, PaymentMethod, ReferenceID, PaymentDate FROM Payments ORDER BY PaymentID DESC";
            SqlCommand cmd = new SqlCommand(sql, cn);

            try
            {
                cn.Open();
                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    if (read.Read())
                    {
                        label7.Text = $"Payment ID: {read["PaymentID"]}";
                        label1.Text = $"Student ID: {read["StudentID"]}";
                        label2.Text = $"Full Name: {read["FullName"]}";
                        label3.Text = $"Amount: {read["Amount"]:C2}"; // Formats as currency
                        label5.Text = $"Contact No: {read["ContactNo"]}";
                        label4.Text = $"Payment Method: {read["PaymentMethod"]}";
                        label6.Text = $"Reference ID: {(read["PaymentMethod"].ToString() == "Cash" ? "Cash" : read["ReferenceID"] ?? "N/A")}"; // Handles cash scenario
                        label8.Text = $"Payment Date: {read["PaymentDate"] ?? "N/A"}";
                    }
                    else
                    {
                        label1.Text = "No payments found";
                        label2.Text = "";
                        label3.Text = "";
                        label4.Text = "";
                        label5.Text = "";
                        label6.Text = "";
                        label7.Text = "";
                        label8.Text = "";
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

        private void label5_Click(object sender, EventArgs e)
        {
           
        }


        private void AddTableRow(Table table, string label, string value)
        {
            table.AddCell(new Cell().Add(new Paragraph(label).SetBorder(Border.NO_BORDER)));
            table.AddCell(new Cell().Add(new Paragraph(value)).SetBorder(Border.NO_BORDER));
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files|*.pdf",
                Title = "Save PDF File",
                FileName = "PaymentReceipt.pdf"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string savePath = saveFileDialog.FileName;

                try
                {
                    using (PdfWriter writer = new PdfWriter(savePath))
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        Document document = new Document(pdf);

                        // Add a centered title
                        document.Add(new Paragraph("PAYMENT RECEIPT")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontSize(20));


                        document.Add(new Paragraph("\n")); // Spacer
                        document.Add(new LineSeparator(new SolidLine())); // Separator line

                        // Create a table with two columns for payment details
                        Table table = new Table(UnitValue.CreatePercentArray(new float[] { 30, 70 })).UseAllAvailableWidth();

                        // Add payment details rows
                        AddTableRow(table, "Payment ID:", label7.Text);
                        AddTableRow(table, "Student ID:", label1.Text);
                        AddTableRow(table, "Full Name:", label2.Text);
                        AddTableRow(table, "Amount:", label3.Text);
                        AddTableRow(table, "Contact No:", label5.Text);
                        AddTableRow(table, "Payment Method:", label4.Text);

                        // Handle Reference ID based on Payment Method
                        string referenceID = (label4.Text.Contains("Cash")) ? "Cash" : label6.Text;
                        AddTableRow(table, "Reference ID:", referenceID);

                        AddTableRow(table, "Payment Date:", label8.Text);

                        document.Add(table);
                        document.Add(new Paragraph("\n")); // Spacer

                        // Footer with a thank you note
                        document.Add(new Paragraph("Thank you for your payment!")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontSize(12));

                        document.Close();
                    }

                    MessageBox.Show("Receipt saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving receipt: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
