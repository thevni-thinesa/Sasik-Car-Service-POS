using Guna.UI2.WinForms;
using newfinalSSS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace newfinalSSS.View
{
    public partial class frmCategoryView : SampleView
    {
        public frmCategoryView()
        {
            InitializeComponent();
        }
        public void LoadData()
        {
            string qry = "SELECT catID, catName FROM category WHERE catName LIKE '%" + txtSearch.Text + "%'";
            DataTable dt = MainClass.GetData(qry);

            // Ensure the DataGridView is cleared only when performing a new search
            guna2DataGridView1.Rows.Clear();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                guna2DataGridView1.Rows.Add(
                    i + 1,  // Serial Number
                    dt.Rows[i]["catID"].ToString(),
                    dt.Rows[i]["catName"].ToString()
                );
            }
        }


        //private void InitializeDataGridView()
        //{
        //    //guna2DataGridView1.Columns.Clear(); // Clear any previous columns

        //    // Adding normal text columns
        //    guna2DataGridView1.Columns.Add("dgvSno", "Sr#."); // Serial number
        //    guna2DataGridView1.Columns.Add("catID", "Category ID");
        //    guna2DataGridView1.Columns.Add("catName", "Category Name");

        //    // ✅ Hide ID column if not needed
        //    guna2DataGridView1.Columns["catID"].Visible = false;

        //    // ✅ Set column widths for better UI
        //    guna2DataGridView1.Columns["dgvSno"].Width = 70;

        //    // ✅ Create "Edit" image column
        //    DataGridViewImageColumn editColumn = new DataGridViewImageColumn();
        //    editColumn.Name = "dgvEdit";
        //    editColumn.HeaderText = "Edit";
        //    editColumn.Image = Properties.Resources.delete__2_; // Load from project resources
        //    editColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        //    editColumn.Width = 50;

        //    // ✅ Create "Delete" image column
        //    DataGridViewImageColumn deleteColumn = new DataGridViewImageColumn();
        //    deleteColumn.Name = "dgvDelete";
        //    deleteColumn.HeaderText = "Delete";
        //    deleteColumn.Image = Properties.Resources.delete__1_; // Load from project resources
        //    deleteColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        //    deleteColumn.Width = 50;

        //    // ✅ Add image columns to the DataGridView
        //    guna2DataGridView1.Columns.Add(editColumn);
        //    guna2DataGridView1.Columns.Add(deleteColumn);
        //}



        private void frmCategoryView_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        public override void btnAdd_Click(object sender, EventArgs e)
        {
            frmCategoryAdd f = new frmCategoryAdd();
            f.ShowDialog();
            LoadData();
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string columnName = guna2DataGridView1.Columns[e.ColumnIndex].Name;
                if (columnName == "dgvEdit")
                {
                    int id = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["dgvid"].Value);
                    string name = guna2DataGridView1.Rows[e.RowIndex].Cells["dgvName"].Value.ToString();

                    frmCategoryAdd frm = new frmCategoryAdd();
                    frm.id = id;
                    frm.txtName.Text = name;

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
                else if (columnName == "dgvDelete")
                {
                    int id = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["dgvid"].Value);
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this category?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        string qry = "DELETE FROM category WHERE catID = @catID";
                        Hashtable ht = new Hashtable { { "@catID", id } };
                        if (MainClass.SQL(qry, ht) > 0)
                        {
                            MessageBox.Show("Category Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Error deleting category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

        }
    }
}
