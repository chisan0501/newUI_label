using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_label
{
    class Mysql_DataProvider : Idbprovider
    {

        public Discovery_result discovery_data(string asset)
        {
            var test = new Discovery_result();

          
            string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();
            String cmdText = "SELECT * from discovery where ictag = '" + asset + "'";
            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    test.search_cpu = (reader["cpu"].ToString());
                    test.search_hdd = (long.Parse(reader["hdd"].ToString()));
                    test.search_manu = (reader["brand"].ToString());
                    test.search_ram = (long.Parse(reader["ram"].ToString()));
                    test.search_model = (reader["model"].ToString());
                    test.search_serial = (reader["serial"].ToString());
                    test.search_optical_drive = (reader["optical_drive"].ToString());

                }
                conn.Close();
            }
            return test;

        }
        public bool ping()
        {
            bool is_connect = false;

            try
            {

                string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";


                MySqlConnection conn = new MySqlConnection(connStr);


                conn.Open();
                is_connect = true;

                conn.Close();

            }
            catch
            {

            }
            return is_connect; 
        }

        public Dictionary<String,int> sku_brand()
         {
            var result = new Dictionary<string, int>();

            string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";
            String cmdText = "select * from magento_sku_brand";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();

            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    result.Add(reader["name"].ToString(), int.Parse(reader["index"].ToString()));
                 
                }
                conn.Close();

            }

            return result;
        }

        public RefrubHistoryObj redisco_data (int asset )
        {

            var result = new RefrubHistoryObj();

            string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";
            //   LabelModel.status = "Connection Successful";
            String cmdText = "select * from rediscovery where ictag ='"+asset+"'";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();

            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                 result =   new RefrubHistoryObj() { asset_tag = int.Parse(reader["ictag"].ToString()), time = DateTime.Parse(reader["time"].ToString()), refurbisher = (reader["refurbisher"].ToString()), sku = (reader["pallet"].ToString()), hdd = (reader["hdd"].ToString()), ram = (reader["ram"].ToString()),cpu = (reader["cpu"].ToString()), made =(reader["brand"].ToString()),model = (reader["model"].ToString()),serial = (reader["serial"].ToString()),optical_drive = (reader["optical_drive"].ToString()) };
                }
                conn.Close();

            }



            return result;
        }

        public bool insert()
        {
            bool sucess = false;

            return sucess;
        }
        public select_imaging imaging_data()
        {
            select_imaging imaging = new select_imaging();
            return imaging;
        }


        public ObservableCollection<RefrubHistoryObj> db_history()
        {
            var result = new ObservableCollection<RefrubHistoryObj>();

            string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";
            //   LabelModel.status = "Connection Successful";
            String cmdText = "select * from rediscovery where time between CURDATE() AND DATE_ADD(CURDATE(), INTERVAL +1 DAY)";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();

            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    result.Add(new RefrubHistoryObj() { asset_tag = int.Parse(reader["ictag"].ToString()),time = DateTime.Parse(reader["time"].ToString()),refurbisher = (reader["refurbisher"].ToString()),sku = (reader["pallet"].ToString()),hdd = (reader["hdd"].ToString()) , ram =(reader["ram"].ToString()) });
                }
                conn.Close();

            }



            return result;
        }

        public LabelModel.magento_ram get_ram (string size )
        {
            var result = new LabelModel.magento_ram();
            string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";
            String cmdText = "select * from magento_html where type = 'ram' and name ='" + size + "'";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();

            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    result.html = (reader["html"].ToString());
                    result.drop_down_value = (reader["drop_down_value"].ToString());
                }
                conn.Close();

            }

            return result;


        }
       
       public LabelModel.magento_hdd get_hdd(string size)
        {
            var result = new LabelModel.magento_hdd();
            string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";
            String cmdText = "select * from magento_html where type = 'hdd' and name ='"+size+"'";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();

            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    result.html = (reader["html"].ToString());
                    result.drop_down_value = (reader["drop_down_value"].ToString());
                }
                conn.Close();

            }

            return result;
        }

        public List<string> sku_list (string channel)
        {

            List<string> result = new List<string>();

            string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";
            String cmdText = "SELECT product from label_menu where name = '"+channel+"'";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();

            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    result.Add(reader["product"].ToString());
                }
                conn.Close();

            }


            return result;


        }

        public List<string> channel_list() {


           
                List<string> result = new List<string>();

                string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";
                String cmdText = "SELECT distinct name from label_menu order by name";
                MySqlConnection conn = new MySqlConnection(connStr);
                MySqlCommand command = conn.CreateCommand();

                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {
                    MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                    while (reader.Read())
                    {
                       result.Add(reader["name"].ToString());
                    }
                    conn.Close();

                }
                return result;
        
            
            
}


        public user_list users ()
        {
            List<String> test = new List<string>();
            user_list user = new user_list();

                string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";
                //   LabelModel.status = "Connection Successful";
                String cmdText = "SELECT user_name from users";
                MySqlConnection conn = new MySqlConnection(connStr);
                MySqlCommand command = conn.CreateCommand();

                conn.Open();
                
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {
                    MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                    while (reader.Read())
                    {
                        test.Add(reader["user_name"].ToString());
                    }
                    conn.Close();
               
            }

            user.users = test;
            return user;

        }

    }
}
