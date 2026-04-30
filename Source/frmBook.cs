using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Linq; // Still used for things like Contains on ComboBox.Items
using System.Text.RegularExpressions;

namespace hotel
{
    public partial class frmBook : Form
    {
        private string connectionString = @"Data Source=DESKTOP-I7ED3VR\SQLEXPRESS;Initial Catalog=HotelManagementDB;Integrated Security=True";

       
        private CustomLinkedList<Booking> bookings = new CustomLinkedList<Booking>();
        private Booking selectedBooking = null;

        public frmBook()
        {
            InitializeComponent();
            PopulateRoomTypes(); // Load room types from DB
            dtpCheckIn.Value = DateTime.Today;
            dtpCheckOut.Value = DateTime.Today.AddDays(1);
            cmbPayment.SelectedIndex = -1; // No default payment status
            // Attach event handler for room type selection changed
            cmbRoomType.SelectedIndexChanged += cmbRoomType_SelectedIndexChanged;
            LoadBookingsFromDatabase();
        }

        private void PopulateRoomTypes()
        {
            cmbRoomType.Items.Clear();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT DISTINCT RoomType FROM Rooms";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cmbRoomType.Items.Add(reader["RoomType"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading room types: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cmbRoomType.SelectedIndex = -1; // No selection initially
        }

        private void LoadAvailableRooms(string roomType = null)
        {
            cmbRoomNumber.Items.Clear();

            if (string.IsNullOrEmpty(roomType))
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT RoomNumber FROM Rooms WHERE Status = 'Available' AND RoomType = @RoomType";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@RoomType", roomType);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cmbRoomNumber.Items.Add(reader["RoomNumber"].ToString());
                    }

