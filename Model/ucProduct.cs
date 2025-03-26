using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace newfinalSSS.Model
{
    public partial class ucProduct : UserControl
    {

        public event EventHandler onSelect = null;
        public ucProduct()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        public int Id { get; set; } // Renamed to follow PascalCase
        public decimal PPrice { get; set; }

        public string PCategory { get; set; }

        public string PName
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        // ✅ Storing image as a file path (string)
        // ✅ Store Image Path Separately
        private string _imagePath = string.Empty;

        // ✅ Load Image from Database (Binary to Image)
        private byte[] _imageData;

        public byte[] PImage
        {
            get { return _imageData; }
            set
            {
                try
                {
                    if (value != null && value.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(value))
                        {
                            txtImage.Image = Image.FromStream(ms);
                            _imageData = value; // ✅ Store binary image data
                        }
                    }
                    else
                    {
                        txtImage.Image = null; // ✅ Set to null if no image exists
                        _imageData = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image from database: " + ex.Message, "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void txtImage_Click(object sender, EventArgs e)
        {
            onSelect?.Invoke(this, e);
        }
    }
}
