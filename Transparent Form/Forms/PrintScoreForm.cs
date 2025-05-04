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
using DGVPrinterHelper;


namespace Transparent_Form
{
    public partial class PrintScoreForm : Form
    {
        ScoreClass score = new ScoreClass();
        DGVPrinter printer = new DGVPrinter();
        public PrintScoreForm()
        {
            InitializeComponent();
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
                            sc.Description) LIKE '%" + textBox_search.Text + "%'";
            DataGridView_score.DataSource = score.getList(new MySqlCommand(searchQuery));
        }

        private void button_print_Click(object sender, EventArgs e)
        {
        
            printer.Title = "Student score list";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = UniversityClass.Instance.uName;
            printer.FooterSpacing = 15;
            printer.printDocument.DefaultPageSettings.Landscape = true;
            printer.PrintDataGridView(DataGridView_score);
        }

        private void PrintScoreForm_Load(object sender, EventArgs e)
        {
            showTable();
        }
       
        public void showTable()
        {
            DataGridView_score.DataSource = score.getList(new MySqlCommand("Select idscore as Id, concat(s.StdFirstName, ' ', s.StdLastName) as Student_Name, c.CourseName as Course, concat(t.TchFirstName, ' ', t.TchLastName) as Teacher_Name, sc.Score, sc.Description from score sc\r\nLeft Join teacher t on t.idteacher=sc.TeacherId\r\nLeft Join student s on s.idstudent=sc.StudentId\r\nLeft Join course c on c.idcourse=sc.CourseId"));
            DataGridView_score.Columns["Id"].Width = 100;
            DataGridView_score.Columns["Score"].Width = 100;
            DataGridView_score.Columns["Student_Name"].HeaderText = "Student Name";
            DataGridView_score.Columns["Teacher_Name"].HeaderText = "Teacher Name";
        }
    }
}
