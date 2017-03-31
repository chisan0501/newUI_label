using modern_label.SF;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_label
{
    class sf
    {
        public static MySqlConnection conn;
        public sf()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["OnlineMySqlConnectionString"];
        }
        
        public sObject search_rma(string ictag)
        {
            string userName;
            string password;
            userName = "cdrain@interconnection.org";
            password = "3Emma3chaulk3!";
            //use default binding and address from app.config
            // string securityToken = "xxxxxxxxxxxxxxx";

            SforceService sfdcBinding = null;
            LoginResult currentLoginResult = null;
            sfdcBinding = new SforceService();
            try
            {
                currentLoginResult = sfdcBinding.login(userName, password);
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                // This is likley to be caused by bad username or password
                sfdcBinding = null;
                throw (ex);
            }
            catch (Exception ex)
            {
                // This is something else, probably comminication
                sfdcBinding = null;
                throw (ex);
            }


            //Change the binding to the new endpoint
            sfdcBinding.Url = currentLoginResult.serverUrl;

            //Create a new session header object and set the session id to that returned by the login
            sfdcBinding.SessionHeaderValue = new SessionHeader();
            sfdcBinding.SessionHeaderValue.sessionId = currentLoginResult.sessionId;

            QueryResult queryResult = null;
            String SOQL = "";

            SOQL = "SELECT Description ,IC_Barcodes__c,RMA_number__c,Production_Findings__c,Channel__c,Date_RMA_Requested__c FROM case where IC_Barcodes__c = '" + ictag + "' ";
            queryResult = sfdcBinding.query(SOQL);

            var result = queryResult.records[0];

            return result;
        }


        public bool update_rma (string id, string finding, string asset_tag)
        {
            bool sucess = false;
            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "UPDATE rma set production_finding = '" + finding + "' WHERE ictag = '" + asset_tag + "'";
                command.ExecuteNonQuery();
                conn.Close();
                sucess = true;
               
                string userName;
                string password;
                userName = "cdrain@interconnection.org";
                password = "3Emma3chaulk3!";
                //use default binding and address from app.config
                // string securityToken = "xxxxxxxxxxxxxxx";
                LoginResult currentLoginResult = null;
                SforceService sfdcBinding = null;

               


                sfdcBinding = new SforceService();
                currentLoginResult = sfdcBinding.login(userName, password);
                sfdcBinding.Url = currentLoginResult.serverUrl;
                sfdcBinding.SessionHeaderValue = new SessionHeader();
                sfdcBinding.SessionHeaderValue.sessionId = currentLoginResult.sessionId;
                var update_case = new Case();

                update_case.Reviewed_by_Production__cSpecified = true;
                update_case.Reviewed_by_Production__c = true;
                update_case.Id = id;
                update_case.Production_Findings__c = finding;




                SaveResult[] saveResults = sfdcBinding.update(new sObject[] { update_case });

                string result = "";
                if (saveResults[0].success)
                {
                    
                    result = "The update of Lead ID " + saveResults[0].id + " was succesful";

                }
                else
                {
                    sucess = false;
                    result = "There was an error updating the Lead. The error returned was " + saveResults[0].errors[0].message;
                }





            }
            catch(Exception e)
            {
                conn.Close();
            }
            
            return sucess;


        }

        public rma_result get_rma_num(string num)
        {
            var rma_result = new rma_result();

            MySqlCommand command = conn.CreateCommand();
          
            String cmdText = "SELECT * from rma where rma_number = '" + num + "' limit 1";
            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    rma_result.channel = (reader["channel"].ToString());
                    rma_result.asset_tag = (reader["ictag"].ToString());
                    rma_result.date_request = DateTime.Parse(reader["date_requested"].ToString());
                    rma_result.production_finding = (reader["production_finding"].ToString());
                    rma_result.rma_description = (reader["description"].ToString());
                    rma_result.rma_number = (reader["rma_number"].ToString());
                    rma_result.serial = (reader["serial"].ToString());
                    rma_result.id = (reader["id"].ToString());
                }
                
            }
            return rma_result;

        }

        public rma_result get_rma_serial(string serial)
        {
            var rma_result = new rma_result();
            MySqlCommand command = conn.CreateCommand();
            
            String cmdText = "SELECT * from rma where serial = '" + serial + "' limit 1";
            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    rma_result.channel = (reader["channel"].ToString());
                    rma_result.asset_tag = (reader["ictag"].ToString());
                    rma_result.date_request = DateTime.Parse(reader["date_requested"].ToString());
                    rma_result.production_finding = (reader["production_finding"].ToString());
                    rma_result.rma_description = (reader["description"].ToString());
                    rma_result.rma_number = (reader["rma_number"].ToString());
                    rma_result.serial = (reader["serial"].ToString());
                    rma_result.id = (reader["id"].ToString());
                }
              
            }
            return rma_result;

        }

        public rma_result get_rma(string asset_tag)
        {
            var rma_result = new rma_result();
            MySqlCommand command = conn.CreateCommand();
          
            String cmdText = "SELECT * from rma where ictag = '" + asset_tag + "' limit 1";
            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    rma_result.channel = (reader["channel"].ToString());
                    rma_result.asset_tag = (reader["ictag"].ToString());
                    rma_result.date_request = DateTime.Parse(reader["date_requested"].ToString());
                    rma_result.production_finding = (reader["production_finding"].ToString());
                    rma_result.rma_description = (reader["description"].ToString());
                    rma_result.rma_number = (reader["rma_number"].ToString());
                    rma_result.serial = (reader["serial"].ToString());
                    rma_result.id = (reader["id"].ToString());
                }
                conn.Close();
            }
            return rma_result;
        }
    
    }
}
