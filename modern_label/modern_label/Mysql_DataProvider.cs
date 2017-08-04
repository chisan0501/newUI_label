using modern_label.SF;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace modern_label
{
    class Mysql_DataProvider 
    {
       
        public static MySqlConnection conn; 

        public Mysql_DataProvider(string source)
        {
            conn = new MySqlConnection();

            this.ping();
        }

        public Mysql_DataProvider()
        {

        }
        

        public void change_connection_string (string source)
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

        public bool copy_discovery_data(string cpu, string model, string manu, string ram, string hdd, string serial, string asset_tag)
        {
            bool result = false;

            try
            {

                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "insert into discovery (cpu ,hdd,ram,brand,model,serial,ictag) values ('" + cpu + "','" + hdd + "','" + ram + "','" + manu + "','" + model + "','" + serial + "','" + asset_tag + "')";
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                result = true;

            }
            catch
            {
                result = false;
            }

            return result;

        }
        public bool copy_rediscovery_data (string cpu,string model, string manu, string ram, string hdd,string serial,string asset_tag)
        {
            bool result = false;

            try
            {

                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "insert into rediscovery (cpu ,hdd,ram,brand,model,serial,ictag) values ('"+cpu+ "','" + hdd + "','" + ram + "','" + manu + "','" + model + "','" + serial + "','" + asset_tag + "')";
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                result = true;

            }
            catch
            {
                result = false;
            }

            return result;

        }

        public bool edit_discovery_data (discovery_result input,string asset_tag)
        {
            bool result = false;
           try
            {
                
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "UPDATE discovery SET cpu = '" + input.cpu + "',hdd='" + input.hdd + "',ram='" + input.ram + "',brand= '" + input.manu + "',model='" + input.model + "',serial = '" + input.serial + "',optical_drive = '" + input.optical_drive + "' WHERE ictag = '" + asset_tag + "'";
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                result = true;
               
            }
            catch
            {

            }
            return result;
        }

        public bool edit_rediscovery_data (search_result input,string asset_tag)
        {

            bool result = false;
            try
            {
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "UPDATE rediscovery SET cpu = '" + input.cpu + "',hdd='" + input.hdd + "',ram='" + input.ram + "',brand= '" + input.manu + "',model='" + input.model + "',serial = '" + input.serial + "',optical_drive = '" + input.optical_drive + "',pallet = '" + input.sku + "' WHERE ictag = '" + asset_tag + "'";
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                result = true;
            }
          catch
            {

            }

            return result;
        }


        public imaging_search_result img_data(string asset)
        {
            var img_result = new imaging_search_result();
          
            
            
            String cmdText = "SELECT * from production_log where ictags = '" + asset + "'";
            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                
                while (reader.Read())
                {
                    img_result.img_wcoa = (reader["wcoa"].ToString());
                    img_result.img_ocoa = (reader["ocoa"].ToString());
                    img_result.img_hdd = (reader["hdd"].ToString());
                    img_result.img_ram = (reader["ram"].ToString());
                    img_result.img_video = (reader["video_card"].ToString());
                    img_result.img_sku = (reader["channel"].ToString());
                    
                }
                conn.Close();
            }

            return img_result;
        }

        public Search_result rediscovery_data(string asset)
        {
            var Search_result = new Search_result();
           

            
            
            
            String cmdText = "SELECT * from rediscovery where ictag = '" + asset + "'";
            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    Search_result.search_cpu = (reader["cpu"].ToString());
                    var temp_hdd = (reader["hdd"].ToString());
                    Regex digitsOnly = new Regex(@"[^\d]");
                    temp_hdd = digitsOnly.Replace(temp_hdd, "");
                    Search_result.is_SSD = (reader["has_SSD"].ToString());
                    Search_result.search_hdd = (long.Parse(temp_hdd));
                    Search_result.search_manu = (reader["brand"].ToString());
                    var temp_ram = reader["ram"].ToString();
                    temp_ram = digitsOnly.Replace(temp_ram, "");
                    Search_result.search_ram = (long.Parse(reader["ram"].ToString()));
                    Search_result.search_model = (reader["model"].ToString());
                    Search_result.search_serial = (reader["serial"].ToString());
                    Search_result.search_optical_drive = (reader["optical_drive"].ToString());
                    Search_result.search_sku = (reader["pallet"].ToString());
                }
                conn.Close();
            }
                return Search_result;
        }

        public Discovery_result discovery_data(string asset)
        {
            var result = new Discovery_result();

          
       
           
            
            String cmdText = "SELECT * from discovery where ictag = '" + asset + "'";
            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    result.search_cpu = (reader["cpu"].ToString());
                    result.search_hdd = (long.Parse(reader["hdd"].ToString()));
                    result.search_manu = (reader["brand"].ToString());
                    result.search_ram = (long.Parse(reader["ram"].ToString()));
                    result.search_model = (reader["model"].ToString());
                    result.search_serial = (reader["serial"].ToString());
                    result.search_optical_drive = (reader["optical_drive"].ToString());
                    
                }
                conn.Close();
            }
            return result;

        }

        public bool localDB_ping()
        {
            bool is_connect = false;
            try
            {
                conn.ConnectionString = ConfigurationManager.AppSettings["LocalMySqlConnectionString"];
                conn.Open();
                is_connect = true;
                conn.Close();
               
            }
            catch
            {

            }


            return is_connect;

        }

        public bool ping()
        {
            bool is_connect = false;

            try
            {


                


                conn.ConnectionString = ConfigurationManager.AppSettings["OnlineMySqlConnectionString"];


                conn.Open();
                is_connect = true;

                conn.Close();

            }
            catch
            {
                conn.ConnectionString = ConfigurationManager.AppSettings["LocalMySqlConnectionString"];
            }
            return is_connect; 
        }



        

       

        public Dictionary<String,string> sku_brand()
         {
            var result = new Dictionary<string, string>();

          try
            {
                String cmdText = "select * from magento_sku_brand";

                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {
                   

                    MySqlCommand command = conn.CreateCommand();

                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                    while (reader.Read())
                    {
                        result.Add(reader["name"].ToString(), reader["sku_name"].ToString());

                    }
                    conn.Close();

                }
            }
            catch
            {

            }

            return result;
        }


        public bool check_exist(string rma_number) {
            var exist = false;

            String cmdText = "select * from rma where rma_number = '" + rma_number + "'";

           

            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlCommand command = conn.CreateCommand();

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                if (reader.Read())
                {
                    exist = true;
                }
                conn.Close();

            }


            return exist;
        }
        public string insert_rma(List<Case> input)
        {

            if (input.Count == 0)
            {
                return "No record Found";

            }
            string message = "SalesForce Entry Insert Complete";

            foreach (var item in input)
            {

                try
                {
                    string strdatetime = item.CreatedDate.HasValue ? item.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty;

                    item.Description = item.Description.Replace("'", "''");
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "Insert into rma (rma_number,case_number,id,channel,date_requested,resolution_code,ictag,description) values ('"+item.RMA_number__c+"','"+item.CaseNumber+"','"+item.Id+"','"+item.Channel__c+"','"+ strdatetime + "','"+item.Return_Resolution__c+"','"+item.IC_Barcodes__c+"','"+item.Description+"')";
                    
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();


                }
                catch (Exception e)
                {
                    message = e.Message;
                }

            }






            return message;
        }
        public string update_rma(List<Case> input) {

            if (input.Count == 0) {
                return "No record Found";

            }
            string message = "SalesForce reSync Complete";

            foreach (var item in input) {

                try
                {

                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "UPDATE rma SET production_finding = '" + item.Production_Findings__c + "',ictag='" + item.IC_Barcodes__c + "',description='" + item.Description + "' where rma_number ='" +item.RMA_number__c+"'";
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();


                }
                catch (Exception e)
                {
                    message = e.Message;
                }

            }
           





            return message;
        }


        public RefrubHistoryObj disco_data(int asset)
        {

            var result = new RefrubHistoryObj();


            //   LabelModel.status = "Connection Successful";
            String cmdText = "select * from discovery where ictag ='" + asset + "'";




            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                conn.Open();

                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    result = new RefrubHistoryObj() { asset_tag = int.Parse(reader["ictag"].ToString()), time = DateTime.Parse(reader["time"].ToString()), hdd = (reader["hdd"].ToString()), ram = (reader["ram"].ToString()), cpu = (reader["cpu"].ToString()), made = (reader["brand"].ToString()), model = (reader["model"].ToString()), serial = (reader["serial"].ToString()), optical_drive = (reader["optical_drive"].ToString()), is_ssd = (reader["has_SSD"].ToString()) };

                }
                conn.Close();
                result.ram = magento_sku.ram_format(result, false);
                result.hdd = magento_sku.hdd_format(false, result);
            }



            return result;
        }

        public RefrubHistoryObj redisco_data (int asset )
        {

            var result = new RefrubHistoryObj();

            
            //   LabelModel.status = "Connection Successful";
            String cmdText = "select * from rediscovery where ictag ='"+asset+"'";
            



            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                conn.Open();

                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                 result =   new RefrubHistoryObj() { asset_tag = int.Parse(reader["ictag"].ToString()), time = DateTime.Parse(reader["time"].ToString()), refurbisher = (reader["refurbisher"].ToString()), sku = (reader["pallet"].ToString()), hdd = (reader["hdd"].ToString()), ram = (reader["ram"].ToString()),cpu = (reader["cpu"].ToString()), made =(reader["brand"].ToString()),model = (reader["model"].ToString()),serial = (reader["serial"].ToString()),optical_drive = (reader["optical_drive"].ToString()),is_ssd= (reader["has_SSD"].ToString()) };
                    
                }
                conn.Close();
                result.ram = magento_sku.ram_format(result, false);
                result.hdd = magento_sku.hdd_format(false,result);
            }



            return result;
        }


        public bool discovery_insert(RefrubHistoryObj input)
        {
            bool sucess = false;
            try
            {
              
              
                MySqlCommand command = conn.CreateCommand();
                conn.Open();
                String cmdText = "Insert into discovery(ictag,time,cpu,serial,brand,model,cpu,hdd,ram)VALUES ('" + input.asset_tag + "','" + "" + "','" + input.cpu + "','"+ input.serial + "','" + input.brand + "','" + input.model + "','" + input.cpu + "','" + input.hdd + "','" + input.ram + "') on Duplicate KEY update hdd='" + input.hdd + "',ram='" + input.ram + "'";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {

                    cmd.ExecuteNonQuery();
                    conn.Close();

                }
                sucess = true;
            }
            catch
            {

            }






            return sucess;
        }

        public bool insert(RefrubHistoryObj input)
        {
            bool sucess = false;
            try
            {
              
                
                conn.Open();
                String cmdText = "Insert into rediscovery(ictag,time,serial,brand,model,cpu,hdd,ram,optical_drive,location,pallet,pre_coa,refurbisher)VALUES ('" + input.asset_tag + "','" + "" + "','" + input.serial + "','" + input.brand + "','" + input.model + "','" + input.cpu + "','" + input.hdd + "','" + input.ram + "','','" + input.channel + "','" + input.sku + "','" + input.pre_coa + "','" + input.refurbisher + "') on Duplicate KEY update hdd='" + input.hdd + "',ram='" + input.ram + "',location='" + input.channel + "',pallet='" + input.sku + "',pre_coa = '" + input.pre_coa + "',refurbisher = '" + input.refurbisher + "'";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    
                }

                sucess = true;
            }
            catch
            {
                
            }

            




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

            try
            {
                //   LabelModel.status = "Connection Successful";
                String cmdText = "select * from rediscovery where time between CURDATE() AND DATE_ADD(CURDATE(), INTERVAL +1 DAY)";

                

                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {
                    MySqlCommand command = conn.CreateCommand();

                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                    while (reader.Read())
                    {
                        result.Add(new RefrubHistoryObj() { asset_tag = int.Parse(reader["ictag"].ToString()), time = DateTime.Parse(reader["time"].ToString()), refurbisher = (reader["refurbisher"].ToString()), sku = (reader["pallet"].ToString()), hdd = (reader["hdd"].ToString()), ram = (reader["ram"].ToString()) });
                    }
                    conn.Close();

                }
            }

            catch
            {
                
            }
            


            return result;
        }


        public LabelModel.magento_brand get_brand (string brand_name)
        {

            var result = new LabelModel.magento_brand();
          
            String cmdText = "select * from magento_html where name = '" + brand_name + "'";
       
          
            

            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlCommand command = conn.CreateCommand();

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    result.name = (reader["name"].ToString());
                    result.dropdown_value = (reader["drop_down_value"].ToString());
                }
                conn.Close();

            }

            

            return result;


        }

        public LabelModel.magento_cpu get_cpu(string condition)
        {

            var result = new LabelModel.magento_cpu();

           
            String cmdText = "select * from magento_html where name = '" + condition + "'";
        

            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {

                MySqlCommand command = conn.CreateCommand();

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    result.html = (reader["html"].ToString());
                    result.dropdown_value = (reader["drop_down_value"].ToString());
                }
                conn.Close();

            }

            return result;
        }

        public string get_misc(string condition)
        {

            string result = "";

          
            String cmdText = "select html from magento_html where name = '"+condition+"'";
   
           

            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlCommand command = conn.CreateCommand();

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    result = (reader["html"].ToString());
                    
                }
                conn.Close();

            }

            return result;
        }

        public bool add_channel (string channel_name,string sku)
        {
            bool sucesss = false;

            try
            {
              
      
               
                String cmdText = "Insert into label_menu(name,product)VALUES ('"+channel_name+"','"+sku+"')";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {
                    MySqlCommand command = conn.CreateCommand();
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    sucesss = true;
                }
            }
           catch {
                sucesss = false;
            }

            return sucesss;


        }

        public LabelModel.magento_ram get_ram (string size )
        {
            var result = new LabelModel.magento_ram();
           
            String cmdText = "select * from magento_html where type = 'ram' and name ='" + size + "'";
          
            

            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlCommand command = conn.CreateCommand();

                conn.Open();
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
           
            String cmdText = "select * from magento_html where type = 'hdd' and name ='"+size+"'";
            
           

            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlCommand command = conn.CreateCommand();

                conn.Open();
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

          try
            {
                String cmdText = "SELECT product from label_menu where name = '" + channel + "'";

              ;

                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {
                    MySqlCommand command = conn.CreateCommand();

                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                    while (reader.Read())
                    {
                        result.Add(reader["product"].ToString());
                    }
                    conn.Close();

                }
            }
            catch
            {

            }
         


            return result;


        }

        public List<string> channel_list() {


           
                List<string> result = new List<string>();

            try
            {
                String cmdText = "SELECT distinct name from label_menu order by name";


                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {

                    MySqlCommand command = conn.CreateCommand();

                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                    while (reader.Read())
                    {
                        result.Add(reader["name"].ToString());
                    }
                    conn.Close();

                }
            }
            catch
            {
                result.Add("");
            }
                return result;
        
            
            
}

       

        public List<String> users ()
        {
            List<String> test = new List<string>();

            try
            {
                String cmdText = "SELECT user_name from users";

                

                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {
                    MySqlCommand command = conn.CreateCommand();

                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                    while (reader.Read())
                    {
                        test.Add(reader["user_name"].ToString());
                    }
                    conn.Close();

                }
            }
            catch
            {
                test.Add("Selected DB is offline");
            }
             
                
            return test;

        }

    }
}
