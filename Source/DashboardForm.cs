using System.Windows.Forms;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using Hotel;

namespace hotel
{
    public partial class DashboardForm : Form
    {

        string connectionString = @"Data Source=DESKTOP-I7ED3VR\SQLEXPRESS;Initial Catalog=HotelManagementDB;Integrated Security=True";
        public DashboardForm()
        {
            InitializeComponent();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            UpdateRoomStatus();
        }
        private void UpdateRoomStatus()
        {
            string updateRoomStatusQuery = @"
        UPDATE Rooms SET Status = 'Available';

        UPDATE Rooms
        SET Status = 'Booked'
        WHERE RoomNumber IN (
            SELECT DISTINCT RoomNumber
            FROM Bookings
            WHERE CAST(GETDATE() AS DATE) >= CheckIn
              AND CAST(GETDATE() AS DATE) < CheckOut
        );";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(updateRoomStatusQuery, conn);
                cmd.ExecuteNonQuery();
            }
        }





        private void pbRoom_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Opening Room Management...", "Navigation", MessageBoxButtons.OK, MessageBoxIcon.Information);


            RoomForm RoomForm = new RoomForm();
            RoomForm.Show();
            this.Hide();
        }

        private void pbBooking_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Opening Booking System...", "Navigation", MessageBoxButtons.OK, MessageBoxIcon.Information);

            frmBook bookingForm= new frmBook(); // ✅ Changed variable name
            bookingForm.Show();
            this.Hide();
        }

        private void pbReport_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Opening Reports...", "Navigation", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ReportForm ReportForm = new ReportForm();
            ReportForm.Show();
            this.Hide();
        }

        private void pbCustomer_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Opening Customer Management...", "Navigation", MessageBoxButtons.OK, MessageBoxIcon.Information);

            CustomerForm customerForm = new CustomerForm();
            customerForm.Show();
            this.Hide();
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LoginForm LoginForm = new LoginForm();
                LoginForm.Show();
                this.Close();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Opening Booking Details...", "Navigation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            BookingDetailsForm bookingDetails = new BookingDetailsForm(); 
            bookingDetails.Show(); 
            this.Hide();
        }
    }
}

