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

namespace newfinalSSS
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string staffID = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role;
            string userID; // To hold the StaffID

            
           

            // ✅ Call IsValidUser () using MainClass
            if (MainClass.IsValidUser(staffID, password, out role, out userID))
            {
               

                // ✅ Redirect based on Role
                switch (role.ToLower())
                {
                    case "manager":
                        this.Hide();
                        frmMain mainForm = new frmMain();
                        mainForm.Show();
                        break;

                    case "technician":
                        this.Hide();
                        frmTechnician technicianForm = new frmTechnician();
                        technicianForm.Show();
                        break;

                    case "salesman":
                        this.Hide();
                        frmSalesMan salesmanForm = new frmSalesMan();
                        salesmanForm.Show();
                        break;

                    default:
                        MessageBox.Show("Access Denied! Your role is not authorized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
            }
            else
            {
                MessageBox.Show("Invalid Credentials!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

