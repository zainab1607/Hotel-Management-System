using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace hotel
{
    public partial class RoomForm : Form
    {
        public RoomForm() { InitializeComponent(); }

        string connectionString = @"Data Source=DESKTOP-I7ED3VR\SQLEXPRESS;Initial Catalog=HotelManagementDB;Integrated Security=True";

        MyStack<RoomAction> undoStack = new MyStack<RoomAction>();
        MyStack<RoomAction> redoStack = new MyStack<RoomAction>();
        MyLinkedList<Room> roomList = new MyLinkedList<Room>();

        // Room data structure
        public class Room
        {
            public int RoomNumber;
            public string RoomType;
            public double Price;
            public string Status;
        }

        // Represents a room action for undo/redo
        public class RoomAction
        {
            public string ActionType;
            public Room RoomData;
            public RoomAction(string actionType, Room room)
            {
                ActionType = actionType;
                RoomData = room;
            }
        }

        // Custom Node class for Linked List
        public class Node<T>
        {
            public T Data;
            public Node<T> Next;

            public Node(T data)
            {
                Data = data;
                Next = null;
            }
        }

        // Custom Linked List class
        public class MyLinkedList<T>
        {
            public Node<T> Head = null;

            public void AddLast(T data)
            {
                Node<T> newNode = new Node<T>(data);
                if (Head == null)
                    Head = newNode;
                else
                {
                    Node<T> current = Head;
                    while (current.Next != null)
                        current = current.Next;
                    current.Next = newNode;
                }
            }

            public void Remove(T data)
            {
                if (Head == null) return;

                if (Head.Data.Equals(data))
                {
                    Head = Head.Next;
                    return;
                }

                Node<T> current = Head;
                while (current.Next != null && !current.Next.Data.Equals(data))
                    current = current.Next;

                if (current.Next != null)
                    current.Next = current.Next.Next;
            }

            public void Clear() => Head = null;

            public System.Collections.Generic.IEnumerable<T> GetAll()
            {
                Node<T> current = Head;
                while (current != null)
                {
                    yield return current.Data;
                    current = current.Next;
                }
            }
        }

        // Custom Stack class
        public class MyStack<T>
        {
            private Node<T> top;

            public void Push(T item)
            {
                Node<T> node = new Node<T>(item);
                node.Next = top;
                top = node;
            }

            public T Pop()
            {
                if (IsEmpty()) throw new InvalidOperationException("Stack is empty");
                T data = top.Data;
                top = top.Next;
                return data;
            }

            public T Peek()
            {
                if (IsEmpty()) throw new InvalidOperationException("Stack is empty");
                return top.Data;
            }

            public void Clear() => top = null;

            public bool IsEmpty() => top == null;

            public int Count
            {
                get
                {
                    int count = 0;
                    Node<T> current = top;
                    while (current != null)
                    {
                        count++;
                        current = current.Next;
                    }
                    return count;
                }
            }
        }

        // Loads rooms from database into custom LinkedList
        private void LoadRooms()
        {
            roomList.Clear();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Rooms", con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Room room = new Room
                        {
                            RoomNumber = (int)reader["RoomNumber"],
                            RoomType = reader["RoomType"].ToString(),
                            Price = Convert.ToDouble(reader["Price"]),
                            Status = reader["Status"].ToString()
                        };
                        roomList.AddLast(room);
                    }
                }
                RefreshDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading rooms: " + ex.Message);
            }
        }

        // Refreshes the DataGrid from custom linked list
        private void RefreshDataGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RoomNumber");
            dt.Columns.Add("RoomType");
            dt.Columns.Add("Price");
            dt.Columns.Add("Status");

            foreach (var room in roomList.GetAll())
                dt.Rows.Add(room.RoomNumber, room.RoomType, room.Price, room.Status);

            dataGridRooms.DataSource = dt;
        }

        private Room FindRoomInList(int roomNum)
        {
            foreach (var room in roomList.GetAll())
            {
                if (room.RoomNumber == roomNum)
                    return room;
            }
            return null;
        }

        private void ClearInputs()
        {
            txtRoomNumber.Clear();
            txtPrice.Clear();
            cmbRoomType.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
        }

        private void RoomForm_Load(object sender, EventArgs e)
        {
            LoadRooms();
        }

        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtRoomNumber.Text, out int roomNum) || roomNum <= 0)
            {
                MessageBox.Show("Enter a valid Room Number.");
                return;
            }

            if (!double.TryParse(txtPrice.Text, out double price) || price <= 0)
            {
                MessageBox.Show("Enter a valid Price.");
                return;
            }

            if (cmbRoomType.SelectedIndex == -1 || cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Room Type and Status.");
                return;
            }

            if (FindRoomInList(roomNum) != null)
            {
                MessageBox.Show("Room already exists.");
                return;
            }

            Room room = new Room
            {
                RoomNumber = roomNum,
                RoomType = cmbRoomType.Text,
                Price = price,
                Status = cmbStatus.Text
            };

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Rooms VALUES (@num, @type, @price, @status)", con);
                    cmd.Parameters.AddWithValue("@num", room.RoomNumber);
                    cmd.Parameters.AddWithValue("@type", room.RoomType);
                    cmd.Parameters.AddWithValue("@price", room.Price);
                    cmd.Parameters.AddWithValue("@status", room.Status);
                    cmd.ExecuteNonQuery();
                }

                roomList.AddLast(room);
                undoStack.Push(new RoomAction("Add", room));
                redoStack.Clear();

                RefreshDataGrid();
                ClearInputs();
                MessageBox.Show("Room added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnUpdateRoom_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtRoomNumber.Text, out int roomNum))
            {
                MessageBox.Show("Enter a valid Room Number.");
                return;
            }

            Room room = FindRoomInList(roomNum);
            if (room == null)
            {
                MessageBox.Show("Room not found.");
                return;
            }

            if (!double.TryParse(txtPrice.Text, out double newPrice) || newPrice <= 0)
            {
                MessageBox.Show("Enter a valid Price.");
                return;
            }

            if (cmbRoomType.SelectedIndex == -1 || cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Select Room Type and Status.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Rooms SET RoomType = @type, Price = @price, Status = @status WHERE RoomNumber = @num", con);
                    cmd.Parameters.AddWithValue("@num", roomNum);
                    cmd.Parameters.AddWithValue("@type", cmbRoomType.Text);
                    cmd.Parameters.AddWithValue("@price", newPrice);
                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                    cmd.ExecuteNonQuery();
                }

                room.RoomType = cmbRoomType.Text;
                room.Price = newPrice;
                room.Status = cmbStatus.Text;

                RefreshDataGrid();
                MessageBox.Show("Room updated.");
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update failed: " + ex.Message);
            }
        }

        private void btnDeleteRoom_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtRoomNumber.Text, out int roomNum))
            {
                MessageBox.Show("Enter a valid Room Number.");
                return;
            }

            Room room = FindRoomInList(roomNum);
            if (room == null)
            {
                MessageBox.Show("Room not found.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Rooms WHERE RoomNumber = @num", con);
                    cmd.Parameters.AddWithValue("@num", roomNum);
                    cmd.ExecuteNonQuery();
                }

                roomList.Remove(room);
                undoStack.Push(new RoomAction("Delete", room));
                redoStack.Clear();

                RefreshDataGrid();
                ClearInputs();
                MessageBox.Show("Room deleted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete failed: " + ex.Message);
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (undoStack.IsEmpty()) return;

            RoomAction action = undoStack.Pop();
            redoStack.Push(action);

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd;

                    if (action.ActionType == "Add")
                    {
                        cmd = new SqlCommand("DELETE FROM Rooms WHERE RoomNumber = @num", con);
                        cmd.Parameters.AddWithValue("@num", action.RoomData.RoomNumber);
                        roomList.Remove(action.RoomData);
                    }
                    else if (action.ActionType == "Delete")
                    {
                        cmd = new SqlCommand("INSERT INTO Rooms VALUES (@num, @type, @price, @status)", con);
                        cmd.Parameters.AddWithValue("@num", action.RoomData.RoomNumber);
                        cmd.Parameters.AddWithValue("@type", action.RoomData.RoomType);
                        cmd.Parameters.AddWithValue("@price", action.RoomData.Price);
                        cmd.Parameters.AddWithValue("@status", action.RoomData.Status);
                        roomList.AddLast(action.RoomData);
                    }
                    else return;

                    cmd.ExecuteNonQuery();
                    RefreshDataGrid();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Undo failed: " + ex.Message);
            }
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            if (redoStack.IsEmpty()) return;

            RoomAction action = redoStack.Pop();
            undoStack.Push(action);

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd;

                    if (action.ActionType == "Add")
                    {
                        cmd = new SqlCommand("INSERT INTO Rooms VALUES (@num, @type, @price, @status)", con);
                        cmd.Parameters.AddWithValue("@num", action.RoomData.RoomNumber);
                        cmd.Parameters.AddWithValue("@type", action.RoomData.RoomType);
                        cmd.Parameters.AddWithValue("@price", action.RoomData.Price);
                        cmd.Parameters.AddWithValue("@status", action.RoomData.Status);
                        roomList.AddLast(action.RoomData);
                    }
                    else if (action.ActionType == "Delete")
                    {
                        cmd = new SqlCommand("DELETE FROM Rooms WHERE RoomNumber = @num", con);
                        cmd.Parameters.AddWithValue("@num", action.RoomData.RoomNumber);
                        roomList.Remove(action.RoomData);
                    }
                    else return;

                    cmd.ExecuteNonQuery();
                    RefreshDataGrid();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Redo failed: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSearchRoomNumber.Text, out int searchNum))
            {
                MessageBox.Show("Enter a valid Room Number.");
                return;
            }

            Room room = FindRoomInList(searchNum);
            if (room == null)
            {
                MessageBox.Show("Room not found.");
                return;
            }

            txtRoomNumber.Text = room.RoomNumber.ToString();
            cmbRoomType.Text = room.RoomType;
            txtPrice.Text = room.Price.ToString();
            cmbStatus.Text = room.Status;

            DataTable dt = new DataTable();
            dt.Columns.Add("RoomNumber");
            dt.Columns.Add("RoomType");
            dt.Columns.Add("Price");
            dt.Columns.Add("Status");
            dt.Rows.Add(room.RoomNumber, room.RoomType, room.Price, room.Status);
            dataGridRooms.DataSource = dt;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchRoomNumber.Clear();
            ClearInputs();
            RefreshDataGrid();
        }

        private void dataGridRooms_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridRooms.Rows[e.RowIndex];
                txtRoomNumber.Text = row.Cells["RoomNumber"].Value.ToString();
                cmbRoomType.Text = row.Cells["RoomType"].Value.ToString();
                txtPrice.Text = row.Cells["Price"].Value.ToString();
                cmbStatus.Text = row.Cells["Status"].Value.ToString();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                LoginForm login = new LoginForm();
                login.Show();
                this.Close();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            DashboardForm dashboard = new DashboardForm();
            dashboard.Show();
            this.Hide();
        }
    }
}

