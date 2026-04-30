using System;
using System.Data;
using System.Data.SqlClient; // Required for SQL database interaction
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; // Required for Chart controls

namespace hotel
{
    public partial class ReportForm : Form
    {
        // Define your connection string here.
        // It's often better to store this in App.config for production apps.
        string connectionString = @"Data Source=DESKTOP-I7ED3VR\SQLEXPRESS;Initial Catalog=HotelManagementDB;Integrated Security=True";

        public ReportForm()
        {
            InitializeComponent();
            // --- IMPORTANT: Manually hooking up event handlers if not done in designer ---
            // This ensures your buttons call the correct methods when clicked.
            // Double-check these in the Properties -> Events section of the designer for each button.
            
            this.btnShowChart.Click += new System.EventHandler(this.btnShowChart_Click);
            this.btnLogut.Click += new System.EventHandler(this.btnLogut_Click);
            // Ensure the form's Load event is also hooked up in the designer to ReportForm_Load
            // (You should remove any hookup to ReportForm_Load_1 if it exists in the designer)
        }

        // --- Event handler for the "Generate Report" button ---
       

        // --- Event handler for the "Show Chart" button ---
        private void btnShowChart_Click(object sender, EventArgs e)
        {
            // Clear existing chart data before adding new data
            chartReport.Series.Clear();
            chartReport.ChartAreas.Clear();
            chartReport.ChartAreas.Add("MainArea"); // Add a chart area name if it doesn't exist

            // --- ComboBox is removed, so we directly set the view type ---
            string viewType = "Month"; // The chart will now always show 'Month' view by default

            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date;

            SqlConnection con = null;
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();

                string query = "";
                string seriesName = "Bookings";

                // Construct the SQL query based on the fixed view type
                switch (viewType)
                {
                    case "Day":
                        query = "SELECT CONVERT(date, CheckIn) AS ReportDate, COUNT(*) AS Bookings FROM Bookings WHERE CheckIn BETWEEN @From AND @To GROUP BY CONVERT(date, CheckIn) ORDER BY CONVERT(date, CheckIn)";
                        seriesName = "Daily Bookings";
                        break;
                    case "Month":
                        // Group by year-month format
                        query = "SELECT FORMAT(CheckIn, 'yyyy-MM') AS ReportDate, COUNT(*) AS Bookings FROM Bookings WHERE CheckIn BETWEEN @From AND @To GROUP BY FORMAT(CheckIn, 'yyyy-MM') ORDER BY FORMAT(CheckIn, 'yyyy-MM')";
                        seriesName = "Monthly Bookings";
                        break;
                    case "Year":
                        query = "SELECT YEAR(CheckIn) AS ReportDate, COUNT(*) AS Bookings FROM Bookings WHERE CheckIn BETWEEN @From AND @To GROUP BY YEAR(CheckIn) ORDER BY YEAR(CheckIn)";
                        seriesName = "Yearly Bookings";
                        break;
                        // No 'default' case needed here as viewType is now fixed to 'Month'
                }

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@From", fromDate);
                // Include the entire 'To' day by adding almost a full day
                cmd.Parameters.AddWithValue("@To", toDate.AddDays(1).AddMilliseconds(-1));

                SqlDataReader reader = cmd.ExecuteReader();

                var series = chartReport.Series.Add(seriesName);
                series.ChartType = SeriesChartType.Column; // Column chart is often good for this data
                series.XValueType = ChartValueType.String; // Treat X-axis as strings (dates, months, years)

                while (reader.Read())
                {
                    string xValue = reader["ReportDate"].ToString();
                    int yValue = Convert.ToInt32(reader["Bookings"]);
                    series.Points.AddXY(xValue, yValue);
                }

                reader.Close(); // Always close the reader
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying chart: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close(); // Ensure connection is closed
                }
            }
        }

        // --- Event handler for the Form's Load event ---
        // This is the ONLY ReportForm_Load method you should have.
        // Make sure it's linked in the designer's Events for the form's Load property.
        private void ReportForm_Load(object sender, EventArgs e)
        {
            // Set default date range
            dtpTo.Value = DateTime.Today;
            // Default to last month or a reasonable range
            dtpFrom.Value = DateTime.Today.AddMonths(-1);
        }

        // --- Event handler for the "Logout" button ---
        // You'll need to implement the actual logout logic here
        private void btnLogut_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Logging out...", "Logout", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Example: Hide this form and show a login form
            this.Hide();
            // LoginForm login = new LoginForm(); // Uncomment if you have a LoginForm
            // login.Show();
            // Or simply close the application if this is the main form
            // Application.Exit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            DashboardForm dashboard = new DashboardForm();
            dashboard.Show();
            this.Hide();
        }

        private void lblTotalRevenue_Click(object sender, EventArgs e)
        {

        }

        private void btnGenerateReort_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Total Bookings
                    SqlCommand cmdBookings = new SqlCommand("SELECT COUNT(*) FROM Bookings", con);
                    int totalBookings = (int)cmdBookings.ExecuteScalar();
                    lblTotalBookings.Text = "Total Bookings: " + totalBookings;

                    // Total Revenue - Corrected: Using 'TotalAmount' and 'PAID' casing
                    SqlCommand cmdRevenue = new SqlCommand("SELECT ISNULL(SUM(TotalAmount), 0) FROM Bookings WHERE PaymentStatus = 'PAID'", con);
                    object revenueResult = cmdRevenue.ExecuteScalar();
                    decimal totalRevenue = Convert.ToDecimal(revenueResult);
                    lblTotalRevenue.Text = "Total Revenue: " + totalRevenue.ToString("C"); // "C" for currency format

                    // Total Customers - Counting distinct GuestIDs from Bookings
                    SqlCommand cmdCustomers = new SqlCommand("SELECT COUNT(DISTINCT GuestID) FROM Bookings", con);
                    int totalCustomers = (int)cmdCustomers.ExecuteScalar();
                    lblTotalCustomers.Text = "Total Customers: " + totalCustomers;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReportForm_Load_1(object sender, EventArgs e)
        {

        }

        private void btnShowChart_Click_1(object sender, EventArgs e)
        {

        }
    }

        // IMPORTANT: The duplicate ReportForm_Load_1 method has been removed.
        // Make sure it is also removed from your .Designer.cs file if it's there,
        // or simply ensure your form's Load event points ONLY to ReportForm_Load.
    }
