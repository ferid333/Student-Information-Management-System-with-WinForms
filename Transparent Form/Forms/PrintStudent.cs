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
    public partial class PrintStudent : Form
    {
        StudentClass student = new StudentClass();
        DGVPrinter printer = new DGVPrinter();
        
        public PrintStudent()
        {
            InitializeComponent();
        }

        private void PrintStudent_Load(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand(
                               @"SELECT s.idstudent as Id, concat(s.StdFirstName, ' ', s.StdLastName) as Name, s.Birthdate, s.Gender,s.Phone,s.Address, m.MajorName as Major ,s.Photo 
                                  FROM `student` s
                                  LEFT JOIN `major` m ON s.MajorId = m.idmajor");

            showData(command);
        }

        
        public void showData(MySqlCommand command)
        {
            DataGridView_student.ReadOnly = true;
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            DataGridView_student.DataSource = student.getList(command);
          
            imageColumn = (DataGridViewImageColumn)DataGridView_student.Columns[7];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        private void button_search_Click(object sender, EventArgs e)
        {
           
            string selectQuery;
            if (radioButton_all.Checked)
            {
                selectQuery = "SELECT* FROM `student`";
            }
            else if (radioButton_male.Checked)
            {
                selectQuery = "SELECT * FROM `student` WHERE `Gender`='Male'";
            }
            else
            {
                selectQuery = "SELECT * FROM `student` WHERE `Gender`='Female'";
            }
            showData(new MySqlCommand(selectQuery));
        }

        private void button_print_Click(object sender, EventArgs e)
        {
            
            printer.Title = "Students list";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "foxlearn";
            printer.FooterSpacing = 15;
            printer.printDocument.DefaultPageSettings.Landscape = true;
            printer.PrintDataGridView(DataGridView_student);
        }
    }
}
