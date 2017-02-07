using MahApps.Metro.Controls.Dialogs;
using MvvmValidation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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



        protected virtual void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));

                switch (propName)
                {


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

        //reuse coa button action 
        public async void ReUseAction()
        {
            bool sucess = mysql_data.resuse_coa(Search_coa);
            if(sucess == true)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Message", "Make Sure to move the paperwork too");
            }
            else
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error", "Task Failed");
            }
           
        }

        private List<string> channel_list;
        public List<string> Channel_list
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
