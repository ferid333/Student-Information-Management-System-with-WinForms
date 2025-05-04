using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace Transparent_Form
{
    public partial class RegisterForm : Form
    {
        StudentClass student = new StudentClass();
        MajorClass major = new MajorClass();
        public RegisterForm()
        {
            InitializeComponent();
        }

 

         bool verify()
        {
            if ((textBox_Fname.Text == "") || (textBox_Lname.Text == "") ||
                (textBox_phone.Text == "") || (textBox_address.Text == "") ||
                (pictureBox_student.Image == null) || comboBox_major.SelectedValue==null)
            {
                return false;
            }
            else
                return true;
        }


        private void RegisterForm_Load(object sender, EventArgs e)
        {
            showTable();
        }
    
        public void showTable()
        {
            MySqlCommand command = new MySqlCommand(
                                @"SELECT s.idstudent as Id, concat(s.StdFirstName, ' ', s.StdLastName) as Name, s.Birthdate, s.Gender,s.Phone,s.Address, m.MajorName as Major ,s.Photo 
                                  FROM `student` s
                                  LEFT JOIN `major` m ON s.MajorId = m.idmajor");

           
            DataGridView_student.DataSource = student.getStudentlist(command);

            DataGridView_student.Columns["Id"].Width = 50;
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn = (DataGridViewImageColumn)DataGridView_student.Columns[7];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;


            comboBox_major.DataSource = major.getMajor(new MySqlCommand("SELECT idmajor as Id, MajorName as Name FROM `major` where IsActive=1"));
            comboBox_major.DisplayMember = "Name";
            comboBox_major.ValueMember = "Id";
        }

        private void button_upload_Click(object sender, EventArgs e)
        {
         
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Photo(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (opf.ShowDialog() == DialogResult.OK)
                pictureBox_student.Image = Image.FromFile(opf.FileName);

        }

        private void button_add_Click(object sender, EventArgs e)
        {
            
            string fname = textBox_Fname.Text;
            string lname = textBox_Lname.Text;
            DateTime bdate = dateTimePicker1.Value;
            string phone = textBox_phone.Text;
            string address = textBox_address.Text;
            string gender = radioButton_male.Checked ? "Male" : "Female";
            int majorId= Convert.ToInt32(comboBox_major.SelectedValue); 


            int born_year = dateTimePicker1.Value.Year;
            int this_year = DateTime.Now.Year;
            if ((this_year - born_year) < 10 || (this_year - born_year) > 100)
            {
                MessageBox.Show("The student age must be between 10 and 100", "Invalid Birthdate", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (verify())
            {
                try
                {
               
                    MemoryStream ms = new MemoryStream();
                    pictureBox_student.Image.Save(ms, pictureBox_student.Image.RawFormat);
                    byte[] img = ms.ToArray();
                    if (student.insertStudent(fname, lname, bdate, gender, phone, address, img,majorId))
                    {
                        showTable();
                        MessageBox.Show("New Student Added", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)

                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Empty Field", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_Fname.Clear();
            textBox_Lname.Clear();
            textBox_phone.Clear();
            textBox_address.Clear();
            radioButton_male.Checked = true;
            dateTimePicker1.Value = DateTime.Now;
            pictureBox_student.Image = null;
            comboBox_major.SelectedIndex = 0;
        }

        private void DataGridView_student_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void comboBox_course_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
