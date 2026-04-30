namespace hotel
{
    partial class ReportForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnLogut = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblTotalBookings = new System.Windows.Forms.Label();
            this.lblTotalRevenue = new System.Windows.Forms.Label();
            this.lblTotalCustomers = new System.Windows.Forms.Label();
            this.btnShowChart = new System.Windows.Forms.Button();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.chartReport = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblToDate = new System.Windows.Forms.Label();
            this.btnGenerateReort = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartReport)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(14)))), ((int)(((byte)(28)))));
            this.panel2.Controls.Add(this.btnBack);
            this.panel2.Controls.Add(this.btnLogut);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Location = new System.Drawing.Point(1, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(265, 585);
            this.panel2.TabIndex = 26;
            // 
            // btnBack
            // 
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnBack.Location = new System.Drawing.Point(148, 348);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(88, 27);
            this.btnBack.TabIndex = 40;
            this.btnBack.Text = "BACK";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnLogut
            // 
            this.btnLogut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogut.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnLogut.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnLogut.Location = new System.Drawing.Point(25, 348);
            this.btnLogut.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLogut.Name = "btnLogut";
            this.btnLogut.Size = new System.Drawing.Size(88, 27);
            this.btnLogut.TabIndex = 9;
            this.btnLogut.Text = "LOGOUT";
            this.btnLogut.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(43, 269);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(172, 19);
            this.label2.TabIndex = 8;
            this.label2.Text = "A home away from home";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(91, 250);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 19);
            this.label3.TabIndex = 7;
            this.label3.Text = "SavvyInn";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::hotel.Properties.Resources.w_removebg_preview__3_;
            this.pictureBox2.Location = new System.Drawing.Point(75, 74);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(108, 115);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // lblTotalBookings
            // 
            this.lblTotalBookings.AutoSize = true;
            this.lblTotalBookings.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblTotalBookings.Location = new System.Drawing.Point(307, 397);
            this.lblTotalBookings.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalBookings.Name = "lblTotalBookings";
            this.lblTotalBookings.Size = new System.Drawing.Size(100, 15);
            this.lblTotalBookings.TabIndex = 27;
            this.lblTotalBookings.Text = "Total Bookings 0";
            // 
            // lblTotalRevenue
            // 
            this.lblTotalRevenue.AutoSize = true;
            this.lblTotalRevenue.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblTotalRevenue.Location = new System.Drawing.Point(511, 397);
            this.lblTotalRevenue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalRevenue.Name = "lblTotalRevenue";
            this.lblTotalRevenue.Size = new System.Drawing.Size(93, 15);
            this.lblTotalRevenue.TabIndex = 28;
            this.lblTotalRevenue.Text = "Total Revenue 0\r\n";
            this.lblTotalRevenue.Click += new System.EventHandler(this.lblTotalRevenue_Click);
            // 
            // lblTotalCustomers
            // 
            this.lblTotalCustomers.AutoSize = true;
            this.lblTotalCustomers.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblTotalCustomers.Location = new System.Drawing.Point(701, 397);
            this.lblTotalCustomers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalCustomers.Name = "lblTotalCustomers";
            this.lblTotalCustomers.Size = new System.Drawing.Size(107, 15);
            this.lblTotalCustomers.TabIndex = 29;
            this.lblTotalCustomers.Text = "Total Customers 0";
            // 
            // btnShowChart
            // 
            this.btnShowChart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowChart.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnShowChart.Location = new System.Drawing.Point(497, 74);
            this.btnShowChart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnShowChart.Name = "btnShowChart";
            this.btnShowChart.Size = new System.Drawing.Size(191, 27);
            this.btnShowChart.TabIndex = 30;
            this.btnShowChart.Text = "SHOW CHART";
            this.btnShowChart.UseVisualStyleBackColor = true;
            this.btnShowChart.Click += new System.EventHandler(this.btnShowChart_Click_1);
            // 
            // dtpFrom
            // 
            this.dtpFrom.Location = new System.Drawing.Point(343, 38);
            this.dtpFrom.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(233, 22);
            this.dtpFrom.TabIndex = 31;
            // 
            // dtpTo
            // 
            this.dtpTo.Location = new System.Drawing.Point(631, 38);
            this.dtpTo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(233, 22);
            this.dtpTo.TabIndex = 32;
            // 
            // chartReport
            // 
            chartArea2.Name = "ChartArea1";
            this.chartReport.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartReport.Legends.Add(legend2);
            this.chartReport.Location = new System.Drawing.Point(291, 144);
            this.chartReport.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chartReport.Name = "chartReport";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartReport.Series.Add(series2);
            this.chartReport.Size = new System.Drawing.Size(570, 216);
            this.chartReport.TabIndex = 34;
            this.chartReport.Text = "chart1";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblFrom.Location = new System.Drawing.Point(288, 38);
            this.lblFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(51, 15);
            this.lblFrom.TabIndex = 36;
            this.lblFrom.Text = "FROM :";
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblToDate.Location = new System.Drawing.Point(594, 38);
            this.lblToDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(29, 15);
            this.lblToDate.TabIndex = 37;
            this.lblToDate.Text = "TO:";
            // 
            // btnGenerateReort
            // 
            this.btnGenerateReort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateReort.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnGenerateReort.Location = new System.Drawing.Point(497, 466);
            this.btnGenerateReort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnGenerateReort.Name = "btnGenerateReort";
            this.btnGenerateReort.Size = new System.Drawing.Size(191, 27);
            this.btnGenerateReort.TabIndex = 39;
            this.btnGenerateReort.Text = "GENERATE REPORT";
            this.btnGenerateReort.UseVisualStyleBackColor = true;
            this.btnGenerateReort.Click += new System.EventHandler(this.btnGenerateReort_Click);
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 523);
            this.Controls.Add(this.btnGenerateReort);
            this.Controls.Add(this.lblToDate);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.chartReport);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.btnShowChart);
            this.Controls.Add(this.lblTotalCustomers);
            this.Controls.Add(this.lblTotalRevenue);
            this.Controls.Add(this.lblTotalBookings);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReportForm";
            this.Load += new System.EventHandler(this.ReportForm_Load_1);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnLogut;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblTotalBookings;
        private System.Windows.Forms.Label lblTotalRevenue;
        private System.Windows.Forms.Label lblTotalCustomers;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnShowChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartReport;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.Button btnGenerateReort;
        private System.Windows.Forms.Button btnBack;
    }
}