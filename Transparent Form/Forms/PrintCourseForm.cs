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
using DGVPrinterHelper;

namespace Transparent_Form
{
    public partial class PrintCourseForm : Form
    {
        CourseClass course = new CourseClass();
        DGVPrinter printer = new DGVPrinter();
        public PrintCourseForm()
        {
            InitializeComponent();
        }

        private void button_search_Click(object sender, EventArgs e)
        {

            string query = "SELECT c.idcourse AS Id, c.coursename AS Name, c.CourseHour AS Hours, c.Description, GROUP_CONCAT(CONCAT(t.TchFirstName, ' ', t.TchLastName) SEPARATOR ', ') AS Teachers FROM studentdb.course c LEFT JOIN studentdb.courseteachers ct ON ct.courseid = c.idcourse LEFT JOIN studentdb.teacher t ON t.idteacher = ct.TeacherID GROUP BY c.idcourse, c.coursename, c.CourseHour, c.Description HAVING CONCAT(c.coursename, ' ', c.Description, ' ', COALESCE(GROUP_CONCAT(CONCAT(t.TchFirstName, ' ', t.TchLastName) SEPARATOR ', '), '')) LIKE '%" + textBox_search.Text + "%'";
            DataGridView_course.DataSource = course.getCourse(new MySqlCommand(query));
            DataGridView_course.Columns["Hours"].Width = 100;
            textBox_search.Clear();
        }

        private void PrintCourseForm_Load(object sender, EventArgs e)
        {
            DataGridView_course.DataSource = course.getCourse(new MySqlCommand("Select idcourse as Id,coursename as Name,CourseHour as Hours,Description,GROUP_CONCAT(CONCAT(t.TchFirstName, ' ', t.TchLastName) SEPARATOR ', ') AS Teachers\r\nfrom studentdb.course c\r\nleft Join studentdb.courseteachers ct on ct.courseid=c.idcourse\r\nleft Join studentdb.teacher t on t.idteacher=ct.TeacherID\r\nGroup by idcourse;"));
            DataGridView_course.Columns["Hours"].Width = 100;
        }

        private void button_print_Click(object sender, EventArgs e)
        {
            printer.Title = "Mdemy Course list";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "Mdemy";
            printer.FooterSpacing = 15;
            printer.printDocument.DefaultPageSettings.Landscape = true;
            printer.PrintDataGridView(DataGridView_course);
        }

    }
}
