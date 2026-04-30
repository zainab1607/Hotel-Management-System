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

namespace hotel
{
    public partial class BookingDetailsForm : Form
    {
        string connectionString = @"Data Source=DESKTOP-I7ED3VR\SQLEXPRESS;Initial Catalog=HotelManagementDB;Integrated Security=True";
        DataTable allBookings;
        RoomBST roomTree = new RoomBST();

        public BookingDetailsForm()
        {
            InitializeComponent();
            LoadBookings();
            cmbSearchBy.Items.AddRange(new string[] { "Guest ID", "Room Number" });
            cmbSearchBy.SelectedIndex = 0;
        }

        private void LoadBookings()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Bookings";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                allBookings = new DataTable();
                da.Fill(allBookings);

                foreach (DataRow row in allBookings.Rows)
                {
                    int roomNum = Convert.ToInt32(row["RoomNumber"]);
                    roomTree.Insert(roomNum, row);
                }

                gvBookings.DataSource = allBookings;
            }
        }

        public class RoomNode
        {
            public int RoomNumber;
            public DataRow BookingDetails;
            public RoomNode Left, Right;

            public RoomNode(int roomNumber, DataRow bookingDetails)
            {
                RoomNumber = roomNumber;
                BookingDetails = bookingDetails;
            }
        }

        public class RoomBST
        {
            private RoomNode root;

            public void Insert(int roomNumber, DataRow bookingDetails)
            {
                root = InsertRec(root, roomNumber, bookingDetails);
            }

            private RoomNode InsertRec(RoomNode root, int roomNumber, DataRow bookingDetails)
            {
                if (root == null)
                    return new RoomNode(roomNumber, bookingDetails);

                if (roomNumber < root.RoomNumber)
                    root.Left = InsertRec(root.Left, roomNumber, bookingDetails);
                else
                    root.Right = InsertRec(root.Right, roomNumber, bookingDetails);

                return root;
            }

            public DataRow Search(int roomNumber)
            {
                RoomNode current = root;
                while (current != null)
                {
                    if (roomNumber == current.RoomNumber)
                        return current.BookingDetails;
                    else if (roomNumber < current.RoomNumber)
                        current = current.Left;
                    else
                        current = current.Right;
                }
                return null;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchBy = cmbSearchBy.SelectedItem.ToString();
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Please enter a value to search.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ResetSearch();
                return;
            }

            if (searchBy == "Guest ID")
            {
                DataRow[] found = allBookings.Select($"GuestID = '{keyword}'");

                if (found.Length > 0)
                {
                    gvBookings.DataSource = found.CopyToDataTable();
                }
                else
                {
                    MessageBox.Show("Guest ID not found.");
                    ResetSearch();
                }
            }
            else if (searchBy == "Room Number")
            {
                if (!int.TryParse(keyword, out int roomNum))
                {
                    MessageBox.Show("Please enter a valid Room Number.");
                    ResetSearch();
                    return;
                }

                DataRow result = roomTree.Search(roomNum);
                if (result != null)
                {
                    DataTable dt = allBookings.Clone();
                    dt.ImportRow(result);
                    gvBookings.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Room not found.");
                    ResetSearch();
                }
            }
        }

        private void cmbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Automatically clear the search and reset grid when criteria changes
            ResetSearch();
        }

        private void ResetSearch()
        {
            txtSearch.Clear();
            gvBookings.DataSource = allBookings;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
        }

        private void BookingDetailsForm_Load(object sender, EventArgs e)
        {
            // Optionally reset form state here
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            DashboardForm dashboard = new DashboardForm();
            dashboard.Show();
            this.Hide();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();                      // Clear the search textbox
            cmbSearchBy.SelectedIndex = 0;          // Reset dropdown to first item ("Guest ID")
            gvBookings.DataSource = allBookings;

        }
    }
}

