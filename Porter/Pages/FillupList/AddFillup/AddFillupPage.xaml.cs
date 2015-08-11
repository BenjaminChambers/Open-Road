using Porter.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Porter.Pages.FillupList.AddFillup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddFillupPage : Page
    {
        Util.ViewModels.FillupForm FormData = new Util.ViewModels.FillupForm();

        public AddFillupPage()
        {
            InitializeComponent();
            InitializeNavigation();

            FillupForm.DataContext = FormData;
        }

        private void OnClickSave(object sender, RoutedEventArgs e)
        {
            Util.Models.Fillup fill = new Util.Models.Fillup();
            FormData.Update(fill);
            using (var db = Util.Database.Connection())
            {
                db.Insert(fill);
            }

            Frame.GoBack();
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
