using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Transparent_Form
{
    class TeacherClass
    {
        DBconnect connect = new DBconnect();
        

        public bool insertTeacher(string fname, string lname, DateTime bdate, string email, string phone, string address,double salary, DateTime jdate ,byte[] img)
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `teacher`(`TchFirstName`, `TchLastName`, `Birthdate`, `Email`, `Phone`, `Address`, `Salary`,`JoiningDate`,`Photo`) VALUES(@fn, @ln, @bd, @eml, @ph, @adr,@slr,@jd, @img)", connect.getconnection);

     
            command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = fname;
            command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = lname;
            command.Parameters.Add("@bd", MySqlDbType.Date).Value = bdate;
            command.Parameters.Add("@eml", MySqlDbType.VarChar).Value = email;
            command.Parameters.Add("@ph", MySqlDbType.VarChar).Value = phone;
            command.Parameters.Add("@adr", MySqlDbType.VarChar).Value = address;
            command.Parameters.Add("@jd",MySqlDbType.Date).Value = jdate;
            command.Parameters.Add("@slr", MySqlDbType.Double).Value = salary;
            command.Parameters.Add("@img", MySqlDbType.Blob).Value = img;

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
      
        public DataTable getTeacherlist(MySqlCommand command)
        {
            command.Connection = connect.getconnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        public DataTable searchTeacher(string searchdata)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM `teacher` WHERE CONCAT(`TchFirstName`,`TchLastName`,`Address`,`Email`) LIKE '%" + searchdata + "%'", connect.getconnection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        
        public bool updateTeacher(int id, string fname, string lname, DateTime bdate, string email, string phone, string address, double salary, DateTime jdate, byte[] img)
        {
            MySqlCommand command = new MySqlCommand("UPDATE `teacher` SET `TchFirstName`=@fn, `TchLastName`=@ln, `Birthdate`=@bd, `Email`=@eml, `Phone`=@ph, `Address`=@adr, `JoiningDate`=@jd, `Salary`=@slr, `Photo`=@img WHERE `idteacher`= @id", connect.getconnection);


            command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = fname;
            command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = lname;
            command.Parameters.Add("@bd", MySqlDbType.Date).Value = bdate;
            command.Parameters.Add("@eml", MySqlDbType.VarChar).Value = email;
            command.Parameters.Add("@ph", MySqlDbType.VarChar).Value = phone;
            command.Parameters.Add("@adr", MySqlDbType.VarChar).Value = address;
            command.Parameters.Add("@jd", MySqlDbType.Date).Value = jdate;
            command.Parameters.Add("@slr", MySqlDbType.Double).Value = salary;
            command.Parameters.Add("@img", MySqlDbType.Blob).Value = img;
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
  
        public bool deleteTeacher(int id)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `teacher` WHERE `idteacher`=@id", connect.getconnection);

            
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
       
        public DataTable getList(MySqlCommand command)
        {
            command.Connection = connect.getconnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
    }
}
