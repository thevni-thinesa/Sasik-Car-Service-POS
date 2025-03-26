using newfinalSSS.Model;
using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;

namespace newfinalSSS.View
{
    public partial class frmCustomerView : SampleView
    {
        public frmCustomerView()
        {
            InitializeComponent();
            LoadCustomerData();
        }

        // ✅ Load Customer Data into DataGridView
        private void LoadCustomerData()
        {
            try
            {
                string qry = @"
                    SELECT cusID, cusName, cusPhone, cusEmail, cusVehicleType, cusVehicleNo, 
                           appointmentDate, createdDate 
                    FROM customer 
                    WHERE cusName LIKE @SearchText";

                Hashtable ht = new Hashtable
                {
                    { "@SearchText", "%" + txtSearch.Text.Trim() + "%" }
                };

                DataTable dt = MainClass.GetData(qry, ht);

                // ✅ Clear DataGridView Before Adding Data
                guna2DataGridView1.Rows.Clear();

                // ✅ Load Data if Available
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        guna2DataGridView1.Rows.Add(
                            guna2DataGridView1.Rows.Count + 1, // Serial No.
                            row["cusID"].ToString(),
                            row["appointmentDate"] != DBNull.Value
                                ? Convert.ToDateTime(row["appointmentDate"]).ToString("yyyy-MM-dd")
                                : "N/A",
                            row["createdDate"] != DBNull.Value
                                ? Convert.ToDateTime(row["createdDate"]).ToString("yyyy-MM-dd HH:mm")
                                : "N/A",
                            row["cusName"].ToString(),
                            row["cusPhone"].ToString(),
                            row["cusEmail"].ToString(),
                            row["cusVehicleType"].ToString(),
                            row["cusVehicleNo"].ToString()
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

        private void frmCustomerView_Load(object sender, EventArgs e)
        {
            LoadCustomerData();
        }

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomerData();
        }

        public override void btnAdd_Click(object sender, EventArgs e)
        {
            using (frmCustomerAdd fr = new frmCustomerAdd())
            {
                fr.ShowDialog();
                LoadCustomerData();
            }
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = guna2DataGridView1.Columns[e.ColumnIndex].Name;

                // ✅ Handle Edit Action
                if (columnName == "dgvedit")
                {
                    DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                    int id = Convert.ToInt32(row.Cells["dgvid"].Value);
                    string name = row.Cells["dgvName"].Value?.ToString() ?? "";
                    string phone = row.Cells["dgvphone"].Value?.ToString() ?? "";
                    string email = row.Cells["dgvemail"].Value?.ToString() ?? "";
                    string vehicleType = row.Cells["dgvvehicletype"].Value?.ToString() ?? "";
                    string vehicleNo = row.Cells["dgvvehicleno"].Value?.ToString() ?? "";

                    // ✅ Ensure Column Exists Before Accessing
                    DateTime? appointment = null;
                    if (guna2DataGridView1.Columns.Contains("dgvAppointment") && row.Cells["dgvAppointment"].Value != DBNull.Value)
                    {
                        appointment = Convert.ToDateTime(row.Cells["dgvAppointment"].Value);
                    }

                    using (frmCustomerAdd frm = new frmCustomerAdd())
                    {
                        frm.id = id;
                        frm.txtcusName.Text = name;
                        frm.txtcusPhone.Text = phone;
                        frm.txtcusEmail.Text = email;
                        frm.cbVehicleType.Text = vehicleType;
                        frm.txtcusVehicleNo.Text = vehicleNo;

                        // ✅ Set Appointment Date Correctly
                        if (appointment.HasValue)
                        {
                            frm.dateTimePicker1.Value = appointment.Value;
                        }
                        else
                        {
                            frm.dateTimePicker1.CustomFormat = " "; // Hide date if NULL
                        }

                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            LoadCustomerData();
                        }
                    }
                }
                guna2DataGridView1.Refresh();

                // ✅ Handle Delete Action
                if (columnName == "dgvdel")
                {
                    int id = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["dgvid"].Value);

                    DialogResult result = MessageBox.Show("Are you sure you want to delete this customer?",
                                                          "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        string qry = "DELETE FROM customer WHERE cusID = @cusID";
                        Hashtable ht = new Hashtable { { "@cusID", id } };

                        if (MainClass.SQL(qry, ht) > 0)
                        {
                            MessageBox.Show("Customer deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadCustomerData();
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
