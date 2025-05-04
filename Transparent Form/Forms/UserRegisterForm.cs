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
    public partial class UserRegisterForm : Form
    {
        UserClass user=new UserClass();

        int isUpdateUser=0;
        public UserRegisterForm()
        {
            InitializeComponent();
        }

        bool verify()
        {
            if ((textBox_username.Text == "") || (textBox_password.Text == ""))
            {
                return false;
            }
            else
                return true;
        }

        private void UserRegisterForm_Load(object sender, EventArgs e)
        {
            showTable();
        }
        public void showTable()
        {
            DataGridView_user.DataSource = user.getUserlist(new MySqlCommand("SELECT iduser, username FROM `user`"));
        }


        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_password.Clear();
            textBox_username.Clear();
            covertAdding();
        }

        private void covertAdding()
        {
            textBox_password.Clear();
            textBox_username.Clear();
            button_clear.Visible = false;
            button_delete.Visible = false;
            isUpdateUser = 0;
            button_add.Text = "Add";
        }

        private void button_add_Click_1(object sender, EventArgs e)
        {
            // add new user
            string username = textBox_username.Text;
            string password = textBox_password.Text;

            if (isUpdateUser == 0)
            {
                if (verify())
                {
                    try
                    {
                        if (user.insertUser(username, password))
                        {
                            showTable();
                            MessageBox.Show("New User Added", "Add User", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)

                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Empty Field", "Add User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                if (verify())
                {
                    try
                    {
                        if (user.updateUser(isUpdateUser,username,password))
                        {
                            showTable();
                            covertAdding();
                            MessageBox.Show("User Updated", "Update User", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)

                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Empty Field", "Update User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        private void DataGridView_user_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string username = DataGridView_user.CurrentRow.Cells[1].Value.ToString();
            int id= Convert.ToInt32(DataGridView_user.CurrentRow.Cells[0].Value);
            textBox_username.Text = username;
            MySqlCommand cmd = new MySqlCommand("SELECT password FROM user WHERE username = @user");
            cmd.Parameters.AddWithValue("@user", username);
            textBox_password.Text = user.getUserPassword(cmd);

            button_clear.Visible = true;
            button_delete.Visible = true;
            isUpdateUser = id;
            button_add.Text = "Update";
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove this user", "Remove User", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (user.deleteUser(isUpdateUser))
                {
                    showTable();
                    covertAdding();
                    MessageBox.Show("User Removed", "Remove User", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
            }
        }
    }
}
