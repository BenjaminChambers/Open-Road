using Porter.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Porter.Pages.MaintenanceList.EditMaintenance
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditMaintenancePage : Page
    {
        public static int MaintenanceID = -1;
        Util.ViewModels.MaintenanceForm FormData;
        MapIcon PushPin = new MapIcon();

        public EditMaintenancePage()
        {
            this.InitializeComponent();
            InitializeNavigation();
        }

        private async void Initialize()
        {
            if (MaintenanceID == -1)
                FormData = new Util.ViewModels.MaintenanceForm();
            else
                FormData = new Util.ViewModels.MaintenanceForm(MaintenanceID);

            MaintenanceForm.DataContext = FormData;
            ReminderBox.SelectedIndex = (int)FormData.ReminderType;

            Geoposition pos = await new Geolocator().GetGeopositionAsync();
            MapControl.Center = pos.Coordinate.Point;
            MapControl.ZoomLevel = 15;
            MapControl.Style = MapStyle.Road;

            FormData.Location = PushPin.Location = pos.Coordinate.Point;
            MapControl.MapElements.Add(PushPin);
        }

        private void OnMapMoved(Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {
            FormData.Location = PushPin.Location = MapControl.Center;
        }

        private async void OnCenterMap(object sender, TappedRoutedEventArgs e)
        {
            FormData.Location = PushPin.Location = MapControl.Center = (await new Geolocator().GetGeopositionAsync()).Coordinate.Point;
        }

        private void OnClickRevert(object sender, RoutedEventArgs e)
        {
            Initialize();
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


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            using (var db = Util.Database.Connection())
            {
                FormData.Update(MaintenanceID);
            }
            this.navigationHelper.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Initialize();
            navigationHelper.OnNavigatedTo(e);
        }
    }
}
