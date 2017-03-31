using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_monitor
{
    public class Monitor_models
    {
        public string Model;
        public string MonitorID;
        public string Manafacture;
        public string Name;
        public string SerialNumber;
        public string SizeDiaginch;
        public List<string> db_select
        {

            get
            {
                var temp = new List<string>();

                temp.Add("Online DB");
                temp.Add("Local DB");
                return temp;
            }
            set { }
        }
    }


}
