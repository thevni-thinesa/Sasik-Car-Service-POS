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
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace newfinalSSS.Model
{
    public partial class frmBillList : SampleAdd
    {
        public frmBillList()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public int MainID = 0;
        private void LoadData()
        {
            try
            {
                string qry = @"SELECT MainID, 
                              COALESCE(status, 'N/A') AS status, 
                              COALESCE(TechnicianName, 'Unknown') AS TechnicianName, 
                              CAST(COALESCE(total, 0) AS DECIMAL(10,2)) AS total 
                       FROM tblMain 
                       WHERE LOWER(status) = 'pending'";

                DataTable dt = MainClass.GetData(qry);

                // Ensure data exists before adding to the DataGridView 
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No pending bills found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Clear existing rows before loading new data
                guna2DataGridView1.Rows.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    guna2DataGridView1.Rows.Add(
                        i + 1,  // Serial Number (dgvSno)
                        dt.Rows[i]["MainID"].ToString(),  // Main ID (dgvid)
                        dt.Rows[i]["status"].ToString(),  // Status (dgvStatus)
                        dt.Rows[i]["TechnicianName"].ToString(),  // Technician Name (dgvTechnician)
                        dt.Rows[i]["total"].ToString()  // Total Amount (dgvTot)
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading pending bills: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string SelectedTechnician { get; private set; } = "";



        private void frmBillList_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string columnName = guna2DataGridView1.Columns[e.ColumnIndex].Name;
                if (columnName == "dgvedit") // Ensure we're editing the bill
                {
                    MainID = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["dgvid"].Value);
                    SelectedTechnician = guna2DataGridView1.Rows[e.RowIndex].Cells["dgvTechnician"].Value.ToString(); // ✅ Get Technician Name
                    this.Close();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
