using MahApps.Metro;
using MahApps.Metro.Controls;
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
using MySql.Data.MySqlClient;
using MahApps.Metro.Controls.Dialogs;

namespace modern_label
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private LabelViewModel labelViewModel;
        private RefrubHistoryObj RefrubHistoryObj;
        public Idbprovider mysql_data = new Mysql_DataProvider();
        public Idbprovider sqlite_data = new SQlite_DataProvider();
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
                    var discovery = mysql_data.discovery_data(search_box.Text);
                    discovery_result_title.Text = search_box.Text;
                    result_title.Text = search_box.Text;
                    discovery_search_cpu.Text = discovery.search_cpu.ToString();
                    discovery_search_hdd.Text = discovery.search_hdd.ToString();
                    discovery_search_ram.Text = discovery.search_ram.ToString();
                    discovery_search_manu.Text = discovery.search_manu;
                    discovery_search_model.Text = discovery.search_model;
                    discovery_search_serial.Text = discovery.search_serial;
                    discovery_search_optical_drive.Text = discovery.search_optical_drive;



                    var search = new search_result(search_box.Text);
                    search_cpu.Text = search.cpu;
                    search_hdd.Text = search.hdd.ToString();
                    search_ram.Text = search.ram.ToString();
                    search_manu.Text = search.manu;
                    search_model.Text = search.model;
                    search_serial.Text = search.serial;
                    search_optical_drive.Text = search.optical_drive;
                    search_sku.Text = search.sku;
                    var img_search = new imaging_search_result(search_box.Text);
                    imaging_title.Text = search_box.Text;
                    imaging_search_wcoa.Text = img_search.img_wcoa;
                    imaging_search_ocoa.Text = img_search.img_ocoa;
                    imaging_search_hdd.Text = img_search.img_hdd.ToString();
                    imaging_search_ram.Text = img_search.img_ram.ToString();
                    imaging_search_video.Text = img_search.img_video;
                    imaging_search_sku.Text = img_search.img_sku;
                }
            }
            catch
            {

            }
        }


        //click event on the edit button from flyout
        private async void search_edit_Click(object sender, RoutedEventArgs e)
        {

           
            string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";

            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            command.CommandText = "UPDATE rediscovery SET cpu = '" + search_cpu.Text + "',hdd='" + search_hdd.Text + "',ram='" + search_ram.Text + "',brand= '" + search_manu.Text + "',model='" + search_model.Text + "',serial = '" + search_serial.Text + "',optical_drive = '" + search_optical_drive.Text + "',pallet = '" + search_sku.Text + "' WHERE ictag = '" + result_title.Text + "'";
            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();

            await this.ShowMessageAsync("Message", "Rediscovery data Updated");

        }

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
            command.CommandText = "UPDATE production_log SET wcoa = '" + imaging_search_wcoa.Text + "',ocoa='" + imaging_search_ocoa.Text + "',RAM='" + imaging_search_ram.Text + "',HDD= '" + imaging_search_hdd.Text + "',channel='" + imaging_search_sku.Text + "',video_card = '" + imaging_search_video.Text + "' WHERE ictags = '" + result_title.Text + "'";
            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();

            await this.ShowMessageAsync("Message", "Imaging data Updated");

        }

        private async void discovery_search_edit_Click(object sender, RoutedEventArgs e)
        {
            string connStr = "Server=MYSQL5013.Smarterasp.net;Database=db_a094d4_icdb;Uid=a094d4_icdb;Pwd=icdb123!;Pooling=true";

            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            command.CommandText = "UPDATE discovery SET cpu = '" + discovery_search_cpu.Text + "',hdd='" + discovery_search_hdd.Text + "',ram='" + discovery_search_ram.Text + "',brand= '" + discovery_search_manu.Text + "',model='" + discovery_search_model.Text + "',serial = '" + discovery_search_serial.Text + "',optical_drive = '" + discovery_search_optical_drive.Text + "' WHERE ictag = '" + result_title.Text + "'";
            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();

            await this.ShowMessageAsync("Message", "Discovery data Updated");
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
    }


    }

