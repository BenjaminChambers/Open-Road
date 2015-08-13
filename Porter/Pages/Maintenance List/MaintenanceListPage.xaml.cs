using Porter.Common;
using Porter.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Porter.Pages.Maintenance_List
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MaintenanceListPage : Page
    {
        ObservableCollection<MaintenanceView> Fillups = new ObservableCollection<MaintenanceView>();
        private MaintenanceView FillupSelection = null;

        public MaintenanceListPage()
        {
            this.InitializeComponent();

            InitializeNavigation();
        }

        public void RefreshDisplay()
        {
            /*
            FillupSelection = null;

            Fillups.Clear();

            using (var db = Util.Database.Connection())
            {
                var data = db.Table<Util.Models.Fillup>().OrderByDescending(item => item.Odometer).ToList();

                if (data.Count > 0)
                {
                    for (int i = 0; i < data.Count - 1; i++)
                        Fillups.Add(new FillupView(data[i], data[i + 1]));

                    Fillups.Add(new FillupView(data[data.Count - 1]));
                }
            }
            */
        }

        private void OnClickAdd(object sender, RoutedEventArgs e)
        {

        }

        private void OnClickEdit(object sender, RoutedEventArgs e)
        {

        }

        private void OnClickDelete(object sender, RoutedEventArgs e)
        {

        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {

        }



        // Navigation stuff
        private void InitializeNavigation()
        {
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public NavigationHelper NavigationHelper { get { return this.navigationHelper; } }
        public ObservableDictionary DefaultViewModel { get { return this.defaultViewModel; } }
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e) { }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e) { }
        protected override void OnNavigatedFrom(NavigationEventArgs e) { this.navigationHelper.OnNavigatedFrom(e); }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            RefreshDisplay();
            this.navigationHelper.OnNavigatedTo(e);
        }

    }
}
