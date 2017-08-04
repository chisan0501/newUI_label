using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD_OfflineCOA
{
    class OfflineModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {

                switch (property)
                {
                    case ("Selected_station"):
                        Message = "You Selected " + Selected_station;
                        
                        break;
                }

                PropertyChanged(this, new PropertyChangedEventArgs(property));


            }
        }

        
        public List<string> Station_dropdown
        {

            get
            {
                _Station_dropdown = new List<string>();
                _Station_dropdown.Add("123");
                return _Station_dropdown;
            }
            

        }

        
        public int SwitchView
        {
            get
            {
                return _switchView;
            }
            set
            {
                if (_switchView != value)
                {
                    _switchView = value;
                    RaisePropertyChanged("SwitchView");
                }
                
            }
        }
        
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        private List<string> _Station_dropdown;
        private int _switchView;
        private string _message;
        private string _selected_station;
        private string _model;
        private string _ram;
        private string _hdd;
        private string _cpu;
        private string _serial;
        private string _ictags;
        private string _sku;
        private string _manu;
        private string _screen;
        private string _video;
        private string _preCOA;

        //public OfflineModel(int switchView, string message,string selected_station, string model, string ram, string hdd, string cpu, string serial, string ictags, string sku, string manu, string screen, string video, string preCOA)
        //{
        //    _Station_dropdown = Station_dropdown;
        //    _switchView = switchView;
        //    _message = message;
        //    _selected_station = selected_station;
        //    _model = model;
        //    _ram = ram;
        //    _hdd = hdd;
        //    _cpu = cpu;
        //    _serial = serial;
        //    _ictags = ictags;
        //    _sku = sku;
        //    _manu = manu;
        //    _screen = screen;
        //    _video = video;
        //    _preCOA = preCOA;
        //}

        public string Selected_station
        {
            get
            {
                return _selected_station;
            }
            set
            {
                if (_selected_station != value)
                {
                    _selected_station = value;
                    RaisePropertyChanged("Selected_station");
                }
            }
        }
        public string Model { get => _model; set => _model = value; }
        public string Ram { get => _ram; set => _ram = value; }
        public string Hdd { get => _hdd; set => _hdd = value; }
        public string Cpu { get => _cpu; set => _cpu = value; }
        public string Serial { get => _serial; set => _serial = value; }
        public string Ictags { get => _ictags; set => _ictags = value; }
        public string Sku { get => _sku; set => _sku = value; }
        public string Manu { get => _manu; set => _manu = value; }
        public string Screen { get => _screen; set => _screen = value; }
        public string Video { get => _video; set => _video = value; }
        public string PreCOA { get => _preCOA; set => _preCOA = value; }
    }

    class COAsModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private string _wcoas;
        private string _ocoas;
        private string _ocoaID;
        private string _wcoaID;
        private int _wcoaCount;
        private int _ocoaCount;

        public COAsModel(string wcoas, string ocoas, string ocoaID, string wcoaID, int wcoaCount, int ocoaCount)
        {
            _wcoas = wcoas;
            _ocoas = ocoas;
            _ocoaID = ocoaID;
            _wcoaID = wcoaID;
            _wcoaCount = wcoaCount;
            _ocoaCount = ocoaCount;
        }

        public string Wcoas { get => _wcoas; set => _wcoas = value; }
        public string Ocoas { get => _ocoas; set => _ocoas = value; }
        public string OcoaID { get => _ocoaID; set => _ocoaID = value; }
        public string WcoaID { get => _wcoaID; set => _wcoaID = value; }
        public int WcoaCount { get => _wcoaCount; set => _wcoaCount = value; }
        public int OcoaCount { get => _ocoaCount; set => _ocoaCount = value; }
    }
}
