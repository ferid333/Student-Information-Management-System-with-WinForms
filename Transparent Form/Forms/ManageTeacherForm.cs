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
    public partial class ManageTeacherForm : Form
    {

        TeacherClass teacher=new TeacherClass();
        public ManageTeacherForm()
        {
            InitializeComponent();
        }

        private void ManageTeacherForm_Load(object sender, EventArgs e)
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

        private void button_upload_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Photo(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (opf.ShowDialog() == DialogResult.OK)
                pictureBox_teacher.Image = Image.FromFile(opf.FileName);
        }
        private void button_clear_Click(object sender, EventArgs e)
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

        private void DataGridView_teacher_Click(object sender, EventArgs e)
        {
            textBox_id.Text = DataGridView_teacher.CurrentRow.Cells[0].Value.ToString();
            textBox_Fname.Text = DataGridView_teacher.CurrentRow.Cells[1].Value.ToString();
            textBox_Lname.Text = DataGridView_teacher.CurrentRow.Cells[2].Value.ToString();

            dateTimePicker_age.Value = (DateTime)DataGridView_teacher.CurrentRow.Cells[3].Value;
            textBox_email.Text= DataGridView_teacher.CurrentRow.Cells[4].Value.ToString();

            textBox_phone.Text = DataGridView_teacher.CurrentRow.Cells[5].Value.ToString();
            textBox_address.Text = DataGridView_teacher.CurrentRow.Cells[6].Value.ToString();
            textBox_salary.Text= DataGridView_teacher.CurrentRow.Cells[7].Value.ToString();
            dateTimePicker_joinDate.Value = (DateTime)DataGridView_teacher.CurrentRow.Cells[8].Value;


            if (DataGridView_teacher.CurrentRow != null)
            {
                var cellValue = DataGridView_teacher.CurrentRow.Cells[9].Value;

                if (cellValue == null || cellValue == DBNull.Value)
                {
                    pictureBox_teacher.Image = null;
                }
                else
                {
                    byte[] img = (byte[])cellValue; 
                    using (MemoryStream ms = new MemoryStream(img))
                    {
                        pictureBox_teacher.Image = Image.FromStream(ms);
                    }
                }
            }


        }

        private void button_search_Click(object sender, EventArgs e)
        {
            DataGridView_teacher.DataSource = teacher.searchTeacher(textBox_search.Text);
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn = (DataGridViewImageColumn)DataGridView_teacher.Columns[9];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        bool verify()
        {
            if ((textBox_Fname.Text == "") || (textBox_Lname.Text == "") ||
                (textBox_phone.Text == "") || (textBox_address.Text == "")
                || (textBox_email.Text == "") || (textBox_salary.Text == "") || (dateTimePicker_age.Text == "")
                || (dateTimePicker_joinDate.Text == "") ||
                (pictureBox_teacher.Image == null))
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
            DateTime bdate = dateTimePicker_age.Value;
            string phone = textBox_phone.Text;
            string address = textBox_address.Text;
            string email = textBox_email.Text;
            DateTime jdate = dateTimePicker_joinDate.Value;
            double salary = Convert.ToDouble(textBox_salary.Text);

           if (verify())
            {
                try
                {
                    MemoryStream ms = new MemoryStream();
                    pictureBox_teacher.Image.Save(ms, pictureBox_teacher.Image.RawFormat);
                    byte[] img = ms.ToArray();
                    if (teacher.updateTeacher(id, fname, lname, bdate, email, phone, address, salary, jdate, img))
                    {
                        showTable();
                        MessageBox.Show("Teacher data update", "Update Teacher", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Empty Field", "Update Teacher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBox_id.Text);

            if (MessageBox.Show("Are you sure you want to remove this teacher", "Remove Teacher", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (teacher.deleteTeacher(id))
                {
                    showTable();
                    MessageBox.Show("Teacher Removed", "Remove Teacher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button_clear.PerformClick();
                }
            }
        }
    }
}
