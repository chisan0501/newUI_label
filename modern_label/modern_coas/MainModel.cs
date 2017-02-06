using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_coas
{
    class MainModel
    {
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
