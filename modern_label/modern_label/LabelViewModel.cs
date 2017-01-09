using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_label
{
    class LabelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;
        protected virtual void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));

                switch (propName) {
                    case "Asset_tag":
                        
                        var discovery_result = new discovery_result(this.asset_tag);
                        break;
                    case "Users_SelectedValue":
                        Welcome_text = this.Users_SelectedValue;
                        
                        break;
                    
                    

                }
                
            }
        }
        public Idbprovider mysql_data = new Mysql_DataProvider();
        public Idbprovider sqlite_data = new SQlite_DataProvider();
        public LabelModel LabelModel { get; set; }
        public LabelViewModel()
        {
            //user dropdown data
            //saving user data to a list
            try
            {
                LabelModel = new LabelModel();
                //user name list
                LabelModel.users = new List<string>();
                LabelModel.db_select = new List<string>();
                LabelModel.db_select.Add("MYSQL");
                LabelModel.db_select.Add("SQLite");
                LabelModel.is_mysql_open = mysql_data.ping();
                LabelModel.is_sqlite_open = sqlite_data.ping();
                //ping sqlite
               
                LabelModel.sqlite_status = "SQLite : Online";
               

                //ping mysql
                
                LabelModel.mysql_status = "MySQl : Online";
               


                
                if (LabelModel.is_mysql_open == false)
                {
                    LabelModel.mysql_status = "MySQl : Offline";
                }
                if (LabelModel.is_sqlite_open == false)
                {
                    LabelModel.sqlite_status = "SQLite : Offline";
                   
                }
            }
            catch {
                //update the status bar message to fail
                
            }
            
        }

        private string welcome_text;
        public string Welcome_text
        {

            get { return "Hello " + welcome_text; }
            set {
                if(value != welcome_text)
                {
                    welcome_text = value;
                    RaisePropertyChanged("Welcome_text");
                }
               
             

            }
        }

        private string asset_tag;
        public string Asset_tag
        {
            get { return this.asset_tag; }
            set { this.asset_tag = value;
                RaisePropertyChanged("Asset_tag");
            }

        }

        public List<string>DB_select
        {
            get { return LabelModel.db_select; }
            set { LabelModel.db_select = value;

            }


        }

        private string users_SelectedValue;
        public string Users_SelectedValue
        {
            get { return this.users_SelectedValue; }
            set { this.users_SelectedValue = value;
                RaisePropertyChanged("Users_SelectedValue");
            }


        }

        public List<string> Users
        {
            get { return LabelModel.users; }
            set { LabelModel.users = value;
                RaisePropertyChanged("Users");
               
            }

        }
        
        public bool Is_Mysql_open
        {
            get { return LabelModel.is_mysql_open; }
            set { LabelModel.is_mysql_open = value; }
        }
        public string Mysql_Status
        {
            get { return LabelModel.mysql_status; }
            set { LabelModel.mysql_status = value; }


        }
        public bool Is_Sqlite_open
        {
            get { return LabelModel.is_sqlite_open; }
            set { LabelModel.is_sqlite_open = value; }
        }
        public string Sqlite_Status
        {
            get { return LabelModel.sqlite_status; }
            set { LabelModel.sqlite_status = value; }


        }
    }

    class imaging_search_result
    {

        public Imaging_search_result Imaging_result { get; set; }
        public imaging_search_result (string asset)
        {
            Imaging_result = new Imaging_search_result();
            string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";

            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();
            String cmdText = "SELECT * from production_log where ictags = '" + asset + "'";
            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader

                while (reader.Read())
                {
                    Imaging_result.imaging_search_wcoa = (reader["wcoa"].ToString());
                    Imaging_result.imaging_search_ocoa = (reader["ocoa"].ToString());
                    Imaging_result.imaging_search_hdd = (reader["hdd"].ToString());
                    Imaging_result.imaging_search_ram = (reader["ram"].ToString());
                    Imaging_result.imaging_search_video = (reader["video_card"].ToString());
                    Imaging_result.imaging_search_sku = (reader["channel"].ToString());
                }
                conn.Close();
            }



            }
        public string img_wcoa {

            get { return Imaging_result.imaging_search_wcoa; }
            set { Imaging_result.imaging_search_wcoa = value; }

        }
        public string img_ocoa
        {

            get { return Imaging_result.imaging_search_ocoa; }
            set { Imaging_result.imaging_search_ocoa = value; }

        }
        public string img_video
        {

            get { return Imaging_result.imaging_search_video; }
            set { Imaging_result.imaging_search_video = value; }

        }
        public string img_sku
        {

            get { return Imaging_result.imaging_search_sku; }
            set { Imaging_result.imaging_search_sku = value; }

        }
        public string img_hdd
        {

            get { return Imaging_result.imaging_search_hdd; }
            set { Imaging_result.imaging_search_hdd = value; }

        }
        public string img_ram
        {

            get { return Imaging_result.imaging_search_ram; }
            set { Imaging_result.imaging_search_ram = value; }

        }
    }


    public class discovery_result 
    {
        public Idbprovider mysql_data = new Mysql_DataProvider();
        public Idbprovider sqlite_data = new SQlite_DataProvider();
        public Discovery_result Discovery_result { get; set; }
        public discovery_result(string asset)
        {
            Discovery_result = new Discovery_result();
           
            Discovery_result = mysql_data.discovery_data(asset);
        }
       
      
        public string cpu
        {
            get { return Discovery_result.search_cpu; }
            set { Discovery_result.search_cpu = value; }
        }
        public string manu
        {
            get { return Discovery_result.search_manu; }
            set { Discovery_result.search_manu = value; }
        }
        public string model
        {
            get { return Discovery_result.search_model; }
            set { Discovery_result.search_model = value; }
        }
        public string serial
        {
            get { return Discovery_result.search_serial; }
            set { Discovery_result.search_serial = value; }
        }
        public string optical_drive
        {
            get { return Discovery_result.search_optical_drive; }
            set { Discovery_result.search_optical_drive = value; }
        }
        public long ram
        {
            get { return Discovery_result.search_ram; }
            set { Discovery_result.search_ram = value; }

        }
        public long hdd
        {
            get { return Discovery_result.search_hdd; }
            set { Discovery_result.search_hdd = value; }
        }
   
        
    }



