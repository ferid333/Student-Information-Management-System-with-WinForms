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
    public partial class ScoreForm : Form
    {
        CourseClass course = new CourseClass();
        StudentClass student = new StudentClass();
        ScoreClass score = new ScoreClass();

        CourseTeachersClass courseTeachers = new CourseTeachersClass();
        public ScoreForm()
        {
            InitializeComponent();
        }
        
        private void showTable()
        {
            DataGridView_score.DataSource = score.getList(new MySqlCommand("Select idscore as Id, concat(s.StdFirstName, ' ', s.StdLastName) as Student_Name, c.CourseName as Course, concat(t.TchFirstName, ' ', t.TchLastName) as Teacher_Name, sc.Score, sc.Description from score sc\r\nLeft Join teacher t on t.idteacher=sc.TeacherId\r\nLeft Join student s on s.idstudent=sc.StudentId\r\nLeft Join course c on c.idcourse=sc.CourseId"));
            DataGridView_score.Columns["Id"].Width = 100;
            DataGridView_score.Columns["Score"].Width = 100;
            DataGridView_score.Columns["Student_Name"].HeaderText = "Student Name";
            DataGridView_score.Columns["Teacher_Name"].HeaderText = "Teacher Name";
        }

        private void ScoreForm_Load(object sender, EventArgs e)
        {
            showTable();
            comboBox_course.DataSource = course.getCourse(new MySqlCommand("SELECT idcourse as Id,Coursename as Name FROM `course`;"));
            comboBox_course.DisplayMember = "Name";
            comboBox_course.ValueMember = "Id";
            comboBox_student.DataSource = student.getStudentlist(new MySqlCommand("Select idstudent as Id, Concat(StdFirstName, ' ', StdLastName) as Name from student"));
            comboBox_student.DisplayMember = "Name";
            comboBox_student.ValueMember = "Id";
        }

      
        private void button_add_Click(object sender, EventArgs e)
        {
            if (textBox_score.Text == "")
            {
                MessageBox.Show("Need score data", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int stdId = Convert.ToInt32(comboBox_student.SelectedValue);
                int cId = Convert.ToInt32(comboBox_course.SelectedValue);
                int tchId= Convert.ToInt32(comboBox_teacher.SelectedValue);
                double scor = Convert.ToDouble(textBox_score.Text);
                string desc = textBox_description.Text;
                if (!score.checkScore(stdId, cId))
                {

                    if (score.insertScore(stdId, cId,tchId, scor, desc))
                    {
                        showTable();
                        button_clear.PerformClick();
                        MessageBox.Show("New score added", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show("Score not added", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("The score for this course are alerady exists", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            comboBox_student.SelectedIndex = 0;
            comboBox_teacher.SelectedIndex = 0;
            comboBox_course.SelectedIndex = 0;
            textBox_score.Clear();
            textBox_description.Clear();
        }


        private void comboBox_course_SelectedValueChanged(object sender, EventArgs e)
        {
            if(int.TryParse(comboBox_course.SelectedValue.ToString(), out int crId))
            {
                comboBox_teacher.DataSource = courseTeachers.getCourseTeachers(crId);
                comboBox_teacher.DisplayMember = "Name";
                comboBox_teacher.ValueMember = "Id";
            }
        }

        private void textBox_score_TextChanged(object sender, EventArgs e)
        {

            if (!double.TryParse(textBox_score.Text, out double value) && textBox_score.Text.Length > 0)
            {
                textBox_score.Text = textBox_score.Text.Remove(textBox_score.Text.Length - 1);
                textBox_score.SelectionStart = textBox_score.Text.Length;
            }
            if (textBox_score.Text.Length > 0 && (Convert.ToDouble(textBox_score.Text) < 0 || Convert.ToDouble(textBox_score.Text) > 100))
            {
                textBox_score.Text = textBox_score.Text.Remove(textBox_score.Text.Length - 1);
                textBox_score.SelectionStart = textBox_score.Text.Length;
            }
          
        }
    }
}
