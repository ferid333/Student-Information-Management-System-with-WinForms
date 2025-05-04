using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace Transparent_Form
{
    public partial class ManageMajorForm : Form
    {

        MajorClass major=new MajorClass();
        public ManageMajorForm()
        {
            InitializeComponent();
        }

        public void adjustDataGrid()
        {
            DataGridView_major.Columns["Id"].Width = 50;
            DataGridView_major.Columns["Active"].Width = 75;
            DataGridView_major.Columns["Credits"].Width = 75;
            DataGridView_major.Columns["Active"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridView_major.Columns["Credits"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public void showTable()
        {
            DataGridView_major.DataSource = null;
            MySqlCommand command = new MySqlCommand(
                                     @"SELECT 
                                        m.idmajor AS Id,
                                        m.MajorName AS Name,
                                        m.MajorCredits AS Credits,
                                        m.MajorDesc AS Description,
                                        m.IsActive as Active
                                      FROM `major` m");


            DataGridView_major.DataSource = major.getMajor(command);
            adjustDataGrid();


        }

        private void ManageMajorForm_Load(object sender, EventArgs e)
        {
            showTable();
        }

        bool verify()
        {
            if ((textBox_Mname.Text == "") || (textBox_credits.Text == "") ||
                (textBox_description.Text == ""))
            {
                return false;
            }
            else
                return true;
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_Mname.Clear();
            textBox_credits.Clear();
            textBox_description.Clear();
            checkBox_isActive.Checked = true;
        }

        private void DataGridView_major_Click(object sender, EventArgs e)
        {
            if (DataGridView_major.CurrentRow != null)
            {

                textBox_majorId.Text = DataGridView_major.CurrentRow.Cells[0].Value.ToString();
                textBox_Mname.Text = DataGridView_major.CurrentRow.Cells[1].Value.ToString();
                textBox_credits.Text = DataGridView_major.CurrentRow.Cells[2].Value.ToString();
                textBox_description.Text = DataGridView_major.CurrentRow.Cells[3].Value.ToString();
                checkBox_isActive.Checked = Convert.ToInt32(DataGridView_major.CurrentRow.Cells[4].Value) == 1;
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (!verify())
            {
                MessageBox.Show("Major is not selected", "Delete Major", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                int id = Convert.ToInt32(textBox_majorId.Text);
                if (MessageBox.Show("Are you sure you want to remove this major", "Delete Major", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (major.deleteMajor(id))
                    {
                        showTable();
                        MessageBox.Show("Major Removed", "Delete Major", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button_clear.PerformClick();
                    }
                }
            }
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            if(!verify())
            {
                MessageBox.Show("Need major data", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int mId = Convert.ToInt32(textBox_majorId.Text);
                string mName = textBox_Mname.Text;
                int mCredits = Convert.ToInt32(textBox_credits.Text);
                string mDesc =textBox_description.Text;
                int misActive = checkBox_isActive.Checked ? 1 : 0;

                if (major.updatemajor(mId, mName, misActive, mCredits, mDesc))
                {
                    showTable();
                    button_clear.PerformClick();
                    MessageBox.Show("Major Edited Complete", "Update Major", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Major is not edited", "Update Major", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
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
            adjustDataGrid();
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
