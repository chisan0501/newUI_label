using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_coas
{
    class scripts
    {

         
       
        public static void win_injection(string wcoa)
        {
            try
            {
                string path = "w:\\windows\\IC";
                StreamReader zreader = new StreamReader(
                    path + "\\Config.bat");
                String line = zreader.ReadToEnd();

                zreader.Close();
                StreamWriter zwriter = new StreamWriter(
                    path + "\\Config.bat");
                zwriter.Write(line.Replace("set win-key=", "set win-key=" + wcoa));
                zwriter.Close();
            }
            catch (FileNotFoundException ex)
            {
                Console.Write(ex.Message);

            }
        }
        public static void office_injection(string ocoa)
        {
            try
            {
                string path = "w:\\windows\\IC";
                
                StreamReader zreader = new StreamReader(
                    path + "\\Config.bat");
                String line = zreader.ReadToEnd();
                zreader.Close();
                StreamWriter zwriter = new StreamWriter(
                    path + "\\Config.bat");
                zwriter.Write(line.Replace("set office-key=", "set office-key=" + ocoa));
                zwriter.Close();

            }
            catch (FileNotFoundException ex)
            {
                Console.Write(ex.Message);

            }

        }

        public static void gen_preconfig (){

           try
            {
                string script_content = Mysql_Data.get_script("preconfig.bat");
                string path = "w:\\windows\\IC";
                path = path+ "\\preconfig.bat";
                StreamWriter batwriter = new StreamWriter(path);
                batwriter.WriteLine(script_content);
               
                batwriter.Close();

                Process.Start(path).WaitForExit();
                File.Delete(path);
            }
           catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        public static void gen_backup()
        {

        
                string script_content = Mysql_Data.get_script("Setup_Recovery_Partition_BIOS.cmd");
                //string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string path = "w:\\Windows\\IC\\Setup_Recovery_Partition_BIOS.cmd";
                StreamWriter batwriter = new StreamWriter(path);
                batwriter.WriteLine(script_content);
                batwriter.Close();

        }

        public static void gen_oem_config (string wcoa)
        {
            string script_content = Mysql_Data.get_script("oem_config.bat");
           
            string path = "w:\\Windows\\IC\\config.bat";
            StreamWriter batwriter = new StreamWriter(path);
            batwriter.WriteLine(script_content);
            batwriter.Close();
            Process.Start(path).WaitForExit();
        }

        public static void gen_config (string wcoa, string ocoa)
        {
          
           
                string script_content = Mysql_Data.get_script("currentMar_config.bat");
                script_content = script_content.Replace("set win-key=", "set win-key=" + wcoa);
                script_content = script_content.Replace("set office-key=", "set office-key=" + ocoa);
              //  string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string path = "w:\\Windows\\IC\\config.bat";
                StreamWriter batwriter = new StreamWriter(path);



                batwriter.WriteLine(script_content);
                batwriter.Close();
           
                Process.Start(path).WaitForExit();
                
            }
      
          

         
        }
    }
