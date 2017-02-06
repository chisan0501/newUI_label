using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_label
{
    class labelVM
    {

        public ObservableCollection<Label_model> Label
        {
            get;
            set;
        }
        public void LoadLabel()
        {
            ObservableCollection<Label_model> label = new ObservableCollection<Label_model>();

            label.Add(new Label_model { Db_list = { "something" } } );
            Label = label;

            
        }

    }
}
