using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
