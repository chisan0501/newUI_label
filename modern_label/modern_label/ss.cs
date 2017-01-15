using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using System.Threading;

namespace modern_label
{
    class ss
    {
        public ss()
        {


        }

        public async void get_customer()
        {

          
                var baseAddress = new Uri("https://ssapi.shipstation.com/");

                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Basic ZmU3YzE2MGMyZjE0NDc1ZDljNWQ0ZWI2ZmMzYmRhOWU6YzRiM2RhMjlkZWZlNDgyOWJlZmRlYTExNmU1N2Q5ZTY=");

                    using (var response = await httpClient.GetAsync("orders?storeID=14810&pageSize=500&page=" + 13))
                    {

                        string responseData = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ss_json.Rootobject>(responseData);

                        for (int i = 0; i < result.orders.Length; i++)
                        {
                            result.orders[i].shipTo.name = result.orders[i].shipTo.name.Replace("'", "");
                            result.orders[i].shipTo.street1 = result.orders[i].shipTo.street1.Replace("'", "");
                            result.orders[i].shipTo.city = result.orders[i].shipTo.city.Replace("'", "");
                            string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";

                            MySqlConnection conn = new MySqlConnection(connStr);
                            MySqlCommand command = conn.CreateCommand();
                            command.CommandText = "insert into ebay (order_id,ship_date,order_date,order_total,ship_name,ship_address,ship_city,ship_state,ship_zip,carrier,order_status,payment_method,tax) values ('" + result.orders[i].orderId + "','" + result.orders[i].shipDate + "','" + result.orders[i].orderDate + "','" + result.orders[i].orderTotal + "','" + result.orders[i].shipTo.name + "','" + result.orders[i].shipTo.street1 + "','" + result.orders[i].shipTo.city + "','" + result.orders[i].shipTo.state + "','" + result.orders[i].shipTo.postalCode + "','" + result.orders[i].carrierCode + "','" + result.orders[i].orderStatus + "','" + result.orders[i].paymentMethod + "','" + result.orders[i].taxAmount + "')";
                            conn.Open();
                            command.ExecuteNonQuery();
                            conn.Close();

                        }


                    }
                }

            }
               
        
        }

    }

