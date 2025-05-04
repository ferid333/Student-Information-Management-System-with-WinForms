using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Transparent_Form.Services;

namespace Transparent_Form
{
    public partial class MainForm : Form
    {
        StudentClass student = new StudentClass();
        CourseClass course = new CourseClass();
        private readonly WeatherService _weatherService = new WeatherService();
        private readonly LocationService _locationService = new LocationService();
        public string LoggedInUsername { get; set; }
        public MainForm()
        {
            InitializeComponent();
            customizeDesign();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            studentCount();

            label_user.Text = LoggedInUsername;
          
            comboBox_course.DataSource = course.getCourse(new MySqlCommand("SELECT idcourse as Id , CourseName as Name FROM `course`"));
            comboBox_course.DisplayMember = "Name";
            comboBox_course.ValueMember = "Id";


            try
            {
                var (lat, lon) = await _locationService.GetCoordinatesAsync(UniversityClass.Instance.uAddress);
                string weather = await _weatherService.GetCurrentWeatherAsync(lat, lon);
                label_weather.Text = weather;
                label_weather.Visible = true;
            }
            catch
            {
                label5.Text = "Weather service offline";
            }
        }

      
        private void studentCount()
        {
            
            label_totalStd.Text = "Total Students : " + student.totalStudent();
            label_maleStd.Text = "Male : " + student.maleStudent();
            label_femaleStd.Text = "Female : " + student.femaleStudent();

        }


        private void customizeDesign()
        {
            panel_stdsubmenu.Visible = false;
            panel_courseSubmenu.Visible = false;
            panel_scoreSubmenu.Visible = false;
            panel_userUniSubMenu.Visible = false;
            panel_teacherSubMenu.Visible = false;
            panel_majorSubMenu.Visible = false;
            label2.Text=UniversityClass.Instance.uName;
            byte[] img = UniversityClass.Instance.uImage;
            using (MemoryStream ms = new MemoryStream(img))
            {
                pictureBox_icon.Image = Image.FromStream(ms);
            }
        }

        private void hideSubmenu()
        {
            if (panel_stdsubmenu.Visible == true)
                panel_stdsubmenu.Visible = false;
            if (panel_courseSubmenu.Visible == true)
                panel_courseSubmenu.Visible = false;
            if (panel_scoreSubmenu.Visible == true)
                panel_scoreSubmenu.Visible = false;
            if (panel_userUniSubMenu.Visible == true)
                panel_userUniSubMenu.Visible = false;
            if (panel_teacherSubMenu.Visible == true)
                panel_teacherSubMenu.Visible = false;
            if (panel_majorSubMenu.Visible == true)
                panel_majorSubMenu.Visible = false;
        }

