using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace newfinalSSS.Model
{
    public partial class frmPOS : Form
    {

        private string loggedInStaffName;
        public frmPOS()
        {
            InitializeComponent();
            loggedInStaffName = MainClass.USER;
            //LoadCustomers();

        }
        private void InitializeComboBox()
        {
            //cbCustomer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cbCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            categoryPanel.Visible = true;
            categoryPanel.Dock = DockStyle.Left;
            InitializeDataGridView();
            InitializeCustomerDataGridView();
            LoadCategoriesAndProducts();
            //LoadTechnicians();
            panelCustomer.Visible = false;
        }

        private void InitializeDataGridView()
        {
            // ✅ Ensure Columns are Cleared First
            guna2DataGridView2.Columns.Clear();

            // ✅ Add Required Columns
            guna2DataGridView2.Columns.Add("dgvIndex", "No."); // Serial number
            guna2DataGridView2.Columns.Add("dgvid", "Product ID");
            guna2DataGridView2.Columns.Add("dgvName", "Product Name");
            guna2DataGridView2.Columns.Add("dgvQty", "Quantity");
            guna2DataGridView2.Columns.Add("dgvPrice", "Price");
            guna2DataGridView2.Columns.Add("dgvAmount", "Amount");

            // ✅ Check if Delete Column Already Exists Before Adding
            if (!guna2DataGridView2.Columns.Contains("dgvDelete"))
            {
                DataGridViewImageColumn dgvDelete = new DataGridViewImageColumn();
                dgvDelete.Name = "dgvDelete";
                dgvDelete.HeaderText = "Action";

                // ✅ Load Delete Icon from Resources Safely
                if (Properties.Resources.delete__1_ != null)
                {
                    dgvDelete.Image = Properties.Resources.delete__1_;
                }

                dgvDelete.ImageLayout = DataGridViewImageCellLayout.Zoom;
                guna2DataGridView2.Columns.Add(dgvDelete);
            }

            // ✅ Set Column Properties
            guna2DataGridView2.Columns["dgvid"].Visible = false; // Hide ID column if not needed
            guna2DataGridView2.Columns["dgvQty"].Width = 50;
            guna2DataGridView2.Columns["dgvPrice"].Width = 60;
            guna2DataGridView2.Columns["dgvAmount"].Width = 100;
            guna2DataGridView2.Columns["dgvDelete"].Width = 50; // Set width for delete button


        }


        private void LoadCategoriesAndProducts()
        {
            try
            {
                string qry = "SELECT * FROM category";

                using (SqlCommand cmd = new SqlCommand(qry, MainClass.con))
                {
                    if (MainClass.con.State != ConnectionState.Open)
                        MainClass.con.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        categoryPanel.Controls.Clear();

                        // Add "Show All" button
                        Guna.UI2.WinForms.Guna2Button allButton = new Guna.UI2.WinForms.Guna2Button
                        {
                            FillColor = Color.FromArgb(95, 61, 204),
                            Size = new Size(197, 45),
                            ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton,
                            Text = "Show All",
                            Font = new Font("Arial", 12, FontStyle.Bold),
                            Tag = 0 // Tag 0 for all products
                        };

                        allButton.Click += (s, e) =>
                        {
                            LoadProducts(0); // Load all products
                        };

                        categoryPanel.Controls.Add(allButton);

                        // Load categories from database
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button
                                {
                                    FillColor = Color.FromArgb(95, 61, 204),
                                    Size = new Size(197, 45),
                                    ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton,
                                    Text = dr["catName"].ToString(),
                                    Tag = dr["catID"],
                                    Font = new Font("Arial", 12, FontStyle.Bold)
                                };

                                b.Click += (s, e) =>
                                {
                                    int selectedCategoryID = Convert.ToInt32(((Guna.UI2.WinForms.Guna2Button)s).Tag);
                                    LoadProducts(selectedCategoryID);
                                };

                                categoryPanel.Controls.Add(b);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No categories found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        // Default: Load all products if no category is selected
                        LoadProducts(0);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Optionally log ex.ToString() for more details
            }
            finally
            {
                if (MainClass.con.State == ConnectionState.Open)
                    MainClass.con.Close();
            }
        }


        private void LoadProducts(int categoryID)
        {
            try
            {
                string qry;

                if (categoryID == 0) // ✅ Show all products
                {
                    qry = "SELECT p.pID, p.pName, p.pPrice, p.pImage, c.catName FROM product p " +
                          "INNER JOIN category c ON c.catID = p.categoryID";
                }
                else // ✅ Show only selected category products
                {
                    qry = "SELECT p.pID, p.pName, p.pPrice, p.pImage, c.catName FROM product p " +
                          "INNER JOIN category c ON c.catID = p.categoryID " +
                          "WHERE p.categoryID = @catID";
                }

                using (SqlCommand cmd = new SqlCommand(qry, MainClass.con))
                {
                    if (categoryID != 0)
                        cmd.Parameters.AddWithValue("@catID", categoryID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        productPanel.Controls.Clear();

                        foreach (DataRow item in dt.Rows)
                        {
                            byte[] imageArray = item["pImage"] != DBNull.Value ? (byte[])item["pImage"] : null;

                            AddItems(Convert.ToInt32(item["pID"]),
                                     item["pName"].ToString(),
                                     item["catName"].ToString(),
                                     Convert.ToDecimal(item["pPrice"]),
                                     imageArray);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void _Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            foreach (var item in categoryPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PCategory.ToLower().Contains(b.Text.Trim().ToLower());
            }
        }

        private void AddItems(int id, string name, string cat, decimal price, byte[] pImage)
        {
            var w = new ucProduct()
            {
                PName = name,
                PPrice = price,
                PCategory = cat,
                PImage = pImage, // ✅ Now using binary image data
                Id = id
            };

            productPanel.Controls.Add(w);

            w.onSelect += (ss, ee) =>
            {
                var wdg = (ucProduct)ss;

                foreach (DataGridViewRow item in guna2DataGridView2.Rows)
                {
                    if (Convert.ToInt32(item.Cells["dgvid"].Value) == wdg.Id)
                    {
                        item.Cells["dgvQty"].Value = Convert.ToInt32(item.Cells["dgvQty"].Value) + 1;
                        item.Cells["dgvAmount"].Value = Convert.ToInt32(item.Cells["dgvQty"].Value) *
                                                         Convert.ToDecimal(item.Cells["dgvPrice"].Value);

                        GetTotal(); // ✅ Only call once
                        UpdateBillProduct(id, Convert.ToInt32(item.Cells["dgvQty"].Value), price);
                        return;
                    }
                }

                // ✅ Ensure delete column receives an image
                guna2DataGridView2.Rows.Add(new object[]
                {
            guna2DataGridView2.Rows.Count + 1, // Serial number
            id,
            name,
            1,
            price,
            price, // Amount = price * quantity (1)
            Properties.Resources.delete__1_ // ✅ Correctly assign delete image
                });

                GetTotal();
                InsertBillProduct(id, 1, price);
            };
        }


        private void InsertBillProduct(int prodID, int qty, decimal price)
        {
            try
            {
                string qry = @"INSERT INTO tblDetails (MainId, prodID, qty, price, amount) 
                       VALUES (@MainId, @prodID, @qty, @price, @amount)";

                using (SqlCommand cmd = new SqlCommand(qry, MainClass.con))
                {
                    cmd.Parameters.AddWithValue("@MainId", mainID);
                    cmd.Parameters.AddWithValue("@prodID", prodID);
                    cmd.Parameters.AddWithValue("@qty", qty);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@amount", qty * price);

                    if (MainClass.con.State != ConnectionState.Open)
                        MainClass.con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding product to bill: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (MainClass.con.State == ConnectionState.Open)
                    MainClass.con.Close();
            }
        }

        private void UpdateBillProduct(int prodID, int qty, decimal price)
        {
            try
            {
                string qry = @"UPDATE tblDetails SET qty = @qty, amount = @amount 
                       WHERE MainId = @MainId AND prodID = @prodID";

                using (SqlCommand cmd = new SqlCommand(qry, MainClass.con))
                {
                    cmd.Parameters.AddWithValue("@MainId", mainID);
                    cmd.Parameters.AddWithValue("@prodID", prodID);
                    cmd.Parameters.AddWithValue("@qty", qty);
                    cmd.Parameters.AddWithValue("@amount", qty * price);

                    if (MainClass.con.State != ConnectionState.Open)
                        MainClass.con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating product in bill: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (MainClass.con.State == ConnectionState.Open)
                    MainClass.con.Close();
            }
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach (Control item in productPanel.Controls)
            {
                if (item is ucProduct pro)
                {
                    pro.Visible = pro.PName.ToLower().Contains(txtSearch.Text.Trim().ToLower());
                }
            }
        }

        private void guna2DataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int count = 0;

            foreach (DataGridViewRow row in guna2DataGridView2.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        private void GetTotal()
        {
            double tot = 0;
            lblTotal.Text = ""; // Ensure it's cleared before calculation

            foreach (DataGridViewRow item in guna2DataGridView2.Rows)
            {
                if (item.Cells["dgvAmount"].Value != null)
                {
                    tot += Convert.ToDouble(item.Cells["dgvAmount"].Value);
                }
            }

            lblTotal.Text = "Rs. " + tot.ToString("N2"); // ✅ Format as Sri Lankan Rupees
        }


        private void btnTechnician_Click(object sender, EventArgs e)
        {

        }

        //private void LoadTechnicians(int defaultIndex = 0)
        //{
        //    try
        //    {
        //        string qry = "SELECT staffID, sName FROM staff WHERE LOWER(sRole) = 'technician'";
        //        DataTable dt = new DataTable();

        //        using (SqlCommand cmd = new SqlCommand(qry, MainClass.con))
        //        {
        //            if (MainClass.con.State != ConnectionState.Open)
        //                MainClass.con.Open();

        //            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        //            {
        //                adapter.Fill(dt);
        //            }
        //        }

        //        if (dt.Rows.Count > 0)
        //        {
        //            cbTechnician.DataSource = null;  // Reset before assigning new data
        //            cbTechnician.DataSource = dt;  // Bind data source
        //            cbTechnician.DisplayMember = "sName";  // Display Technician Name
        //            cbTechnician.ValueMember = "staffID";  // Store Technician ID

        //            // Ensure valid index before setting
        //            if (defaultIndex >= 0 && defaultIndex < dt.Rows.Count)
        //            {
        //                cbTechnician.SelectedIndex = defaultIndex;  // Set to the desired index
        //            }
        //            else
        //            {
        //                cbTechnician.SelectedIndex = 0;  // Default to first item
        //            }

        //            cbTechnician.Update(); // Ensure UI refresh
        //            cbTechnician.Refresh();
        //        }
        //        else
        //        {
        //            cbTechnician.DataSource = null;
        //            MessageBox.Show("No technicians found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error loading technicians: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        if (MainClass.con.State == ConnectionState.Open)
        //            MainClass.con.Close();
        //    }
        //}

        private void cbTechnician_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // 📌 Method to Get Selected Technician Index
        //private int GetSelectedTechnicianIndex()
        //{
        //    return cbTechnician.SelectedIndex;
        //}


        private int MainID = 0; // Initialize MainID with 0 (new order by default)
        private int DetailID = 0; // Initialize DetailID to 0 (new detail entry by default)


        private void btnKOT_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate if a technician is selected
                //if (cbTechnician.SelectedIndex == -1 || string.IsNullOrEmpty(cbTechnician.Text))
                //{
                //    MessageBox.Show("Please select a technician.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                // Validate total amount
                if (!double.TryParse(lblTotal.Text.Replace("Rs.", "").Trim(), out double total) || total <= 0)
                {
                    MessageBox.Show("Invalid total amount.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                //string technicianName = cbTechnician.Text.Trim();
                double received = 0;   // Default received amount (For KOT, it’s not necessary)
                double change = 0;     // Default change (KOT is typically unpaid)

                // ✅ Get the next Bill ID (MainID + 1) before inserting the record
                int nextBillID = GetNextBillID();

                // SQL Query for tblMain (Insert or Update)
                string qry1;
                int mainID = MainID;

                if (mainID == 0)
                {
                    qry1 = @"INSERT INTO tblMain (aDate, aTime, TechnicianName, status, total, received, change)
                     VALUES (@aDate, @aTime, @TechnicianName, @status, @total, @received, @change);
                     SELECT SCOPE_IDENTITY();"; // Retrieve the new MainID
                }
                else
                {
                    qry1 = @"UPDATE tblMain SET TechnicianName = @TechnicianName, status = @status, total = @total,
                     received = @received, change = @change WHERE MainID = @MainID";
                }

                // Execute Main Order Query
                using (SqlCommand cmd = new SqlCommand(qry1, MainClass.con))
                {
                    cmd.Parameters.AddWithValue("@aDate", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToString("HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@TechnicianName", loggedInStaffName);
                    cmd.Parameters.AddWithValue("@status", "Pending");
                    cmd.Parameters.AddWithValue("@total", total);
                    cmd.Parameters.AddWithValue("@received", received);
                    cmd.Parameters.AddWithValue("@change", change);

                    if (mainID != 0)
                        cmd.Parameters.AddWithValue("@MainID", mainID);

                    if (MainClass.con.State != ConnectionState.Open)
                        MainClass.con.Open();

                    if (mainID == 0)
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                            mainID = Convert.ToInt32(result);
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // ✅ Store the current Bill ID
                MainID = nextBillID;

                // If order created successfully, insert products into tblDetails
                if (mainID > 0)
                {
                    foreach (DataGridViewRow row in guna2DataGridView2.Rows)
                    {
                        if (row.Cells["dgvid"].Value != null &&
                            row.Cells["dgvQty"].Value != null &&
                            row.Cells["dgvPrice"].Value != null &&
                            row.Cells["dgvAmount"].Value != null)
                        {
                            if (int.TryParse(row.Cells["dgvid"].Value.ToString(), out int prodID) &&
                                int.TryParse(row.Cells["dgvQty"].Value.ToString(), out int qty) &&
                                double.TryParse(row.Cells["dgvPrice"].Value.ToString(), out double price) &&
                                double.TryParse(row.Cells["dgvAmount"].Value.ToString(), out double amount))
                            {
                                string qry2;
                                if (DetailID == 0)
                                {
                                    qry2 = @"INSERT INTO tblDetails (MainId, prodID, qty, price, amount)
                                     VALUES (@MainId, @prodID, @qty, @price, @amount)";
                                }
                                else
                                {
                                    qry2 = @"UPDATE tblDetails SET qty = @qty, price = @price, amount = @amount
                                     WHERE DetailID = @DetailID";
                                }

                                using (SqlCommand cmd = new SqlCommand(qry2, MainClass.con))
                                {
                                    cmd.Parameters.AddWithValue("@MainId", mainID);
                                    cmd.Parameters.AddWithValue("@prodID", prodID);
                                    cmd.Parameters.AddWithValue("@qty", qty);
                                    cmd.Parameters.AddWithValue("@price", price);
                                    cmd.Parameters.AddWithValue("@amount", amount);

                                    if (DetailID != 0)
                                        cmd.Parameters.AddWithValue("@DetailID", DetailID);

                                    if (MainClass.con.State != ConnectionState.Open)
                                        MainClass.con.Open();

                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid data format in the product list.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Some product details are missing.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Trigger KOT Print
                    PrintDialog printDialog1 = new PrintDialog();
                    printDialog1.Document = printDocument1;

                    DialogResult result = printDialog1.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        printDocument1.Print();
                    }

                    // Reset Fields After Successful Order Processing
                    guna2DataGridView2.Rows.Clear();
                    lblTotal.Text = "$0.00";
                    //cbTechnician.SelectedIndex = -1;

                    MainID = 0;
                    DetailID = 0;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing KOT: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (MainClass.con.State != ConnectionState.Closed)
                    MainClass.con.Close();
            }

        }
        private int mainID = 0;
        private void btnBillList_Click(object sender, EventArgs e)
        {
            using (frmBillList f = new frmBillList())
            {
                f.ShowDialog();

                if (f.MainID > 0) // ✅ Check if a valid bill was selected
                {
                    mainID = f.MainID; // ✅ Assign Bill ID
                    LoadEnteries(); // ✅ Load the products for the bill

                    // ✅ Set the Technician Name in the ComboBox
                    //string technicianName = f.SelectedTechnician;
                    //if (!string.IsNullOrEmpty(technicianName))
                    //{
                    //    int index = cbTechnician.FindStringExact(technicianName);
                    //    if (index >= 0)
                    //    {
                    //        cbTechnician.SelectedIndex = index; // ✅ Select if exists
                    //    }
                    //    else
                    //    {
                    //        cbTechnician.Text = technicianName; // ✅ If not in list, show as text
                    //    }
                    //}
                }
            }
        }

        private void LoadEnteries()
        {
            try
            {
                string qry2 = @"SELECT d.DetailID, d.prodID, p.pName, d.qty, d.price, d.amount 
                        FROM tblDetails d
                        INNER JOIN product p ON d.prodID = p.pID
                        WHERE d.MainID = @MainID";

                DataTable dtDetails = new DataTable();

                using (SqlCommand cmd = new SqlCommand(qry2, MainClass.con))
                {
                    cmd.Parameters.AddWithValue("@MainID", mainID);

                    if (MainClass.con.State != ConnectionState.Open)
                        MainClass.con.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtDetails);
                    }
                }

                // ✅ Debugging: Check if any data was fetched
                if (dtDetails.Rows.Count == 0)
                {
                    MessageBox.Show("No products found for this bill.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // ✅ Clear DataGridView safely
                guna2DataGridView2.Rows.Clear();

                // ✅ Add data safely
                foreach (DataRow row in dtDetails.Rows)
                {
                    guna2DataGridView2.Rows.Add(
                        guna2DataGridView2.Rows.Count + 1, // Serial number
                        row["prodID"],
                        row["pName"],
                        row["qty"],
                        row["price"],
                        row["amount"]
                    );
                }

                guna2DataGridView2.Refresh();
                GetTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bill products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (MainClass.con.State == ConnectionState.Open)
                    MainClass.con.Close();
            }
        }

        private int GetNextBillID()
        {
            int nextBillID = 1; // Default in case the table is empty
            try
            {
                string qry = "SELECT ISNULL(MAX(MainID), 0) + 1 FROM tblMain"; // Get last MainID and add 1

                using (SqlCommand cmd = new SqlCommand(qry, MainClass.con))
                {
                    if (MainClass.con.State != ConnectionState.Open)
                        MainClass.con.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        nextBillID = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving Bill ID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (MainClass.con.State != ConnectionState.Closed)
                    MainClass.con.Close();
            }
            return nextBillID;
        }


        
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int pageWidth = e.PageBounds.Width;
            int startX = 20;
            int startY = 20;

            Font headerFont = new Font("Arial", 20, FontStyle.Bold);
            Font subHeaderFont = new Font("Arial", 14, FontStyle.Bold);
            Font textFont = new Font("Arial", 10, FontStyle.Regular);
            Font totalFont = new Font("Arial", 12, FontStyle.Bold);

            // 📌 Define Right-Aligned String Format for Price & Total
            StringFormat rightAlignFormat = new StringFormat();
            rightAlignFormat.Alignment = StringAlignment.Far; // Right align text

            // Center Alignment Helper Function
            int CenterText(string text, Font font)
            {
                return (pageWidth - (int)e.Graphics.MeasureString(text, font).Width) / 2;
            }

            // 📌 Print Logo (if available in Resources)
            try
            {
                Image logo = Properties.Resources.llgo; // Ensure logo.png is added to Resources
                if (logo != null)
                {
                    int logoWidth = 200;
                    int logoHeight = 183;
                    int logoX = (pageWidth - logoWidth) / 2;
                    e.Graphics.DrawImage(logo, logoX, startY, logoWidth, logoHeight);
                    startY += logoHeight + 20;
                }
            }
            catch (Exception) { }

            // 📌 Print Centered Header with Correct Bill ID
            string title = "Sasik Service Station";
            string subtitle = "Estimation Report";
            string billID = $"Estimation ID: {MainID}";

            e.Graphics.DrawString(title, headerFont, Brushes.Black, CenterText(title, headerFont), startY);
            startY += 30;
            e.Graphics.DrawString(subtitle, subHeaderFont, Brushes.Black, CenterText(subtitle, subHeaderFont), startY);
            startY += 30;
            e.Graphics.DrawString(billID, textFont, Brushes.Black, CenterText(billID, textFont), startY);
            startY += 30;

            // 📌 Print Contact Below Subtitle
            string contact = "Landline: 0372 267 991";
            e.Graphics.DrawString(contact, textFont, Brushes.Black, CenterText(contact, textFont), startY);
            startY += 40;

            // 📌 Print Date & Technician Details
            e.Graphics.DrawString($"Date: {DateTime.Now.ToShortDateString()}  Time: {DateTime.Now.ToShortTimeString()}",
                                  textFont, Brushes.Black, startX, startY);
            startY += 25;
            e.Graphics.DrawString($"Technician: {loggedInStaffName}", textFont, Brushes.Black, startX, startY);
            startY += 40;

            // 🔹 Print Selected Customer Details
            e.Graphics.DrawString($"Customer Name: {selectedCustomerName}", textFont, Brushes.Black, startX, startY);
            startY += 20;
            e.Graphics.DrawString($"Phone: {selectedCustomerPhone}", textFont, Brushes.Black, startX, startY);
            startY += 20;
            e.Graphics.DrawString($"Email: {selectedCustomerEmail}", textFont, Brushes.Black, startX, startY);
            startY += 20;
            e.Graphics.DrawString($"Vehicle Type: {selectedCustomerVehicleType}", textFont, Brushes.Black, startX, startY);
            startY += 20;
            e.Graphics.DrawString($"Vehicle No: {selectedCustomerVehicleNo}", textFont, Brushes.Black, startX, startY);
            startY += 40;

            // 📌 Adjust Column Positions (Increase Space Between Price & Total)
            int colItem = startX;
            int colQty = pageWidth / 2 - 160;  // Shift Quantity a bit left
            int colPrice = colQty + 140;       // Move Price further right
            int colTotal = colPrice + 140;     // Increase spacing for Total column

            // 📌 Print Table Header (Aligned)
            e.Graphics.DrawString("Item", subHeaderFont, Brushes.Black, colItem, startY);
            e.Graphics.DrawString("Qty", subHeaderFont, Brushes.Black, colQty, startY);
            e.Graphics.DrawString("Price", subHeaderFont, Brushes.Black, colPrice, startY);
            e.Graphics.DrawString("Total", subHeaderFont, Brushes.Black, colTotal, startY);
            startY += 30;

            e.Graphics.DrawLine(Pens.Black, startX, startY, pageWidth - startX, startY);
            startY += 10;

            // 📌 Print Item List (Right-Aligned Prices and Totals)
            double totalAmount = 0;
            foreach (DataGridViewRow row in guna2DataGridView2.Rows)
            {
                if (row.Cells["dgvName"].Value != null && row.Cells["dgvQty"].Value != null &&
                    row.Cells["dgvPrice"].Value != null && row.Cells["dgvAmount"].Value != null)
                {
                    string itemName = row.Cells["dgvName"].Value.ToString();
                    int quantity = Convert.ToInt32(row.Cells["dgvQty"].Value);
                    double price = Convert.ToDouble(row.Cells["dgvPrice"].Value);
                    double amount = Convert.ToDouble(row.Cells["dgvAmount"].Value);
                    totalAmount += amount;

                    string priceText = "Rs. " + price.ToString("N2");
                    string amountText = "Rs. " + amount.ToString("N2");

                    e.Graphics.DrawString(itemName, textFont, Brushes.Black, colItem, startY);
                    e.Graphics.DrawString(quantity.ToString(), textFont, Brushes.Black, colQty, startY);

                    // ✅ Right Align Price & Total
                    e.Graphics.DrawString(priceText, textFont, Brushes.Black,
                                          new Rectangle(colPrice - 50, startY, 100, 20), rightAlignFormat);
                    e.Graphics.DrawString(amountText, textFont, Brushes.Black,
                                          new Rectangle(colTotal - 50, startY, 100, 20), rightAlignFormat);

                    startY += 25;
                }
            }

            // 📌 Print Footer with Properly Aligned "Total"
            startY += 10;
            e.Graphics.DrawLine(Pens.Black, startX, startY, pageWidth - startX, startY);
            startY += 10;

            // ✅ Ensure "Total:" label is under the Price column
            e.Graphics.DrawString("Total:", totalFont, Brushes.Black,
                                  new Rectangle(colPrice - 50, startY, 100, 20), rightAlignFormat);

            // ✅ Ensure Total Amount is under the Total column
            string totalText = "Rs. " + totalAmount.ToString("N2");
            e.Graphics.DrawString(totalText, totalFont, Brushes.Black,
                                  new Rectangle(colTotal - 50, startY, 140, 20), rightAlignFormat);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        //private void LoadCustomers(int defaultIndex = 0)
        //{
        //    try
        //    {
        //        string qry = "SELECT cusID, cusName FROM customer"; // Get customer list

        //        DataTable dt = new DataTable();
        //        using (SqlCommand cmd = new SqlCommand(qry, MainClass.con))
        //        {
        //            if (MainClass.con.State != ConnectionState.Open)
        //                MainClass.con.Open();

        //            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        //            {
        //                adapter.Fill(dt);
        //            }
        //        }

        //        if (dt.Rows.Count > 0)
        //        {
        //            cbCustomer.DataSource = dt;  // Bind Data
        //            cbCustomer.DisplayMember = "cusName";  // Show Customer Name
        //            cbCustomer.ValueMember = "cusID";  // Store Customer ID

        //            // ✅ Enable AutoComplete with customer names
        //            AutoCompleteStringCollection autoComplete = new AutoCompleteStringCollection();
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                autoComplete.Add(row["cusName"].ToString());
        //            }
        //            cbCustomer.AutoCompleteCustomSource = autoComplete;

        //            // Set Default Index
        //            cbCustomer.SelectedIndex = defaultIndex < dt.Rows.Count ? defaultIndex : 0;

        //            cbCustomer.Update(); // Refresh UI
        //            cbCustomer.Refresh();
        //        }
        //        else
        //        {
        //            cbCustomer.DataSource = null;
        //            cbCustomer.AutoCompleteCustomSource = null;
        //            MessageBox.Show("No customers found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error loading customers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        if (MainClass.con.State != ConnectionState.Closed)
        //            MainClass.con.Close();
        //    }
        //}



        private void cbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cbCustomer.SelectedIndex >= 0)
            //{
            //    if (cbCustomer.SelectedValue is DataRowView rowView)
            //    {
            //        int selectedCusID = Convert.ToInt32(rowView["cusID"]);  // Correctly fetch `cusID`
            //        GetCustomerDetails(selectedCusID);
            //    }
            //    else if (cbCustomer.SelectedValue is int selectedCusID)
            //    {
            //        GetCustomerDetails(selectedCusID);  // Already an integer, use it
            //    }
            //}
        }

        // ✅ Declare these at the top inside the frmPOS class


        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == guna2DataGridView2.Columns["dgvDelete"].Index && e.RowIndex >= 0)
            {
                int prodID = Convert.ToInt32(guna2DataGridView2.Rows[e.RowIndex].Cells["dgvid"].Value);

                // ✅ Confirm before deleting
                DialogResult result = MessageBox.Show("Are you sure you want to delete this product?",
                                                      "Delete Confirmation",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    // ✅ Remove from DataGridView
                    guna2DataGridView2.Rows.RemoveAt(e.RowIndex);

                    // ✅ Remove from Database
                    DeleteBillProduct(prodID);

                    // ✅ Update total
                    GetTotal();
                }
            }
        }

        private void DeleteBillProduct(int prodID)
        {
            try
            {
                string qry = "DELETE FROM tblDetails WHERE MainId = @MainId AND prodID = @prodID";

                using (SqlCommand cmd = new SqlCommand(qry, MainClass.con))
                {
                    cmd.Parameters.AddWithValue("@MainId", mainID);
                    cmd.Parameters.AddWithValue("@prodID", prodID);

                    if (MainClass.con.State != ConnectionState.Open)
                        MainClass.con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting product from bill: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (MainClass.con.State == ConnectionState.Open)
                    MainClass.con.Close();
            }
        }

        private void btnSalesman_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate if a technician is selected
                //if (cbTechnician.SelectedIndex == -1 || string.IsNullOrEmpty(cbTechnician.Text))
                //{
                //    MessageBox.Show("Please select a technician.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                // Validate total amount
                if (!double.TryParse(lblTotal.Text.Replace("Rs.", "").Trim(), out double total) || total <= 0)
                {
                    MessageBox.Show("Invalid total amount.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ Insert into saleMain with "Pending" Status
                //string technicianName = cbTechnician.Text.Trim();
                double received = 0;
                double change = 0;
                string saleQry = @"
                                INSERT INTO saleMain (aDate, aTime, Technician, total, received, change, status)
                                VALUES (@aDate, @aTime, @Technician, @total, @received, @change, 'Pending');
                                SELECT SCOPE_IDENTITY();"; // Retrieve new SaleID

                int saleID = 0;
                using (SqlCommand cmd = new SqlCommand(saleQry, MainClass.con))
                {
                    cmd.Parameters.AddWithValue("@aDate", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToString("HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@Technician", loggedInStaffName);
                    cmd.Parameters.AddWithValue("@total", total);
                    cmd.Parameters.AddWithValue("@received", received);
                    cmd.Parameters.AddWithValue("@change", change);

                    if (MainClass.con.State != ConnectionState.Open)
                        MainClass.con.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        saleID = Convert.ToInt32(result);
                }

                // ✅ Insert products into saleDetails
                if (saleID > 0)
                {
                    foreach (DataGridViewRow row in guna2DataGridView2.Rows)
                    {
                        if (row.Cells["dgvid"].Value != null &&
                            row.Cells["dgvQty"].Value != null &&
                            row.Cells["dgvPrice"].Value != null &&
                            row.Cells["dgvAmount"].Value != null)
                        {
                            if (int.TryParse(row.Cells["dgvid"].Value.ToString(), out int prodID) &&
                                int.TryParse(row.Cells["dgvQty"].Value.ToString(), out int qty) &&
                                double.TryParse(row.Cells["dgvPrice"].Value.ToString(), out double price) &&
                                double.TryParse(row.Cells["dgvAmount"].Value.ToString(), out double amount))
                            {
                                string detailQry = @"
                                                    INSERT INTO saleDetails (SaleID, prodID, qty, SellingPrice, amount)
                                                    VALUES (@SaleID, @prodID, @qty, @SellingPrice, @amount)";

                                using (SqlCommand cmd = new SqlCommand(detailQry, MainClass.con))
                                {
                                    cmd.Parameters.AddWithValue("@SaleID", saleID);
                                    cmd.Parameters.AddWithValue("@prodID", prodID);
                                    cmd.Parameters.AddWithValue("@qty", qty);
                                    cmd.Parameters.AddWithValue("@SellingPrice", price);
                                    cmd.Parameters.AddWithValue("@amount", amount);

                                    if (MainClass.con.State != ConnectionState.Open)
                                        MainClass.con.Open();

                                    cmd.ExecuteNonQuery();
                                }

                                // ✅ Update Product Stock
                                string updateStockQry = "UPDATE product SET pStock = pStock - @qty WHERE pID = @prodID";
                                using (SqlCommand cmd = new SqlCommand(updateStockQry, MainClass.con))
                                {
                                    cmd.Parameters.AddWithValue("@qty", qty);
                                    cmd.Parameters.AddWithValue("@prodID", prodID);

                                    if (MainClass.con.State != ConnectionState.Open)
                                        MainClass.con.Open();

                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    

                    MessageBox.Show("Bill successfully sent to sales!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ✅ Reset Fields
                    guna2DataGridView2.Rows.Clear();
                    lblTotal.Text = "Rs. 0.00";
                    //cbTechnician.SelectedIndex = -1;
                    MainID = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending bill to sales: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (MainClass.con.State != ConnectionState.Closed)
                    MainClass.con.Close();
            }

        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            panelCustomer.Visible = !panelCustomer.Visible;
            LoadCustomerData();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadCustomerData();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void InitializeCustomerDataGridView()
        {
            guna2DataGridView1.Columns.Clear(); // Clear existing columns

            // Add required columns
            guna2DataGridView1.Columns.Add("dgvSelect", "Select"); // Column for selection
            guna2DataGridView1.Columns.Add("cusID", "Customer ID");
            guna2DataGridView1.Columns.Add("dgvName", "Name");
            guna2DataGridView1.Columns.Add("dgvEmail", "Email");
            guna2DataGridView1.Columns.Add("dgvPhone", "Phone");
            guna2DataGridView1.Columns.Add("dgvVehicleType", "Vehicle Type");
            guna2DataGridView1.Columns.Add("dgvVehicleNo", "Vehicle No");
            guna2DataGridView1.Columns.Add("appointmentDate", "Appointment Date");

            // Set properties for columns if needed
            guna2DataGridView1.Columns["cusID"].Visible = false; // Hide ID column if not needed
        }
        private void LoadCustomerData()
        {
            try
            {
                string qry = @"
                            SELECT cusID, cusName, cusPhone, cusEmail, cusVehicleType, cusVehicleNo, 
                                   appointmentDate, createdDate 
                            FROM customer 
                            WHERE cusName LIKE '%' + @SearchText + '%'";

                // Add date filter
                if (dateTimePicker1.Value.Date != DateTime.Now.Date) // Check if the selected date is not today
                {
                    qry += " AND CAST(appointmentDate AS DATE) = @AppointmentDate"; // Use CAST to compare only the date part
                }

                Hashtable ht = new Hashtable
                {
                    { "@SearchText", txtSearch.Text.Trim() },
                    { "@AppointmentDate", dateTimePicker1.Value.Date } // Add the selected date
                };

                DataTable dt = MainClass.GetData(qry, ht);

                // Clear DataGridView Before Adding Data
                guna2DataGridView1.Rows.Clear();

                // Load Data if Available
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        // Convert DateTime safely
                        string appointmentDate = row["appointmentDate"] != DBNull.Value && DateTime.TryParse(row["appointmentDate"].ToString(), out DateTime apptDate)
                            ? apptDate.ToString("yyyy-MM-dd")
                            : "N/A";

                        guna2DataGridView1.Rows.Add(
                            guna2DataGridView1.Rows.Count + 1, // Serial No.
                            row["cusID"]?.ToString() ?? "N/A",
                            row["cusName"]?.ToString() ?? "N/A",
                            row["cusEmail"]?.ToString() ?? "N/A",
                            row["cusPhone"]?.ToString() ?? "N/A",
                            row["cusVehicleType"]?.ToString() ?? "N/A",
                            row["cusVehicleNo"]?.ToString() ?? "N/A",
                            appointmentDate
                        );
                    }
                }
                guna2DataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customer data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string selectedCustomerName = "";
        private string selectedCustomerPhone = "";
        private string selectedCustomerEmail = "";
        private string selectedCustomerVehicleType = "";
        private string selectedCustomerVehicleNo = "";
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && guna2DataGridView1.Columns[e.ColumnIndex].Name == "dgvSelect")
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                selectedCustomerName = row.Cells["dgvName"].Value?.ToString() ?? "N/A";
                selectedCustomerPhone = row.Cells["dgvPhone"].Value?.ToString() ?? "N/A";
                selectedCustomerEmail = row.Cells["dgvEmail"].Value?.ToString() ?? "N/A";
                selectedCustomerVehicleType = row.Cells["dgvVehicleType"].Value?.ToString() ?? "N/A";
                selectedCustomerVehicleNo = row.Cells["dgvVehicleNo"].Value?.ToString() ?? "N/A";

                // ✅ Start Print Process
                //PrintDocument printDoc = new PrintDocument();
                //printDoc.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);

                //PrintDialog printDialog = new PrintDialog
                //{
                //    Document = printDoc
                //};

                //if (printDialog.ShowDialog() == DialogResult.OK)
                //{
                //    printDoc.Print();
                //}
            }
        }

        
    }
}
