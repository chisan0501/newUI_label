using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Management;
using System.Windows.Forms;
using System.Windows.Input;
using MvvmValidation;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace modern_monitor
{
    class MainViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {


        #region properties
        Monitor_models main_model = new Monitor_models();
        private bool _canExecute;
        public static string db_source;
        private readonly IDialogCoordinator _dialogCoordinator;
        public Mysql_Data mysql_data = new Mysql_Data(db_source);
        public event PropertyChangedEventHandler PropertyChanged = null;
        private NotifyDataErrorInfoAdapter NotifyDataErrorInfoAdapter { get; set; }

        protected ValidationHelper Validator { get; private set; }
        #endregion



        #region fields 

        #endregion




        private string asset_tag;
        public string Asset_tag
        {
            get
            {
                return asset_tag;
            }
            set
            {
                asset_tag = value;
                RaisePropertyChanged("Asset_tag");
            }
        }

        private string db_select_item;
        public string Db_select_item
        {
            get { return db_select_item; }
            set
            {
                if (value != db_select_item)
                {
                    db_select_item = value;
                    RaisePropertyChanged("Db_select_item");
                }
            }

        }

        private List<string> monitor_list;
        public List<string> Monitor_list
        {
            get
            {
                return monitor_list;
            }
            set
            {
                monitor_list = value;
                RaisePropertyChanged("Monitor_list");
            }
        }

        public class CommandHandler : ICommand
        {
            private Action _action;
            private bool _canExecute;
            public CommandHandler(Action action, bool canExecute)
            {
                _action = action;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                _action();
            }
        }

        private string selected_device;
        public string Selected_device
        {
            get
            {
                return selected_device;
            }
            set
            {
                selected_device = value;
                RaisePropertyChanged("Selected_device");
            }
        }
        private string sqlite_Status;
        public string Sqlite_Status
        {
            get
            {
                return sqlite_Status;
            }
            set
            {
                sqlite_Status = value;
                RaisePropertyChanged("Sqlite_Status");
            }
        }
        private string mysql_Status;
        public string Mysql_Status
        {
            get
            {
                return mysql_Status;
            }
            set
            {
                mysql_Status = value;
                RaisePropertyChanged("Mysql_Status");
            }
        }

        public static int get_networkDrive()
        {
            var result = new List<string>();
            int count = 0;
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
            count = result.Count;
            return count;

        }


        private string selected_resou;
        public string Selected_resou
        {
            get
            {
                return selected_resou;

            }
            set
            {
                selected_resou = value;
                RaisePropertyChanged("Selected_resou");
            }
        }

        public MainViewModel(IDialogCoordinator dialogCoordinator)
        {
            Validator = new ValidationHelper();

            NotifyDataErrorInfoAdapter = new NotifyDataErrorInfoAdapter(Validator);
            Validator.AddRequiredRule(() => Asset_tag, "Asset Tag is required");
            Validator.AddRequiredRule(() => Selected_resou, "Please Select Resolution");
            Validator.AddRequiredRule(() => Selected_device, "Please Select a Device");
            NotifyDataErrorInfoAdapter.ErrorsChanged += OnErrorsChanged;

            _canExecute = true;
            _dialogCoordinator = dialogCoordinator;
            Mysql_Status = mysql_data.live_ping();
            Sqlite_Status = mysql_data.localDB_ping();
         
            
            //Validator.AddRequiredRule(() => Users_SelectedValue, "User is required");
            //Validator.AddRequiredRule(() => Computer_type_value, "Computer Type is required");
            //Validator.AddRequiredRule(() => Selected_channel, "Channel is required");
            //Validator.AddRequiredRule(() => Selected_sku, "SKU is required");
            
        }
        
        
        private void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            // Notify the UI that the property has changed so that the validation error gets displayed (or removed).
            RaisePropertyChanged(e.PropertyName);
            return;
        }
        private string manu;
        private string model;
        private string name;
        private string monitor_ID;
        private string serial;
        private string size;
        private List<String> resou;
        public Monitor_models get_monitor_detail(string model)
        {
            Monitor_models result = new Monitor_models();

            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_MonitorDetails  where Model = '" + model + "'");

                foreach (ManagementObject queryObj in searcher.Get())
                {



                    Model = (queryObj["Model"].ToString());
                    Monitor_ID = (queryObj["MonitorID"].ToString());
                    var manafacture = queryObj["MonitorID"].ToString();
                    Manu = manafacture.Substring(0, 2);
                    Manu = manu_database(Manu);
                    Name = (queryObj["Name"].ToString());
                    Serial = (queryObj["SerialNumber"].ToString());
                    Size = (queryObj["SizeDiagInch"].ToString());

                    var rounded = Math.Round(double.Parse(size), 0, MidpointRounding.AwayFromZero);
                    result.SizeDiaginch = rounded.ToString();
                    Resou = monitor_resou();

                    //round_up();

                    //five.Items.Add(monitor_size);



                }
            }
            catch (ManagementException e)
            {
                //  ("An error occurred while querying for WMI data: " + e.Message);
            }
            return result;
        }

        public void reuse_commmand_action()
        {

        }

        private ICommand reuse_commmand;
        public ICommand Reuse_commmand
        {
            get
            {
                return reuse_commmand ?? ( reuse_commmand = new CommandHandler(() => reuse_commmand_action(), _canExecute)); 
            }
        }

        private ICommand save_data;
        public ICommand Save_data
        {
            get
            {
                return save_data ?? (save_data = new CommandHandler(() => save_data_action(), _canExecute));

            }
        }

        public async void save_data_action()
        {
            if (get_networkDrive() == 0)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error", "No Network Drive Mapped");
                return;
            }
            ValidationResult validationResult = Validator.ValidateAll();
            string message = "Please Check the following input: \n\r";
            foreach (var item in validationResult.ErrorList)
            {
                message += item.ErrorText + "\n\r";

            }
            if (validationResult.IsValid == false)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error", message);
                return;
            }
            Monitor_models result = new Monitor_models();
            result.Model = Model;
            result.Manafacture = Manu;
            result.MonitorID = Monitor_ID;
            result.Name = Name;
            result.SerialNumber = Serial;
            result.SizeDiaginch = Size;
            write_xml();
            try
            {
                mysql_data.write_data(result, Asset_tag, Selected_resou);
            }
            catch (Exception e)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error", e.Message);
            }
           


        }

        private string manu_database(string manafacture)
        {
            string result = "";
            try
            {
                string dbHost = "MYSQL5013.Smarterasp.net";
                string dbUser = "a094d4_icdb";
                string dbPass = "icdb123!";
                string dbName = "db_a094d4_icdb";
                string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
                MySqlConnection conn = new MySqlConnection(connStr);
                MySqlCommand command = conn.CreateCommand();
                conn.Open();
                command.CommandText = "Select manu from monitor_setting where manu_wmi like '%" + manafacture + "%'";
                MySqlDataReader objreader = command.ExecuteReader();
                while (objreader.Read())
                {
                    if (objreader["manu"].ToString() != null)
                    {
                        result = (objreader["manu"].ToString());
                    }
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("An error has occurred: " + ex.Message);
            }
            return result;
        }

        private List<string> monitor_resou()
        {
            List<string> result = new List<string>();
            result.Add("");
            for (int i = 0; i < Screen.AllScreens.Length; i++)
            {
                var height = Screen.AllScreens[i].Bounds.Height;
                var width = Screen.AllScreens[i].Bounds.Width;
                result.Add(width + "x" + height);
            }





            return result;

        }

        public async void discover_monitor_action()
        {

            MvvmValidation.ValidationResult validationResult = Validator.Validate(() => Asset_tag);
            string message = "Please Check the following input: \n\r";
            foreach (var item in validationResult.ErrorList)
            {
                message += item.ErrorText + "\n\r";

            }
            if (validationResult.IsValid == false)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error", message);
                return;
            }
            Monitor_list = new List<string>();
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_MonitorDetails");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Monitor_list.Add(queryObj["Model"].ToString());
                  
                }


            }
            catch (ManagementException e)
            {
                //  ("An error occurred while querying for WMI data: " + e.Message);
            }
        }


        private ICommand selected_device_detail;
        public ICommand Selected_device_detail
        {
            get
            {

                return selected_device_detail ?? (selected_device_detail = new CommandHandler(() => get_monitor_detail(Selected_device), _canExecute));
            }
        }



        


        private ICommand discover_monitor;
        public ICommand Discover_monitor
        {
            get
            {
                return discover_monitor ?? (discover_monitor = new CommandHandler(() => discover_monitor_action(), _canExecute));

            }
        }



        public List<string> DB_select
        {
            get
            {
                return main_model.db_select;
            }
        }

        public string Manu
        {
            get
            {
                return manu;
            }

            set
            {
                manu = value;
                RaisePropertyChanged("Manu");
            }
        }

        private string onlineDB_setting;
        public string OnlineDB_setting
        {
            get
            {
                var result = ConfigurationManager.AppSettings["OnlineMySqlConnectionString"];
                return result;
            }
            set
            {
                onlineDB_setting = value;
                RaisePropertyChanged("OnlineDB_setting");
            }
        }

        private string localDB_setting;
        public string LocalDB_setting
        {
            get
            {
                var result = ConfigurationManager.AppSettings["LocalMySqlConnectionString"];
                return result;
            }

            set
            {
                localDB_setting = value;
                RaisePropertyChanged("LocalDB_setting");
            }
        }
        public string Model
        {
            get
            {
                return model;
            }

            set
            {
                model = value;
                RaisePropertyChanged("Model");
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string Monitor_ID
        {
            get
            {
                return monitor_ID;
            }

            set
            {
                monitor_ID = value;
                RaisePropertyChanged("Monitor_ID");
            }
        }

        public string Serial
        {
            get
            {
                return serial;
            }

            set
            {
                serial = value;
                RaisePropertyChanged("Serial");
            }
        }

        public string Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
                RaisePropertyChanged("Size");
            }
        }

        public List<String> Resou
        {
            get
            {
                return resou;
            }

            set
            {
                resou = value;
                RaisePropertyChanged("Resou");
            }
        }
        public IEnumerable GetErrors(string propertyName)
        {
            return NotifyDataErrorInfoAdapter.GetErrors(propertyName);
        }

        public bool HasErrors
        {

            get { return NotifyDataErrorInfoAdapter.HasErrors; }
        }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add { NotifyDataErrorInfoAdapter.ErrorsChanged += value; }
            remove { NotifyDataErrorInfoAdapter.ErrorsChanged -= value; }
        }


        protected virtual void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));

                switch (propName)
                {
                    case "Asset_tag":

                        break;
                    case "Db_select_item":
                        switch (Db_select_item)
                        {

                            case ("Online DB"):

                                db_source = "Online DB";
                                mysql_data.change_connection_string(db_source);

                                break;


                            case ("Local DB"):
                                db_source = "Local DB";

                                mysql_data.change_connection_string(db_source);

                                break;


                        }
                        break;
                }
            }

        }

        public async void write_xml()
        {
            //prepare the xml file to be uploaded to mapped driver, by default the drive letter is y
            StreamReader reader = new StreamReader("source.txt");
            string line = reader.ReadToEnd();
            reader.Close();
            StreamWriter writer = new StreamWriter("y:/" + Asset_tag + ".xml");
            writer.Write(line.Replace("DELL 1907FP", Model));
            writer.Close();
            reader = new StreamReader("y:/" + Asset_tag + ".xml");


            string line5 = reader.ReadToEnd();
            reader.Close();
            writer = new StreamWriter("y:/" + Asset_tag + ".xml");
            writer.Write(line5.Replace("null", Size));
            writer.Close();

            reader = new StreamReader("y:/" + Asset_tag + ".xml");
            string line6 = reader.ReadToEnd();
            reader.Close();
            writer = new StreamWriter("y:/" + Asset_tag + ".xml");
            writer.Write(line6.Replace("900 x 1600", Selected_resou));
            writer.Close();


            reader = new StreamReader("y:/" + Asset_tag + ".xml");
            string line2 = reader.ReadToEnd();
            reader.Close();
            writer = new StreamWriter("y:/" + Asset_tag + ".xml");
            writer.Write(line2.Replace("22501", Asset_tag));
            writer.Close();

            reader = new StreamReader("y:/" + Asset_tag + ".xml");
            string line3 = reader.ReadToEnd();
            reader.Close();
            writer = new StreamWriter("y:/" + Asset_tag + ".xml");
            writer.Write(line3.Replace("19.1", Size));

            writer.Close();

            reader = new StreamReader("y:/" + Asset_tag + ".xml");
            string line4 = reader.ReadToEnd();
            reader.Close();
            writer = new StreamWriter("y:/" + Asset_tag + ".xml");
            writer.Write(line4.Replace("seriall", Serial));
            writer.Close();

            

            await _dialogCoordinator.ShowMessageAsync(this, "Message", "XML Created");
        }

    }
}
