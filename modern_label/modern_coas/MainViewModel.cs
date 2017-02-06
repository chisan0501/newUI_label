using MahApps.Metro.Controls.Dialogs;
using MvvmValidation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_coas
{
    class MainViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {

        public static string db_source;
        public MainModel mainModel = new MainModel();
        public Mysql_Data mysql_data = new Mysql_Data(db_source);


        //constructor for mainview
        public MainViewModel(IDialogCoordinator dialogCoordinator)
        {

            _dialogCoordinator = dialogCoordinator;
            try
            {
                Mysql_Status = mysql_data.live_ping();
                Sqlite_Status = mysql_data.localDB_ping();
                Validator = new ValidationHelper();
                NotifyDataErrorInfoAdapter = new NotifyDataErrorInfoAdapter(Validator);
            }
            catch
            {

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
