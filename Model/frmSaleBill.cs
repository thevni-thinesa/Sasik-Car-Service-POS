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
    public partial class frmSaleBill : SampleAdd
    {
        public frmSaleBill()
        {
            InitializeComponent();
            LoadData();
        }

        
        private void LoadData()
        {
            try
            {
                string qry = @"
                        SELECT SaleID, 
                               COALESCE(status, 'Pending') AS status, 
                               COALESCE(Technician, 'Unknown') AS Technician, 
                               CAST(COALESCE(total, 0) AS DECIMAL(10,2)) AS total 
                        FROM saleMain
                        WHERE LOWER(status) = 'pending'";


                DataTable dt = MainClass.GetData(qry);

                // Ensure data exists before adding to the DataGridView 
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No pending sales orders found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Clear existing rows before loading new data
                guna2DataGridView1.Rows.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    guna2DataGridView1.Rows.Add(
                        i + 1,  // Serial Number (dgvSno)
                        dt.Rows[i]["SaleID"].ToString(),  // Sale ID (dgvid)
                        dt.Rows[i]["status"].ToString(),  // Sale Status (dgvSaleStatus)
                        dt.Rows[i]["Technician"].ToString(),  // Technician Name (dgvTechnician)
                        dt.Rows[i]["total"].ToString()  // Total Amount (dgvTot)
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading pending sales orders: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public int SaleID = 0;
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string columnName = guna2DataGridView1.Columns[e.ColumnIndex].Name;
                if (columnName == "dgvedit") // Ensure we're editing the bill
                {
                    SaleID = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["dgvSaleID"].Value);
                    //SelectedTechnician = guna2DataGridView1.Rows[e.RowIndex].Cells["dgvTechnician"].Value.ToString(); // ✅ Get Technician Name
                    this.Close();
                }
            }

        }
    }
}
