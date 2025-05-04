using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Transparent_Form
{
    class UserClass
    {
        DBconnect connect = new DBconnect();

        public bool insertUser(string username, string password)
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `user`(`username`, `password`) VALUES(@username, @password)", connect.getconnection);

            //@fn, @ln, @bd, @gd, @ph, @adr, @img
            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;
            connect.openConnect();
            if (command.ExecuteNonQuery() == 1)
            {
                connect.closeConnect();
                return true;
            }
            else
            {
                connect.closeConnect();
                return false;
            }

        }

        public DataTable getUserlist(MySqlCommand command)
        {
            command.Connection = connect.getconnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public string getUserPassword(MySqlCommand command)
        {
            string password = "";

            try
            {
                command.Connection = connect.getconnection;
                connect.getconnection.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    password = result.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connect.getconnection.Close();
            }

            return password;
        }

       
        public bool updateUser(int id, string username, string password)
        {
            MySqlCommand command = new MySqlCommand("UPDATE `user` SET `username`=@username,`password`=@password WHERE  `iduser`= @id", connect.getconnection);

            //@id,@fn, @ln, @bd, @gd, @ph, @adr, @img
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;
            connect.openConnect();
            if (command.ExecuteNonQuery() == 1)
            {
                connect.closeConnect();
                return true;
            }
            else
            {
                connect.closeConnect();
                return false;
            }

        }
        public bool deleteUser(int id)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `user` WHERE `iduser`=@id", connect.getconnection);

            //@id
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

            connect.openConnect();
            if (command.ExecuteNonQuery() == 1)
            {
                connect.closeConnect();
                return true;
            }
            else
            {
                connect.closeConnect();
                return false;
            }

        }
    }
}
