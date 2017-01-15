using DYMO.Label.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace modern_label
{
    class LabelModel
    {


        public List<string> printer_list
        {
            get {
                List<string> result = new List<string>();

                foreach (IPrinter printer in new Printers())
                    {
                    
                    result.Add(printer.Name);
                    
                    }
                return result;

            }
            set { }
        }

        public List<string> grade_list
        {

            get
            {
                List<string> temp = new List<string>();
                temp.Add("A");
                temp.Add("B");
                temp.Add("C");
                return temp;
            }
            set { }

        }
        public BitmapImage Preview { get;set;}
        public List<string> users { get; set; }
        public List<string> computer_type
        {

            get
            {
                List<string> temp = new List<string>();
                temp.Add("Desktop");
                temp.Add("Laptop");
                return temp;
            }


            set { }

        }



           
        public List<string> db_select { get; set; }
        public bool is_mysql_open { get; set; }
        public string mysql_status { get; set; }
        public bool is_sqlite_open { get; set; }
        public string sqlite_status { get; set; }
        public int refurb_machine { get; set; }
        


    }
   
    public class RefrubHistoryObj
    {

        public int asset_tag { get; set; }
        public DateTime time { get; set; }
        public string refurbisher { get; set; }
        public string sku { get; set; }
        public string hdd { get; set; }
        public string ram { get; set; }
        public string model { get; set; }
        public string made { get; set; }
        public string cpu { get; set; }
        public string serial { get; set; }
        
        public string channel { get; set; }
        public string selected_printer { get; set; }
       
    }

  

    public class Discovery_result
    {

        public string search_cpu { get; set; }
        public string search_manu { get; set; }
        public long search_ram { get; set; }
        public long search_hdd { get; set; }
        public string search_serial { get; set; }
        public string search_optical_drive { get; set; }
        public string search_model { get; set; }
       

    }

    class Submit_data
    {
        public int submit_asset { get; set; } 
        public string submit_preCOA { get; set; }




    }

    class Search_result
    {
        public string search_cpu { get; set; }
        public string search_manu { get; set; }
        public long search_ram { get; set; }
        public long search_hdd { get; set; }
        public string search_serial { get; set; }
        public string search_optical_drive { get; set; }
        public string search_model { get; set; }
        public string search_sku { get; set; }
    }
    class  Imaging_search_result {

        public string imaging_search_wcoa { get; set; }
        public string imaging_search_ocoa { get; set; }
        public string imaging_search_ram { get; set; }
        public string imaging_search_hdd { get; set; }
        
        public string imaging_search_video { get; set; }
      
        public string imaging_search_sku { get; set; }

    }
}


