using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_monitor
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
            //ping Local DB and return message
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


       
        public string write_data (Monitor_models input,string asset,string resou)
        {
            string result = "";

            string cmdText = "insert into monitor_log (manu,ictag,monitor_ID,serial,size,resou,model) values ('" + input.Manafacture + "','" + asset + "','" + input.MonitorID + "','" + input.SerialNumber+"','"+input.SizeDiaginch+"','"+resou+"','"+input.Model+"') on duplicate key update serial = '"+input.SerialNumber+"',monitor_ID = '"+input.MonitorID+"', model = '"+input.Model+"'";
            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }


            return result;
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

    }
}
