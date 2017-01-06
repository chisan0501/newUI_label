using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_label
{
    class LabelModel
    {
        public List<string> users { get; set; }
        public List<string> db_select { get; set; }
        public bool is_mysql_open { get; set; }
        public string mysql_status { get; set; }
        public bool is_sqlite_open { get; set; }
        public string sqlite_status { get; set; }
        
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


