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
    public partial class frmTechnician : Form
    {
        public frmTechnician()
        {
            InitializeComponent();
        }

        public void AddControls(Form f)
        {
            centerPanel.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            centerPanel.Controls.Add(f);
            f.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            frmLogin f = new frmLogin();
            f.Show();
            this.Close();
        }

        private void btnEatimation_Click(object sender, EventArgs e)
        {
            frmPOS fr = new frmPOS();
            fr.Show();
        }

        private void btnMyAccount_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(MainClass.USERID))
            {
                MessageBox.Show("No logged-in user found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Now USERID should contain the correct StaffID
            string loggedInStaffID = MainClass.USERID;
            AddControls(new frmMyAccount(loggedInStaffID));
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
           
        }

        private void frmTechnician_Load(object sender, EventArgs e)
        {

        }
    }
}
