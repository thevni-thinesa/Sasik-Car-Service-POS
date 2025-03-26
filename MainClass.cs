using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using BCrypt.Net;

namespace newfinalSSS
{
    public class MainClass
    {
        public static readonly string ConnectionString =
       "Data Source=DESKTOP-5BCEB9U\\SQLEXPRESS;Initial Catalog=SSS2;Integrated Security=True;TrustServerCertificate=True";

        // User Property
        private static string user;
        private static string userID;

        public static SqlConnection con = new SqlConnection(ConnectionString);

        public static bool IsValidUser(string staffID, string password, out string role, out string userID)
        {
            bool isValid = false;
            role = string.Empty;
            userID = string.Empty; // Initialize userID

            string qry = "SELECT Name, Role, Password, StaffID FROM [user] WHERE StaffID = @staffID";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@staffID", staffID);

                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHashedPassword = reader["Password"].ToString();
                                string userName = reader["Name"].ToString();
                                role = reader["Role"].ToString();
                                userID = reader["StaffID"].ToString(); // Get StaffID

                                if (BCrypt.Net.BCrypt.Verify(password, storedHashedPassword))
                                {
                                    isValid = true;
                                    MainClass.USERID = userID; // ✅ Corrected: Store StaffID here
                                    MainClass.USER = userName; // ✅ Store username for reference
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            return isValid;
        }


        public static string USER
        {
            get { return user; }
            private set { user = value; }
        }
       // This will hold the StaffID

        public static string USERID
        {
            get { return userID; }
            private set { userID = value; }
        }

        public static int SQL(string qry, Hashtable ht)
        {
            int res = 0;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.CommandType = CommandType.Text;

                    foreach (DictionaryEntry item in ht)
                    {
                        cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                    }

                    try
                    {
                        con.Open();
                        res = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("SQL Execution Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }finally 
                    { 
                        con.Close(); 
                    }
                }
            }
            return res;
        }

        //Loading data from database

        public static void LoadData(string qry, DataGridView gv, ListBox lb)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();

                        da.Fill(dt); // This will automatically open & close the connection

                        // Ensure the DataGridView is properly bound
                        gv.DataSource = dt;

                        // Assign DataPropertyName based on ListBox items
                        for (int i = 0; i < lb.Items.Count; i++)
                        {
                            string colName1 = lb.Items[i].ToString();  // Get the column name from the ListBox

                            if (gv.Columns.Contains(colName1) && dt.Columns.Contains(colName1))
                            {
                                gv.Columns[colName1].DataPropertyName = colName1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Execution Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void gv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Guna.UI2.WinForms.Guna2DataGridView gv = (Guna.UI2.WinForms.Guna2DataGridView)sender;
            int count = 0;

            foreach(DataGridViewRow row in gv.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        public static void BlurBackground(Form Model)
        {
            Form Background = new Form();
            using (Model)
            {
                Background.StartPosition = FormStartPosition.Manual;
                Background.FormBorderStyle = FormBorderStyle.None;
                Background.Opacity = 0.5d;
                Background.BackColor = Color.Black;
                // Background.Site = frmMain.Instance.Size;
                //Background.Location = frmMain.Instance.Location;
                Background.ShowInTaskbar = false;
                Background.Show();
                Model.Owner = Background;
                Model.ShowDialog(Background);
                Background.Dispose();
            }
        }

        //For CB Fill
        public static void CBFill(string qry, ComboBox CB, string displayColumn, string valueColumn)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString)) // Ensure correct connection handling
                {
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();

                        adapter.Fill(dt); // Fill the DataTable

                        // ✅ Ensure columns exist before setting them
                        if (dt.Columns.Contains(displayColumn) && dt.Columns.Contains(valueColumn))
                        {
                            CB.DisplayMember = displayColumn;
                            CB.ValueMember = valueColumn;
                            CB.DataSource = dt;
                            CB.SelectedIndex = -1; // Set default selection
                        }
                        else
                        {
                            MessageBox.Show("Error: The specified column names do not exist in the query result.", "Column Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static DataTable GetData(string qry, Hashtable parameters = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    if (parameters != null)
                    {
                        foreach (DictionaryEntry param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key.ToString(), param.Value);
                        }
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }



    }
}

