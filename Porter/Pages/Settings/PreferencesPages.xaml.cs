using Microsoft.OneDrive.Sdk.WinStore;
using Porter.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Porter.Pages.Settings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PreferencesPage : Page
    {
        public PreferencesPage()
        {
            InitializeComponent();
            InitializeNavigation();

            SetTextLabels();
        }

        private void SetTextLabels()
        {
            if (Util.Settings.PreferGPM)
                PreferGPMBox.Tag = "Gallons per 100mi";
            else
                PreferGPMBox.Tag = "Miles per Gallon";

            if (Util.Settings.SaveToOneDrive)
            {
                AllowOneDrive.Tag = "Saving Data to OneDrive";
                RestoreFromOneDrive.Visibility = Visibility.Visible;
            }
            else
            {
                AllowOneDrive.Tag = "No online backup";
                RestoreFromOneDrive.Visibility = Visibility.Collapsed;
            }
        }

        private void OnToggleEfficiency(object sender, RoutedEventArgs e)
        {
            Util.Settings.PreferGPM = !Util.Settings.PreferGPM;
            SetTextLabels();
        }

        private void OnToggleOneDrive(object sender, RoutedEventArgs e)
        {
            Util.Settings.SaveToOneDrive = !Util.Settings.SaveToOneDrive;
            SetTextLabels();
        }

        private void OnRestoreOneDrive(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RestoreDataPage));
        }

        private void OnSaveOneDrive(object sender, RoutedEventArgs e)
        {
            Util.Database.UploadAsync();
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
