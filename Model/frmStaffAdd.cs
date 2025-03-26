using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using BCrypt.Net; // ✅ Import for password hashing
using System.Data.SqlClient;

namespace newfinalSSS.Model
{
    public partial class frmStaffAdd : SampleAdd
    {
        public frmStaffAdd()
        {
            InitializeComponent();
        }

        private void cbCat_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public int id = 0;
        public override void btnSave_Click(object sender, EventArgs e)
        {
            // ✅ Validate Role Selection
            if (string.IsNullOrEmpty(cbRole.Text))
            {
                MessageBox.Show("Please select a role!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ Validate Password and Confirm Password
            if (id == 0) // New record
            {
                if (string.IsNullOrEmpty(txtsPassword.Text) || string.IsNullOrEmpty(txtsConfirmPassword.Text))
                {
                    MessageBox.Show("Please enter a password and confirm it!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txtsPassword.Text != txtsConfirmPassword.Text)
                {
                    MessageBox.Show("Passwords do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else // Existing record
            {
                // If updating, check if new password is provided
                if (!string.IsNullOrEmpty(txtsPassword.Text) || !string.IsNullOrEmpty(txtsConfirmPassword.Text))
                {
                    if (txtsPassword.Text != txtsConfirmPassword.Text)
                    {
                        MessageBox.Show("Passwords do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            string qry = "";
            Hashtable ht = new Hashtable(); // Declare and initialize ht here

            if (id == 0) // ✅ Insert New Record
            {
                // ✅ Generate Auto-Incremented StaffID
                string rolePrefix = GetRolePrefix(cbRole.Text);
                string newStaffID = GenerateStaffID(rolePrefix);

                if (string.IsNullOrEmpty(newStaffID))
                {
                    MessageBox.Show("Error generating Staff ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                qry = "INSERT INTO [user] (Name, Phone, email, Role, NIC, StaffID, Password, ConfirmPassword) " +
                      "VALUES (@Name, @Phone, @Email, @Role, @NIC, @StaffID, @Password, @ConfirmPassword)";

                ht.Add("@StaffID", newStaffID); // ✅ Assign Auto-Generated StaffID

                // ✅ Hash the Password before storing
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(txtsPassword.Text);
                ht.Add("@Password", hashedPassword); // ✅ Store hashed password
                ht.Add("@ConfirmPassword", hashedPassword); // Store hashed password in ConfirmPassword
            }
            else // ✅ Update Existing Record (Keep StaffID unchanged)
            {
                qry = "UPDATE [user] SET Name = @Name, Phone = @Phone, email = @Email, Role = @Role, NIC = @NIC ";

                // Check if a new password is provided
                if (!string.IsNullOrEmpty(txtsPassword.Text))
                {
                    // Validate ConfirmPassword
                    if (string.IsNullOrEmpty(txtsConfirmPassword.Text))
                    {
                        MessageBox.Show("Please confirm the new password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (txtsPassword.Text != txtsConfirmPassword.Text)
                    {
                        MessageBox.Show("Passwords do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(txtsPassword.Text);
                    qry += ", Password = @Password, ConfirmPassword = @ConfirmPassword "; // Add password update to query
                    ht.Add("@Password", hashedPassword); // ✅ Store hashed password
                    ht.Add("@ConfirmPassword", hashedPassword); // Store hashed password in ConfirmPassword
                }

                qry += "WHERE UserID = @id";
                ht.Add("@id", id); // Add UserID to the parameters
            }

            ht.Add("@Name", txtsName.Text);
            ht.Add("@Phone", txtsPhone.Text);
            ht.Add("@Email", txtEmail.Text);
            ht.Add("@Role", cbRole.Text);
            ht.Add("@NIC", txtsNIC.Text);

            if (MainClass.SQL(qry, ht) > 0)
            {
                MessageBox.Show("Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                id = 0;
                txtsName.Text = "";
                txtsPhone.Text = "";
                txtEmail.Text = "";
                txtsNIC.Text = "";
                txtsPassword.Text = "";
                txtsConfirmPassword.Text = "";
                cbRole.SelectedIndex = -1;
                txtsName.Focus();
            }
        }

        // ✅ Fetch Existing StaffID from Database for an Update
        private string GetExistingStaffID(int userId)
        {
            string staffID = "";
            string qry = "SELECT StaffID FROM [user] WHERE UserID = @UserID";

            using (SqlConnection con = MainClass.con)
            {
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    if (con.State != ConnectionState.Open)
                        con.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        staffID = result.ToString();
                    }
                }
            }
            return staffID;
        }


        private void frmStaffAdd_Load(object sender, EventArgs e)
        {

        }

        // ✅ Get Role Prefix for StaffID
        private string GetRolePrefix(string role)
        {
            switch (role.ToLower())
            {
                case "manager": return "M";
                case "technician": return "T";
                case "mechanic": return "ME";
                case "electrician": return "E";
                case "salesman": return "S";
                case "car wash attendant": return "C";
                case "security": return "SE";
                default: return "";
            }
        }

        // ✅ Generate the Next Staff ID Based on Role
        private string GenerateStaffID(string rolePrefix)
        {
            if (string.IsNullOrEmpty(rolePrefix))
                return "";

            string newStaffID = "";
            string qry = "SELECT MAX(StaffID) FROM [user] WHERE StaffID LIKE @RolePrefix + '%'";

            //// Check if the connection is initialized
            //if (MainClass.con == null || string.IsNullOrEmpty(MainClass.con.ConnectionString))
            //{
            //    throw new InvalidOperationException("Database connection has not been initialized.");
            //}

            using (SqlConnection con = MainClass.con)
            {
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@RolePrefix", rolePrefix);

                    if (con.State != ConnectionState.Open)
                        con.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        string lastStaffID = result.ToString();
                        int lastNumber = int.Parse(lastStaffID.Substring(rolePrefix.Length));
                        newStaffID = rolePrefix + (lastNumber + 1).ToString("D3");
                    }
                    else
                    {
                        newStaffID = rolePrefix + "001";
                    }
                }
            }
            return newStaffID;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
