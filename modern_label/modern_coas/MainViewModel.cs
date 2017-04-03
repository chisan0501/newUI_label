using MahApps.Metro.Controls.Dialogs;
using MvvmValidation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace modern_coas
{
    class MainViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private bool _canExecute;
        public static string db_source;
        public MainModel mainModel = new MainModel();
        public Mysql_Data mysql_data = new Mysql_Data(db_source);
       
        
        //constructor for mainview
        public MainViewModel(IDialogCoordinator dialogCoordinator)
        {
            
            _canExecute = true;
            _dialogCoordinator = dialogCoordinator;
            try
            {
                Channel_list = mysql_data.get_channel_list();
                Mysql_Status = mysql_data.live_ping();
                Sqlite_Status = mysql_data.localDB_ping();
                
                Validator = new ValidationHelper();
                NotifyDataErrorInfoAdapter = new NotifyDataErrorInfoAdapter(Validator);
            }
            catch
            {

            }
        }

        public void enter_coa_action()
        {
            mysql_data.report(obj_onload, Enter_wcoa, Enter_ocoa, Selected_station);
            scripts.gen_backup();
            scripts.gen_config(Enter_wcoa, Enter_ocoa);
            cleanup();
        }

        private ICommand enter_coa;
        public ICommand Enter_coa
        {
            get
            {
                return enter_coa ?? (enter_coa = new CommandHandler(() => enter_coa_action(), _canExecute));

            }
        }


        private ICommand activate_oem;
        public ICommand Activate_oem
        {
            get
            {
                return activate_oem ?? (activate_oem = new CommandHandler(() => activate_oem_action(), _canExecute));

            }
        }
        public void activate_oem_action()
        {

            mysql_data.report(obj_onload, Oem_coa, "", Selected_station);
            scripts.gen_backup();
            scripts.gen_oem_config(wcoa);
            cleanup();
        }

        public void cleanup()   
        {
            File.Delete("C:\\Windows\\IC\\config.bat");
            
            Application.Current.MainWindow.Close();
        }

        public void activate_mar_action()
        {


            //input keys to config.bat
            mysql_data.report(obj_onload, Wcoa, Ocoa, Selected_station);
            scripts.gen_backup();
            scripts.gen_config(Wcoa, Ocoa);
            cleanup();
        }


        public async void reuse_command_action()
        {

            var result = await _dialogCoordinator.ShowMessageAsync(this, "Message", "Return the Current COAs to Database?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
                AnimateShow = true,
                AnimateHide = false
            });

            try
            {
                if (result.ToString() == "Affirmative")
                {
                    if (!string.IsNullOrEmpty(Wcoa_id))
                    {
                        mysql_data.resuse_coa(Wcoa_id);
                    }
                    if (!string.IsNullOrEmpty(Ocoa_id))
                    {
                        mysql_data.resuse_coa(Ocoa_id);
                    }
                    await _dialogCoordinator.ShowMessageAsync(this, "Message", "Task Complete");
                }
                else
                {
                    return;
                }
            }

            catch (Exception e) {
                await _dialogCoordinator.ShowMessageAsync(this, "Error", "Task Failed /n/r "+ e.Message );

            }




        }

        private ICommand reuse_command;
        public ICommand Reuse_command
        {
            get
            {
                return reuse_command ?? (reuse_command = new CommandHandler(() => reuse_command_action(), _canExecute));
            }
        }

           
        private ICommand activate_mar;
        public ICommand Activate_mar
        {
            get
            {
                return activate_mar ?? (activate_mar = new CommandHandler(() => activate_mar_action(), _canExecute));

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

        private readonly IDialogCoordinator _dialogCoordinator;
        protected ValidationHelper Validator { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged = null;
        private NotifyDataErrorInfoAdapter NotifyDataErrorInfoAdapter { get; set; }
        private void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            // Notify the UI that the property has changed so that the validation error gets displayed (or removed).
            RaisePropertyChanged(e.PropertyName);
            return;
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

        public obj_on_load obj_onload;

        protected virtual void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));

                switch (propName)
                {

                 
                    case ("Oem_coa"):
                        break;
                    case ("Asset_assign"):
                        break;
                    case ("SKU_assigned"):
                        
                        break;
                    case ("Selected_channel"):
                       
                        Next_wcoa = mysql_data.get_next_wcoa(Selected_channel, Selected_station);
                        Next_ocoa = mysql_data.get_next_wcoa(Selected_channel, Selected_station);
                        Wcoa_dropdown_name = mysql_data.wcoa_station_name(Selected_station);
                        Ocoa_dropdown_name = mysql_data.ocoa_station_name(Selected_station);
                        var result = mysql_data.get_data(Selected_channel, Wcoa_dropdown_name, Ocoa_dropdown_name);
                        Wcoa_id = result.wcoa_id;
                        Ocoa_id = result.ocoa_id;
                        Wcoa = result.wcoa;
                        Ocoa = result.ocoa;
                        Wcoa_num = result.wcoa_count;
                        Ocoa_num = result.ocoa_count;
                        mysql_data.Query_cleanup(Wcoa, Ocoa);
                        scripts.gen_preconfig();
                       Mar_enable = true;
                        break;

                    
                    case ("Search_COA"):

                        Search_coa_result = mysql_data.search_coa(Search_coa);

                        break;
                    case ("Selected_station"):
                        Tab_enable = true;
                        break;
                    case "Db_select_item":
                        
                        switch (Db_select_item)
                        {
                            
                            case ("Online DB"):

                                db_source = "Online DB";
                                mysql_data.change_connection_string(db_source);
                                Station_list.Clear();
                                Station_list = mysql_data.Station();
                                obj_onload = mysql_data.sku_assigned(serial);
                                SKU_assigned = obj_onload.sku;
                                Asset_assign = obj_onload.ictag;
                                break;


                            case ("Local DB"):
                                db_source = "Local DB";

                                mysql_data.change_connection_string(db_source);
                                Station_list.Clear();
                                Station_list = mysql_data.Station();
                                break;
                           
                               
                        }
                        break;
                }
            }
        }
        //button event for reuse coas
        private ICommand reuse_coa;
        public ICommand Reuse_coa
        {
            get
            {
                return reuse_coa ?? (reuse_coa = new CommandHandler(() => ReUseAction(), _canExecute));
            }
        }

        public async void StartDriver()
        {
           var result =  await _dialogCoordinator.ShowMessageAsync(this, "Message", "Start Snappy Driver?",MessageDialogStyle.AffirmativeAndNegative,new MetroDialogSettings
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
                AnimateShow = true,
                AnimateHide = false
            });
        
        }

        //reuse coa button action 
        public async void ReUseAction()
        {
            bool sucess = mysql_data.resuse_coa(Search_coa);
            if(sucess == true)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Message", "Task Completed,Make Sure to move the paperwork too");
            }
            else
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error", "Task Failed");
            }
           
        }


        private bool mar_enable;
        public bool Mar_enable
        {
            get
            {
                return mar_enable;
            }
            set
            {
                mar_enable = value;
                RaisePropertyChanged("Mar_enable");
            }
        }

        private string oem_coa;
        public string Oem_coa
        {
            get
            {
                return oem_coa;
            }
            set
            {
                oem_coa = value;
                RaisePropertyChanged("Oem_coa");
            }
        }
        private string wcoa_dropdown_name;
        public string  Wcoa_dropdown_name
        {
            get
            {
                return wcoa_dropdown_name;
            }
            set
            {
                wcoa_dropdown_name = value;
                RaisePropertyChanged("Wcoa_dropdown_name");
            }
        }
        private string ocoa_dropdown_name;
        public string Ocoa_dropdown_name
        {
            get
            {
                return ocoa_dropdown_name;
            }
            set
            {
                ocoa_dropdown_name = value;
                RaisePropertyChanged("Ocoa_dropdown_name");
            }
        }
        private int ocoa_num;
        public int Ocoa_num
        {
            get { return ocoa_num; }
            set
            {
                ocoa_num = value;
                RaisePropertyChanged("Ocoa_num");
            }
        }
        private int wcoa_num;
        public int Wcoa_num
        {
            get { return wcoa_num; }
            set
            {
                wcoa_num = value;
                RaisePropertyChanged("Wcoa_num");
            }
        }
        private string wcoa_id;
        public string Wcoa_id
        {
            get { return wcoa_id; }
            set
            {
                wcoa_id = value;
                RaisePropertyChanged("Wcoa_id");
            }
        }
        private string ocoa_id;
        public string Ocoa_id
        {
            get { return ocoa_id; }
            set
            {
                ocoa_id = value;
                RaisePropertyChanged("Ocoa_id");
            }
        }
        private string ocoa;
        public string Ocoa
        {
            get
            {
                return ocoa;
            }
            set
            {
                ocoa = value;
                RaisePropertyChanged("Ocoa");
            }
        }

        private string wcoa;
        public string Wcoa
        {
            get
            {
                return wcoa;
            }

            set
            {
                wcoa = value;
                RaisePropertyChanged("Wcoa");
            }
        }

        private List<string> next_ocoa;
        public  List<string> Next_ocoa
        {
            get
            {
                return next_ocoa;
            }
            set
            {
                next_ocoa = value;
                RaisePropertyChanged("Next_ocoa");
            }
        }
        private List<string> next_wcoa;
        public List<string>  Next_wcoa
        {
            get { return next_wcoa; }
            set
            {
                next_wcoa = value;
                RaisePropertyChanged("Next_Wcoa");
            }
        }

        public string serial
        {
            get
            {
                return mainModel.serial;
            }
        }

        private Dictionary<string,string> channel_list;
        public Dictionary<string, string> Channel_list
        {
            get
            {
                return channel_list;
            }
            set
            {
                channel_list = value;
                RaisePropertyChanged("Channel_list");
            }
        }

        private string search_coa_result;
        public string Search_coa_result
        {
            get
            {
                return search_coa_result;
            }
            set
            {
              search_coa_result = value;
                RaisePropertyChanged("Search_coa_result");
            }
        }


        private string asset_assign;
        public string Asset_assign
        {
            get
            {
                return asset_assign;
            }
            set
            {
                asset_assign = value;
                RaisePropertyChanged("Asset_assign");
            }
        }

        private string sku_assigned;
        public string SKU_assigned
        {
            get
            {
                return sku_assigned;
            }
            set
            {
                sku_assigned = value;
                RaisePropertyChanged("SKU_assigned");
            }
        }

        private string search_coa;
        public string Search_coa
        {
            get
            {
                return search_coa;
            }
            set
            {
                search_coa = value;
                RaisePropertyChanged("Search_COA");
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



        private string selected_channel;
        public string Selected_channel
        {
            get
            {
                return selected_channel;
            }
            set
            {
                selected_channel = value;
                RaisePropertyChanged("Selected_channel"); 
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
        
        private string selected_station;
        public string Selected_station
        {
            get
            {
                return selected_station;
            }
            set
            {
                selected_station = value;
                RaisePropertyChanged("Selected_station");
            }
        }

        public List<string> DB_select
        {
            get { return mainModel.db_select; }
            set
            {
                mainModel.db_select = value;

            }


        }



        private string network_Driver_status;
        public string Network_Driver_status
        {
            get
            {
                
                var drives = MainModel.get_networkDrive();
                string result = drives.Count + " Drive(s) Mapped\n";
                foreach (var item in drives)
                {
                    
                    result += item;
                }
                return result;
            }
            set
            {
                network_Driver_status = value;
                RaisePropertyChanged("Network_Driver_status");
            }
        }

        private string driver_status;
        public string Driver_status
        {
            get
            {
                driver_status = MainModel.driver();
                if(driver_status == "")
                {

                    
                   
                    return "No Missing Drivers";
                }
                else
                {
                    
                    
               //     Process.Start(@"c:\windows\IC\coas\driver.bat").WaitForExit();
                    return MainModel.driver();
                }
               
            }
            set
            {
                driver_status = value;
                RaisePropertyChanged("DriverStatus");
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
        private List<string> station_list { get {
                return mysql_data.Station();
            } set { }
        } 
        public List<string> Station_list
        {
            get
            {
                return station_list;
            }
            set
            {
                station_list = value;
                RaisePropertyChanged("Station_list");
            }

        }

        private string enter_ocoa;
        public string Enter_ocoa
        {
            get
            {
                return enter_ocoa;

            }
            set
            {
                enter_ocoa = value;
                RaisePropertyChanged("Enter_ocoa");
            }
        }

        private string enter_wcoa;
        public string Enter_wcoa
        {
            get
            {
                return enter_wcoa;
            }
            set
            {
                enter_wcoa = value;
                RaisePropertyChanged("Enter_wcoa");
            }
        }


        private bool tab_enable;
        public bool Tab_enable
        {
            get
            {
                return tab_enable;
            }
            set
            {

                tab_enable = value;
                RaisePropertyChanged("tab_enable");
            }
        }
    }
}
