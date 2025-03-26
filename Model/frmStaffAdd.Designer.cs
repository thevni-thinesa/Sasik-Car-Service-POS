namespace newfinalSSS.Model
{
    partial class frmStaffAdd
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtsName = new Guna.UI2.WinForms.Guna2TextBox();
            this.cbRole = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtsNIC = new Guna.UI2.WinForms.Guna2TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtsPhone = new Guna.UI2.WinForms.Guna2TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtsPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.Password = new System.Windows.Forms.Label();
            this.txtsConfirmPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Size = new System.Drawing.Size(141, 32);
            this.label1.Text = "Staff Details";
            // 
            // txtsName
            // 
            this.txtsName.AutoRoundedCorners = true;
            this.txtsName.BorderRadius = 18;
            this.txtsName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtsName.DefaultText = "";
            this.txtsName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtsName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtsName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtsName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtsName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtsName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtsName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtsName.Location = new System.Drawing.Point(186, 162);
            this.txtsName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtsName.Name = "txtsName";
            this.txtsName.PasswordChar = '\0';
            this.txtsName.PlaceholderText = "";
            this.txtsName.SelectedText = "";
            this.txtsName.Size = new System.Drawing.Size(311, 39);
            this.txtsName.TabIndex = 10;
            // 
            // cbRole
            // 
            this.cbRole.AutoRoundedCorners = true;
            this.cbRole.BackColor = System.Drawing.Color.Transparent;
            this.cbRole.BorderRadius = 17;
            this.cbRole.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRole.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbRole.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbRole.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbRole.ItemHeight = 30;
            this.cbRole.Items.AddRange(new object[] {
            "Manager",
            "Technician",
            "Mechanic",
            "Electrician",
            "Salesman",
            "Car Wash Attendant",
            "Security"});
            this.cbRole.Location = new System.Drawing.Point(186, 363);
            this.cbRole.Name = "cbRole";
            this.cbRole.Size = new System.Drawing.Size(311, 36);
            this.cbRole.TabIndex = 9;
            this.cbRole.SelectedIndexChanged += new System.EventHandler(this.cbCat_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(113, 363);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "Role :";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(102, 162);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "Name :";
            // 
            // txtsNIC
            // 
            this.txtsNIC.AutoRoundedCorners = true;
            this.txtsNIC.BorderRadius = 18;
            this.txtsNIC.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtsNIC.DefaultText = "";
            this.txtsNIC.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtsNIC.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtsNIC.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtsNIC.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtsNIC.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtsNIC.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtsNIC.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtsNIC.Location = new System.Drawing.Point(186, 305);
            this.txtsNIC.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtsNIC.Name = "txtsNIC";
            this.txtsNIC.PasswordChar = '\0';
            this.txtsNIC.PlaceholderText = "";
            this.txtsNIC.SelectedText = "";
            this.txtsNIC.Size = new System.Drawing.Size(311, 39);
            this.txtsNIC.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(119, 321);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 23);
            this.label4.TabIndex = 11;
            this.label4.Text = "NIC :";
            // 
            // txtsPhone
            // 
            this.txtsPhone.AutoRoundedCorners = true;
            this.txtsPhone.BorderRadius = 18;
            this.txtsPhone.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtsPhone.DefaultText = "";
            this.txtsPhone.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtsPhone.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtsPhone.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtsPhone.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtsPhone.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtsPhone.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtsPhone.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtsPhone.Location = new System.Drawing.Point(186, 209);
            this.txtsPhone.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtsPhone.Name = "txtsPhone";
            this.txtsPhone.PasswordChar = '\0';
            this.txtsPhone.PlaceholderText = "";
            this.txtsPhone.SelectedText = "";
            this.txtsPhone.Size = new System.Drawing.Size(311, 39);
            this.txtsPhone.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(99, 210);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 23);
            this.label5.TabIndex = 13;
            this.label5.Text = "Phone :";
            // 
            // txtsPassword
            // 
            this.txtsPassword.AutoRoundedCorners = true;
            this.txtsPassword.BorderRadius = 18;
            this.txtsPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtsPassword.DefaultText = "";
            this.txtsPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtsPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtsPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtsPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtsPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtsPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtsPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtsPassword.Location = new System.Drawing.Point(186, 410);
            this.txtsPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtsPassword.Name = "txtsPassword";
            this.txtsPassword.PasswordChar = '\0';
            this.txtsPassword.PlaceholderText = "";
            this.txtsPassword.SelectedText = "";
            this.txtsPassword.Size = new System.Drawing.Size(311, 39);
            this.txtsPassword.TabIndex = 18;
            // 
            // Password
            // 
            this.Password.AutoSize = true;
            this.Password.Location = new System.Drawing.Point(76, 410);
            this.Password.Name = "Password";
            this.Password.Size = new System.Drawing.Size(89, 23);
            this.Password.TabIndex = 17;
            this.Password.Text = "Password :";
            // 
            // txtsConfirmPassword
            // 
            this.txtsConfirmPassword.AutoRoundedCorners = true;
            this.txtsConfirmPassword.BorderRadius = 18;
            this.txtsConfirmPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtsConfirmPassword.DefaultText = "";
            this.txtsConfirmPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtsConfirmPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtsConfirmPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtsConfirmPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtsConfirmPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtsConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtsConfirmPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtsConfirmPassword.Location = new System.Drawing.Point(186, 463);
            this.txtsConfirmPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtsConfirmPassword.Name = "txtsConfirmPassword";
            this.txtsConfirmPassword.PasswordChar = '\0';
            this.txtsConfirmPassword.PlaceholderText = "";
            this.txtsConfirmPassword.SelectedText = "";
            this.txtsConfirmPassword.Size = new System.Drawing.Size(311, 38);
            this.txtsConfirmPassword.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 463);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(155, 23);
            this.label7.TabIndex = 19;
            this.label7.Text = "Confirm Password :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(105, 257);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 23);
            this.label6.TabIndex = 21;
            this.label6.Text = "Email :";
            // 
            // txtEmail
            // 
            this.txtEmail.AutoRoundedCorners = true;
            this.txtEmail.BorderRadius = 19;
            this.txtEmail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEmail.DefaultText = "";
            this.txtEmail.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtEmail.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtEmail.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtEmail.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtEmail.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtEmail.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtEmail.Location = new System.Drawing.Point(186, 257);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.PasswordChar = '\0';
            this.txtEmail.PlaceholderText = "";
            this.txtEmail.SelectedText = "";
            this.txtEmail.Size = new System.Drawing.Size(311, 40);
            this.txtEmail.TabIndex = 22;
            // 
            // frmStaffAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 657);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtsConfirmPassword);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtsPassword);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.txtsPhone);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtsNIC);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtsName);
            this.Controls.Add(this.cbRole);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "frmStaffAdd";
            this.Text = "Staff Details";
            this.Load += new System.EventHandler(this.frmStaffAdd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Guna.UI2.WinForms.Guna2TextBox txtsName;
        public Guna.UI2.WinForms.Guna2ComboBox cbRole;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        public Guna.UI2.WinForms.Guna2TextBox txtsNIC;
        private System.Windows.Forms.Label label4;
        public Guna.UI2.WinForms.Guna2TextBox txtsPhone;
        private System.Windows.Forms.Label label5;
        public Guna.UI2.WinForms.Guna2TextBox txtsPassword;
        private System.Windows.Forms.Label Password;
        public Guna.UI2.WinForms.Guna2TextBox txtsConfirmPassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        public Guna.UI2.WinForms.Guna2TextBox txtEmail;
    }
}