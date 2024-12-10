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
    public partial class StudentNotification : Form
    {
        public StudentNotification()
        {
            InitializeComponent();
        }

        private void StudentNotification_Load(object sender, EventArgs e)
        {
            LoadNotifications();
            this.FormBorderStyle = FormBorderStyle.None;
            
        }


        private void LoadNotifications()
        {
            listBox1.Items.Clear(); // Clear previous items in the list box

            using (SqlConnection conn = new SqlConnection(@"Data Source=XIANVILLE;Initial Catalog=adminDSM;Integrated Security=True"))
            {
                conn.Open();
                // Update query to also select the sender's username
                string query = "SELECT Message, DateSent, NotificationID, SenderUsername FROM Notifications WHERE RecipientUsername = @Recipient AND IsRead = 0";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Recipient", CurrentUser.Username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string message = reader["Message"].ToString();
                        string dateSent = Convert.ToDateTime(reader["DateSent"]).ToString("yyyy-MM-dd HH:mm:ss");
                        int notificationID = Convert.ToInt32(reader["NotificationID"]); // Assuming there's a unique ID for each notification
                        string sender = reader["SenderUsername"].ToString(); // Read sender's username

                        // Add the message along with the sender's information to the ListBox
                        listBox1.Items.Add($"From : {sender}\n Message : {message}\n Sent on : {dateSent}\n");

                        // Mark the message as read in the database
                        MarkAsRead(notificationID);
                    }
                }
            }
        }


        private void MarkAsRead(int notificationID)
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source=XIANVILLE;Initial Catalog=adminDSM;Integrated Security=True"))
            {
                conn.Open();
                string updateQuery = "UPDATE Notifications SET IsRead = 1 WHERE NotificationID = @NotificationID";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@NotificationID", notificationID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
