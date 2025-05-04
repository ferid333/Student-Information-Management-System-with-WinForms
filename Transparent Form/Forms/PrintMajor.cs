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
    public partial class PrintMajor : Form
    {
        MajorClass major=new MajorClass();
        DGVPrinter printer = new DGVPrinter();
        public PrintMajor()
        {
            InitializeComponent();
        }

        public void showTable()
        {

            MySqlCommand command = new MySqlCommand(
                                     @"SELECT 
                                        m.idmajor AS Id,
                                        m.MajorName AS Name,
                                        m.MajorCredits AS Credits,
                                        m.MajorDesc AS Description,
                                        m.IsActive as Active
                                      FROM `major` m");


            DataGridView_major.DataSource = major.getMajor(command);

            DataGridView_major.Columns["Id"].Width = 50;
            DataGridView_major.Columns["Active"].Width = 75;
            DataGridView_major.Columns["Credits"].Width = 75;
            DataGridView_major.Columns["Active"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridView_major.Columns["Credits"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //DataGridView_major.CellFormatting += (sender, e) =>
            //{
              
            //};
        }

        private void PrintMajor_Load(object sender, EventArgs e)
        {
            showTable();
        }

        private void button_print_Click(object sender, EventArgs e)
        {
            printer.Title = "University major list";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = UniversityClass.Instance.uName;
            printer.FooterSpacing = 15;
            printer.printDocument.DefaultPageSettings.Landscape = true;
            printer.PrintDataGridView(DataGridView_major);
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            string searchQuery = @"SELECT 
                                        m.idmajor AS Id,
                                        m.MajorName AS Name,
                                        m.MajorCredits AS Credits,
                                        m.MajorDesc AS Description,
                                        m.IsActive as Active
                     FROM major m
                     WHERE CONCAT(m.MajorName, ' ', m.MajorCredits, ' ',
                           m.MajorDesc) LIKE '%" + textBox_search.Text + "%'";
            DataGridView_major.DataSource = major.getMajor(new MySqlCommand(searchQuery));

            DataGridView_major.Columns["Id"].Width = 50;
            DataGridView_major.Columns["Active"].Width = 75;
            DataGridView_major.Columns["Credits"].Width = 75;
            DataGridView_major.Columns["Active"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridView_major.Columns["Credits"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void DataGridView_major_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == DataGridView_major.Columns["Active"].Index && e.Value != null)
            {
                bool isActive = Convert.ToBoolean(e.Value);
                e.Value = isActive ? "✅" : "X";
                e.CellStyle.ForeColor = isActive ? Color.Green : Color.Red;
                e.FormattingApplied = true;
            }
        }
    }
}