        private void showSubmenu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                hideSubmenu();
                submenu.Visible = true;
            }
            else
                submenu.Visible = false;
        }

        private void button_std_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_stdsubmenu);
        }
        #region StdSubmenu
        private void button_registration_Click(object sender, EventArgs e)
        {
            openChildForm(new RegisterForm());
            hideSubmenu();
            
        }

        private void button_manageStd_Click(object sender, EventArgs e)
        {
            openChildForm(new ManageStudentForm());
            hideSubmenu();
        }

        private void button_status_Click(object sender, EventArgs e)
        {
            hideSubmenu();
        }

        private void button_stdPrint_Click(object sender, EventArgs e)
        {
            openChildForm(new PrintStudent());
            hideSubmenu();
        }

        #endregion StdSubmenu

        private void button_teacher_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_teacherSubMenu);
        }
        #region TeacherSubMenu
        private void button_teacherReg_Click(object sender, EventArgs e)
        {
            openChildForm(new TeacherRegisterForm());
            hideSubmenu();
        }

        private void button_teacherMng_Click(object sender, EventArgs e)
        {
            openChildForm(new ManageTeacherForm());
            hideSubmenu();
        }

        private void button_teacherPrt_Click(object sender, EventArgs e)
        {
            openChildForm(new PrintTeacher());
            hideSubmenu();
        }

        #endregion TeacherSubMenu
        private void button_course_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_courseSubmenu);
        }
        #region CourseSubmenu
        private void button_newCourse_Click(object sender, EventArgs e)
        {
            openChildForm(new CourseForm());
            //...
            //..Your code
            //...
            hideSubmenu();
        }

        private void button_manageCourse_Click(object sender, EventArgs e)
        {
            openChildForm(new ManageCourseForm());
            //...
            //..Your code
            //...
            hideSubmenu();
        }

        private void button_coursePrint_Click(object sender, EventArgs e)
        {
            openChildForm(new PrintCourseForm());
            //...
            //..Your code
            //...
            hideSubmenu();
        }
        #endregion CourseSubmenu

        private void button_score_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_scoreSubmenu);
        }
        
        #region ScoreSubmenu
        private void button_newScore_Click(object sender, EventArgs e)
        {
            openChildForm(new ScoreForm());
            hideSubmenu();
        }

        private void button_manageScore_Click(object sender, EventArgs e)
        {
            openChildForm(new ManageScoreForm());
            
            hideSubmenu();
        }

        private void button_scorePrint_Click(object sender, EventArgs e)
        {
            openChildForm(new PrintScoreForm());
            hideSubmenu();
        }


        #endregion ScoreSubmenu

        private void button_major_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_majorSubMenu);
        }
        #region MajorSubMenu
        private void button_newMajor_Click(object sender, EventArgs e)
        {
            openChildForm(new MajorRegister());
            hideSubmenu();
        }

        private void button_manageMajor_Click(object sender, EventArgs e)
        {
            openChildForm(new ManageMajorForm());

            hideSubmenu();
        }

        private void button_majorPrint_Click(object sender, EventArgs e)
        {
            openChildForm(new PrintMajor());
            hideSubmenu();
        }

        #endregion MajorSubMenu
        private void button_userUni_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_userUniSubMenu);
        }

        #region User / Uni
        private void button_user_Click(object sender, EventArgs e)
        {
            openChildForm(new UserRegisterForm());
            hideSubmenu();
        }
        private void button_uni_Click(object sender, EventArgs e)
        {
            openChildForm(new UniversitySettingsForm());
            hideSubmenu();

        }
        #endregion User / Uni
        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panel_main.Controls.Add(childForm);
            panel_main.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            
        }



        private void comboBox_course_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string queryM = $@"
            //    SELECT COUNT(*) 
            //    FROM student s
            //    LEFT JOIN score sc ON s.idstudent = sc.StudentId
            //    LEFT JOIN course c ON sc.CourseId = c.idcourse
            //    WHERE s.Gender = 'Male' AND c.idcourse ={Convert.ToInt32(comboBox_course.SelectedValue)}";

            //string queryF = $@"
            //        SELECT COUNT(*) 
            //        FROM student s
            //        LEFT JOIN score sc ON s.idstudent = sc.StudentId
            //        LEFT JOIN course c ON sc.CourseId = c.idcourse
            //        WHERE s.Gender = 'Female' AND c.idcourse ={Convert.ToInt32(comboBox_course.SelectedValue)}";
            //label_cmale.Text = "Male : " + student.exeCount(queryM);
            //label_cfemale.Text = "Female : " + student.exeCount(queryF);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button_dashboard_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
                activeForm.Close();
            panel_main.Controls.Add(panel_cover);
            customizeDesign();
            studentCount();
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            this.Hide();
            login.Show();
        }

        private void comboBox_course_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(comboBox_course.SelectedValue?.ToString(), out int courseid))
            {
               
                return;
            }
            string queryM = $@"
                SELECT COUNT(*) 
                FROM student s
                LEFT JOIN score sc ON s.idstudent = sc.StudentId
                LEFT JOIN course c ON sc.CourseId = c.idcourse
                WHERE s.Gender = 'Male' AND c.idcourse ={Convert.ToInt32(comboBox_course.SelectedValue)}";

            string queryF = $@"
                    SELECT COUNT(*) 
                    FROM student s
                    LEFT JOIN score sc ON s.idstudent = sc.StudentId
                    LEFT JOIN course c ON sc.CourseId = c.idcourse
                    WHERE s.Gender = 'Female' AND c.idcourse ={Convert.ToInt32(comboBox_course.SelectedValue)}";
            label_cmale.Text = "Male : " + student.exeCount(queryM);
            label_cfemale.Text = "Female : " + student.exeCount(queryF);
        }

      
    }
}