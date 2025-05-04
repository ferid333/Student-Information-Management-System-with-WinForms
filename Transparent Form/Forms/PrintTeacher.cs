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
using MySql.Data.MySqlClient;

namespace Transparent_Form
{
    public partial class PrintTeacher : Form
    {
        TeacherClass teacher=new TeacherClass();
        DGVPrinter printer = new DGVPrinter();
        public PrintTeacher()
        {
            InitializeComponent();
        }

        private void button_print_Click(object sender, EventArgs e)
        {
            printer.Title = "Teachers list";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "foxlearn";
            printer.FooterSpacing = 15;
            printer.printDocument.DefaultPageSettings.Landscape = true;
            printer.PrintDataGridView(DataGridView_teacher);
        }

        private void PrintTeacher_Load(object sender, EventArgs e)
        {
            showData(new MySqlCommand("SELECT idteacher as Id, TchFirstName as Firstname, TchLastName as Lastname, Birthdate, Email, Phone, Address, Salary, JoiningDate, Photo FROM studentdb.teacher;"));
        }

        public void showData(MySqlCommand command)
        {
            DataGridView_teacher.ReadOnly = true;
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            DataGridView_teacher.DataSource = teacher.getList(command);
            
            imageColumn = (DataGridViewImageColumn)DataGridView_teacher.Columns[9];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            DataGridView_teacher.DataSource = teacher.searchTeacher(textBox_search.Text);
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn = (DataGridViewImageColumn)DataGridView_teacher.Columns[9];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            textBox_search.Clear();
        }
    }
}
