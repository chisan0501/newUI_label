using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace modern_coas
{
    class MainModel
    {
        public static List<string> get_networkDrive()
        {
            var result = new List<string>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
               "select * from Win32_MappedLogicalDisk");
            foreach (ManagementObject drive in searcher.Get())
            {

                string path = (Regex.Match(
                    drive["ProviderName"].ToString(),
                    @"\\\\([^\\]+)")).ToString();
                string device_id = drive["DeviceID"].ToString();

                result.Add(device_id + path + "\n");
            }

            return result;

        }
        public static string driver()
        {
            string pnp_id = "";
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2",
                 "Select * from Win32_PnPEntity WHERE ConfigManagerErrorCode <> 0");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj.Properties["PNPDeviceID"] != null)
                    {
                        pnp_id += queryObj.Properties["PNPDeviceID"].Value.ToString();
                    }
                }
                
            }
            catch (ManagementException arr)
            {
                
            }
            return pnp_id;
        }


        public static string screen()
        {
            string screen;
            try
            {

                double diagonal = 0;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("\\root\\wmi", "SELECT * FROM WmiMonitorBasicDisplayParams");
                foreach (ManagementObject mo in searcher.Get())
                {
                    double width = (byte)mo["MaxHorizontalImageSize"] / 2.54;
                    double height = (byte)mo["MaxVerticalImageSize"] / 2.54;
                    diagonal = Math.Sqrt(width * width + height * height);
                }
                screen = Math.Round(diagonal, 1).ToString();
            }
            catch
            {
                screen = "N/A";
            }
            return screen;
        }
        public static string video_card()
        {
            string video = "";
            ManagementObjectSearcher searcher =
                     new ManagementObjectSearcher("root\\cimv2",
                     "SELECT * FROM Win32_VideoController");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                video = queryObj["Name"].ToString();
            }
            return video;
        }
        public string serial
        {
            get
            {
                string serial = "";
                try
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_BIOS");
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        serial = "null";
                        if (queryObj.Properties["SerialNumber"].Value.ToString() != null)
                        {
                            serial = queryObj.Properties["SerialNumber"].Value.ToString();
                        }

                    }
                    return serial;
                }

                catch
                {
                    return serial;
                }

            }
        }
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


   

    public class coa_obj
    {
        public string wcoa;
        public string wcoa_id;
        public string ocoa;
        public string ocoa_id;
        public int wcoa_count;
        public int ocoa_count;
    }

    public class obj_on_load
    {
        public string ictag;
        public string serial;
        public string hdd;
        public string ram;
        public string cpu;
        public string sku;
        public string manu;
        public string screen;
        public string video;
        public string model;
        public string pre_coa;
    }
}
