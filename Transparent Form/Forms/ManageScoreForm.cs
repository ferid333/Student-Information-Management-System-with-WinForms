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
    public partial class ManageScoreForm : Form
    {
        CourseClass course = new CourseClass();
        ScoreClass score = new ScoreClass();
        CourseTeachersClass courseTeachers= new CourseTeachersClass();
        StudentClass student = new StudentClass();
        public ManageScoreForm()
        {
            InitializeComponent();
        }

        private void ManageScoreForm_Load(object sender, EventArgs e)
        {
            showTable();
        }
        private void showTable()
        {
            DataGridView_score.DataSource = score.getList(new MySqlCommand("Select idscore as Id, concat(s.StdFirstName, ' ', s.StdLastName) as Student_Name, c.CourseName as Course, concat(t.TchFirstName, ' ', t.TchLastName) as Teacher_Name, sc.Score, sc.Description from score sc\r\nLeft Join teacher t on t.idteacher=sc.TeacherId\r\nLeft Join student s on s.idstudent=sc.StudentId\r\nLeft Join course c on c.idcourse=sc.CourseId"));
            DataGridView_score.Columns["Id"].Width = 100;
            DataGridView_score.Columns["Score"].Width = 100;
            DataGridView_score.Columns["Student_Name"].HeaderText = "Student Name";
            DataGridView_score.Columns["Teacher_Name"].HeaderText = "Teacher Name";
            
        }



        private void button_Update_Click(object sender, EventArgs e)
        {
            if (textBox_score.Text == "" || comboBox_teacher.Text=="" || comboBox_student.Text=="" || comboBox_course.Text=="")
            {
                MessageBox.Show("Need score data", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int sId= Convert.ToInt32(textBox_scoreId.Text);
                int stdId = Convert.ToInt32(comboBox_student.SelectedValue);
                int cId = Convert.ToInt32(comboBox_course.SelectedValue);
                int tchId = Convert.ToInt32(comboBox_teacher.SelectedValue);
                double scor = Convert.ToDouble(textBox_score.Text);
                string desc = textBox_description.Text;

                if (score.updateScore(sId,stdId, cId, tchId, scor, desc))
                {
                    showTable();
                    button_clear.PerformClick();
                    MessageBox.Show("Score Edited Complete", "Update Score", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Score not edit", "Update Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {

            if(textBox_score.Text == "" || comboBox_teacher.Text == "" || comboBox_student.Text == "" || comboBox_course.Text == "")
            {
                MessageBox.Show("Score is not selected", "Delete Score", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                int id = Convert.ToInt32(textBox_scoreId.Text);
                if (MessageBox.Show("Are you sure you want to remove this score", "Delete Score", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (score.deleteScore(id))
                    {
                        showTable();
                        MessageBox.Show("Score Removed", "Delete Score", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button_clear.PerformClick();
                    }
                }
            }
                
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            comboBox_student.DataSource=null;
            comboBox_teacher.DataSource = null;
            comboBox_course.DataSource = null;
            textBox_score.Clear();
            textBox_description.Clear();
            textBox_scoreId.Clear();
        }

        private void DataGridView_score_Click(object sender, EventArgs e)
        {

            if (DataGridView_score.CurrentRow != null)
            {

                textBox_scoreId.Text = DataGridView_score.CurrentRow.Cells[0].Value.ToString();
                comboBox_student.DataSource = student.getStudentlist(new MySqlCommand("Select idstudent as Id, Concat(StdFirstName, ' ', StdLastName) as Name from student"));
                comboBox_student.DisplayMember = "Name";
                comboBox_student.ValueMember = "Id";

                comboBox_student.Text = DataGridView_score.CurrentRow.Cells[1].Value.ToString();

                comboBox_course.DataSource = course.getCourse(new MySqlCommand("SELECT idcourse as Id,Coursename as Name FROM `course`;"));
                comboBox_course.DisplayMember = "Name";
                comboBox_course.ValueMember = "Id";

                comboBox_course.Text = DataGridView_score.CurrentRow.Cells[2].Value.ToString();
                textBox_score.Text = DataGridView_score.CurrentRow.Cells[4].Value.ToString();
                textBox_description.Text = DataGridView_score.CurrentRow.Cells[5].Value.ToString();
                comboBox_teacher.Text= DataGridView_score.CurrentRow.Cells[3].Value.ToString();
            }
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            string searchQuery = @"SELECT sc.idscore, 
                      CONCAT(s.StdFirstName, ' ', s.StdLastName) AS Student_Name, 
                      c.CourseName AS Course_Name, 
                      CONCAT(t.TchFirstName, ' ', t.TchLastName) AS Teacher_Name, 
                      sc.Score, 
                      sc.Description 
                      FROM studentdb.score sc
                      LEFT JOIN studentdb.teacher t ON t.idteacher = sc.TeacherId
                      LEFT JOIN studentdb.student s ON s.idstudent = sc.StudentId
                      LEFT JOIN studentdb.course c ON c.idcourse = sc.CourseId
                      WHERE CONCAT(s.StdFirstName, ' ', s.StdLastName, ' ',
                            c.CourseName, ' ',
                            t.TchFirstName, ' ', t.TchLastName, ' ',
                            sc.Score, ' ',
                            sc.Description) LIKE '%"+ textBox_search.Text+ "%'";
            DataGridView_score.DataSource = score.getList(new MySqlCommand(searchQuery));
            
        }

        private void comboBox_course_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox_course.SelectedValue!=null && int.TryParse(comboBox_course.SelectedValue.ToString(), out int crId))
            {
                comboBox_teacher.DataSource = courseTeachers.getCourseTeachers(crId);
                comboBox_teacher.DisplayMember = "Name";
                comboBox_teacher.ValueMember = "Id";
            }
        }
    }
}
