using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transparent_Form
{
    class ScoreClass
    {
        DBconnect connect = new DBconnect();
       
        public bool insertScore(int stdid, int cId,int tchId, double scor, string desc)
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `score`(`StudentId`, `CourseId`,`TeacherId` ,`Score`, `Description`) VALUES (@stid,@cid,@tchid,@sco,@desc)", connect.getconnection);
            
            command.Parameters.Add("@stid", MySqlDbType.Int32).Value = stdid;
            command.Parameters.Add("@cid", MySqlDbType.Int32).Value = cId;
            command.Parameters.Add("@tchid", MySqlDbType.Int32).Value = tchId;
            command.Parameters.Add("@sco", MySqlDbType.Double).Value = scor;
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
        
        public DataTable getList(MySqlCommand command)
        {
            command.Connection = connect.getconnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        
        public bool checkScore(int stdId, int cId)
        {
            DataTable table = getList(new MySqlCommand("SELECT * FROM `score` WHERE `StudentId`= '" + stdId + "' AND `CourseId`= '" + cId + "'"));
            if (table.Rows.Count > 0)
            { return true; }
            else
            { return false; }
        }
       
        public bool updateScore(int sId, int stdId, int cId, int tchId, double scor, string desc)
        {
            MySqlCommand command = new MySqlCommand("UPDATE `score` SET `Score`=@sco,`Description`=@desc ,`TeacherId`=@tchid, `StudentId`=@stdid, `CourseId`=@cid WHERE `idscore`=@sid", connect.getconnection);

            command.Parameters.Add("@sid", MySqlDbType.Int32).Value = sId;
            command.Parameters.Add("@stdid", MySqlDbType.Int32).Value = stdId;
            command.Parameters.Add("@cid", MySqlDbType.Int32).Value = cId;
            command.Parameters.Add("@tchid", MySqlDbType.Int32).Value = tchId;
            command.Parameters.Add("@sco", MySqlDbType.Double).Value = scor;
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
       
        public bool deleteScore(int id)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `score` WHERE `idscore`=@id", connect.getconnection);

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
