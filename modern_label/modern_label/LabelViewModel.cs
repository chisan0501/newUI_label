using DYMO.Label.Framework;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MvvmValidation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace modern_label
{
    class LabelViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {

        #region Properties
        public string rma_id;
        private bool _canExecute;

        public Mysql_DataProvider mysql_data = new Mysql_DataProvider(db_source);
        // public Idbprovider sqlite_data = new SQlite_DataProvider();
        public RefrubHistoryObj RefrubHistoryObj { get; set; }
        public static string db_source;
        public DYMO.Label.Framework.ILabel current_label;
        private readonly IDialogCoordinator _dialogCoordinator;
        protected ValidationHelper Validator { get; private set; }
        private NotifyDataErrorInfoAdapter NotifyDataErrorInfoAdapter { get; set; }

        public sf sf { get; set; }
        public rma_result rma_result { get; set; }
        public LabelModel LabelModel { get; set; }
        public discovery_result discovery_result { get; set; }
        public search_result search_result { get; set; }

        #endregion


        #region method


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

        public async void customAction()
        {

            MvvmValidation.ValidationResult validationResult = Validator.ValidateAll();
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
            string sku = "Empty";

            var listing = new magento_listing();

            RefrubHistoryObj = mysql_data.redisco_data(submit_asset);
            if (RefrubHistoryObj.cpu == null)
            {
                message = "Asset Not Found in Rediscovery.";
                await _dialogCoordinator.ShowMessageAsync(this, "Error", message);
                return;
            }

            RefrubHistoryObj.refurbisher = this.Users_SelectedValue;
            RefrubHistoryObj.sku = sku + grade_Selected_value;
            RefrubHistoryObj.channel = Selected_channel;
            RefrubHistoryObj.selected_printer = Selected_printer;
            RefrubHistoryObj.type = Computer_type_value;
            RefrubHistoryObj.pre_coa = Submit_precoa;
            RefrubHistoryObj.grade = grade_Selected_value;

            switch (selected_channel)
            {
                case "Tableau (Laptop)":
                case "Tableau (Desktop)":
                case "NGO":
                case "OEM (Desktop)":
                case "OEM (Laptop)":
                case "Mar (Desktop)":
                case "Mar (Laptop)":
                    //format the sku before passing on
                    string temp_cpu;
                    //create the temp variable to hold the sku construct info 
                    var temp_brand = magento_sku.compute_difference(RefrubHistoryObj.made, magento_sku.brand_name());
                    RefrubHistoryObj.brand = temp_brand;
                    if(selected_channel == "NGO")
                    {
                         temp_cpu = magento_sku.ngo_title(RefrubHistoryObj);
                    }
                    else
                    {
                         temp_cpu = magento_sku.comput_title(RefrubHistoryObj);
                    }
                    


                    RefrubHistoryObj.ram = magento_sku.ram_format(RefrubHistoryObj, false);
                    RefrubHistoryObj.hdd = magento_sku.hdd_format(false, RefrubHistoryObj);
                    RefrubHistoryObj.grade = grade_Selected_value;

                    var magento_listing = listing.listing_info(RefrubHistoryObj);
                    RefrubHistoryObj = magento_sku.format_sku(RefrubHistoryObj);
                
                        RefrubHistoryObj.sku = RefrubHistoryObj.brand + "_" + RefrubHistoryObj.model + "_" + temp_cpu + "_" + RefrubHistoryObj.ram + "_" + RefrubHistoryObj.hdd + RefrubHistoryObj.type + RefrubHistoryObj.grade;
                    
                 
                    break;
                case "Online Order":
                    await _dialogCoordinator.ShowInputAsync(this, "Online Order", "Please Enter Order #").ContinueWith(t => sku = (t.Result));
                    RefrubHistoryObj.sku = sku;
                    break;
                case "My Channel is not Listed":
                    await _dialogCoordinator.ShowInputAsync(this, "Custom Channel", "Please Enter Channel Name").ContinueWith(t => sku = (t.Result));
                    RefrubHistoryObj.sku = sku;

                    break;
                default:
                    RefrubHistoryObj.sku = Selected_sku + grade_Selected_value;

                    break;

            }
            Label_Is_SSD = RefrubHistoryObj.is_ssd;
            Label_make = RefrubHistoryObj.made;
            Label_model = RefrubHistoryObj.model;
            Label_cpu = RefrubHistoryObj.cpu;
            Label_ram = RefrubHistoryObj.ram;
            Label_hdd = RefrubHistoryObj.hdd;
            Label_serial = RefrubHistoryObj.serial;
            switch (selected_channel)
            {
                case "OEM (Desktop)":
                case "OEM (Laptop)":
                    RefrubHistoryObj.sku = "OEM_" + RefrubHistoryObj.sku;
                    break;
                case "NGO":
                    RefrubHistoryObj.sku = RefrubHistoryObj.sku + "_NGO";
                    break;
                case "Tableau (Desktop)":
                case "Tableau (Laptop)":
                    RefrubHistoryObj.sku = RefrubHistoryObj.sku + "_Tab";
                    break;
            }
            Label_sku = RefrubHistoryObj.sku;
            
            var img_result = mysql_data.img_data(submit_asset.ToString());
            Img_wcoa = img_result.img_wcoa;
            Img_ocoa = img_result.img_ocoa;
            Img_sku = img_result.img_sku;
            Img_ram = img_result.img_ram;
            Img_hdd = img_result.img_hdd;
            Img_video = img_result.img_video;

            var dymo = new Dymo_provider();

            current_label = dymo.generate_label(RefrubHistoryObj);
            Preview = dymo.generate_preview(current_label);
            Printer_enable = true;







        }
        public void printAction()
        {
            //validate if magento have exisiting listing
            //insert listing if listing is non exisit
            if (Selected_channel != "Online Order")
            {
                magento_listing mage = new magento_listing();
                mage.get_exisiting(RefrubHistoryObj);
                //mage.update_qty("SAMSUNG_MS-7125_AMD_16_200_DK", "2", "1976");



            }
            if (Copy_discovery == true)
            {
                mysql_data.discovery_insert(RefrubHistoryObj);
            }
            LabelWriterPrintParams prms = new LabelWriterPrintParams(1, "print job 1", DYMO.Label.Framework.FlowDirection.LeftToRight, RollSelection.Auto, LabelWriterPrintQuality.BarcodeAndGraphics);
            current_label.Print(Selected_printer, prms);
            bool sucess = mysql_data.insert(RefrubHistoryObj);
        }

        public async void EditAction()
        {
            string message = "";
            Rma_finding = "Refrubisher: " + Users_SelectedValue + "\r\nDate received: " + Today + "\r\nDiagnostic finding: " + Rma_comment + Next_process;
            bool sucess = sf.update_rma(rma_id, rma_finding, rma_ictag);
            if (sucess == true)
            {
                message = "RMA Info Updated";
                await _dialogCoordinator.ShowMessageAsync(this, "Message", message);
            }
            else
            {
                message = "An error has occurred";
                await _dialogCoordinator.ShowMessageAsync(this, "Error", message);
            }
        }

        public async void AddChannelAction()
        {
            if (string.IsNullOrEmpty(Add_channel))
            {
                string message = "Channel Name cant be Empty";
                await _dialogCoordinator.ShowMessageAsync(this, "Error", message);
            }
            else
            {
                string sku = "";
                await _dialogCoordinator.ShowInputAsync(this, "Additional Information Required", "Please Enter SKU").ContinueWith(t => sku = (t.Result));
                var result = mysql_data.add_channel(Add_channel, sku);

                if (result == true)
                {
                    string message = "Channel Added";
                    await _dialogCoordinator.ShowMessageAsync(this, "Message", message);
                }
                else
                {
                    string message = "Something went Wrong";
                    await _dialogCoordinator.ShowMessageAsync(this, "Error", message);
                }
            }

        }
        #endregion

        #region fields

        private ObservableCollection<RefrubHistoryObj> myList;
        public ObservableCollection<RefrubHistoryObj> MyList
        {

            get { return myList; }
            set
            {
                myList = value;
                RaisePropertyChanged("MyList");
            }

        }



        private string submit_precoa;
        public string Submit_precoa
        {

            get { return submit_precoa; }
            set
            {
                submit_precoa = value;
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
            set
            {
                selected_channel = value;
                RaisePropertyChanged("Selected_channel");
            }

        }

        private List<string> Channel_list;
        public List<string> channel_list
        {
            get { return Channel_list; }
            set
            {
                Channel_list = value;
                RaisePropertyChanged("Channel_list");

            }

        }


        private int submit_asset;
        public int Submit_asset
        {

            get { return submit_asset; }
            set
            {

                if (value != submit_asset)
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


            get { return "User : " + user_status; }
            set
            {
                if (value != user_status)
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
            set
            {
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
            set
            {
                if (value != welcome_text)
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
            set
            {
                this.asset_tag = value;
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


        private List<LabelModel.computer_type> computer_type;
        public List<LabelModel.computer_type> Computer_type
        {
            get { return computer_type; }
            set
            {
                computer_type = value;
                RaisePropertyChanged("computer_type");
            }

        }
        private List<string> db_select;
        public List<string> DB_select
        {
            get { return db_select; }
            set
            {
                db_select = value;
                RaisePropertyChanged("Db_select");
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
            set
            {
                this.users_SelectedValue = value;
                RaisePropertyChanged("Users_SelectedValue");
            }


        }
        private List<string> users;
        public List<string> Users
        {
            get { return users; }
            set
            {
                users = value;
                RaisePropertyChanged("Users");

            }

        }
        private bool is_mysql_open;
        public bool Is_Mysql_open
        {
            get { return is_mysql_open; }
            set { is_mysql_open = value; RaisePropertyChanged("is_Mysql_open"); }
        }

        private string mysql_status;
        public string Mysql_Status
        {
            get { return mysql_status; }
            set { mysql_status = value; RaisePropertyChanged("mysql_status"); }


        }
        private bool is_sqlite_open;
        public bool Is_Sqlite_open
        {
            get { return is_sqlite_open; }
            set { is_sqlite_open = value; RaisePropertyChanged("is_sqlite_open"); }
        }


        private string sqlite_status;
        public string Sqlite_Status
        {
            get { return sqlite_status; }
            set { sqlite_status = value; RaisePropertyChanged("sqlite_status"); }


        }

        private int grade_index;
        public int Grade_index
        {
            get
            {
                return grade_index;
            }
            set
            {
                grade_index = value;
                RaisePropertyChanged("Grade_index");
            }
        }

        private BitmapImage preview;
        public BitmapImage Preview
        {
            get
            {
                return preview;
            }
            set
            {
                preview = value;
                RaisePropertyChanged("Preview");
            }
        }
        //enable history button flyout



        private string rma_comment;
        public string Rma_comment
        {
            get { return rma_comment; }
            set
            {
                rma_comment = value;
                RaisePropertyChanged("rma_comment");
            }
        }



        private string rma_rma_search;
        public string Rma_rma_search
        {
            get
            {
                return rma_rma_search;
            }
            set
            {
                rma_rma_search = value;
                RaisePropertyChanged("Rma_rma_search");
            }
        }

        private string rma_serial_search;
        public string Rma_serial_search
        {
            get { return rma_serial_search; }
            set
            {
                rma_serial_search = value;
                RaisePropertyChanged("Rma_serial_search");
            }
        }

        private string rma_asset;
        public string Rma_asset
        {

            get { return rma_asset; }
            set
            {
                rma_asset = value;
                RaisePropertyChanged("rma_asset");
            }


        }

        private string img_ram;
        public string Img_ram
        {
            get
            {
                return img_ram;
            }
            set
            {
                img_ram = value;
                RaisePropertyChanged("img_ram");
            }
        }
        private string img_hdd;
        public string Img_hdd
        {
            get
            {
                return img_hdd;
            }
            set
            {

                img_hdd = value;
                RaisePropertyChanged("img_hdd");
            }
        }
        private string img_video;
        public string Img_video
        {
            get
            {
                return img_video;
            }
            set
            {
                img_video = value;
                RaisePropertyChanged("img_video");
            }
        }
        private string img_sku;
        public string Img_sku
        {
            get
            {
                return img_sku; ;
            }
            set
            {
                img_sku = value;
                RaisePropertyChanged("img_sku");
            }
        }

        private string img_ocoa;
        public string Img_ocoa
        {
            get
            {
                return img_ocoa;
            }
            set
            {
                img_ocoa = value;
                RaisePropertyChanged("img_ocoa");
            }
        }
        private string img_wcoa;
        public string Img_wcoa
        {
            get { return img_wcoa; }
            set
            {
                img_wcoa = value;
                RaisePropertyChanged("img_wcoa");
            }
        }

        private string rediscovery_search_optical;
        public string Rediscovery_search_optical
        {
            get { return rediscovery_search_optical; }
            set
            {
                rediscovery_search_optical = value;
                RaisePropertyChanged("rediscovery_search_optical");
            }
        }

        private string rediscovery_search_serial;
        public string Rediscovery_search_serial
        {
            get { return rediscovery_search_serial; }
            set
            {
                rediscovery_search_serial = value;
                RaisePropertyChanged("rediscovery_search_serial");
            }
        }

        private long rediscovery_search_hdd;
        public long Rediscovery_search_hdd
        {
            get { return rediscovery_search_hdd; }
            set
            {
                rediscovery_search_hdd = value;
                RaisePropertyChanged("rediscovery_search_hdd");
            }
        }

        public long rediscovery_search_ram;
        public long Rediscovery_search_ram
        {
            get { return rediscovery_search_ram; }
            set
            {
                rediscovery_search_ram = value;
                RaisePropertyChanged("rediscovery_search_ram");
            }
        }

        public string rediscovery_search_manu;
        public string Rediscovery_search_manu
        {
            get { return rediscovery_search_manu; }
            set
            {
                rediscovery_search_manu = value;
                RaisePropertyChanged("rediscovery_search_manu");
            }
        }

        public string rediscovery_search_model;
        public string Rediscovery_search_model
        {
            get { return rediscovery_search_model; }
            set
            {
                rediscovery_search_model = value;
                RaisePropertyChanged("rediscovery_search_model");
            }
        }

        private string rediscovery_search_cpu;
        public string Rediscovery_search_cpu
        {
            get { return rediscovery_search_cpu; }
            set
            {
                rediscovery_search_cpu = value;
                RaisePropertyChanged("rediscovery_search_cpu");
            }
        }

        private string rediscovery_search_sku;
        public string Rediscovery_search_sku
        {
            get { return rediscovery_search_sku; }
            set
            {
                rediscovery_search_sku = value;
                RaisePropertyChanged("rediscovery_search_sku");
            }
        }

        private string search_optical;
        public string Search_optical
        {
            get { return search_optical; }
            set
            {
                search_optical = value;
                RaisePropertyChanged("Search_optical");
            }
        }


       
        


        private string search_serial;
        public string Search_serial
        {
            get { return search_serial; }
            set
            {
                search_serial = value;
                RaisePropertyChanged("Search_serial");
            }
        }

        private long search_hdd;
        public long Search_hdd
        {
            get { return search_hdd; }
            set
            {
                search_hdd = value;
                RaisePropertyChanged("Search_hdd");
            }
        }

        public long search_ram;
        public long Search_ram
        {
            get { return search_ram; }
            set
            {
                search_ram = value;
                RaisePropertyChanged("Search_ram");
            }
        }

        public string search_manu;
        public string Search_manu
        {
            get { return search_manu; }
            set
            {
                search_manu = value;
                RaisePropertyChanged("Search_manu");
            }
        }

        public string search_model;
        public string Search_model
        {
            get { return search_model; }
            set
            {
                search_model = value;
                RaisePropertyChanged("Search_model");
            }
        }

        private string search_cpu;
        public string Search_cpu
        {
            get { return search_cpu; }
            set
            {
                search_cpu = value;
                RaisePropertyChanged("Search_cpu");
            }
        }

        private bool copy_discovery;
        public bool Copy_discovery
        {

            get
            {
                return copy_discovery;
            }
            set
            {
                copy_discovery = value;
                RaisePropertyChanged("Copy_discovery");
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


        private string label_is_ssd;
        public string Label_Is_SSD
        {
            get { return label_is_ssd; }
            set
            {
                label_is_ssd = value;
                RaisePropertyChanged("label_Is_SSD");
            }
        }

        private string label_sku;
        public string Label_sku
        {

            get { return label_sku; }
            set
            {
                label_sku = value;
                RaisePropertyChanged("Label_sku");
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
            set
            {
                label_make = value;
                RaisePropertyChanged("Label_make");

            }


        }

        public List<string> printer_list;
        public List<string> Printer_list
        {
            get { return printer_list; }
            set
            {
                printer_list = value;
                RaisePropertyChanged("printer_list");

            }


        }



        private string selected_printer;
        public string Selected_printer
        {
            get { return selected_printer; }
            set
            {
                selected_printer = value;
                RaisePropertyChanged("Selected_printer");

            }



        }



        public List<LabelModel.grade> grade_list;
        public List<LabelModel.grade> Grade_list
        {
            get { return grade_list; }
            set
            {
                grade_list = value;
                RaisePropertyChanged("Grade_list");
            }
        }


        private string grade_selected_value;
        public string grade_Selected_value
        {
            get { return grade_selected_value; }
            set
            {
                grade_selected_value = value;

                RaisePropertyChanged("grade_Selected_value");
            }
        }

        private string selected_sku;
        public string Selected_sku
        {

            get { return selected_sku; }

            set
            {
                selected_sku = value;
                RaisePropertyChanged("Selected_sku");

            }

        }


        private int db_index;
        public int Db_index
        {
            get
            {
                return db_index;
            }
            set
            {
                db_index = value;
                RaisePropertyChanged("Db_index");
            }
        }


        private ICommand edit_rma;
        public ICommand Edit_rma
        {
            get
            {
                return edit_rma ?? (edit_rma = new CommandHandler(() => EditAction(), _canExecute));
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
        private string rma_finding;
        public string Rma_finding
        {

            get { return rma_finding; }
            set
            {
                rma_finding = value;
                RaisePropertyChanged("rma_finding");
            }

        }

        private string rma_channel;
        public string Rma_channel
        {
            get { return rma_channel; }
            set
            {
                rma_channel = value;
                RaisePropertyChanged("rma_channel");
            }

        }

        private string rma_number;
        public string Rma_number
        {
            get { return rma_number; }
            set
            {
                rma_number = value;
                RaisePropertyChanged("rma_number");
            }
        }

        private DateTime rma_date;
        public DateTime Rma_date
        {
            get { return rma_date; }
            set
            {
                rma_date = value;
                RaisePropertyChanged("rma_date");
            }
        }

        private string rma_ictag;
        public string Rma_ictag
        {
            get { return rma_ictag; }
            set
            {
                rma_ictag = value;
                RaisePropertyChanged("rma_ictag");
            }
        }
        private string rma_serial;
        public string Rma_serial
        {
            get { return rma_serial; }
            set
            {
                rma_serial = value;
                RaisePropertyChanged("rma_serial");
            }
        }


        public DateTime Today
        {
            get { return DateTime.Now; }
            set
            {
                Today = value;
                RaisePropertyChanged("today");
            }

        }


        private bool refrub_checked;
        public bool Refrub_checked
        {
            get { return refrub_checked; }
            set
            {
                refrub_checked = value;
                RaisePropertyChanged("Refrub_checked");
            }
        }
        private bool recycle_checked;
        public bool Recycle_checked
        {
            get { return recycle_checked; }
            set
            {
                recycle_checked = value;
                RaisePropertyChanged("Recycle_checked");
            }
        }
        private bool ebay_checked;
        public bool Ebay_checked
        {
            get { return ebay_checked; }
            set
            {
                ebay_checked = value;
                RaisePropertyChanged("Ebay_checked");
            }
        }
        private string rma_desc;
        public string Rma_desc
        {
            get { return rma_desc; }
            set
            {
                rma_desc = value;
                RaisePropertyChanged("rma_desc");
            }
        }

        private string next_process;
        public string Next_process
        {
            get { return next_process; }
            set
            {
                next_process = value;
                RaisePropertyChanged("next_process");
            }
        }

        private ICommand addChannelCommand;
        public ICommand AddChannelCommand
        {
            get
            {

                return addChannelCommand ?? (addChannelCommand = new CommandHandler(() => AddChannelAction(), _canExecute));

            }
        }

        private bool enable_grade = true;
        public bool Enable_grade
        {
            get
            {
                return enable_grade;
            }
            set
            {
                enable_grade = value;
                RaisePropertyChanged("Enable_grade");
            }
        }

        private string add_channel;
        public string Add_channel
        {
            get
            {
                return add_channel;
            }
            set
            {
                add_channel = value;
            }
        }
        private ICommand print;
        public ICommand Print
        {
            get
            {
                return print ?? (print = new CommandHandler(() => printAction(), _canExecute));
            }


        }

        #endregion

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
                discovery_result = new discovery_result();
               
                LabelModel = new LabelModel();
                //add db select list
                DB_select = LabelModel.db_select;
                Computer_type = LabelModel.computer_dropdown;
                Grade_list = LabelModel.grade_list;
                Printer_list = LabelModel.printer_list;
                channel_list = mysql_data.channel_list();
                _canExecute = true;
            
                
                //ping db
                is_mysql_open = mysql_data.ping();
                //get user
                Users = mysql_data.users();
              

                submit_precoa = "00999-999-000-999";

                Enable_history_btn = true;

                this.Db_index = 1 ;
                Sqlite_Status = "Local Database : Offline";
                Mysql_Status = "MySQl Database: Offline";
                if (mysql_data.ping() == true)
                { 
                    Mysql_Status = "MySQl Database: Online";
                    this.Db_index = 0;
                }
                if (LabelModel.is_sqlite_open == false)
                {
                    Sqlite_Status = "Local Database : Online";
                    
                }
            }
            catch (Exception e)
            {
                //update the status bar message to fail
              
                 
                
            }

        }

   


        public event PropertyChangedEventHandler PropertyChanged = null;
        protected virtual async void RaisePropertyChanged(string propName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));

                switch (propName)
                {
                  
                    case "Rma_rma_search":
                        sf = new sf();
                        rma_result = sf.get_rma_num(Rma_rma_search);
                        Rma_number = rma_result.rma_number;
                        Rma_channel = rma_result.channel;
                        Rma_date = rma_result.date_request;
                        Rma_desc = rma_result.rma_description;
                        Rma_ictag = rma_result.asset_tag;
                        Rma_serial = rma_result.serial;
                        rma_id = rma_result.id;
                        break;
                    case "Db_index":

                        switch (Db_index)
                        {
                            case (0):
                                Enable_history_btn = true;
                                db_source = "Online DB";
                                mysql_data.change_connection_string(db_source);
                                channel_list = mysql_data.channel_list();
                              
                                Users = mysql_data.users();



                                break;


                            case (1):

                                Enable_history_btn = false;
                                db_source = "Local DB";
                                mysql_data.change_connection_string(db_source);
                                channel_list = mysql_data.channel_list();
                                
                                Users = mysql_data.users();


                                // Users = sql_lite_user_list.users;
                                break;
                        }
                        break;
                    //gererate list for flyout on histroy button
                    case "History_fly":

                        MyList = new ObservableCollection<RefrubHistoryObj>();
                        MyList = mysql_data.db_history();

                        break;

                    case "Selected_sku":

                        break;
                    case "Rma_serial_search":
                        sf = new sf();
                        rma_result = sf.get_rma_serial(rma_serial_search);
                        Rma_number = rma_result.rma_number;
                        Rma_channel = rma_result.channel;
                        Rma_date = rma_result.date_request;
                        Rma_desc = rma_result.rma_description;
                        Rma_ictag = rma_result.asset_tag;
                        Rma_serial = rma_result.serial;
                        rma_id = rma_result.id;
                        break;
                    case "rma_asset":
                        sf = new sf();
                        rma_result = sf.get_rma(Rma_asset);
                        Rma_number = rma_result.rma_number ;
                        Rma_channel = rma_result.channel;
                        Rma_date = rma_result.date_request;
                        Rma_desc = rma_result.rma_description;
                        Rma_ictag = rma_result.asset_tag;
                        Rma_serial = rma_result.serial;
                        Rma_comment = rma_result.production_finding;
                        rma_id = rma_result.id;
                       

                        break;

                    case "Refrub_checked":
                        Next_process = "\r\nNext process: Refrubish";
                        break;
                    case "Recycle_checked":
                        Next_process = "\r\nNext process: Recycle";
                        break;
                    case "Ebay_checked":
                        Next_process = "\r\nNext process: Ebay";
                        break;
                    case "Asset_tag":

                    var discovery_temp = new discovery_result(Asset_tag);
                        discovery_result = discovery_temp;
                        Search_optical = discovery_temp.optical_drive;
                        Search_cpu = discovery_temp.cpu;
                        Search_hdd = discovery_temp.hdd;
                        Search_manu = discovery_temp.manu;
                        Search_ram = discovery_temp.ram;
                        Search_model = discovery_temp.model;
                        Search_serial = discovery_temp.serial; 
                        
                        var rediscovery_temp = new search_result(Asset_tag);
                        search_result = rediscovery_temp;
                        Rediscovery_search_cpu = rediscovery_temp.cpu;
                        Rediscovery_search_optical = rediscovery_temp.optical_drive;
                        Rediscovery_search_hdd = rediscovery_temp.hdd;
                        Rediscovery_search_manu = rediscovery_temp.manu;
                        Rediscovery_search_ram = rediscovery_temp.ram;
                        Rediscovery_search_model = rediscovery_temp.model;
                        Rediscovery_search_serial = rediscovery_temp.serial;
                        Rediscovery_search_sku = rediscovery_temp.sku;
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
                        if (!string.IsNullOrEmpty(Selected_channel))
                        {
                            Sku_list = mysql_data.sku_list(selected_channel);
                            if (Selected_channel.Contains("TechSoup (Desktop)") || Selected_channel.Contains("Techsoup (Laptop)") || Selected_channel.Contains("Good 360 (Laptop)") || Selected_channel.Contains("Good 360 (Desktop)") || Selected_channel.Contains("NGO") || Selected_channel.Contains("Tableau (Desktop)") || Selected_channel.Contains("Tableau (Laptop)"))
                            {
                                Grade_index = 0;
                                Enable_grade = false;
                            }
                            else
                            {
                                Grade_index = -1;
                                Enable_grade = true;
                            }

                        }

                        break;
                    case "Copy_discovery":

                         break;

                    case "Label_model":
                        if (current_label != null)
                        {
                            current_label.SetObjectText("manu", Label_model);
                            this.RefrubHistoryObj.model = Label_model;
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
                            this.RefrubHistoryObj.ram = Label_ram;
                        }
                        break;
                    case "Label_hdd":
                        if (current_label != null)
                        {
                            current_label.SetObjectText("hdd", Label_hdd + "GB");
                            this.RefrubHistoryObj.hdd = Label_hdd;
                        }
                        break;
                    case "Label_sku":
                        if (current_label != null)
                        {
                            try
                            {
                                current_label.SetObjectText("pallet", Label_sku);
                                current_label.SetObjectText("BARCODE", Label_sku);
                                RefrubHistoryObj.sku = Label_sku;
                            }
                         
                            
                           catch
                            {
                              
                                await _dialogCoordinator.ShowMessageAsync(this, "Error", "SKU Length Exceed Allowed Label Width");
                            }

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

        private ICommand _copyTo_discovery_btn;
        public ICommand copyTo_discovery_btn
        {
            get
            {
                return _clickCommand ?? (_copyTo_discovery_btn = new CommandHandler(() => edit_discoveryAction(), _canExecute));
            }
        }

        private async void edit_discoveryAction()
        {
            var result = mysql_data.copy_discovery_data(Rediscovery_search_cpu, Rediscovery_search_model, Rediscovery_search_manu, Rediscovery_search_ram.ToString(), Rediscovery_search_hdd.ToString(), Rediscovery_search_serial, Asset_tag);

            if (result == true)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Message", "Data Copied to discovery Table");
            }
            else
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error", "Data might have already exisit in discovery table");
            }

        }

        //copy data to rediscovery table
        private ICommand _copyTo_rediscovery_btn;
        public ICommand copyTo_rediscovery_btn
        {
            get
            {
                return _clickCommand ?? (_copyTo_rediscovery_btn = new CommandHandler(() => edit_rediscoveryAction(), _canExecute));
            }
        }

        //discovery search_edit button 
        private ICommand _discovery_search_edit;
        public ICommand discovery_search_edit
        {
            get
            {
                return _clickCommand ?? (_discovery_search_edit = new CommandHandler(() => discovery_search_edit_action(), _canExecute));
            }
        }


        private async void edit_rediscoveryAction()
        {
        var  result =   mysql_data.copy_rediscovery_data(discovery_result.cpu, discovery_result.model, discovery_result.manu, discovery_result.ram.ToString(), discovery_result.hdd.ToString(), discovery_result.serial, Asset_tag);

            if (result == true)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Message", "Data Copied to Rediscovery Table");
            }
            else
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error", "Data might have already exisit in Rediscovery table");
            }
            
        }

        private async void discovery_search_edit_action()
        {
            discovery_result.model = Search_model;
            discovery_result.hdd = Search_hdd;
            discovery_result.ram = Search_ram;
            discovery_result.serial = Search_serial;
            discovery_result.optical_drive = Search_optical;
            bool result = mysql_data.edit_discovery_data(discovery_result, Asset_tag);

            await _dialogCoordinator.ShowMessageAsync(this, "Message", "discovery data Updated");
        }

        //search_edit button command
        private ICommand _search_edit;
        public ICommand search_edit
        {
            get
            {
                return _clickCommand ?? (_search_edit = new CommandHandler(() => search_edit_action(), _canExecute));
            }
        }

        private async void search_edit_action()
        {
            search_result.model = Rediscovery_search_model;
            search_result.sku = Rediscovery_search_sku;
            search_result.hdd = Rediscovery_search_hdd;
            search_result.ram = Rediscovery_search_ram;
            search_result.serial = Rediscovery_search_serial;
            search_result.optical_drive = Rediscovery_search_optical;
            bool result = mysql_data.edit_rediscovery_data(search_result, Asset_tag);
        
            await _dialogCoordinator.ShowMessageAsync(this,"Message", "Rediscovery data Updated");
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

     

        
        
        
        
      
    }

   


   



   


}
