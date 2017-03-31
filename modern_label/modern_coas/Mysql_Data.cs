using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Diagnostics;

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

        public static string get_script(string script_name)
        {
            string result = "";


            conn.Open();
            string cmdText = "select content from scripts where name ='"+script_name+"'";

            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                    {
                        result = ((reader["content"].ToString()));
                    }
                    conn.Close();

            return result;


        }
       
        public void Query_cleanup(string windows_coa, string office_coa)
        {
            //insert current coas to coas histroy table and remove current coas from coas table
            try
            {
               
                String cmdText = "Insert into coas_history select * from coas WHERE PK = '" + windows_coa + "' ON DUPLICATE KEY Update License_Type = 'Citizenship' ";
                String delText = "Delete from coas where PK = '" + windows_coa + "'";
                String cmdText2 = "Insert into coas_history select * from coas WHERE PK = '" + office_coa + "' ON DUPLICATE KEY Update License_Type = 'Citizenship'";
                String delText2 = "Delete from coas where PK = '" + office_coa + "'";
                
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                using (MySqlCommand cmd = new MySqlCommand(delText, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                using (MySqlCommand cmd = new MySqlCommand(cmdText2, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                using (MySqlCommand cmd = new MySqlCommand(delText2, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
               
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
       
        public coa_obj get_data(string channel,string wcoa_location,string ocoa_location)
        {
            //search by serial for sku that was assigned to this computer

            coa_obj result = new coa_obj();
            MySqlCommand command = conn.CreateCommand();
            conn.Open();
            String cmdText = "Select (SELECT PK from coas where Recipient_Organization_Name = '" + channel + "' AND Recipient_Type !='USED' AND Product_Name like '%Windows%' AND location = '" + wcoa_location + "' order by row_id Asc limit 1) as Windows_COA, (SELECT COA_ID from coas where Recipient_Organization_Name = '" + channel + "' AND Recipient_Type !='USED' AND Product_Name like '%Windows%' AND location = '" + wcoa_location + "' order by row_id Asc limit 1 ) as Windows_COA_ID, (SELECT PK from coas where Recipient_Organization_Name = '" + channel + "' AND Recipient_Type !='USED' AND Product_Name like '%Office%' AND location = '" + ocoa_location + "' order by row_id Asc limit 1 ) as Office_COA , (SELECT COA_ID from coas where Recipient_Organization_Name = '" + channel + "' AND Recipient_Type !='USED' AND Product_Name like '%Office%' AND location = '" + ocoa_location + "' order by row_id Asc limit 1 ) as Office_COA_ID , (SELECT Count(*) from coas where Recipient_Organization_Name = '" + channel + "' AND Recipient_Type !='USED' AND Product_Name like '%Office%' AND location = '" + ocoa_location + "') as office_count , (SELECT Count(*) from coas where Recipient_Organization_Name = '" + channel + "' AND Recipient_Type !='USED' AND Product_Name like '%Windows%' AND location = '" + wcoa_location + "') as windows_count";
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {



                result.wcoa = (reader["Windows_COA"].ToString());
                result.wcoa_id = (reader["Windows_COA_ID"].ToString());
                result.wcoa_count = int.Parse((reader["windows_count"].ToString()));
                result.ocoa = (reader["Office_COA"].ToString());
                result.ocoa_id = (reader["Office_COA_ID"].ToString());
                result.ocoa_count = int.Parse((reader["office_count"].ToString()));
                
            

            }
            conn.Close();

            return result;
        }
        public obj_on_load sku_assigned (string serial)
        {

            var result = new obj_on_load();
            MySqlCommand command = conn.CreateCommand();
            conn.Open();
            String cmdText = "SELECT * FROM rediscovery where serial='" + serial + "' order by time desc limit 1";
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
             
                result.model = (reader["model"].ToString());
                result.cpu = (reader["cpu"].ToString());
                result.hdd = (reader["hdd"].ToString());
                result.manu = (reader["brand"].ToString());
                result.ram = (reader["ram"].ToString());
                result.serial = (reader["serial"].ToString());
                result.sku = (reader["pallet"].ToString());
                result.ictag = (reader["ictag"].ToString());
                result.pre_coa = (reader["pre_coa"].ToString());

            }
            conn.Close();


            return result;

        }
        

       
        public void report(obj_on_load asset, string wcoa, string ocoa,string station)
        {





            string screen = "";
            if (station == "Station 1")
            {
                screen = MainModel.screen();
            }
            else
            {
                screen = "NA";
            }



            try
            {







                conn.Open();
                String cmdText = "INSERT INTO production_log (pre_coa, time, wcoa, ocoa , Manufacture, Model, CPU , RAM , HDD , serial, channel, location,video_card,screen_size,ictags) VALUES ('" + asset.pre_coa + "','" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "','" + wcoa + "','" + ocoa + "','" + asset.manu + "','" + asset.model + "','" + asset.cpu + "','" + asset.ram + "MB','" + asset.hdd + "GB', '" + asset.serial + "','" + asset.sku + "','" + station + "','" + MainModel.video_card() + "','" + screen + "','" + asset.ictag + "') ON DUPLICATE KEY UPDATE time = '"+ DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "'";
                MySqlCommand cmd = new MySqlCommand(cmdText, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {

            }

        }
        public List<string> get_next_wcoa(string channel_name,string station)
        {
             switch (station)
            {
                case "Station 1":
                    station = "wcoa_s1";
                    break;
                case "Station 2":
                    station = "wcoa_s2";
                    break;
                case "Retail":
                    station = "wcoa_r";
                    break;
                default:
                    break;
            }

                
            List<string> result = new List<string>();

            string cmdText = "SELECT pk from coas where Recipient_Organization_Name ='"+channel_name+ "' AND Recipient_Type !='USED' AND Product_Name like '%Windows%' AND location = '" + station + "' order by row_id Asc limit 5";
            
            //      MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();

            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    
                    result.Add(reader["pk"].ToString());
                }
                conn.Close();

            }

            return result;
        }
        public List<string> get_next_ocoa(string channel_name, string station)
        {
            switch (station)
            {
                case "Station 1":
                    station = "ocoa_s1";
                    break;
                case "Station 2":
                    station = "ocoa_s2";
                    break;
                case "Retail":
                    station = "ocoa_r";
                    break;
                default:
                    break;
            }


            List<string> result = new List<string>();

            string cmdText = "SELECT pk from coas where Recipient_Organization_Name ='" + channel_name + "' AND Recipient_Type !='USED' AND Product_Name like '%Office%' AND location = '" + station + "' order by row_id Asc limit 5";

            //      MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();

            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {

                    result.Add(reader["pk"].ToString());
                }
                conn.Close();

            }

            return result;
        }
        public Dictionary<string,string> get_channel_list()
        {
            var result = new Dictionary<string, string>();

            String cmdText = "SELECT channel_name,full_name from setting where background != 'false' AND background != ''";
            //      MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();

            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    result.Add((reader["channel_name"].ToString()), (reader["full_name"].ToString()));
                  
                }
                conn.Close();

            }

            return result;
        }


      

        public bool resuse_coa (string coa_id)
        {
            bool sucess = false;
            try
            {
                
                conn.Open();
                string cmdText = "insert into coas select * from coas_history where COA_ID='" + coa_id + "'";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                   
                
                sucess = true;
            }
            catch(Exception e)
            {
                
            }

            return sucess;
        }

        public string search_coa (string coa_id)
        {
            string result = "";

            try
            {
                String cmdText = "SELECT pk from coas_history where COA_ID = '"+coa_id+"' limit 1";
                //      MySqlConnection conn = new MySqlConnection(connStr);
                MySqlCommand command = conn.CreateCommand();

                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {
                    MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                    while (reader.Read())
                    {
                        result = (reader["pk"].ToString());
                    }
                    conn.Close();

                }

            }
            catch
            {

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

        public string wcoa_station_name (string station)
        {
            string result = "";

            try
            {
                String cmdText = "SELECT station_dropdown_value from station_setting where station_name ='"+station+"' and description = 'Windows' ";
                //      MySqlConnection conn = new MySqlConnection(connStr);
                MySqlCommand command = conn.CreateCommand();

                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {
                    MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                    while (reader.Read())
                    {
                         result = (reader["station_dropdown_value"].ToString());
                    }
                    conn.Close();

                }

            }
            catch
            {

            }

            return result;
        }
        public string ocoa_station_name(string station)
        {
            string result = "";

            try
            {
                String cmdText = "SELECT station_dropdown_value from station_setting where station_name ='" + station + "' and description = 'Office' ";
                //      MySqlConnection conn = new MySqlConnection(connStr);
                MySqlCommand command = conn.CreateCommand();

                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {
                    MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                    while (reader.Read())
                    {
                        result = (reader["station_dropdown_value"].ToString());
                    }
                    conn.Close();

                }

            }
            catch
            {

            }

            return result;
        }
        public List<String> Station()
        {
            List<String> station_list = new List<string>();
            

           try
            {
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

            }
            catch
            {

            }


            return station_list;

        }
    }
}
