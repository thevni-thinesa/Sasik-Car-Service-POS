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

namespace newfinalSSS.Model
{
    public partial class frmCustomerAdd : SampleAdd
    {
        public frmCustomerAdd()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        public int id = 0;
        public override void btnSave_Click(object sender, EventArgs e)
        {
            string qry = "";

            if (id == 0) // ✅ Insert new record
            {
                qry = "INSERT INTO customer (cusName, cusPhone, cusEmail, cusVehicleType, cusVehicleNo, appointmentDate, createdDate) " +
                      "VALUES (@Name, @Phone, @Email, @Type, @VehicleNo, @appointmentDate, @createdDate)";
            }
            else // ✅ Update existing record
            {
                qry = "UPDATE customer " +
                      "SET cusName = @Name, cusPhone = @Phone, cusEmail = @Email, " +
                      "cusVehicleType = @Type, cusVehicleNo = @VehicleNo, " +
                      "appointmentDate = @appointmentDate, createdDate = @createdDate " +
                      "WHERE cusID = @id";
            }

            Hashtable ht = new Hashtable
            {
                { "@id", id },
                { "@Name", txtcusName.Text },
                { "@Phone", txtcusPhone.Text },
                { "@Email", txtcusEmail.Text },
                { "@Type", cbVehicleType.Text },
                { "@VehicleNo", txtcusVehicleNo.Text },
                { "@appointmentDate", dateTimePicker1.Value }, // ✅ Store appointment date
                { "@createdDate", DateTime.Now } // ✅ Automatically set current date/time
            };

            if (MainClass.SQL(qry, ht) > 0)
            {
                MessageBox.Show("Saved Successfully");

                // ✅ Reset Fields
                id = 0;
                txtcusName.Text = "";
                txtcusPhone.Text = "";
                txtcusEmail.Text = "";
                cbVehicleType.SelectedIndex = -1;
                txtcusVehicleNo.Text = "";
                dateTimePicker1.Value = DateTime.Now; 
                txtcusName.Focus();
            }
        }
    }
}
