using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_label
{
    class Label_model : INotifyPropertyChanged
    {
        //dropdown list for users on main window


        private List<string> db_list;
        public List<string> Db_list
        {

            get { return db_list; 
        }
            set
            {

                if (value != db_list)
                {
                    db_list = value;
                    RaisePropertyChanged("Db_list");
                }

            }
        }

        private List<string> users;
        public List<string> Users
        {
            get { return users; }
            set
            {

                if (value != users)
                {
                    users = value;
                    RaisePropertyChanged("Users");
                }
            }


        }



        //binding event
        public event PropertyChangedEventHandler PropertyChanged = null;
        protected virtual void RaisePropertyChanged(string propName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }

        }
    }
}
