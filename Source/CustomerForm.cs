using hotel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hotel
{
    public partial class CustomerForm : Form
    {
        string connectionString = @"Data Source=DESKTOP-I7ED3VR\SQLEXPRESS;Initial Catalog=HotelManagementDB;Integrated Security=True";
        CustomerBST customerTree = new CustomerBST();

        public CustomerForm()
        {
            InitializeComponent();
            LoadCustomersToBST();

            cmbSearchBy.Items.Add("Name");
            cmbSearchBy.Items.Add("CNIC");
            cmbSearchBy.SelectedIndex = 0;
        }

        private void LoadCustomersToBST()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
    SELECT g.GuestName, g.CNIC, g.Email, g.Phone, b.RoomNumber, b.CheckIn, b.CheckOut
    FROM Guests g
    INNER JOIN Bookings b ON g.GuestID = b.GuestID", conn);


                SqlDataReader reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);
                dgvCustomerDetails.DataSource = dt;

                foreach (DataRow row in dt.Rows)
                {
                    Customer customer = new Customer
                    {
                        GuestName = row["GuestName"].ToString(),
                        CNIC = row["CNIC"].ToString(),
                        Email = row["Email"].ToString(),
                        Phone = row["Phone"].ToString(),
                        RoomNumber = row["RoomNumber"].ToString(),
                        CheckIn = Convert.ToDateTime(row["CheckIn"]),      // new
                        CheckOut = Convert.ToDateTime(row["CheckOut"])     // new
                    };

                    customerTree.Insert(customer.GuestName, customer); // Key = GuestName
                }

            }
            dgvCustomerDetails.Columns["CheckIn"].DefaultCellStyle.Format = "yyyy-MM-dd";
            dgvCustomerDetails.Columns["CheckOut"].DefaultCellStyle.Format = "yyyy-MM-dd";

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearchName.Text.Trim().ToLower();
            string searchBy = cmbSearchBy.SelectedItem.ToString();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Enter a keyword to search.");
                return;
            }

            List<Customer> results = customerTree.SearchPartial(keyword, searchBy);

            if (results.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("GuestName");
                dt.Columns.Add("CNIC");
                dt.Columns.Add("Email");
                dt.Columns.Add("Phone");
                dt.Columns.Add("RoomNumber");
                dt.Columns.Add("CheckIn", typeof(DateTime));    // new
                dt.Columns.Add("CheckOut", typeof(DateTime));   // new

                foreach (var c in results)
                {
                    dt.Rows.Add(c.GuestName, c.CNIC, c.Email, c.Phone, c.RoomNumber, c.CheckIn, c.CheckOut);
                }


                dgvCustomerDetails.DataSource = dt;
            }
            else
            {
                MessageBox.Show("No matching customers found.");
                dgvCustomerDetails.DataSource = null;
            }
            dgvCustomerDetails.Columns["CheckIn"].DefaultCellStyle.Format = "yyyy-MM-dd";
            dgvCustomerDetails.Columns["CheckOut"].DefaultCellStyle.Format = "yyyy-MM-dd";

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            new DashboardForm().Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                new LoginForm().Show();
                this.Close();
            }
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {

        }

        private void dgvCustomerDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearchName.Clear();
            dgvCustomerDetails.DataSource = null;
            customerTree = new CustomerBST(); // Clear the old tree
            LoadCustomersToBST(); // Reload all customers
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    // ===================== BST CLASSES =======================

    public class Customer
    {
        public string GuestName { get; set; }
        public string CNIC { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string RoomNumber { get; set; }
        public DateTime CheckIn { get; set; }   // new
        public DateTime CheckOut { get; set; }  // new
    }

    public class BSTNode
    {
        public string Key;
        public Customer Data;
        public BSTNode Left;
        public BSTNode Right;

        public BSTNode(string key, Customer data)
        {
            Key = key.ToLower();
            Data = data;
            Left = Right = null;
        }
    }

    public class CustomerBST
    {
        private BSTNode root;

        public void Insert(string key, Customer data)
        {
            root = InsertRec(root, key.ToLower(), data);
        }

        private BSTNode InsertRec(BSTNode root, string key, Customer data)
        {
            if (root == null)
                return new BSTNode(key, data);

            if (string.Compare(key, root.Key) < 0)
                root.Left = InsertRec(root.Left, key, data);
            else
                root.Right = InsertRec(root.Right, key, data);

            return root;
        }

        public List<Customer> SearchPartial(string keyword, string searchBy)
        {
            List<Customer> result = new List<Customer>();
            keyword = keyword.ToLower();
            InOrderSearch(root, keyword, searchBy, result);
            return result;
        }

        private void InOrderSearch(BSTNode node, string keyword, string searchBy, List<Customer> result)
        {
            if (node == null) return;

            InOrderSearch(node.Left, keyword, searchBy, result);

            string field = searchBy == "CNIC" ? node.Data.CNIC.ToLower() : node.Data.GuestName.ToLower();
            if (field.Contains(keyword))
            {
                result.Add(node.Data);
            }

            InOrderSearch(node.Right, keyword, searchBy, result);
        }
    }
}