class search_result
    {
        public Search_result Search_result { get; set; }
        public search_result(string asset)
        {
            Search_result = new Search_result();
            //this is the viewModel for search flyout
            string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";

            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();
            String cmdText = "SELECT * from rediscovery where ictag = '" + asset + "'";
            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    Search_result.search_cpu = (reader["cpu"].ToString());
                    Search_result.search_hdd = (long.Parse(reader["hdd"].ToString()));
                    Search_result.search_manu = (reader["brand"].ToString());
                    Search_result.search_ram = (long.Parse(reader["ram"].ToString()));
                    Search_result.search_model = (reader["model"].ToString());
                    Search_result.search_serial = (reader["serial"].ToString());
                    Search_result.search_optical_drive = (reader["optical_drive"].ToString());
                    Search_result.search_sku  = (reader["pallet"].ToString());
                }
                conn.Close();
            }
        }
            public string cpu
        {
            get { return Search_result.search_cpu; }
            set { Search_result.search_cpu = value; }
        }
        public string manu
        {
            get { return Search_result.search_manu; }
            set { Search_result.search_manu = value; }
        }
        public string model
        {
            get { return Search_result.search_model; }
            set { Search_result.search_model = value; }
        }
        public string serial
        {
            get { return Search_result.search_serial; }
            set { Search_result.search_serial = value; }
        }
        public string optical_drive
        {
            get { return Search_result.search_optical_drive; }
            set { Search_result.search_optical_drive = value; }
        }
        public long ram
        {
            get { return Search_result.search_ram; }
            set { Search_result.search_ram = value; }

        }
        public long hdd
        {
            get { return Search_result.search_hdd; }
            set { Search_result.search_hdd = value; }
        }
        public string sku
        {
            get { return Search_result.search_sku; }
            set { Search_result.search_sku = value; }
        }
    }


}
