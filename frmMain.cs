using newfinalSSS.Model;
using newfinalSSS.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace newfinalSSS
{
    public partial class frmMain : Form
    {
        
        public frmMain()
        {
            InitializeComponent();

        }

        // Corrected 'AddControls' to work with instance panels
        public void AddControls(Form f)
        {
          centralPanel.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            centralPanel.Controls.Add(f);
            f.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            AddControls(new frmProductsView());
        }

        private void lblUserName_Click(object sender, EventArgs e)
        {
           

        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblUser_Click(object sender, EventArgs e)
        {
            
        }

        private void lblUName_Click(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(MainClass.USER))
            //{
            //    lblUserName.Text = MainClass.USER;
            //}
            //else
            //{
            //    lblUserName.Text = "No user logged in";
            //}
        }

        private void centralPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            AddControls(new frmHome());
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            AddControls(new frmCategoryView());
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
          
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            AddControls(new frmStaffView());
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            AddControls(new frmCustomerView());
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            frmLogin f = new frmLogin();
            f.Show();
            this.Close();
        }

        private void btnMyAccount_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(MainClass.USERID))
            {
                MessageBox.Show("No logged-in user found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Now USERID should contain the correct StaffID
            string loggedInStaffID = MainClass.USERID;
            AddControls(new frmMyAccount(loggedInStaffID));
            
        }

    }
}
