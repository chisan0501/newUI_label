using MahApps.Metro;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using MahApps.Metro.Controls.Dialogs;
using System.Diagnostics;

namespace modern_label
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {


        private LabelViewModel labelViewModel;
        private RefrubHistoryObj RefrubHistoryObj;
      //  Mysql_DataProvider  mysql_data = new Mysql_DataProvider();
        //public Idbprovider sqlite_data = new SQlite_DataProvider();
        public MainWindow()
        {
           

            Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current,
                                    ThemeManager.GetAccent("Teal"),
                                    ThemeManager.GetAppTheme("BaseDark"));
            InitializeComponent();
            labelViewModel = new LabelViewModel(DialogCoordinator.Instance);
            RefrubHistoryObj = new RefrubHistoryObj();
            DataContext = labelViewModel;
           


        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {

        }



        private void Tile_Loaded(object sender, RoutedEventArgs e)
        {

            time.Content = DateTime.Now.Date;
        }
         

        //tab button control
        private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                int newIndex = main_tab.SelectedIndex + 1;
                if (newIndex >= main_tab.Items.Count)
                    newIndex = 0;
                main_tab.SelectedIndex = newIndex;
            }

            if (e.Key == Key.Left)
            {
                int newIndex = main_tab.SelectedIndex - 1;
                if (newIndex < 0)
                    newIndex = main_tab.Items.Count - 1;
                main_tab.SelectedIndex = newIndex;
            }

        }

        private void search_box_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
               if (e.Key == Key.Enter)
                {
                    
                   discovery_result.IsOpen = true;
                    search_result.IsOpen = true;
                    search_box.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
            catch
            {

            }
        }


        //click event on the edit button from flyout
        

        private void setting_btn_Click(object sender, RoutedEventArgs e)
        {
            setting_flyout.IsOpen = true;
        }

        private void Tile_Click_1(object sender, RoutedEventArgs e)
        {
            //var ss = new ss();
            //ss.get_customer();
        }



        private async void imaging_edit_Click(object sender, RoutedEventArgs e)
        {
            string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";

            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            command.CommandText = "UPDATE production_log SET RAM='" + imaging_search_ram.Text + "',HDD= '" + imaging_search_hdd.Text + "',channel='" + imaging_search_sku.Text + "',video_card = '" + imaging_search_video.Text + "' WHERE ictags = '" + imaging_title.Text + "'";
            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();

            await this.ShowMessageAsync("Message", "Imaging data Updated");

        }

     
        private void search_box_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            search_box.Text = "";
        }

        private void db_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void Pre_COA_GotFocus(object sender, RoutedEventArgs e)
        {
            Pre_COA.SelectAll();
        }

        private void Asset_tag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var request = new TraversalRequest(FocusNavigationDirection.Next);
                request.Wrapped = true;
                Asset_tag.MoveFocus(request);
            }
            }

        private void db_select_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void grade_dropdown_Loaded(object sender, RoutedEventArgs e)
        {
            grade_dropdown.SelectedIndex = 0;
        }

        private void computerType_drop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        private void channel_dropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (channel_dropdown.SelectedItem != null)
            {
                switch (channel_dropdown.SelectedItem.ToString())
                {
                    case "City Of Seattle (Desktop)":
                    case "City Of Seattle (Laptop)":
                    case "NGO":
                    case "OEM (Laptop)":
                    case "OEM (Desktop)":
                    case "Mar (Desktop)":
                    case "Mar (Laptop)":
                    case "Online Order":
                    case "My Channel is not Listed":
                        sku_dropdown.SelectedIndex = 0;
                        break;
                }
            }
               
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Asset_tag.Text = string.Empty;
            Pre_COA.Text = "00999-999-000-999";


            //foreach (var c in win_sp.Children)
            //{
            //    if (c.GetType() == typeof(TextBox))
            //    {
            //        ((TextBox)(c)).Text = string.Empty;
            //    }

            //}
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://ic.icdb.name/Dymo/apple");
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var request = new TraversalRequest(FocusNavigationDirection.Next);
                request.Wrapped = true;
                search_rma_box.MoveFocus(request);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Rma_comment_box.Text = "";
            rmaNum_search_rma_box.Text = "";
            search_rma_box.Text = "";

        }
        private MetroWindow accentThemeTestWindow;

        private void ChangeAppStyleButtonClick(object sender, RoutedEventArgs e)
        {
            if (accentThemeTestWindow != null)
            {
                accentThemeTestWindow.Activate();
                return;
            }

            accentThemeTestWindow = new AccentStyleWindow();
            accentThemeTestWindow.Owner = this;
            accentThemeTestWindow.Closed += (o, args) => accentThemeTestWindow = null;
            accentThemeTestWindow.Left = this.Left + this.ActualWidth / 2.0;
            accentThemeTestWindow.Top = this.Top + this.ActualHeight / 2.0;
            accentThemeTestWindow.Show();
        }

        private void rma_search_rma_box_KeyDown(object sender, KeyEventArgs g)
        {
            if (g.Key == Key.Enter)
            {
                var request = new TraversalRequest(FocusNavigationDirection.Next);
                request.Wrapped = true;
                rmaNum_search_rma_box.MoveFocus(request);
            }
        }
        private void serial_search_rma_box_KeyDown(object sender, KeyEventArgs f)
        {
            if (f.Key == Key.Enter)
            {
                var request = new TraversalRequest(FocusNavigationDirection.Next);
                request.Wrapped = true;
                serial_search_rma_box.MoveFocus(request);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            Asset_tag.Focus();
            Asset_tag.Clear();
        }

        private void search_box_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

        }
    }
    


    }

