using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MvvmValidation;
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

namespace MahApps.Metro.Application1
{
    class MainViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
       public MainViewModel()
        {
           
        }
     
        private NotifyDataErrorInfoAdapter NotifyDataErrorInfoAdapter { get; set; }
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


        public event PropertyChangedEventHandler PropertyChanged = null;
        protected virtual void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));

                switch (propName)
                {

                }
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
                RaisePropertyChanged("Tab_enable");
            }
        }
    }
}
