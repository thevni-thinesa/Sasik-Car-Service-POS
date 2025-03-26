using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace newfinalSSS.Model
{
    public partial class frmMyAccount : Form
    {
        private string currentStaffID;

        public frmMyAccount(string staffID)
        {
            InitializeComponent();
            currentStaffID = staffID;
            LoadUserDetails();
        }

        private void frmMyAccount_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentStaffID))
            {
                LoadUserDetails();
            }
            else
            {
                MessageBox.Show("Invalid Staff ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserDetails()
        {
            UserDetails user = GetUserDetails(currentStaffID);

            if (user != null)
            {
                lblName.Text = user.Name ?? "N/A";
                lblPhone.Text = user.Phone ?? "N/A";
                lblEmail.Text = user.Email ?? "N/A";
                lblNIC.Text = user.NIC ?? "N/A";
                lblStaffID.Text = user.StaffID ?? "N/A";
            }
            else
            {
                MessageBox.Show("User details not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private UserDetails GetUserDetails(string staffID)
        {
            UserDetails user = null;
            string query = "SELECT UserID, Name, Phone, NIC, Role, StaffID, email " +
                           "FROM [dbo].[user] " +
                           "WHERE StaffID = @StaffID";

            using (SqlCommand cmd = new SqlCommand(query, MainClass.con))
            {
                cmd.Parameters.AddWithValue("@StaffID", staffID);

                try
                {
                    if (MainClass.con.State != ConnectionState.Open)
                        MainClass.con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new UserDetails
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                                NIC = reader.IsDBNull(reader.GetOrdinal("NIC")) ? null : reader.GetString(reader.GetOrdinal("NIC")),
                                Role = reader.IsDBNull(reader.GetOrdinal("Role")) ? null : reader.GetString(reader.GetOrdinal("Role")),
                                StaffID = reader.IsDBNull(reader.GetOrdinal("StaffID")) ? null : reader.GetString(reader.GetOrdinal("StaffID")),
                                Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email"))
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (MainClass.con.State == ConnectionState.Open)
                        MainClass.con.Close();
                }
            }
            return user;
        }

        private void btnPassword_Click(object sender, EventArgs e)
        {
            panelPassword.Visible = !panelPassword.Visible;
        }

        private void frmMyAccount_Load_1(object sender, EventArgs e)
        {
            panelPassword.Visible = false;
        }

        private void btnNewPasswordSave_Click(object sender, EventArgs e)
        {
            string oldPassword = txtOldPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string reNewPassword = txtReNewPassword.Text.Trim();

            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(reNewPassword))
            {
                MessageBox.Show("Please fill in all password fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if the old password is correct
            if (!IsValidOldPassword(currentStaffID, oldPassword))
            {
                MessageBox.Show("Old password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if new passwords match
            if (newPassword != reNewPassword)
            {
                MessageBox.Show("New passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hash the new password before storing it
            string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            // Update password in database
            if (UpdatePassword(currentStaffID, hashedNewPassword))
            {
                MessageBox.Show("Password updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOldPassword.Clear();
                txtNewPassword.Clear();
                txtReNewPassword.Clear();
            }
            else
            {
                MessageBox.Show("Failed to update password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private bool IsValidOldPassword(string staffID, string oldPassword)
        {
            string query = "SELECT Password FROM [user] WHERE StaffID = @StaffID";
            string storedHashedPassword = "";

            using (SqlCommand cmd = new SqlCommand(query, MainClass.con))
            {
                cmd.Parameters.AddWithValue("@StaffID", staffID);

                try
                {
                    if (MainClass.con.State != ConnectionState.Open)
                        MainClass.con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            storedHashedPassword = reader["Password"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                finally
                {
                    if (MainClass.con.State == ConnectionState.Open)
                        MainClass.con.Close();
                }
            }

            // Compare old password with stored hashed password
            return BCrypt.Net.BCrypt.Verify(oldPassword, storedHashedPassword);
        }

        private bool UpdatePassword(string staffID, string hashedPassword)
        {
            string query = "UPDATE [user] SET Password = @Password, confirmPassword = @Password WHERE StaffID = @StaffID";

            using (SqlCommand cmd = new SqlCommand(query, MainClass.con))
            {
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                cmd.Parameters.AddWithValue("@StaffID", staffID);

                try
                {
                    if (MainClass.con.State != ConnectionState.Open)
                        MainClass.con.Open();

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0; // Returns true if update was successful
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                finally
                {
                    if (MainClass.con.State == ConnectionState.Open)
                        MainClass.con.Close();
                }
            }
        }



    }

    public class UserDetails
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string NIC { get; set; }
        public string Role { get; set; }
        public string StaffID { get; set; }
        public string Email { get; set; }
    }
}
