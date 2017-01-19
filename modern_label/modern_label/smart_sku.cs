using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace modern_label
{
    class smart_sku
    {
        string global_model = "";
        public string lenovo_model(string model)
        {
            global_model = model;
            string url = "http://support.lenovo.com/services/us/en/ContentService/GetProducts?productId=" + model;
            var lenovo = json_to_list(url);
            if (lenovo != null && lenovo.Count != 0)
            {
                model = lenovo[0].Name;
            }


            return model;
        }
        public List<Lenovo> json_to_list(string url)
        {

            List<Lenovo> laptop_inv = new List<Lenovo>();
            var w = new WebClient();
            var json_data = w.DownloadString(url);
            if (!string.IsNullOrEmpty(json_data))
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<List<Lenovo>>(json_data);
                    laptop_inv = result;
                }
                catch
                {



                }
                // laptop_inv = result;

            }

            return laptop_inv;
        }
    }
}