                    if (cmbRoomNumber.Items.Count == 0)
                    {
                        MessageBox.Show("No available rooms found for the selected room type.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading available rooms: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRoomType.SelectedItem != null)
            {
                string selectedType = cmbRoomType.SelectedItem.ToString();
                LoadAvailableRooms(selectedType);
                cmbRoomNumber.SelectedIndex = -1;
            }
            else
            {
                cmbRoomNumber.Items.Clear();
            }
        }

        private void LoadBookingsFromDatabase()
        {
            bookings.Clear();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT b.BookingID, b.GuestID, g.GuestName, g.Phone, g.CNIC, g.Email,
                                            b.RoomNumber, b.CheckIn, b.CheckOut, b.PaymentStatus
                                       FROM Bookings b
                                       INNER JOIN Guests g ON b.GuestID = g.GuestID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Booking booking = new Booking
                        {
                            BookingID = Convert.ToInt32(reader["BookingID"]),
                            GuestID = Convert.ToInt32(reader["GuestID"]),
                            GuestName = reader["GuestName"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            CNIC = reader["CNIC"].ToString(),
                            Email = reader["Email"].ToString(),
                            RoomNumber = reader["RoomNumber"].ToString(),
                            CheckIn = Convert.ToDateTime(reader["CheckIn"]),
                            CheckOut = Convert.ToDateTime(reader["CheckOut"]),
                            PaymentStatus = reader["PaymentStatus"].ToString()
                        };
                        bookings.AddLast(booking);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bookings from database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillFormFields(Booking booking)
        {
            txtGuestName.Text = booking.GuestName;
            txtPhone.Text = booking.Phone;
            txtCnic.Text = booking.CNIC;
            txtEmail.Text = booking.Email;

            string roomTypeForBooking = GetRoomTypeForRoomNumber(booking.RoomNumber);

            // Temporarily detach to avoid clearing room numbers
            cmbRoomType.SelectedIndexChanged -= cmbRoomType_SelectedIndexChanged;

            if (!string.IsNullOrEmpty(roomTypeForBooking) && cmbRoomType.Items.Contains(roomTypeForBooking))
            {
                cmbRoomType.SelectedItem = roomTypeForBooking;
            }
            else
            {
                cmbRoomType.SelectedIndex = -1;
            }

            // Reattach event
            cmbRoomType.SelectedIndexChanged += cmbRoomType_SelectedIndexChanged;

            // Add the booked room number if not present (since booked rooms aren't in available list)
            if (!cmbRoomNumber.Items.Contains(booking.RoomNumber))
            {
                cmbRoomNumber.Items.Add(booking.RoomNumber);
            }
            cmbRoomNumber.SelectedItem = booking.RoomNumber;

            dtpCheckIn.Value = booking.CheckIn;
            dtpCheckOut.Value = booking.CheckOut;

            if (cmbPayment.Items.Contains(booking.PaymentStatus))
            {
                cmbPayment.SelectedItem = booking.PaymentStatus;
            }
            else
            {
                cmbPayment.SelectedIndex = -1;
            }
        }

        private string GetRoomTypeForRoomNumber(string roomNumber)
        {
            string roomType = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT RoomType FROM Rooms WHERE RoomNumber = @RoomNumber";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@RoomNumber", roomNumber);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        roomType = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting room type: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return roomType;
        }

        private void ClearForm()
        {
            txtGuestName.Clear();
            txtPhone.Clear();
            txtCnic.Clear();
            txtEmail.Clear();

            cmbRoomType.SelectedIndex = -1;
            // The original code commented this out: cmbRoomNumber.Items.Clear();
            // This is generally correct, as LoadAvailableRooms will handle populating it based on room type selection.
            cmbRoomNumber.SelectedIndex = -1; // Clear selected room number

            cmbPayment.SelectedIndex = -1;

            dtpCheckIn.Value = DateTime.Today;
            dtpCheckOut.Value = DateTime.Today.AddDays(1);

            txtsearch.Clear();
            selectedBooking = null;
        }

        private bool ValidateForm()
        {
            // Guest Name Validation
            if (string.IsNullOrWhiteSpace(txtGuestName.Text))
            {
                MessageBox.Show("Guest Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGuestName.Focus();
                return false;
            }
            if (!Regex.IsMatch(txtGuestName.Text, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Guest Name can only contain letters and spaces.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGuestName.Focus();
                return false;
            }

            // Phone Number Validation (e.g., 11 digits)
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Phone Number cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }
            if (!Regex.IsMatch(txtPhone.Text, @"^\d{11}$")) // Example for 11 digits
            {
                MessageBox.Show("Phone Number must be 11 digits long and contain only numbers.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            // CNIC Validation (e.g., 13 digits)
            if (string.IsNullOrWhiteSpace(txtCnic.Text))
            {
                MessageBox.Show("CNIC cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCnic.Focus();
                return false;
            }
            if (!Regex.IsMatch(txtCnic.Text, @"^\d{13}$")) // Example for 13 digits
            {
                MessageBox.Show("CNIC must be 13 digits long and contain only numbers.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCnic.Focus();
                return false;
            }

            // Email Validation
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }
            if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Please enter a valid Email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Room Type Validation
            if (cmbRoomType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Room Type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbRoomType.Focus();
                return false;
            }

            // Room Number Validation
            if (cmbRoomNumber.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Room Number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbRoomNumber.Focus();
                return false;
            }

            // Payment Status Validation
            if (cmbPayment.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Payment Status.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbPayment.Focus();
                return false;
            }

            // Check-In Date Validation (not in the past for new bookings)
            if (selectedBooking == null && dtpCheckIn.Value.Date < DateTime.Today.Date)
            {
                MessageBox.Show("Check-In Date cannot be in the past for a new booking.", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpCheckIn.Focus();
                return false;
            }
            // Check-Out Date Validation
            if (dtpCheckOut.Value.Date <= dtpCheckIn.Value.Date)
            {
                MessageBox.Show("Check-Out Date must be after Check-In Date.", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpCheckOut.Focus();
                return false;
            }

            return true;
        }

        // Add booking
        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Insert guest first
                    string insertGuest = @"INSERT INTO Guests (GuestName, Phone, CNIC, Email)
                                           OUTPUT INSERTED.GuestID
                                           VALUES (@GuestName, @Phone, @CNIC, @Email)";

                    SqlCommand cmdGuest = new SqlCommand(insertGuest, conn);
                    cmdGuest.Parameters.AddWithValue("@GuestName", txtGuestName.Text);
                    cmdGuest.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    cmdGuest.Parameters.AddWithValue("@CNIC", txtCnic.Text);
                    cmdGuest.Parameters.AddWithValue("@Email", txtEmail.Text);

                    int guestId = (int)cmdGuest.ExecuteScalar();

                    // Insert booking
                    string insertBooking = @"INSERT INTO Bookings (GuestID, RoomNumber, CheckIn, CheckOut, PaymentStatus)
                                             VALUES (@GuestID, @RoomNumber, @CheckIn, @CheckOut, @PaymentStatus)";

                    SqlCommand cmdBook = new SqlCommand(insertBooking, conn);
                    cmdBook.Parameters.AddWithValue("@GuestID", guestId);
                    cmdBook.Parameters.AddWithValue("@RoomNumber", cmbRoomNumber.SelectedItem.ToString());
                    cmdBook.Parameters.AddWithValue("@CheckIn", dtpCheckIn.Value);
                    cmdBook.Parameters.AddWithValue("@CheckOut", dtpCheckOut.Value);
                    cmdBook.Parameters.AddWithValue("@PaymentStatus", cmbPayment.SelectedItem.ToString());
                    cmdBook.ExecuteNonQuery();

                    // Update room status to Booked
                    string updateRoom = "UPDATE Rooms SET Status='Booked' WHERE RoomNumber=@RoomNumber";
                    SqlCommand cmdRoom = new SqlCommand(updateRoom, conn);
                    cmdRoom.Parameters.AddWithValue("@RoomNumber", cmbRoomNumber.SelectedItem.ToString());
                    cmdRoom.ExecuteNonQuery();
                }

                MessageBox.Show("Booking added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadBookingsFromDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding booking: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Update booking
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedBooking == null)
            {
                MessageBox.Show("Select a booking to update.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateForm()) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Update guest info
                    string updateGuest = @"UPDATE Guests SET GuestName=@GuestName, Phone=@Phone, CNIC=@CNIC, Email=@Email WHERE GuestID=@GuestID";
                    SqlCommand cmdGuest = new SqlCommand(updateGuest, conn);
                    cmdGuest.Parameters.AddWithValue("@GuestName", txtGuestName.Text);
                    cmdGuest.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    cmdGuest.Parameters.AddWithValue("@CNIC", txtCnic.Text);
                    cmdGuest.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmdGuest.Parameters.AddWithValue("@GuestID", selectedBooking.GuestID);
                    cmdGuest.ExecuteNonQuery();

                    // Update booking info
                    string updateBooking = @"UPDATE Bookings SET RoomNumber=@RoomNumber, CheckIn=@CheckIn, CheckOut=@CheckOut, PaymentStatus=@PaymentStatus WHERE BookingID=@BookingID";
                    SqlCommand cmdBook = new SqlCommand(updateBooking, conn);
                    cmdBook.Parameters.AddWithValue("@RoomNumber", cmbRoomNumber.SelectedItem.ToString());
                    cmdBook.Parameters.AddWithValue("@CheckIn", dtpCheckIn.Value);
                    cmdBook.Parameters.AddWithValue("@CheckOut", dtpCheckOut.Value);
                    cmdBook.Parameters.AddWithValue("@PaymentStatus", cmbPayment.SelectedItem.ToString());
                    cmdBook.Parameters.AddWithValue("@BookingID", selectedBooking.BookingID);
                    cmdBook.ExecuteNonQuery();

                    // If room changed, update room status accordingly
                    if (selectedBooking.RoomNumber != cmbRoomNumber.SelectedItem.ToString())
                    {
                        // Set old room to Available
                        string updateOldRoom = "UPDATE Rooms SET Status='Available' WHERE RoomNumber=@OldRoomNumber";
                        SqlCommand cmdOldRoom = new SqlCommand(updateOldRoom, conn);
                        cmdOldRoom.Parameters.AddWithValue("@OldRoomNumber", selectedBooking.RoomNumber);
                        cmdOldRoom.ExecuteNonQuery();

                        // Set new room to Booked
                        string updateNewRoom = "UPDATE Rooms SET Status='Booked' WHERE RoomNumber=@NewRoomNumber";
                        SqlCommand cmdNewRoom = new SqlCommand(updateNewRoom, conn);
                        cmdNewRoom.Parameters.AddWithValue("@NewRoomNumber", cmbRoomNumber.SelectedItem.ToString());
                        cmdNewRoom.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Booking updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadBookingsFromDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating booking: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Delete booking
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedBooking == null)
            {
                MessageBox.Show("Select a booking to delete.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this booking?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        // Update room status to Available before deleting booking
                        string updateRoomStatusQuery = "UPDATE Rooms SET Status='Available' WHERE RoomNumber=@RoomNumber";
                        SqlCommand cmdUpdateRoomStatus = new SqlCommand(updateRoomStatusQuery, conn);
                        cmdUpdateRoomStatus.Parameters.AddWithValue("@RoomNumber", selectedBooking.RoomNumber);
                        cmdUpdateRoomStatus.ExecuteNonQuery();

                        // Delete booking
                        string deleteBooking = "DELETE FROM Bookings WHERE BookingID=@BookingID";
                        SqlCommand cmdDeleteBooking = new SqlCommand(deleteBooking, conn);
                        cmdDeleteBooking.Parameters.AddWithValue("@BookingID", selectedBooking.BookingID);
                        cmdDeleteBooking.ExecuteNonQuery();

                        // Optional: Delete guest if no other bookings are associated with them
                        string checkGuestBookings = "SELECT COUNT(*) FROM Bookings WHERE GuestID = @GuestID";
                        SqlCommand cmdCheckGuestBookings = new SqlCommand(checkGuestBookings, conn);
                        cmdCheckGuestBookings.Parameters.AddWithValue("@GuestID", selectedBooking.GuestID);
                        int bookingCount = (int)cmdCheckGuestBookings.ExecuteScalar();

                        if (bookingCount == 0)
                        {
                            string deleteGuest = "DELETE FROM Guests WHERE GuestID=@GuestID";
                            SqlCommand cmdDeleteGuest = new SqlCommand(deleteGuest, conn);
                            cmdDeleteGuest.Parameters.AddWithValue("@GuestID", selectedBooking.GuestID);
                            cmdDeleteGuest.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Booking deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    LoadBookingsFromDatabase();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting booking: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Clear button
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        // Search bookings by Guest Name or Guest ID
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtsearch.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a Guest Name or Guest ID to search.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            selectedBooking = null;

            // Try to parse the search term as an integer for Guest ID search
            bool isNumericSearch = int.TryParse(searchTerm, out int searchId);

            foreach (var booking in bookings) // This now iterates over our custom linked list
            {
                // Prioritize exact Guest ID matches
                if (isNumericSearch && (booking.GuestID == searchId))
                {
                    selectedBooking = booking;
                    FillFormFields(booking);
                    return;
                }
                // Case-insensitive search by GuestName (partial match)
                if (booking.GuestName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    selectedBooking = booking;
                    FillFormFields(booking);
                    return;
                }
            }

            MessageBox.Show("Booking not found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLogut_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Hide();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            DashboardForm dashboard = new DashboardForm();
            dashboard.Show();
            this.Hide();
        }

        private void frmBook_Load(object sender, EventArgs e)
        {
            // Any specific load operations for the form can go here if needed.
        }
    }

    // Your Booking class remains the same
    public class Booking
    {
        public int BookingID { get; set; }
        public int GuestID { get; set; }
        public string GuestName { get; set; }
        public string Phone { get; set; }
        public string CNIC { get; set; }
        public string Email { get; set; }
        public string RoomNumber { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string PaymentStatus { get; set; }
    }

    // --- TRUE CUSTOM SINGLY LINKED LIST IMPLEMENTATION ---

    // Represents a single node in the linked list
    public class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Next { get; set; }

        public Node(T data)
        {
            Data = data;
            Next = null; // No next node initially
        }
    }

    // Implements a basic singly linked list
    public class CustomLinkedList<T> : System.Collections.Generic.IEnumerable<T>
    {
        private Node<T> head; // Points to the first node in the list
        private Node<T> tail; // Points to the last node in the list
        private int count;    // Stores the number of elements in the list

        public CustomLinkedList()
        {
            head = null;
            tail = null;
            count = 0;
        }

        // Adds an item to the end of the list
        public void AddLast(T item)
        {
            Node<T> newNode = new Node<T>(item);
            if (head == null)
            {
                // If the list is empty, the new node is both the head and the tail
                head = newNode;
                tail = newNode;
            }
            else
            {
                // Otherwise, link the current tail's next to the new node, and update the tail
                tail.Next = newNode;
                tail = newNode;
            }
            count++;
        }

        // Clears all items from the list
        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }

        // Returns the number of elements in the list
        public int Count
        {
            get { return count; }
        }

        // Implementation for IEnumerable<T> to allow foreach loops
        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            Node<T> current = head;
            while (current != null)
            {
                yield return current.Data; // Returns the data and pauses execution
                current = current.Next;    // Moves to the next node
            }
        }

        // Explicit implementation for the non-generic IEnumerable interface
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}