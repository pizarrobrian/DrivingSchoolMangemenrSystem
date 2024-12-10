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
using iText.Kernel.Pdf;                 // For working with PDF files
using iText.Layout;                      // For working with layout elements (e.g., Paragraph, Table)
using iText.Layout.Element;              // For layout elements such as Paragraph, Cell, and Table
using iText.Layout.Properties;           // For layout properties like alignment, fonts, etc.
using iText.Kernel.Pdf.Canvas;           // For canvas-specific operations (if needed)
using iText.IO.Image;                   // For working with images in the PDF (if needed)
using iText.Kernel.Geom;                 // For working with geometry (page sizes, etc.)



namespace Driving_Management_System
{
    public partial class StudentSchedule : Form
    {


        public StudentSchedule()
        {
            InitializeComponent();



        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void StudentSchedule_Load(object sender, EventArgs e)
        {
            DisplaySchedule();
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void DisplaySchedule()
        {
            try
            {
                // Define the query and connection
                string query = "SELECT LessonType, LessonDate, Duration, StudentName, Vehicle FROM Lesson WHERE Username = @Username";
                using (SqlConnection connStudent = new SqlConnection("data source=localhost; database=Sche; Integrated Security=True;"))
                {
                    if (connStudent.State == ConnectionState.Closed)
                        connStudent.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connStudent))
                    {
                        cmd.Parameters.AddWithValue("@Username", CurrentUser.Username);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate each TextBox with the respective values
                                textBox1.Text = reader["LessonType"].ToString();
                                textBox2.Text = Convert.ToDateTime(reader["LessonDate"]).ToShortDateString();
                                textBox3.Text = reader["Duration"].ToString();
                                textBox4.Text = reader["StudentName"].ToString();
                                textBox5.Text = reader["Vehicle"].ToString();
                            }
                            else
                            {
                                // Clear textboxes if no record found
                                ClearTextBoxes();
                                MessageBox.Show("No schedule yet.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to clear textboxes
        private void ClearTextBoxes()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
        }



        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files|*.pdf",
                Title = "Save as PDF",
                FileName = "LessonDetails.pdf"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (PdfWriter writer = new PdfWriter(saveFileDialog.FileName))
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        pdf.SetDefaultPageSize(PageSize.A4);
                        Document document = new Document(pdf);

                        // Add a title
                        document.Add(new Paragraph("Lesson Details")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontSize(18)
                            .SetMarginBottom(20));

                        // Create a table with two columns (Label, Value)
                        Table table = new Table(2)
                            .UseAllAvailableWidth()
                            .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                        // Add rows for each TextBox
                        AddRowToTable(table, "Lesson Type", textBox1.Text);
                        AddRowToTable(table, "Lesson Date", textBox2.Text);
                        AddRowToTable(table, "Duration", textBox3.Text);
                        AddRowToTable(table, "Instructor Name", textBox4.Text);
                        AddRowToTable(table, "Vehicle", textBox5.Text);

                        document.Add(table);
                        document.Close();

                        MessageBox.Show("PDF file saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving the PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Helper method to add a row to the PDF table
        private void AddRowToTable(Table table, string label, string value)
        {
            // Label cell
            table.AddCell(new Cell()
                .Add(new Paragraph(label)
                .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                .SetPadding(5)
                .SetTextAlignment(TextAlignment.LEFT)));

            // Value cell
            table.AddCell(new Cell()
                .Add(new Paragraph(value))
                .SetPadding(5)
                .SetTextAlignment(TextAlignment.LEFT));
        }


    }
}