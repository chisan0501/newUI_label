using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offline_coa
{
    class OfflineModel : INotifyPropertyChanged
    {

        private string _wcoa;
        private string _ocoa;
        private string _date;
        private string _model;
        private string _manufacture;
        private string _asset_tag;
        private string _serial;
        private string _ram;
        private string _hdd;
        private string _sku;
        private string _cpu;
        private string _preCoa;
        private string _location;
        private string _videoCard;
        private string _screenSize;
            
        public string Wcoa
        {
            get
            {
                return _wcoa;
            }

            set
            {
                _wcoa = value;
                RaisePropertyChanged("Wcoa");
            }
        }

        public string Ocoa
        {
            get
            {
                return _ocoa;
            }

            set
            {
                _ocoa = value;
                RaisePropertyChanged("Ocoa");
            }
        }

        public string Date
        {
            get
            {
                return _date;
            }

            set
            {
                _date = value;
                RaisePropertyChanged("Date");
            }
        }

        public string Model
        {
            get
            {
                return _model;
            }

            set
            {
                _model = value;
                RaisePropertyChanged("Model");
            }
        }

        public string Manufacture
        {
            get
            {
                return _manufacture;
            }

            set
            {
                _manufacture = value;
                RaisePropertyChanged("Manufacture");
            }
        }

        public string Asset_tag
        {
            get
            {
                return _asset_tag;
            }

            set
            {
                _asset_tag = value;
                RaisePropertyChanged("Asset_tag");
            }
        }

        public string Serial
        {
            get
            {
                return _serial;
            }

            set
            {
                _serial = value;
                RaisePropertyChanged("Serial");
            }
        }

        public string Ram
        {
            get
            {
                return _ram;
            }

            set
            {
                _ram = value;
                RaisePropertyChanged("Ram");
            }
        }

        public string Hdd
        {
            get
            {
                return _hdd;
            }

            set
            {
                _hdd = value;
                RaisePropertyChanged("HDD");
            }
        }

        public string Sku
        {
            get
            {
                return _sku;
            }

            set
            {
                _sku = value;
                RaisePropertyChanged("Sku");
            }
        }

        public string Cpu
        {
            get
            {
                return _cpu;
            }

            set
            {
                _cpu = value;
                RaisePropertyChanged("CPU");
            }
        }

        public string PreCoa
        {
            get
            {
                return _preCoa;
            }

            set
            {
                _preCoa = value;
                RaisePropertyChanged("PreCOA");
            }
        }

        public string Location
        {
            get
            {
                return _location;
            }

            set
            {
                _location = value;
                RaisePropertyChanged("location");
            }
        }

        public string VideoCard
        {
            get
            {
                return _videoCard;
            }

            set
            {
                _videoCard = value;
                RaisePropertyChanged("VideoCard");
            }
        }

        public string ScreenSize
        {
            get
            {
                return _screenSize;
            }

            set
            {
                _screenSize = value;
                RaisePropertyChanged("ScreenSize");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }



    }
}
