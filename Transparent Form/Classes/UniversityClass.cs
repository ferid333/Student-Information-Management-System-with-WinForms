using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Transparent_Form
{
    public class UniversityClass
    {

        public static UniversityClass _instance;

        private static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\config.json");
        public string uName { get; set; }
        public string uAddress { get; set; }
        public byte[] uImage { get; set; }

        public UniversityClass()
        {          
        }

        public static UniversityClass Instance
        {

            get
            {
                if (_instance == null)
                {
                    _instance = new UniversityClass();
                    _instance.LoadSettings();
                }
                   
                return _instance;
            }
        }

        public static void updateUniversity(string uName, string uAddress, byte[] uImage)
        {
            _instance.uName = uName;
            _instance.uAddress = uAddress;
            _instance.uImage = uImage;

            _instance.SaveSettings();
        }

        public void LoadSettings()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    _instance = JsonConvert.DeserializeObject<UniversityClass>(json);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading settings: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("File Error ");
            }
        }

        public void SaveSettings()
        {
            try
            {
                string json = JsonConvert.SerializeObject(this);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving settings: " + ex.Message);
            }
        }
    }
}
