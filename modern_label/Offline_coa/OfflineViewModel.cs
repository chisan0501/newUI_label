using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offline_coa
{
    class OfflineViewModel : INotifyPropertyChanged
    {
        public OfflineViewModel()
        {

            Offline = new OfflineModel() { Wcoa = "123" };
        }
        private OfflineModel _Offline;
        public OfflineModel Offline
        {
            get
            {
                return _Offline;
            }
            set
            {
                _Offline = value;
                RaisePropertyChanged("OfflineModel");
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
