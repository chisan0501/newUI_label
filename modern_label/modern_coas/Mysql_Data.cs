using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace modern_coas
{
    class Mysql_Data
    {
        public static MySqlConnection conn;

        public Mysql_Data(string source)
        {
            conn = new MySqlConnection();

            if (source == "Local DB")
            {
               
                conn.ConnectionString = ConfigurationManager.AppSettings["LocalMySqlConnectionString"];
            }
            else
            {

                conn.ConnectionString = ConfigurationManager.AppSettings["OnlineMySqlConnectionString"];
            }
        }

        public string localDB_ping()
        {
            string is_connect = "Local DB : Offline";
            try
            {
                conn.ConnectionString = ConfigurationManager.AppSettings["LocalMySqlConnectionString"];
                conn.Open();
                is_connect = "Local DB : Online";
                conn.Close();

            }
            catch
            {

            }


            return is_connect;

        }

        public string live_ping()
        {
            string is_connect = "MySQL DB : Offline";

            try
            {





                conn.ConnectionString = ConfigurationManager.AppSettings["OnlineMySqlConnectionString"];


                conn.Open();
                is_connect = "MySQL DB : Online";

                conn.Close();

            }
            catch
            {

            }
            return is_connect;
        }

        public void change_connection_string(string source)
        {

            if (source == "Local DB")
            {

                conn.ConnectionString = ConfigurationManager.AppSettings["LocalMySqlConnectionString"];
            }
            else
            {

                conn.ConnectionString = ConfigurationManager.AppSettings["OnlineMySqlConnectionString"];
            }
        }
        public List<String> Station()
        {
            List<String> station_list = new List<string>();
            

           
            String cmdText = "SELECT distinct station_name from station_setting";
            //      MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();

            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    station_list.Add(reader["station_name"].ToString());
                }
                conn.Close();

            }

          
            return station_list;

        }
    }
}
