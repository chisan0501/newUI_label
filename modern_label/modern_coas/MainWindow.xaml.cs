using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Configuration;
using System.Media;
using System.Threading;

namespace modern_coas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            MainViewModel main = new MainViewModel(DialogCoordinator.Instance);
            this.DataContext = main;
            InitializeComponent();

            SystemSounds.Beep.Play();
            Thread.Sleep(300);
            SystemSounds.Beep.Play();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
        }

        private void Pre_COA_KeyDown(object sender, KeyEventArgs e)
        {
            string input = search_coa_box.Text;
          
            StringBuilder sb = new StringBuilder();
            switch(input.Length)
            {
                case 5:
                case 9:
                case 13:
                      search_coa_box.Text = search_coa_box.Text + "-";
                    break;
                case 17:
                 //  search_coa_box.Text.Replace("--", "-");
                    break;
                   
            }
            if(search_coa_box.Text.Length != 0)
            {
                search_coa_box.SelectionStart = search_coa_box.Text.Length + 1;
            }
       


        }

        private void search_coa_box_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
    
}
