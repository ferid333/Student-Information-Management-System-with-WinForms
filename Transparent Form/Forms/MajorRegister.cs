using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Transparent_Form
{
    public partial class MajorRegister : Form
    {

        MajorClass major=new MajorClass();
        public MajorRegister()
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
        }
        private void MajorRegister_Load(object sender, EventArgs e)
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


        private void textBox_credits_TextChanged(object sender, EventArgs e)
        {
            if (!double.TryParse(textBox_credits.Text, out double value) && textBox_credits.Text.Length > 0)
            {
                textBox_credits.Text = textBox_credits.Text.Remove(textBox_credits.Text.Length - 1);
                textBox_credits.SelectionStart = textBox_credits.Text.Length;
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_Mname.Clear();
            textBox_credits.Clear();
            textBox_description.Clear();
            checkBox_isActive.Checked = true;
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            string mname = textBox_Mname.Text;
            int mcedits = Convert.ToInt32(textBox_credits.Text);
            string desc = textBox_description.Text;
            int misActive = checkBox_isActive.Checked ? 1 : 0;

             if (verify())
            {
                try
                {

                    if (major.insetMajor(mname, misActive, mcedits, desc))
                    {
                        showTable();
                        MessageBox.Show("New Major Added", "Add Major", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button_clear.PerformClick();
                    }
                }
                catch (Exception ex)

                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Empty Field", "Add Major", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

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
