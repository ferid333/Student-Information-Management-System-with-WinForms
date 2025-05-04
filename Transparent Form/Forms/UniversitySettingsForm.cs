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

namespace Transparent_Form
{
    public partial class UniversitySettingsForm : Form
    {
        
        public UniversitySettingsForm()
        {
            InitializeComponent();
            textBox_uniName.Text = UniversityClass.Instance.uName;
            textBox_uniAddress.Text = UniversityClass.Instance.uAddress;
            if (UniversityClass.Instance.uImage != null)
            {
                byte[] img = UniversityClass.Instance.uImage;
                using (MemoryStream ms = new MemoryStream(img))
                {
                    pictureBox_icon.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox_icon.Image = null;
            }
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        bool verify()
        {
            if ((textBox_uniName.Text == "") || (textBox_uniAddress.Text == "") ||
                (pictureBox_icon.Image == null))
            {
                return false;
            }
            else
                return true;
        }
        private void button_upload_Click(object sender, EventArgs e)
        {

            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Photo(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (opf.ShowDialog() == DialogResult.OK)
                pictureBox_icon.Image = Image.FromFile(opf.FileName);
    }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_uniAddress.Clear();
            textBox_uniName.Clear();
            pictureBox_icon.Image = null;
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            if (verify()) { 
            

                string uName=textBox_uniName.Text;
                string uAddress=textBox_uniAddress.Text;
                MemoryStream ms = new MemoryStream();
                pictureBox_icon.Image.Save(ms, pictureBox_icon.Image.RawFormat);
                byte[] img = ms.ToArray();

                UniversityClass.updateUniversity(uName, uAddress, img);

                MessageBox.Show("University Settings Updated", "Update University", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Empty Field", "Update University", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
