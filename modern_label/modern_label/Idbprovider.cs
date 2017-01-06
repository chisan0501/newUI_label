using System;
using System.Collections.Generic;
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
    }
}
