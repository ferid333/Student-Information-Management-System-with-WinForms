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
    public partial class ManageStudentForm : Form
    {
        StudentClass student = new StudentClass();
        MajorClass major = new MajorClass();
        public ManageStudentForm()
        {
            InitializeComponent();
        }

        private void ManageStudentForm_Load(object sender, EventArgs e)
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
        }

       
        private void DataGridView_student_Click(object sender, EventArgs e)
        {
            textBox_id.Text = DataGridView_student.CurrentRow.Cells[0].Value.ToString();
            textBox_Fname.Text = DataGridView_student.CurrentRow.Cells[1].Value.ToString().Split()[0];
            textBox_Lname.Text = DataGridView_student.CurrentRow.Cells[1].Value.ToString().Split()[1];

            dateTimePicker1.Value = (DateTime)DataGridView_student.CurrentRow.Cells[2].Value;
            if (DataGridView_student.CurrentRow.Cells[3].Value.ToString() == "Male")
                radioButton_male.Checked = true;

            textBox_phone.Text = DataGridView_student.CurrentRow.Cells[4].Value.ToString();
            textBox_address.Text = DataGridView_student.CurrentRow.Cells[5].Value.ToString();
            byte[] img = (byte[])DataGridView_student.CurrentRow.Cells[7].Value;
            MemoryStream ms = new MemoryStream(img);
            pictureBox_student.Image = Image.FromStream(ms);

            comboBox_major.DataSource = major.getMajor(new MySqlCommand("SELECT idmajor as Id, MajorName as Name FROM `major` where IsActive=1"));
            comboBox_major.DisplayMember = "Name";
            comboBox_major.ValueMember = "Id";

            comboBox_major.Text = DataGridView_student.CurrentRow.Cells[6].Value.ToString();
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_id.Clear();
            textBox_Fname.Clear();
            textBox_Lname.Clear();
            textBox_phone.Clear();
            textBox_address.Clear();
            radioButton_male.Checked = true;
            dateTimePicker1.Value = DateTime.Now;
            pictureBox_student.Image = null;
            comboBox_major.DataSource = null;
        }

        private void button_upload_Click(object sender, EventArgs e)
        {
         
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Photo(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (opf.ShowDialog() == DialogResult.OK)
                pictureBox_student.Image = Image.FromFile(opf.FileName);
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            DataGridView_student.DataSource = student.searchStudent(textBox_search.Text);
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn = (DataGridViewImageColumn)DataGridView_student.Columns[7];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }
     
        bool verify()
        {
            if ((textBox_Fname.Text == "") || (textBox_Lname.Text == "") ||
                (textBox_phone.Text == "") || (textBox_address.Text == "") ||
                (pictureBox_student.Image == null) || (comboBox_major.SelectedValue==null))
            {
                return false;
            }
            else
                return true;
        }

        private void button_update_Click(object sender, EventArgs e)
        {
      
            int id = Convert.ToInt32(textBox_id.Text);
            string fname = textBox_Fname.Text;
            string lname = textBox_Lname.Text;
            DateTime bdate = dateTimePicker1.Value;
            string phone = textBox_phone.Text;
            string address = textBox_address.Text;
            string gender = radioButton_male.Checked ? "Male" : "Female";
            int majorId = Convert.ToInt32(comboBox_major.SelectedValue);

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
                    if (student.updateStudent(id, fname, lname, bdate, gender, phone, address, img,majorId))
                    {
                        showTable();
                        MessageBox.Show("Student data update", "Update Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button_clear.PerformClick();
                    }
                }
                catch (Exception ex)

                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Empty Field", "Update Student", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            
            int id = Convert.ToInt32(textBox_id.Text);
            
            if (MessageBox.Show("Are you sure you want to remove this student", "Remove Student", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (student.deleteStudent(id))
                {
                    showTable();
                    MessageBox.Show("Student Removed", "Remove student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button_clear.PerformClick();
                }
            }
        }


    }
}
