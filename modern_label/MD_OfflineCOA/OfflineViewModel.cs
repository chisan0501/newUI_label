using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD_OfflineCOA
{
    class OfflineViewModel
    {
        public OfflineModel OfflineModel { get; set; }
        public OfflineViewModel()
        {

            var thisOffline = new OfflineModel();
            thisOffline.SwitchView = 2;
            OfflineModel = thisOffline;



        }
        

        
        
        

        



    }
}
