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
using MySql.Data.MySqlClient;

namespace Transparent_Form
{
    public partial class TeacherRegisterForm : Form
    {

        TeacherClass teacher=new TeacherClass();
        public TeacherRegisterForm()
        {
            InitializeComponent();
        }


        bool verify()
        {
            if ((textBox_Fname.Text == "") || (textBox_Lname.Text == "") ||
                (textBox_phone.Text == "") || (textBox_address.Text == "") 
                || (textBox_email.Text=="")|| (textBox_salary.Text == "") || (dateTimePicker_age.Text=="")
                || (dateTimePicker_joinDate.Text == "") ||
                (pictureBox_teacher.Image == null))
            {
                return false;
            }
            else
                return true;
        }
        private void TeacherRegisterForm_Load(object sender, EventArgs e)
        {
            showTable();
        }
        public void showTable()
        {
            DataGridView_teacher.DataSource = teacher.getTeacherlist(new MySqlCommand("SELECT idteacher as Id, TchFirstName as Firstname, TchLastName as Lastname, Birthdate, Email, Phone, Address, Salary, JoiningDate, Photo FROM studentdb.teacher;"));
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn = (DataGridViewImageColumn)DataGridView_teacher.Columns[9];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }


        private void textBox_salary_TextChanged(object sender, EventArgs e)
        {
            if (!double.TryParse(textBox_salary.Text, out double value) && textBox_salary.Text.Length > 0)
            {
                textBox_salary.Text = textBox_salary.Text.Remove(textBox_salary.Text.Length - 1);
                textBox_salary.SelectionStart = textBox_salary.Text.Length;
            }
        }
        private void button_add_Click(object sender, EventArgs e)
        {
            string fname = textBox_Fname.Text;
            string lname = textBox_Lname.Text;
            DateTime bdate = dateTimePicker_age.Value;
            string phone = textBox_phone.Text;
            string address = textBox_address.Text;
            string email=textBox_email.Text;
            DateTime jdate=dateTimePicker_joinDate.Value;
            double salary=Convert.ToDouble(textBox_salary.Text);

            if (verify())
            {
                try
                {
                    MemoryStream ms = new MemoryStream();
                    pictureBox_teacher.Image.Save(ms, pictureBox_teacher.Image.RawFormat);
                    byte[] img = ms.ToArray();
                    if (teacher.insertTeacher(fname, lname, bdate, email, phone, address, salary,jdate,img))
                    {
                        showTable();
                        clearInputs();
                        MessageBox.Show("New Teacher Added", "Add Teacher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)

                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Empty Field", "Add Teacher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button_upload_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Photo(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (opf.ShowDialog() == DialogResult.OK)
                pictureBox_teacher.Image = Image.FromFile(opf.FileName);
        }

        private void clearInputs()
        {
            textBox_address.Clear();
            textBox_phone.Clear();
            textBox_Fname.Clear();
            textBox_Lname.Clear();
            textBox_salary.Clear();
            textBox_email.Clear();
            dateTimePicker_age.Value = DateTime.Now;
            dateTimePicker_joinDate.Value = DateTime.Now;
            pictureBox_teacher.Image = null;
        }
        private void button_clear_Click(object sender, EventArgs e)
        {
           clearInputs();
        }
    }
}
