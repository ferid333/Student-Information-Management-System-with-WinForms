using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Transparent_Form
{
    public partial class ManageCourseForm : Form
    {
        CourseClass course = new CourseClass();
        TeacherClass teacher = new TeacherClass();
        CourseTeachersClass courseTeachers= new CourseTeachersClass();
        public ManageCourseForm()
        {
            InitializeComponent();
        }

        private void ManageCourseForm_Load(object sender, EventArgs e)
        {
            showData();

        }
       
        private void showData()
        {
             
            DataGridView_course.DataSource = course.getCourse(new MySqlCommand("Select idcourse as Id,coursename as Name,CourseHour as Hours,Description,GROUP_CONCAT(CONCAT(t.TchFirstName, ' ', t.TchLastName) SEPARATOR ', ') AS Teachers\r\nfrom studentdb.course c\r\nleft Join studentdb.courseteachers ct on ct.courseid=c.idcourse\r\nleft Join studentdb.teacher t on t.idteacher=ct.TeacherID\r\nGroup by idcourse;"));
            DataGridView_course.Columns["Hours"].Width = 100;
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_id.Clear();
            textBox_Cname.Clear();
            textBox_Chour.Clear();
            textBox_description.Clear();
            listBox1.SelectedItems.Clear();
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            if (textBox_Cname.Text == "" || textBox_Chour.Text == ""|| textBox_id.Text.Equals("") || listBox1.SelectedItems.Count<=0)
            {
                MessageBox.Show("Need Course data", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            { 
                int id = Convert.ToInt32(textBox_id.Text);
                string cName = textBox_Cname.Text;
                int chr = Convert.ToInt32(textBox_Chour.Text);
                string desc = textBox_description.Text;
                var teachers = listBox1.SelectedItems;

                if (course.updateCourse(id, cName, chr, desc) && courseTeachers.updateCourseTeacher(id, teachers))
                {
                   
                        showData();
                        button_clear.PerformClick();
                        MessageBox.Show("Course update successfuly", "Update Course", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Error-Course not Edit", "Update Course", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (textBox_id.Text.Equals(""))
            {
                MessageBox.Show("Need Course Id", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    int id = Convert.ToInt32(textBox_id.Text);
                    if (course.deletCourse(id))
                    {
                        showData();
                        button_clear.PerformClick();
                        MessageBox.Show("course Deleted", "Removed Course", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
                catch (Exception ex)

                {
                    MessageBox.Show(ex.Message, "Removed Course", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DataGridView_course_Click(object sender, EventArgs e)
        {
            textBox_id.Text = DataGridView_course.CurrentRow.Cells[0].Value.ToString();
            textBox_Cname.Text = DataGridView_course.CurrentRow.Cells[1].Value.ToString();
            textBox_Chour.Text = DataGridView_course.CurrentRow.Cells[2].Value.ToString();
            textBox_description.Text = DataGridView_course.CurrentRow.Cells[3].Value.ToString();
            addSelectedTeachers();
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            string query = "SELECT c.idcourse AS Id, c.coursename AS Name, c.CourseHour AS Hours, c.Description, GROUP_CONCAT(CONCAT(t.TchFirstName, ' ', t.TchLastName) SEPARATOR ', ') AS Teachers FROM studentdb.course c LEFT JOIN studentdb.courseteachers ct ON ct.courseid = c.idcourse LEFT JOIN studentdb.teacher t ON t.idteacher = ct.TeacherID GROUP BY c.idcourse, c.coursename, c.CourseHour, c.Description HAVING CONCAT(c.coursename, ' ', c.Description, ' ', COALESCE(GROUP_CONCAT(CONCAT(t.TchFirstName, ' ', t.TchLastName) SEPARATOR ', '), '')) LIKE '%" + textBox_search.Text + "%'";
            DataGridView_course.DataSource = course.getCourse(new MySqlCommand(query));
            DataGridView_course.Columns["Hours"].Width = 100;
            textBox_search.Clear();
        }

       private void addSelectedTeachers()
        {
            listBox1.Items.Clear();
            var teacherTable = teacher.getTeacherlist(new MySqlCommand("SELECT idteacher, CONCAT(TchFirstName, ' ', TchLastName) AS TeacherName FROM `teacher`"));

            foreach (DataRow row in teacherTable.Rows)
            {
                listBox1.Items.Add(new KeyValuePair<int, string>(
                    Convert.ToInt32(row["idteacher"]),
                    row["TeacherName"].ToString()
                ));
            }
            listBox1.DisplayMember = "Value";
            listBox1.ValueMember = "Key";
            var teachers= DataGridView_course.CurrentRow.Cells[4].Value.ToString().Split(',');
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                KeyValuePair<int, string> teacher = (KeyValuePair<int, string>)listBox1.Items[i];
                if (teachers.Any(x=>x.Trim()== teacher.Value.ToString()))
                {
                    listBox1.SetSelected(i, true);
                }
                else
                {
                    listBox1.SetSelected(i, false);
                }
            }
        }
    }
}
