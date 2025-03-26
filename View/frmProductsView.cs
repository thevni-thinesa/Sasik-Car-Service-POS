using newfinalSSS.Model;
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

namespace newfinalSSS.View
{
    public partial class frmProductsView : SampleView
    {
        public frmProductsView()
        {
            InitializeComponent();
        }

        public void LoadData()
        {
            string qry = @"SELECT p.pID, p.pName, p.pPrice, p.categoryID, c.catName, p.pImage, p.pStock, p.pStockPrice 
               FROM product p 
               INNER JOIN category c ON p.categoryID = c.catID
               WHERE p.pName LIKE @SearchText";

            Hashtable ht = new Hashtable();
            ht.Add("@SearchText", "%" + txtSearch.Text.Trim() + "%"); // ✅ Secure search query

            DataTable dt = MainClass.GetData(qry, ht); // ✅ Using parameterized query

            guna2DataGridView1.Rows.Clear();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                guna2DataGridView1.Rows.Add(
                    i + 1, // Serial Number
                    dt.Rows[i]["pID"].ToString(),
                    dt.Rows[i]["pName"].ToString(),
                    dt.Rows[i]["catName"].ToString(),
                    dt.Rows[i]["categoryID"].ToString(),
                    dt.Rows[i]["pStock"].ToString(),
                    dt.Rows[i]["pStockPrice"].ToString(),
                    dt.Rows[i]["pPrice"].ToString()
                    //dt.Rows[i]["pImage"].ToString()
                );
            }


        }
        private void frmProductsView_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        public override void btnAdd_Click(object sender, EventArgs e)
        {
            frmProductAdd f = new frmProductAdd();
            f.ShowDialog();
            LoadData();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = guna2DataGridView1.Columns[e.ColumnIndex].Name;

                if (columnName == "dgvedit")
                {
                    try
                    {
                        // ✅ Safely Retrieve ID
                        if (!int.TryParse(guna2DataGridView1.Rows[e.RowIndex].Cells["dgvid"]?.Value?.ToString(), out int id))
                        {
                            MessageBox.Show("Invalid Product ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string name = guna2DataGridView1.Rows[e.RowIndex].Cells["dgvName"]?.Value?.ToString() ?? "";

                        // ✅ Safe Parsing for Integers
                        int categoryId = 0;
                        int.TryParse(guna2DataGridView1.Rows[e.RowIndex].Cells["dgvcatid"]?.Value?.ToString(), out categoryId);

                        int stock = 0;
                        int.TryParse(guna2DataGridView1.Rows[e.RowIndex].Cells["dgvStock"]?.Value?.ToString(), out stock);

                        // ✅ Retrieve Prices (pPrice & pStockPrice) from Database
                        double pPrice = 0;
                        float pStockPrice = 0;
                        Image productImage = null;
                        byte[] imageBytes = null;

                        string query = "SELECT pPrice, pStockPrice, pImage FROM product WHERE pID = @id";

                        using (SqlCommand cmd = new SqlCommand(query, MainClass.con))
                        {
                            cmd.Parameters.AddWithValue("@id", id);

                            if (MainClass.con.State != ConnectionState.Open)
                                MainClass.con.Open();

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // ✅ Safe Parsing for Floating-Point Values
                                    if (!double.TryParse(reader["pPrice"]?.ToString(), out pPrice))
                                        pPrice = 0;

                                    if (!float.TryParse(reader["pStockPrice"]?.ToString(), out pStockPrice))
                                        pStockPrice = 0;

                                    if (reader["pImage"] != DBNull.Value)
                                    {
                                        imageBytes = (byte[])reader["pImage"];
                                        using (MemoryStream ms = new MemoryStream(imageBytes))
                                        {
                                            productImage = Image.FromStream(ms);
                                        }
                                    }
                                }
                            }
                        }

                        // ✅ Open Edit Form with Data
                        frmProductAdd frm = new frmProductAdd
                        {
                            id = id
                        };
                        frm.txtpName.Text = name;
                        frm.txtpnetprice.Text = pStockPrice.ToString("F2"); // ✅ Display with 2 decimal places
                        frm.txtpStock.Text = stock.ToString();
                        frm.LoadCategories(categoryId);

                        if (productImage != null)
                        {
                            frm.txtImage.Image = productImage;
                        }

                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            LoadData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading product details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (columnName == "dgvdel")
                {
                    try
                    {
                        if (!int.TryParse(guna2DataGridView1.Rows[e.RowIndex].Cells["dgvid"]?.Value?.ToString(), out int id))
                        {
                            MessageBox.Show("Invalid Product ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        DialogResult result = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            string qry = "DELETE FROM product WHERE pID = @pID";
                            Hashtable ht = new Hashtable { { "@pID", id } };

                            if (MainClass.SQL(qry, ht) > 0)
                            {
                                MessageBox.Show("Product Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Error deleting product.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

    }
}
