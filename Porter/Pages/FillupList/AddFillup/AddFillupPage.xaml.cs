using Porter.Common;
using System;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Maps;
using Porter.Util.Fillup;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Porter.Pages.FillupList.AddFillup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddFillupPage : Page
    {
        FillupForm FormData = new FillupForm();
        MapIcon PushPin = new MapIcon();

        public AddFillupPage()
        {
            InitializeComponent();
            InitializeNavigation();
            InitializeLocation();

            FillupForm.DataContext = FormData;
        }

        private async void InitializeLocation()
        {
            Geoposition pos = await new Geolocator().GetGeopositionAsync();
            MapControl.Center = pos.Coordinate.Point;
            MapControl.ZoomLevel = 15;
            MapControl.Style = MapStyle.Road;

            FormData.Location = PushPin.Location = pos.Coordinate.Point;
            MapControl.MapElements.Add(PushPin);
        }

        private void OnMapMoved(MapControl sender, object args)
        {
            FormData.Location = PushPin.Location = MapControl.Center;
        }

        private void OnClickSave(object sender, RoutedEventArgs e)
        {
            Util.Metrics.TrackFillup();

            FormData.Insert();

            Frame.GoBack();
        }

        private async void OnCenterMap(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            FormData.Location = PushPin.Location = MapControl.Center = (await new Geolocator().GetGeopositionAsync()).Coordinate.Point;
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
        protected override void OnNavigatedTo(NavigationEventArgs e) { this.navigationHelper.OnNavigatedTo(e); }
        protected override void OnNavigatedFrom(NavigationEventArgs e) { this.navigationHelper.OnNavigatedFrom(e); }

    }
}
