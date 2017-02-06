using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace modern_label
{
    class search_result
    {
        public Search_result Search_result { get; set; }
        Mysql_DataProvider mysql_data = new Mysql_DataProvider(LabelViewModel.db_source);
        
        public search_result() : this("12345")
        {

        }

        public search_result(string asset)
        {
            Search_result = new Search_result();

            Search_result = mysql_data.rediscovery_data(asset);

            //this is the viewModel for search flyout


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
