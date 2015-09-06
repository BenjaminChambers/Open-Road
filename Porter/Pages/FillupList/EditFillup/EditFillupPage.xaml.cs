using Porter.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Devices.Geolocation;
using System;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Porter.Pages.FillupList.EditFillup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditFillupPage : Page
    {
        public static int FillupID = -1;
        Util.ViewModels.FillupForm FormData = null;
        MapIcon PushPin = new MapIcon();

        public EditFillupPage()
        {
            InitializeComponent();
            InitializeNavigation();
        }

        private void InitializeData()
        {
            FormData = new Util.ViewModels.FillupForm();
            FormData.From(FillupID);
            FillupForm.DataContext = FormData;

            MapControl.Center = PushPin.Location = FormData.Location;
            MapControl.ZoomLevel = 15;
            MapControl.MapElements.Add(PushPin);
        }

        private void OnMapMoved(MapControl sender, object args)
        {
            FormData.Location = PushPin.Location = MapControl.Center;
        }

        private void OnRevert(object sender, RoutedEventArgs e)
        {
            InitializeData();
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



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            InitializeData();
            this.navigationHelper.OnNavigatedTo(e);
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (FormData != null)
            {
                FormData.Update(FillupID);
            }

            this.navigationHelper.OnNavigatedFrom(e);
        }
    }
}
