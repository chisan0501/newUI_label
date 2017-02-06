using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_label
{
    public class discovery_result
    {


        public discovery_result() :this("12345")
        {

        }

        

        Mysql_DataProvider mysql_data = new Mysql_DataProvider(LabelViewModel.db_source);
        public Idbprovider sqlite_data = new SQlite_DataProvider();
        public Discovery_result Discovery_result { get; set; }
        public discovery_result(string asset)
        {
           // Discovery_result = new Discovery_result();

            Discovery_result = mysql_data.discovery_data(asset);
        }


        public string cpu
        {
            get { return Discovery_result.search_cpu; }
            set
            {
                Discovery_result.search_cpu = value;

            }
        }
        public string manu
        {
            get { return Discovery_result.search_manu; }
            set { Discovery_result.search_manu = value; }
        }
        public string model
        {
            get { return Discovery_result.search_model; }
            set { Discovery_result.search_model = value; }
        }
        public string serial
        {
            get { return Discovery_result.search_serial; }
            set { Discovery_result.search_serial = value; }
        }
        public string optical_drive
        {
            get { return Discovery_result.search_optical_drive; }
            set { Discovery_result.search_optical_drive = value; }
        }
        public long ram
        {
            get { return Discovery_result.search_ram; }
            set { Discovery_result.search_ram = value; }

        }
        public long hdd
        {
            get { return Discovery_result.search_hdd; }
            set { Discovery_result.search_hdd = value; }
        }


    }
}
