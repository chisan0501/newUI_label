using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_label
{
    class SQlite_DataProvider : Idbprovider
    {
        public bool ping()
        {
            bool is_online = false;
            try
            {
                

                
                
                var sqlite_connect = new SQLiteConnection("Data source=Z:\\inventory.db3;FailIfMissing=True");
                sqlite_connect.Open();
                is_online = true;
            }
            catch
            {

            }
            return is_online;
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

        public user_list users()
        {
            List<String> test = new List<string>();
            user_list user_list = new user_list();
            using (SQLiteConnection connect = new SQLiteConnection("Data source=Z:\\inventory.db3;FailIfMissing=True"))
            {
                connect.Open();
                using (SQLiteCommand fmd = connect.CreateCommand())
                {
                    fmd.CommandText = @"SELECT user_name from users";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        test.Add(Convert.ToString(r["user_name"]));

                    }
                }
            }
            user_list.users = test;
            return user_list;

        }
        public Discovery_result discovery_data(string asset)
        {
            Discovery_result result = new Discovery_result();

            return result;
        }
    }
}
