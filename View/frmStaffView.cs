using newfinalSSS.Model;
using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;

namespace newfinalSSS.View
{
    public partial class frmStaffView : SampleView
    {
        public frmStaffView()
        {
            InitializeComponent();
            LoadData();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void LoadData()
        {
            string qry = "SELECT UserID, Name, Phone, Role, NIC, StaffID, email FROM [user] " +
                         "WHERE Name LIKE @Search OR StaffID LIKE @Search";

            Hashtable ht = new Hashtable { { "@Search", "%" + txtSearch.Text + "%" } };

            DataTable dt = MainClass.GetData(qry, ht); // ✅ Now works correctly

            if (dt.Rows.Count > 0)
            {
                guna2DataGridView1.Rows.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    guna2DataGridView1.Rows.Add(
                        i + 1, // dgvSno
                        dt.Rows[i]["UserID"].ToString(), // dgvid
                        dt.Rows[i]["Name"].ToString(), // dgvName
                        dt.Rows[i]["Phone"].ToString(),
                        dt.Rows[i]["email"].ToString(), // dgvPhone
                        dt.Rows[i]["Role"].ToString(), // dgvRole
                        dt.Rows[i]["NIC"].ToString(), // dgvNIC
                        dt.Rows[i]["StaffID"].ToString() // dgvStaffID
                    );
                }
            }
        }



        private void frmStaffView_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        public override void btnAdd_Click(object sender, EventArgs e)
        {
            frmStaffAdd fr = new frmStaffAdd();
            fr.ShowDialog();
            LoadData();
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell != null && guna2DataGridView1.CurrentCell.OwningColumn != null)
            {
                if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
                {
                    if (guna2DataGridView1.CurrentRow != null)
                    {
                        DataGridViewRow row = guna2DataGridView1.CurrentRow;

                        int id = Convert.ToInt32(row.Cells["dgvid"].Value);
                        string name = row.Cells["dgvName"].Value.ToString();
                        string phone = row.Cells["dgvPhone"].Value.ToString();
                        string email = row.Cells["dgvEmail"].Value.ToString();
                        string role = row.Cells["dgvRole"].Value.ToString();
                        string nic = row.Cells["dgvNIC"].Value.ToString();
                        string staffID = row.Cells["dgvStaffID"].Value.ToString();

                        frmStaffAdd frm = new frmStaffAdd();
                        frm.id = id;
                        frm.txtsName.Text = name;
                        frm.txtsPhone.Text = phone;
                        frm.cbRole.Text = role;
                        frm.txtsNIC.Text = nic;
                        frm.txtEmail.Text = email;

                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            LoadData();
                        }
                    }
                }
            }

            if (guna2DataGridView1.CurrentCell != null && guna2DataGridView1.CurrentCell.OwningColumn != null)
            {
                if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
                {
                    if (guna2DataGridView1.CurrentRow != null)
                    {
                        DataGridViewRow row = guna2DataGridView1.CurrentRow;
                        int id = Convert.ToInt32(row.Cells["dgvid"].Value);

                        DialogResult result = MessageBox.Show("Are you sure you want to delete this staff member?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            string qry = "DELETE FROM [user] WHERE UserID = @UserID";
                            Hashtable ht = new Hashtable { { "@UserID", id } };

                            if (MainClass.SQL(qry, ht) > 0)
                            {
                                MessageBox.Show("Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Error deleting record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }
    }
}
