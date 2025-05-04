using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Transparent_Form
{
    public partial class CourseForm : Form
    {
        CourseClass course = new CourseClass();
        TeacherClass teacher = new TeacherClass();
        CourseTeachersClass courseTeachers = new CourseTeachersClass();
        public CourseForm()
        {
            InitializeComponent();
           

        }

        private void button_add_Click(object sender, EventArgs e)
        {
            if (textBox_Cname.Text == "" || textBox_Chour.Text == "" || listBox1.SelectedItems.Count<=0)
            {
                MessageBox.Show("Need Course data", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                string cName = textBox_Cname.Text;
                int chr = Convert.ToInt32(textBox_Chour.Text);
                string desc = textBox_description.Text;
                var selectedTeachers = listBox1.SelectedItems;

                if (course.insetCourse(cName, chr, desc))
                {
                    MySqlCommand command = new MySqlCommand("Select idcourse from course where coursename=@cn");
                    command.Parameters.Add("@cn", MySqlDbType.VarChar).Value = cName;
                    var tableC= course.getCourse(command);
                    DataRow row = tableC.Rows[0];
                    int courseid = Convert.ToInt32(row["idcourse"]);

                    if (courseTeachers.insertCourseTeacher(courseid, selectedTeachers))
                    {
                        showData();
                        button_clear.PerformClick();
                        MessageBox.Show("New course inserted", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Course not insert", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("Course not insert", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_Cname.Clear();
            textBox_Chour.Clear();
            textBox_description.Clear();
            listBox1.SelectedItems.Clear();
        }

        private void CourseForm_Load(object sender, EventArgs e)
        {
            showData();
        }
        private void showData()
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
            DataGridView_course.DataSource = course.getCourse(new MySqlCommand("Select idcourse as Id,coursename as Name,CourseHour as Hours,Description,GROUP_CONCAT(CONCAT(t.TchFirstName, ' ', t.TchLastName) SEPARATOR ', ') AS Teachers\r\nfrom studentdb.course c\r\nleft Join studentdb.courseteachers ct on ct.courseid=c.idcourse\r\nleft Join studentdb.teacher t on t.idteacher=ct.TeacherID\r\nGroup by idcourse;"));
            DataGridView_course.Columns["Hours"].Width = 100;
        }
    }
}
