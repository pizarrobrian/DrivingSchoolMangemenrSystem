
namespace Driving_Management_System
{
    partial class SalaryInstructor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.InstructorIDCbox = new System.Windows.Forms.ComboBox();
            this.SalaryAmount = new System.Windows.Forms.TextBox();
            this.SetSalary = new System.Windows.Forms.Button();
            this.UpdateSalary = new System.Windows.Forms.Button();
            this.dataGridView1Salary = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1Salary)).BeginInit();
            this.SuspendLayout();
            // 
            // InstructorIDCbox
            // 
            this.InstructorIDCbox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.InstructorIDCbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InstructorIDCbox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InstructorIDCbox.FormattingEnabled = true;
            this.InstructorIDCbox.Location = new System.Drawing.Point(279, 50);
            this.InstructorIDCbox.Name = "InstructorIDCbox";
            this.InstructorIDCbox.Size = new System.Drawing.Size(189, 27);
            this.InstructorIDCbox.TabIndex = 0;
            this.InstructorIDCbox.SelectedIndexChanged += new System.EventHandler(this.InstructorIDCbox_SelectedIndexChanged);
            // 
            // SalaryAmount
            // 
            this.SalaryAmount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SalaryAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SalaryAmount.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SalaryAmount.Location = new System.Drawing.Point(669, 252);
            this.SalaryAmount.Name = "SalaryAmount";
            this.SalaryAmount.Size = new System.Drawing.Size(207, 26);
            this.SalaryAmount.TabIndex = 1;
            this.SalaryAmount.TextChanged += new System.EventHandler(this.SalaryAmount_TextChanged);
            // 
            // SetSalary
            // 
            this.SetSalary.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SetSalary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SetSalary.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetSalary.Location = new System.Drawing.Point(669, 306);
            this.SetSalary.Name = "SetSalary";
            this.SetSalary.Size = new System.Drawing.Size(97, 37);
            this.SetSalary.TabIndex = 2;
            this.SetSalary.Text = "Set";
            this.SetSalary.UseVisualStyleBackColor = true;
            this.SetSalary.Click += new System.EventHandler(this.SetSalary_Click);
            // 
            // UpdateSalary
            // 
            this.UpdateSalary.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UpdateSalary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpdateSalary.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateSalary.Location = new System.Drawing.Point(779, 306);
            this.UpdateSalary.Name = "UpdateSalary";
            this.UpdateSalary.Size = new System.Drawing.Size(97, 37);
            this.UpdateSalary.TabIndex = 3;
            this.UpdateSalary.Text = "Update";
            this.UpdateSalary.UseVisualStyleBackColor = true;
            this.UpdateSalary.Click += new System.EventHandler(this.UpdateSalary_Click);
            // 
            // dataGridView1Salary
            // 
            this.dataGridView1Salary.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataGridView1Salary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1Salary.Location = new System.Drawing.Point(170, 125);
            this.dataGridView1Salary.Name = "dataGridView1Salary";
            this.dataGridView1Salary.Size = new System.Drawing.Size(444, 431);
            this.dataGridView1Salary.TabIndex = 4;
            this.dataGridView1Salary.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1Salary_CellContentClick);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(485, 47);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 32);
            this.button1.TabIndex = 5;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SalaryInstructor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuBar;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1Salary);
            this.Controls.Add(this.UpdateSalary);
            this.Controls.Add(this.SetSalary);
            this.Controls.Add(this.SalaryAmount);
            this.Controls.Add(this.InstructorIDCbox);
            this.Name = "SalaryInstructor";
            this.Text = "comm";
            this.Load += new System.EventHandler(this.SalaryInstructor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1Salary)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox InstructorIDCbox;
        private System.Windows.Forms.TextBox SalaryAmount;
        private System.Windows.Forms.Button SetSalary;
        private System.Windows.Forms.Button UpdateSalary;
        private System.Windows.Forms.DataGridView dataGridView1Salary;
        private System.Windows.Forms.Button button1;
    }
}