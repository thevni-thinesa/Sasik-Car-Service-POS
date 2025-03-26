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

namespace newfinalSSS.Model
{
    public partial class frmPOSsale : Form
    {
        private int SaleID = 0; // ✅ Define SaleID
        private string loggedInStaffName;

        public frmPOSsale()
        {
            InitializeComponent();
            loggedInStaffName = MainClass.USER;
        }

        private void btnBillList_Click(object sender, EventArgs e)
        {
            using (frmSaleBill frm = new frmSaleBill())
            {
                frm.ShowDialog();

                if (frm.SaleID > 0) // ✅ Check if a valid SaleID was selected
                {
                    SaleID = frm.SaleID; // ✅ Assign SaleID from frmSaleBill
                    LoadEntries(); // ✅ Load products for the selected Sale
                }
            }
        }

        private void LoadEntries()
        {
            try
            {
                // ✅ Query to fetch sale details
                string qry = @"
            SELECT d.DetailID, d.prodID, p.pName, d.qty, d.SellingPrice, d.amount 
            FROM saleDetails d
            INNER JOIN product p ON d.prodID = p.pID
            WHERE d.SaleID = @SaleID";

                DataTable dtDetails = new DataTable();

                using (SqlCommand cmd = new SqlCommand(qry, MainClass.con))
                {
                    cmd.Parameters.AddWithValue("@SaleID", SaleID); // ✅ Use SaleID

                    if (MainClass.con.State != ConnectionState.Open)
                        MainClass.con.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtDetails);
                    }
                }

                // ✅ Query to fetch total amount from saleMain
                string totalQry = "SELECT total FROM saleMain WHERE SaleID = @SaleID";

                using (SqlCommand cmd = new SqlCommand(totalQry, MainClass.con))
                {
                    cmd.Parameters.AddWithValue("@SaleID", SaleID);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        lblTotal.Text = "Rs. " + Convert.ToDecimal(result).ToString("N2"); // ✅ Format as currency
                    }
                    else
                    {
                        lblTotal.Text = "Rs. 0.00"; // ✅ Default value
                    }
                }

                // ✅ Check if any data was fetched
                if (dtDetails.Rows.Count == 0)
                {
                    MessageBox.Show("No products found for this sale.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // ✅ Clear DataGridView safely
                guna2DataGridView2.Rows.Clear();

                // ✅ Add data safely from saleDetails
                foreach (DataRow row in dtDetails.Rows)
                {
                    guna2DataGridView2.Rows.Add(
                        guna2DataGridView2.Rows.Count + 1, // Serial number
                        row["prodID"],
                        row["pName"],
                        row["qty"],
                        row["SellingPrice"], // ✅ Use Selling Price instead of price
                        row["amount"]
                    );
                }

                guna2DataGridView2.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading sale products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (MainClass.con.State == ConnectionState.Open)
                    MainClass.con.Close();
            }
        }

