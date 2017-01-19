using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_label
{
    public interface Idbprovider
    {
        bool ping();
        bool insert();
        select_imaging imaging_data();
        user_list users();
        Discovery_result discovery_data(string asset);
        ObservableCollection<RefrubHistoryObj> db_history();
        List<string> channel_list();
        List<string> sku_list(string channel);
        RefrubHistoryObj redisco_data(int asset);
        Dictionary<String, int> sku_brand();
    }
}
