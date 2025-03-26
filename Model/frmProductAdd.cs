using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace newfinalSSS.Model
{
    public partial class frmProductAdd : SampleAdd
    {
        public frmProductAdd()
        {
            InitializeComponent();
        }

        public int id = 0;
        public int cID = 0;

        string filepath;
        Byte[] imageByteArray;


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Filter = "Image Files|*.jpg;*.png;*.jpeg",
                    Title = "Select Product Image"
                };

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    filepath = ofd.FileName;

                    if (!string.IsNullOrWhiteSpace(filepath) && File.Exists(filepath))
                    {
                        txtImage.Image = new Bitmap(filepath);
                    }
                    else
                    {
                        MessageBox.Show("Error: Invalid file selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmProductAdd_Load(object sender, EventArgs e)
        {
            try
            {
                LoadCategories(cID); // ✅ Load categories before setting the selected value

                if (id > 0)
                {
                    ForUpdateLoaddata();
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = id == 0
                    ? "INSERT INTO product (pName, pPrice, categoryID, pImage, pStock, pStockPrice) VALUES (@Name, @price, @cat, @img, @stock, @netPrice)"
                    : "UPDATE product SET pName = @Name, pPrice = @price, categoryID = @cat, pImage = @img, pStock = @stock, pStockPrice = @netPrice WHERE pID = @id";

                // ✅ Convert Image to Byte Array
                byte[] imageByteArray = null;
                if (txtImage.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        txtImage.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        imageByteArray = ms.ToArray();
                    }
                }

                // ✅ Convert and Validate Price and Stock
                decimal netPrice, calculatedPrice;
                int stock;

                string netPriceText = txtpnetprice.Text.Replace("Rs.", "").Trim();
                string stockText = txtpStock.Text.Trim();

                if (!decimal.TryParse(netPriceText, out netPrice) || netPrice < 0)
                {
                    MessageBox.Show("Invalid net price format. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(stockText, out stock) || stock < 0)
                {
                    MessageBox.Show("Invalid stock value. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ Calculate Final Price (pPrice) = netPrice + 10% Markup
                calculatedPrice = netPrice + (netPrice * 0.10m);
                //lblCalculatedPrice.Text = "Rs. " + calculatedPrice.ToString("N2"); // ✅ Update Label in UI

                // ✅ Create Parameters
                Hashtable ht = new Hashtable
                    {
                        { "@id", id },
                        { "@Name", txtpName.Text },
                        { "@price", calculatedPrice }, // ✅ Store Calculated Price (pPrice)
                        { "@cat", Convert.ToInt32(cbCat.SelectedValue) },
                        { "@img", imageByteArray ?? new byte[0] }, // Handle null images
                        { "@stock", stock },
                        { "@netPrice", netPrice }
                    };

                // ✅ Execute Query
                if (MainClass.SQL(qry, ht) > 0)
                {
                    MessageBox.Show("Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ✅ Reset Form Fields
                    id = 0;
                    cID = 0;
                    txtpName.Text = "";
                    txtpStock.Text = "";
                    txtpnetprice.Text = "";
                    //lblCalculatedPrice.Text = ""; // ✅ Clear Price Label
                    cbCat.SelectedIndex = 0;
                    txtImage.Image = null; // Reset image

                    txtpName.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }



        private void ForUpdateLoaddata()
        {
            try
            {
                string qry = "SELECT pName, pPrice, categoryID, pImage, pStock, pStockPrice FROM product WHERE pID = @id";

                using (SqlCommand cmd = new SqlCommand(qry, MainClass.con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            txtpName.Text = dt.Rows[0]["pName"].ToString();

                            // ✅ Get pPrice from DB
                            decimal pStockPrice = Convert.ToDecimal(dt.Rows[0]["pStockPrice"]);

                            // ✅ Apply 10% increase to pPrice
                            decimal calculatedPrice = pStockPrice + (pStockPrice * 0.10m);

                            // ✅ Show Net Price (User Input) and Calculated Price
                            txtpnetprice.Text =  pStockPrice.ToString("F2"); // Actual price
                            //lblCalculatedPrice.Text = "Rs. " + calculatedPrice.ToString("N2"); // Display calculated price (10% increase)

                            // ✅ Load Stock Quantity
                            txtpStock.Text = dt.Rows[0]["pStock"] != DBNull.Value ? dt.Rows[0]["pStock"].ToString() : "0";

                            // ✅ Load Category Properly
                            if (dt.Rows[0]["categoryID"] != DBNull.Value)
                            {
                                cID = Convert.ToInt32(dt.Rows[0]["categoryID"]);
                                cbCat.SelectedValue = cID;
                            }
                            else
                            {
                                cbCat.SelectedIndex = -1;
                            }

                            // ✅ Load Image from Database
                            if (dt.Rows[0]["pImage"] != DBNull.Value)
                            {
                                byte[] imageArray = (byte[])dt.Rows[0]["pImage"];
                                using (MemoryStream ms = new MemoryStream(imageArray))
                                {
                                    txtImage.Image = Image.FromStream(ms);
                                }
                            }
                            else
                            {
                                txtImage.Image = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading product data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public void LoadCategories(int? selectedCategoryID = null)
        {
            try
            {
                string qry = "SELECT catID, catName FROM category";
                DataTable dt = new DataTable();

                using (SqlCommand cmd = new SqlCommand(qry, MainClass.con))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    cbCat.DataSource = dt;
                    cbCat.DisplayMember = "catName";
                    cbCat.ValueMember = "catID";
                    cbCat.SelectedIndex = -1; // ✅ Prevent binding errors

                    if (selectedCategoryID.HasValue)
                    {
                        int catIdToSelect = dt.AsEnumerable()
                            .Where(row => row.Field<int>("catID") == selectedCategoryID.Value)
                            .Select(row => row.Field<int>("catID"))
                            .FirstOrDefault();

                        if (catIdToSelect != 0) // Ensure category exists
                        {
                            cbCat.SelectedValue = catIdToSelect;
                        }
                    }
                }
                else
                {
                    cbCat.DataSource = null; // ✅ Prevent binding errors when no categories exist
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