        private void txtReceived_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // ✅ Check if Enter key is pressed
            {
                try
                {
                    // ✅ Parse the received amount
                    if (!decimal.TryParse(txtReceived.Text.Trim(), out decimal received) || received < 0)
                    {
                        MessageBox.Show("Invalid received amount. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtReceived.Focus();
                        return;
                    }

                    // ✅ Extract and parse the total amount from lblTotal
                    string totalText = lblTotal.Text.Replace("Rs.", "").Trim();
                    if (!decimal.TryParse(totalText, out decimal total))
                    {
                        MessageBox.Show("Invalid total amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // ✅ Calculate the change
                    decimal change = received - total;

                    // ✅ Display the change in lblChange
                    lblChange.Text = "Rs. " + change.ToString("N2");

                    // ✅ If change is negative, show a warning
                    if (change < 0)
                    {
                        MessageBox.Show("Received amount is less than the total amount!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error calculating change: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ Validate Sale ID
                if (SaleID == 0)
                {
                    MessageBox.Show("No sale selected. Please select a sale first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ Validate the received amount
                if (!decimal.TryParse(txtReceived.Text.Trim(), out decimal received) || received < 0)
                {
                    MessageBox.Show("Invalid received amount. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtReceived.Focus();
                    return;
                }

                // ✅ Extract and parse the total amount from lblTotal
                string totalText = lblTotal.Text.Replace("Rs.", "").Trim();
                if (!decimal.TryParse(totalText, out decimal total))
                {
                    MessageBox.Show("Invalid total amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ Calculate the change
                decimal change = received - total;
                lblChange.Text = "Rs. " + change.ToString("N2");

                // ✅ If received amount is less than total, show a warning
                if (change < 0)
                {
                    MessageBox.Show("Received amount is less than the total amount!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ SQL Query to Update `saleMain` Table
                string updateQuery = @"
                UPDATE saleMain 
                SET received = @received, change = @change, status = 'Completed' 
                WHERE SaleID = @SaleID";

                using (SqlCommand cmd = new SqlCommand(updateQuery, MainClass.con))
                {
                    cmd.Parameters.AddWithValue("@SaleID", SaleID);
                    cmd.Parameters.AddWithValue("@received", received);
                    cmd.Parameters.AddWithValue("@change", change);

                    if (MainClass.con.State != ConnectionState.Open)
                        MainClass.con.Open();

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // ✅ Print Receipt BEFORE resetting SaleID
                        PrintDialog printDialog1 = new PrintDialog
                        {
                            Document = printDocument1
                        };

                        DialogResult result = printDialog1.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            printDocument1.Print();
                        }

                        // ✅ Sale Successfully Completed
                        MessageBox.Show("Sale completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // ✅ Reset Fields AFTER printing
                        txtReceived.Text = "";
                        lblChange.Text = "Rs. 0.00";
                        lblTotal.Text = "Rs. 0.00";
                        guna2DataGridView2.Rows.Clear();
                        SaleID = 0;  // ✅ Reset SaleID only after printing is done
                    }
                    else
                    {
                        MessageBox.Show("Failed to update sale. Try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error completing sale: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (MainClass.con.State == ConnectionState.Open)
                    MainClass.con.Close();
            }
        }


        private int CenterText(string text, Font font, int pageWidth)
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                int textWidth = (int)g.MeasureString(text, font).Width;
                return (pageWidth - textWidth) / 2;
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                // ✅ Fetch Sale Details before printing
                string saleQuery = @"SELECT SaleID, Technician, aDate, aTime, total, received, change 
                             FROM saleMain WHERE SaleID = @SaleID";
                string technicianName = "N/A";
                string saleDate = DateTime.Now.ToShortDateString();
                string saleTime = DateTime.Now.ToShortTimeString();
                decimal total = 0, received = 0, change = 0;

                using (SqlCommand cmd = new SqlCommand(saleQuery, MainClass.con))
                {
                    cmd.Parameters.AddWithValue("@SaleID", SaleID);

                    if (MainClass.con.State != ConnectionState.Open)
                        MainClass.con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            technicianName = reader["Technician"].ToString();
                            saleDate = Convert.ToDateTime(reader["aDate"]).ToShortDateString();
                            saleTime = reader["aTime"].ToString();
                            total = Convert.ToDecimal(reader["total"]);
                            received = Convert.ToDecimal(reader["received"]);
                            change = Convert.ToDecimal(reader["change"]);
                        }
                    }
                }

                int pageWidth = e.PageBounds.Width;
                int startX = 20;
                int startY = 20;

                Font headerFont = new Font("Arial", 20, FontStyle.Bold);
                Font subHeaderFont = new Font("Arial", 14, FontStyle.Bold);
                Font textFont = new Font("Arial", 10, FontStyle.Regular);
                Font totalFont = new Font("Arial", 12, FontStyle.Bold);

                StringFormat rightAlignFormat = new StringFormat { Alignment = StringAlignment.Far };

                // 📌 Print Logo
                try
                {
                    Image logo = Properties.Resources.llgo;
                    if (logo != null)
                    {
                        int logoWidth = 200, logoHeight = 183;
                        int logoX = (pageWidth - logoWidth) / 2;
                        e.Graphics.DrawImage(logo, logoX, startY, logoWidth, logoHeight);
                        startY += logoHeight + 20;
                    }
                }
                catch (Exception) { }

                // 📌 Print Headers
                string title = "Sasik Service Station";
                string subtitle = "Final Bill";
                string billID = $"Sale ID: {SaleID}";

                e.Graphics.DrawString(title, headerFont, Brushes.Black, CenterText(title, headerFont, pageWidth), startY);
                startY += 30;
                e.Graphics.DrawString(subtitle, subHeaderFont, Brushes.Black, CenterText(subtitle, subHeaderFont, pageWidth), startY);
                startY += 30;
                e.Graphics.DrawString(billID, textFont, Brushes.Black, CenterText(billID, textFont, pageWidth), startY);
                startY += 30;

                // 📌 Print Date & Technician Details
                e.Graphics.DrawString($"Date: {saleDate}  Time: {saleTime}", textFont, Brushes.Black, startX, startY);
                startY += 25;
                e.Graphics.DrawString($"Cashier: {loggedInStaffName}", textFont, Brushes.Black, startX, startY);
                startY += 25;
                e.Graphics.DrawString($"Technician: {technicianName}", textFont, Brushes.Black, startX, startY);
                startY += 40;

                // 📌 Print Table Header
                int colItem = startX;
                int colQty = pageWidth / 2 - 160;
                int colPrice = colQty + 140;
                int colTotal = colPrice + 140;

                e.Graphics.DrawString("Item", subHeaderFont, Brushes.Black, colItem, startY);
                e.Graphics.DrawString("Qty", subHeaderFont, Brushes.Black, colQty, startY);
                e.Graphics.DrawString("Price", subHeaderFont, Brushes.Black, colPrice, startY);
                e.Graphics.DrawString("Total", subHeaderFont, Brushes.Black, colTotal, startY);
                startY += 30;
                e.Graphics.DrawLine(Pens.Black, startX, startY, pageWidth - startX, startY);
                startY += 10;

                // 📌 Fetch & Print Sale Products
                string detailsQuery = @"SELECT p.pName, d.qty, d.SellingPrice, d.amount 
                                FROM saleDetails d
                                INNER JOIN product p ON d.prodID = p.pID
                                WHERE d.SaleID = @SaleID";

                using (SqlCommand cmd = new SqlCommand(detailsQuery, MainClass.con))
                {
                    cmd.Parameters.AddWithValue("@SaleID", SaleID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string itemName = reader["pName"].ToString();
                            int quantity = Convert.ToInt32(reader["qty"]);
                            decimal price = Convert.ToDecimal(reader["SellingPrice"]);
                            decimal amount = Convert.ToDecimal(reader["amount"]);

                            string priceText = "Rs. " + price.ToString("N2");
                            string amountText = "Rs. " + amount.ToString("N2");

                            e.Graphics.DrawString(itemName, textFont, Brushes.Black, colItem, startY);
                            e.Graphics.DrawString(quantity.ToString(), textFont, Brushes.Black, colQty, startY);
                            e.Graphics.DrawString(priceText, textFont, Brushes.Black, new Rectangle(colPrice - 50, startY, 100, 20), rightAlignFormat);
                            e.Graphics.DrawString(amountText, textFont, Brushes.Black, new Rectangle(colTotal - 50, startY, 100, 20), rightAlignFormat);

                            startY += 25;
                        }
                    }
                }

                // 📌 Print Total, Received & Change
                startY += 10;
                e.Graphics.DrawLine(Pens.Black, startX, startY, pageWidth - startX, startY);
                startY += 10;

                e.Graphics.DrawString("Total:", totalFont, Brushes.Black, new Rectangle(colPrice - 50, startY, 100, 20), rightAlignFormat);
                e.Graphics.DrawString("Rs. " + total.ToString("N2"), totalFont, Brushes.Black, new Rectangle(colTotal - 50, startY, 140, 20), rightAlignFormat);
                startY += 25;

                e.Graphics.DrawString("Received:", totalFont, Brushes.Black, new Rectangle(colPrice - 50, startY, 100, 20), rightAlignFormat);
                e.Graphics.DrawString("Rs. " + received.ToString("N2"), totalFont, Brushes.Black, new Rectangle(colTotal - 50, startY, 140, 20), rightAlignFormat);
                startY += 25;

                e.Graphics.DrawString("Change:", totalFont, Brushes.Black, new Rectangle(colPrice - 50, startY, 100, 20), rightAlignFormat);
                e.Graphics.DrawString("Rs. " + change.ToString("N2"), totalFont, Brushes.Black, new Rectangle(colTotal - 50, startY, 140, 20), rightAlignFormat);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error printing final bill: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
