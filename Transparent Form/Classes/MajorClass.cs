using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace Transparent_Form
{
    public class MajorClass
    {
        DBconnect connect = new DBconnect();

        public bool insetMajor(string mName, int isActive, int mCredits, string desc)
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `major`(`MajorName`, `IsActive`, `MajorCredits`,`MajorDesc`) VALUES (@cn,@ict,@mcrd,@desc)", connect.getconnection);

            command.Parameters.Add("@cn", MySqlDbType.VarChar).Value = mName;
            command.Parameters.Add("@ict", MySqlDbType.Byte).Value = isActive;
            command.Parameters.Add("@mcrd", MySqlDbType.Int32).Value = mCredits;
            command.Parameters.Add("@desc", MySqlDbType.VarChar).Value = desc;
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


        public DataTable getMajor(MySqlCommand command)
        {
            command.Connection = connect.getconnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public bool updatemajor(int id, string mName, int isActive, int mCredits, string desc)
        {
            MySqlCommand command = new MySqlCommand("UPDATE `major` SET `MajorName`=@mname, `IsActive`=@active, `MajorCredits`=@credits, `MajorDesc`=@desc WHERE `idmajor`=@id",connect.getconnection);

            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            command.Parameters.Add("@mname", MySqlDbType.VarChar, 45).Value = mName;
            command.Parameters.Add("@active", MySqlDbType.Byte).Value = isActive;
            command.Parameters.Add("@credits", MySqlDbType.Int32).Value = mCredits;
            command.Parameters.Add("@desc", MySqlDbType.VarChar, 100).Value = desc;
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
        public bool deleteMajor(int id)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `major` WHERE `idmajor`=@id", connect.getconnection);
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
