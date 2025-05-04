using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.ListBox;

namespace Transparent_Form
{
    class CourseTeachersClass
    {
        DBconnect connect = new DBconnect();
        
        public bool insertCourseTeacher(int courseid, ListBox.SelectedObjectCollection teachers)
        {
            try
            {
                connect.openConnect();

                foreach (KeyValuePair<int, string> teacher in teachers)
                {
                    using (MySqlCommand command = new MySqlCommand("INSERT INTO `courseteachers`(`CourseId`, `TeacherId`) VALUES (@cid, @tid)", connect.getconnection))
                    {
                        command.Parameters.Add("@cid", MySqlDbType.Int32).Value = courseid;
                        command.Parameters.Add("@tid", MySqlDbType.Int32).Value = teacher.Key;
                        command.ExecuteNonQuery();
                    }
                }
                connect.closeConnect();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }


        public DataTable getCourseTeachers(int crId)
        {
            MySqlCommand command = new MySqlCommand(
                    @"SELECT t.idteacher as Id, CONCAT(t.TchFirstName, ' ', t.TchLastName) AS Name
                    FROM studentdb.course c
                    LEFT JOIN studentdb.courseteachers ct ON ct.courseid = c.idcourse
                    LEFT JOIN studentdb.teacher t ON t.idteacher = ct.TeacherID
                    WHERE c.idcourse = @cid",
                    connect.getconnection);

            command.Parameters.Add("@cid", MySqlDbType.VarChar).Value= crId;

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public bool updateCourseTeacher(int courseid, ListBox.SelectedObjectCollection teachers)
        {
            try
            {
                connect.openConnect();

                using (MySqlCommand deleteCommand = new MySqlCommand("DELETE FROM courseteachers WHERE CourseId = @cid", connect.getconnection))
                {
                    deleteCommand.Parameters.Add("@cid", MySqlDbType.Int32).Value = courseid;
                    deleteCommand.ExecuteNonQuery();
                }

                if(insertCourseTeacher(courseid,teachers))
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }
    }
}
