﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace modern_label
{
    class SQlite_DataProvider : Idbprovider
    {
        public bool add_channel(string channel_name, string sku)
        {
            bool sucesss = false;
            return sucesss; }

        public ObservableCollection<RefrubHistoryObj> db_history()
        {

            var result = new ObservableCollection<RefrubHistoryObj>();
            return result;

        }


        public List<string> sku_list(string channel)
        {

            List<string> result = new List<string>();



            return result;


        }

        public bool discovery_insert(RefrubHistoryObj obj)
        {
            var result = false;

            return result;
        }

        public Dictionary<String, int> sku_brand()
        {
            var result = new Dictionary<string, int>();

            return result;
        }

        public List<string> channel_list()
        {

            List<string> result = new List<string>();

            return result;
        }


        public RefrubHistoryObj redisco_data(int asset)
        {

            var result = new RefrubHistoryObj();

            return result;
        }

        public bool ping()
        {
            bool is_online = false;
            try
            {
                

                
                
               // var sqlite_connect = new SQLiteConnection("Data source=Z:\\inventory.db3;FailIfMissing=True");
               // sqlite_connect.Open();
                is_online = true;
            }
            catch
            {

            }
            return is_online;
        }
        public bool insert(RefrubHistoryObj insert)
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
         //   using (SQLiteConnection connect = new SQLiteConnection("Data source=Z:\\inventory.db3;FailIfMissing=True"))
            {
             //   connect.Open();
           //     using (SQLiteCommand fmd = connect.CreateCommand())
                {
                    //fmd.CommandText = @"SELECT user_name from users";
                    //fmd.CommandType = CommandType.Text;
                    //SQLiteDataReader r = fmd.ExecuteReader();
                    //while (r.Read())
                    //{
                    //    test.Add(Convert.ToString(r["user_name"]));

                    //}
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
