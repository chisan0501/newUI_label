using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Speech.Synthesis;
using System.Diagnostics;
using System.Threading;
using System.Management;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace modern_coas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
           try
            {
                MainViewModel main = new MainViewModel(DialogCoordinator.Instance);
                this.DataContext = main;

                var driver_status = MainModel.driver();
                if (driver_status == "")
                {

                }
                else
                {
                    //start driver detection
                    Process.Start(@"x:\coas\driver.bat").WaitForExit();

                }
            }
            catch
            {

            }  
          
            InitializeComponent();
        }

     
    
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SpeechSynthesizer reader = new SpeechSynthesizer();
            
            reader.SpeakAsync("Testing");
        }


        
        private void Pre_COA_KeyDown(object sender, KeyEventArgs e)
        {
            //auto format the COAs when data is input
            if (e.Key == Key.OemMinus)
            {
                e.Handled = true;
            }
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OEM_keydown(object sender, KeyEventArgs e)
        {
            string input = oem_coa_box.Text;
            if (e.Key == Key.OemMinus)
            {
                e.Handled = true;
            }
            StringBuilder sb = new StringBuilder();
            switch (input.Length)
            {
                case 5:
                case 11:
                case 17:
                case 23:
                    oem_coa_box.Text = oem_coa_box.Text + "-";
                    break;
                case 29:
                    oem_coa_box.Text = oem_coa_box.Text.ToUpper();
                    //  search_coa_box.Text.Replace("--", "-");
                    break;
            }
            if (oem_coa_box.Text.Length != 0)
            {
                oem_coa_box.SelectionStart = oem_coa_box.Text.Length + 1;
            }

        }

        private void enter_wcoa_KeyDown(object sender, KeyEventArgs e)
        {
            string input = enter_wcoa.Text;
            if (e.Key == Key.OemMinus)
            {
                e.Handled = true;
            }
            StringBuilder sb = new StringBuilder();
            switch (input.Length)
            {
                case 5:
                case 11:
                case 17:
                case 23:
                    enter_wcoa.Text = enter_wcoa.Text + "-";
                    break;
                case 29:
                    enter_wcoa.Text = enter_wcoa.Text.ToUpper();
                    //  search_coa_box.Text.Replace("--", "-");
                    break;
            }
            if (enter_wcoa.Text.Length != 0)
            {
                enter_wcoa.SelectionStart = enter_wcoa.Text.Length + 1;
            }
        }

        private void enter_ocoa_KeyDown(object sender, KeyEventArgs e)
        {
            string input = enter_ocoa.Text;
            if (e.Key == Key.OemMinus)
            {
                e.Handled = true;
            }
            StringBuilder sb = new StringBuilder();
            switch (input.Length)
            {
                case 5:
                case 11:
                case 17:
                case 23:
                    enter_ocoa.Text = enter_ocoa.Text + "-";
                    break;
                case 29:
                    enter_ocoa.Text = enter_ocoa.Text.ToUpper();
                    //  search_coa_box.Text.Replace("--", "-");
                    break;
            }
            if (enter_ocoa.Text.Length != 0)
            {
                enter_ocoa.SelectionStart = enter_ocoa.Text.Length + 1;
            }
        }

       
    }
    
}
