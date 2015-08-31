using Porter.Common;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Porter.Pages.Main
{
    public sealed partial class MainPage : Page
    {
        Util.ViewModels.FillupForm FormData = new Util.ViewModels.FillupForm();
        ObservableCollection<Views.FillupStatsView> fillupStats = new ObservableCollection<Views.FillupStatsView>();

        public MainPage()
        {
            this.InitializeComponent();

            SetupForms();
            SetupAnimations();

            InitializeNavigation();
        }

        private void SetupForms()
        {
            FormData = new Util.ViewModels.FillupForm();
            QuickFillupForm.DataContext = FormData;
        }

        private void SetupAnimations()
        {
            Timeline fade = (Timeline)Resources["NotificationFade"];
            Storyboard.SetTarget(fade, NotificationPanel);
            Timeline slide = (Timeline)Resources["NotificationSlide"];
            Storyboard.SetTarget(slide, NotificationPanelTransform);
        }

        private void RefreshDisplay()
        {
            fillupStats.Clear();
            DetailListView.Items.Clear();

            if (!Util.Settings.HideFillupRecent) fillupStats.Add(new Views.FillupStatsView(0, "Latest Fill-up"));
            if (!Util.Settings.HideFillupMonthly) fillupStats.Add(new Views.FillupStatsView(31, "Monthly Gas Usage"));
            if (!Util.Settings.HideFillupAnnual) fillupStats.Add(new Views.FillupStatsView(366, "Annual Gas Usage"));
            if (!Util.Settings.HideFillupTotal) fillupStats.Add(new Views.FillupStatsView(-1, "Total Gas Usage"));

            foreach (Views.FillupStatsView view in fillupStats)
                DetailListView.Items.Add(view);

            Util.LiveTile.Render();
        }

        private void OnTapHamburger(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void OnClickCustomize(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Customization.CustomizationPage));
        }

        private void OnClickPreferences(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings.PreferencesPage));
        }

        private void OnViewMaintenance(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MaintenanceList.MaintenanceListPage));
        }

        private void OnViewFillups(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FillupList.FillupListPage));
        }

        private void OnAddFillup(object sender, RoutedEventArgs e)
        {
            Util.Metrics.TrackFillup();

            Storyboard fade = (Storyboard)Resources["NotificationFade"];
            Storyboard slide = (Storyboard)Resources["NotificationSlide"];
            fade.Begin();
            slide.Begin();

            using (var db = Util.Database.Connection())
            {
                db.Insert(FormData.ToFillup());
            }

            SetupForms();
            RefreshDisplay();
        }




        // Navigation
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            RefreshDisplay();
            this.navigationHelper.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e) { this.navigationHelper.OnNavigatedFrom(e); }


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
    }
}
