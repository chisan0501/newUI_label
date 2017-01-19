using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MvvmValidation;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace modern_label
{
    class LabelViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
       

        public DYMO.Label.Framework.ILabel current_label;
        private readonly IDialogCoordinator _dialogCoordinator;
        protected ValidationHelper Validator { get; private set; }
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


        public LabelViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
            //user dropdown data
            //saving user data to a list
            try
            {
                
                Validator = new ValidationHelper();
                NotifyDataErrorInfoAdapter = new NotifyDataErrorInfoAdapter(Validator);
                Validator.AddRequiredRule(() => Selected_printer, "Printer is required");
                Validator.AddRequiredRule(() => Users_SelectedValue, "User is required");
                Validator.AddRequiredRule(() => Computer_type_value, "Computer Type is required");
                Validator.AddRequiredRule(() => Selected_channel, "Channel is required");
                Validator.AddRequiredRule(() => Selected_sku, "SKU is required");
                NotifyDataErrorInfoAdapter.ErrorsChanged += OnErrorsChanged;
                RefrubHistoryObj = new RefrubHistoryObj();
                LabelModel = new LabelModel();
                _canExecute = true;
                //user name list
                LabelModel.users = new List<string>();
                LabelModel.db_select = new List<string>();

                LabelModel.db_select.Add("MYSQL");
                LabelModel.db_select.Add("SQLite");
                LabelModel.is_mysql_open = mysql_data.ping();
                LabelModel.is_sqlite_open = sqlite_data.ping();

                submit_precoa = "00999-999-000-999";


                //ping sqlite

                LabelModel.sqlite_status = "SQLite : Online";


                //ping mysql

                LabelModel.mysql_status = "MySQl : Online";




                if (LabelModel.is_mysql_open == false)
                {
                    LabelModel.mysql_status = "MySQl : Offline";
                }
                if (LabelModel.is_sqlite_open == false)
                {
                    LabelModel.sqlite_status = "SQLite : Offline";

                }
            }
            catch
            {
                //update the status bar message to fail

            }

        }

        public async void customAction() {
            MvvmValidation.ValidationResult validationResult = Validator.ValidateAll();
            string message = "Please Check the following input: \n\r";
            foreach (var item in validationResult.ErrorList)
            {
                message += item.ErrorText + "\n\r";

            }
            if(validationResult.IsValid == false)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error", message);
                return;
            }
            string sku = "Empty";
            var listing = new magento_listing();
            RefrubHistoryObj = mysql_data.redisco_data(submit_asset);
            RefrubHistoryObj.refurbisher = this.Users_SelectedValue;
            RefrubHistoryObj.sku = sku + grade_Selected_value;
            RefrubHistoryObj.channel = Selected_channel;
            RefrubHistoryObj.selected_printer = Selected_printer;
            RefrubHistoryObj.type = Computer_type_value;
            RefrubHistoryObj.pre_coa = Submit_precoa;
            RefrubHistoryObj.grade = grade_Selected_value;

            switch (selected_channel)
            {
                case "Mar (Desktop)":
                case "Mar (Laptop)":
                    //format the sku before passing on
                    
                    //create the temp variable to hold the sku construct info 
                    var temp_brand = magento_sku.compute_difference(RefrubHistoryObj.made, magento_sku.brand_name());
                    RefrubHistoryObj.brand = temp_brand;
                     var temp_cpu = magento_sku.comput_title(RefrubHistoryObj);
                    var temp_ram = magento_sku.ram_format(RefrubHistoryObj, false);
                    var temp_hdd = magento_sku.hdd_format(false, RefrubHistoryObj);
                    RefrubHistoryObj.sku = temp_brand + "_" + RefrubHistoryObj.model + "_" + temp_cpu + "_" + temp_ram + "_" + temp_hdd + RefrubHistoryObj.type + RefrubHistoryObj.grade;
                    var magento_listing = listing.listing_info(RefrubHistoryObj);
                    RefrubHistoryObj = magento_sku.format_sku(RefrubHistoryObj);
                    break;
                case "Online Order":                  
                    await _dialogCoordinator.ShowInputAsync(this, "Online Order", "Please Enter Order #").ContinueWith(t => sku = (t.Result));
                    break;
                case "My Channel is not Listed":
                    await _dialogCoordinator.ShowInputAsync(this, "Custom Channel", "Please Enter Channel Name").ContinueWith(t => sku = (t.Result));
                    Selected_channel = sku + grade_Selected_value;
                    break;
                default:
                    RefrubHistoryObj.sku = Selected_sku + grade_Selected_value;
                    break;
               
            }
            Label_make = RefrubHistoryObj.made;
            Label_model = RefrubHistoryObj.model;
            Label_cpu = RefrubHistoryObj.cpu;
            Label_ram = RefrubHistoryObj.ram;
            Label_hdd = RefrubHistoryObj.hdd;
            Label_serial = RefrubHistoryObj.serial;
            
            var dymo = new Dymo_provider();
           
            current_label = dymo.generate_label(RefrubHistoryObj);
            Preview = dymo.generate_preview(current_label);
            Printer_enable = true;
        }
        public void printAction()
        {
            //validate if magento have exisiting listing
            //insert listing if listing is non exisit
            magento_listing mage = new magento_listing();
            mage.get_exisiting(RefrubHistoryObj);
           // current_label.Print(Selected_printer);
        }

        private ICommand print;
        public ICommand Print
        {
            get
            {
                return print ?? (print = new CommandHandler(() => printAction(), _canExecute));
            }


        }

        private ICommand showInputDialogCommand;

        public ICommand ShowInputDialogCommand
        {
            get
            {
                return showInputDialogCommand ?? (showInputDialogCommand = new CommandHandler(() => customAction(), _canExecute));
                
            }
        }
        public event PropertyChangedEventHandler PropertyChanged = null;
        protected virtual void RaisePropertyChanged(string propName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));

                switch (propName) {
                    //gererate list for flyout on histroy button
                    case "History_fly":

                        MyList = new ObservableCollection<RefrubHistoryObj>();
                        MyList = mysql_data.db_history();

                        break;
                      
                    case "Selected_sku":

                        break;

                    case "Asset_tag":
                        
                        var discovery_result = new discovery_result(this.asset_tag);
                        break;
                     //chnage text on user selected
                    case "Users_SelectedValue":
                        Welcome_text = this.Users_SelectedValue;
                        User_status = this.Users_SelectedValue;
                        break;
                    case "Computer_type_selected":
                        Computer_type_status = "Computer Type : " + Computer_type_selected;
                        break;
                    case "Submit_asset":
                        
                        break;
                    case "Selected_channel":
                       
                       Sku_list = mysql_data.sku_list(selected_channel);

                         break;
                    case "Label_model":
                        if (current_label != null) {
                            current_label.SetObjectText("manu", Label_model); 
                        }
                    
                        break;
                    case "Label_cpu":
                        if (current_label != null)
                        {
                            current_label.SetObjectText("cpu", Label_cpu);
                        }
                        break;
                    case "Label_ram":
                        if (current_label != null)
                        {
                            current_label.SetObjectText("ram", Label_ram + "GB");
                        }
                        break;
                    case "Label_hdd":
                        if (current_label != null)
                        {
                            current_label.SetObjectText("hdd", Label_hdd + "GB");
                        }
                        break;

                    case "Db_select_item":
                       
                  
                    switch (Db_select_item)
                        {
                            case ("MYSQL"):
                                Enable_history_btn = true;
                                
                                var user_list = mysql_data.users();
                                Users.Clear();
                                Users = user_list.users;

                                break;


                            case ("SQLite"):

                                Enable_history_btn = false;
                                var sql_lite_user_list = mysql_data.users();
                                Users.Clear();
                                Users = sql_lite_user_list.users;
                                break;
                        }
                        break;


                }
                
            }
        }
        //command class for binding event 
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

        
        public  BitmapImage Preview
        {
            get
            {
                return LabelModel.Preview;
            }
            set
            {
                LabelModel.Preview = value;
                RaisePropertyChanged("Preview");
            }
        }
        //enable history button flyout
        public void MyAction()
        {
          
            History_fly = true;
        }
        public async void GenAction()
        {
            switch (Selected_channel)
            {
                case "Mar (Desktop)":
                case "Mar (Laptop)":
                case "OEM (Desktop)":
                case "OEM (Laptop)":
                    string sku_gen = "";
                    MessageDialogResult result = await ShowMessage("SKU Assigned For : " + Submit_asset, sku_gen, MessageDialogStyle.Affirmative).ConfigureAwait(false);
                    break;
                case "My Channel is not Listed":
                  
                    break;
                
            }

           
        }

        private bool printer_enable;
        public bool Printer_enable
        {
            get
            {
                return printer_enable;
            }
            set
            {
                printer_enable = value;
                RaisePropertyChanged("Printer_enable");
            }
        }

        private string label_serial;
        public string Label_serial
        {

            get { return label_serial; }
            set
            {
                label_serial = value;
                RaisePropertyChanged("Label_serial");
            }

        }

        private string label_hdd;
        public string Label_hdd
        {
            get { return label_hdd; }
            set
            {
                label_hdd = value;
                RaisePropertyChanged("Label_hdd");
            }
        }

        private string label_ram;
        public string Label_ram
        {

            get { return label_ram; }
            set
            {
                label_ram = value;
                RaisePropertyChanged("Label_ram");
            }

        }


        private string label_cpu;
        public string Label_cpu
        {

            get { return label_cpu; }
            set
            {
                label_cpu = value;
                RaisePropertyChanged("Label_cpu");
            }


        }


        private string label_model;
        public string Label_model
        {


            get { return label_model; }
            set
            {
                label_model = value;
                RaisePropertyChanged("Label_model");

            }
        }

        private string label_make;
        public string Label_make
        {
            get { return label_make; }
            set {
                label_make = value;
                RaisePropertyChanged("Label_make");

            }


        }


        public List<string> Printer_list
        {
            get { return LabelModel.printer_list; }
            set { LabelModel.printer_list = value; }


        }

     

        private string selected_printer;
        public string Selected_printer
        {
            get { return selected_printer; }
            set { selected_printer = value;
                RaisePropertyChanged("Selected_printer");

            }



        }


       

        public List<LabelModel.grade> Grade_list
        {
            get { return LabelModel.grade_list; }
            set { LabelModel.grade_list = value;
                RaisePropertyChanged("Grade_list");
            }
        }


        private string grade_selected_value;
        public string grade_Selected_value
        {
            get { return grade_selected_value; } 
            set {
                grade_selected_value = value;
                RaisePropertyChanged("grade_Selected_value");
            }
        }

        private string selected_sku;
        public string Selected_sku
        {

            get { return selected_sku; }

            set { selected_sku = value;
                RaisePropertyChanged("Selected_sku");

            }

        }


        



        public async Task<MessageDialogResult> ShowMessage(string title,string message, MessageDialogStyle dialogStyle)
        {
            var metroWindow = (Application.Current.MainWindow as MetroWindow);
            metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;

            return await metroWindow.ShowMessageAsync(title, message, dialogStyle, metroWindow.MetroDialogOptions);
        }
        
        //sku flyout command
        private ICommand _genCommand;
        public ICommand GenCommand
        {
            get
            {
                return _genCommand ?? (_genCommand = new CommandHandler(() => GenAction(), _canExecute));

            }

        }

        //histroy click command
        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => MyAction(), _canExecute));
            }
        }

     

        private bool _canExecute;
        public Idbprovider mysql_data = new Mysql_DataProvider();
        public Idbprovider sqlite_data = new SQlite_DataProvider();
        public RefrubHistoryObj RefrubHistoryObj { get; set; }
        public LabelModel LabelModel { get; set; }
        private ObservableCollection<RefrubHistoryObj> myList;
        public ObservableCollection<RefrubHistoryObj> MyList {

            get { return myList; }
            set { myList = value;
                RaisePropertyChanged("MyList");
            }

        }



        private string submit_precoa;
        public string Submit_precoa
        {

            get { return submit_precoa; }
            set { submit_precoa = value;
                RaisePropertyChanged("Submit_precoa");
            }

        }

        private List<string> sku_list;
        public List<string> Sku_list
        {

            get { return sku_list; }

            set
            {

                sku_list = value;
                RaisePropertyChanged("Sku_list");
            }

        }

        private string selected_channel;
        public string Selected_channel
        {

            get { return selected_channel; }
            set { selected_channel = value;
                RaisePropertyChanged("Selected_channel");
            }

        }


        public List<string> channel_list
        {
            get { return mysql_data.channel_list(); }
            set {}

        }
     

        private int submit_asset;
        public int Submit_asset
        {

            get { return submit_asset; }
            set {

                if(value!= submit_asset)
                submit_asset = value;
                RaisePropertyChanged("Submit_asset");
            }

        }
       
      


        private bool history_fly;
        public bool History_fly
        {
            get { return history_fly; } 
            set
            {
                if (value != history_fly)
                {
                    history_fly = value;

                    RaisePropertyChanged("History_fly");
                }
               

            }
           
        }


        private string user_status;
        public string User_status
        {


            get { return "User : " +user_status; }
            set
            {
                if(value != user_status)
                {
                    user_status = value;
                    RaisePropertyChanged("User_status");
                }


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

        private Boolean enable_history_btn;
        public Boolean Enable_history_btn
        {
            get { return enable_history_btn; }
            set { 
                if (value != enable_history_btn)
                {
                    enable_history_btn = value;
                   
                    RaisePropertyChanged("Enable_history_btn");
                }
                    
                    
                } 

        }
        private string welcome_text;
        public string Welcome_text
        {

            get { return "Hello " + welcome_text; }
            set {
                if(value != welcome_text)
                {
                    welcome_text = value;
                    RaisePropertyChanged("Welcome_text");
                }
               
             

            }
        }


      

        private string asset_tag;
        public string Asset_tag
        {
            get { return this.asset_tag; }
            set { this.asset_tag = value;
                RaisePropertyChanged("Asset_tag");
            }

        }

        private string computer_type_status;
        public string Computer_type_status
        {
            get { return this.computer_type_status; }
            set
            {
                this.computer_type_status = value;
                RaisePropertyChanged("Computer_type_status");
            }

        }



        public List<LabelModel.computer_type>Computer_type
        {
            get { return LabelModel.computer_dropdown; }
            set { LabelModel.computer_dropdown = value; }

        }

        public List<string>DB_select
        {
            get { return LabelModel.db_select; }
            set { LabelModel.db_select = value;

            }


        }

        private string computer_type_value;
        public string Computer_type_value
        {
            get { return this.computer_type_value; }
            set
            {


                computer_type_value = value;
                RaisePropertyChanged("Computer_type_value");


            }


        }
        private string computer_type_selected;
        public string Computer_type_selected
        {
            get { return this.computer_type_selected; }
            set
            {

                    computer_type_selected = value;
                    RaisePropertyChanged("Computer_type_selected");  
            }


        }

        private string users_SelectedValue;
        public string Users_SelectedValue
        {
            get { return this.users_SelectedValue; }
            set { this.users_SelectedValue = value;
                RaisePropertyChanged("Users_SelectedValue");
            }


        }

        public List<string> Users
        {
            get { return LabelModel.users; }
            set { LabelModel.users = value;
                RaisePropertyChanged("Users");
               
            }

        }
        
        public bool Is_Mysql_open
        {
            get { return LabelModel.is_mysql_open; }
            set { LabelModel.is_mysql_open = value; }
        }
        public string Mysql_Status
        {
            get { return LabelModel.mysql_status; }
            set { LabelModel.mysql_status = value; }


        }
        public bool Is_Sqlite_open
        {
            get { return LabelModel.is_sqlite_open; }
            set { LabelModel.is_sqlite_open = value; }
        }
        public string Sqlite_Status
        {
            get { return LabelModel.sqlite_status; }
            set { LabelModel.sqlite_status = value; }


        }
    }

    class imaging_search_result
    {

        public Imaging_search_result Imaging_result { get; set; }
        public imaging_search_result (string asset)
        {
            Imaging_result = new Imaging_search_result();
            string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";

            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();
            String cmdText = "SELECT * from production_log where ictags = '" + asset + "'";
            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader

                while (reader.Read())
                {
                    Imaging_result.imaging_search_wcoa = (reader["wcoa"].ToString());
                    Imaging_result.imaging_search_ocoa = (reader["ocoa"].ToString());
                    Imaging_result.imaging_search_hdd = (reader["hdd"].ToString());
                    Imaging_result.imaging_search_ram = (reader["ram"].ToString());
                    Imaging_result.imaging_search_video = (reader["video_card"].ToString());
                    Imaging_result.imaging_search_sku = (reader["channel"].ToString());
                }
                conn.Close();
            }



            }
        public string img_wcoa {

            get { return Imaging_result.imaging_search_wcoa; }
            set { Imaging_result.imaging_search_wcoa = value; }

        }
        public string img_ocoa
        {

            get { return Imaging_result.imaging_search_ocoa; }
            set { Imaging_result.imaging_search_ocoa = value; }

        }
        public string img_video
        {

            get { return Imaging_result.imaging_search_video; }
            set { Imaging_result.imaging_search_video = value; }

        }
        public string img_sku
        {

            get { return Imaging_result.imaging_search_sku; }
            set { Imaging_result.imaging_search_sku = value; }

        }
        public string img_hdd
        {

            get { return Imaging_result.imaging_search_hdd; }
            set { Imaging_result.imaging_search_hdd = value; }

        }
        public string img_ram
        {

            get { return Imaging_result.imaging_search_ram; }
            set { Imaging_result.imaging_search_ram = value; }

        }
    }


    public class discovery_result 
    {
        public Idbprovider mysql_data = new Mysql_DataProvider();
        public Idbprovider sqlite_data = new SQlite_DataProvider();
        public Discovery_result Discovery_result { get; set; }
        public discovery_result(string asset)
        {
            Discovery_result = new Discovery_result();
           
            Discovery_result = mysql_data.discovery_data(asset);
        }
       
      
        public string cpu
        {
            get { return Discovery_result.search_cpu; }
            set { Discovery_result.search_cpu = value; }
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



class search_result
    {
        public Search_result Search_result { get; set; }
        public search_result(string asset)
        {
            Search_result = new Search_result();
            //this is the viewModel for search flyout
            string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";

            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();
            String cmdText = "SELECT * from rediscovery where ictag = '" + asset + "'";
            using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
            {
                MySqlDataReader reader = cmd.ExecuteReader(); //execure the reader
                while (reader.Read())
                {
                    Search_result.search_cpu = (reader["cpu"].ToString());
                    Search_result.search_hdd = (long.Parse(reader["hdd"].ToString()));
                    Search_result.search_manu = (reader["brand"].ToString());
                    Search_result.search_ram = (long.Parse(reader["ram"].ToString()));
                    Search_result.search_model = (reader["model"].ToString());
                    Search_result.search_serial = (reader["serial"].ToString());
                    Search_result.search_optical_drive = (reader["optical_drive"].ToString());
                    Search_result.search_sku  = (reader["pallet"].ToString());
                }
                conn.Close();
            }
        }
            public string cpu
        {
            get { return Search_result.search_cpu; }
            set { Search_result.search_cpu = value; }
        }
        public string manu
        {
            get { return Search_result.search_manu; }
            set { Search_result.search_manu = value; }
        }
        public string model
        {
            get { return Search_result.search_model; }
            set { Search_result.search_model = value; }
        }
        public string serial
        {
            get { return Search_result.search_serial; }
            set { Search_result.search_serial = value; }
        }
        public string optical_drive
        {
            get { return Search_result.search_optical_drive; }
            set { Search_result.search_optical_drive = value; }
        }
        public long ram
        {
            get { return Search_result.search_ram; }
            set { Search_result.search_ram = value; }

        }
        public long hdd
        {
            get { return Search_result.search_hdd; }
            set { Search_result.search_hdd = value; }
        }
        public string sku
        {
            get { return Search_result.search_sku; }
            set { Search_result.search_sku = value; }
        }
    }


}
