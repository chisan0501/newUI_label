using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_label
{
    class dbData
    {
        
    }
public class select_imaging
    {
        public string ictag { get; set; }
        public string cpu { get; set; }
        public string ram { get; set; }
        public string hdd { get; set; }
        public string wcoa { get; set; }
        public string ocoa { get; set; }
    }
    public class user_list
    {

        public List<string> users { get; set; }

    }
    public class Discovery
    {

        public string search_cpu { get; set; }
        public string search_manu { get; set; }
        public long search_ram { get; set; }
        public long search_hdd { get; set; }
        public string search_serial { get; set; }
        public string search_optical_drive { get; set; }
        public string search_model { get; set; }


    }
}
