using Porter.Common;
using Porter.Util.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Porter.Pages.FillupList
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FillupListPage : Page
    {
        ObservableCollection<FillupView> Fillups = new ObservableCollection<FillupView>();
        private FillupView FillupSelection = null;

        public FillupListPage()
        {
            InitializeComponent();
            InitializeNavigation();

            FillupList.DataContext = Fillups;
            RefreshDisplay();
        }

        public void RefreshDisplay()
        {
            FillupSelection = null;

            Fillups.Clear();

            using (var db = Util.Database.Connection())
            {
                var data = db.Table<Util.Models.Fillup>().OrderByDescending(item => item.Odometer).ToList();

                if (data.Count > 0)
                {
                    for (int i = 0; i < data.Count-1; i++)
                        Fillups.Add(new FillupView(data[i], data[i + 1]));

                    Fillups.Add(new FillupView(data[data.Count-1]));
                }
            }
        }

        private void OnClickDelete(object sender, RoutedEventArgs e)
        {
            if (FillupSelection != null)
            {
                using (var db = Util.Database.Connection())
                {
                    var toDelete = db.Get<Util.Models.Fillup>(FillupSelection.ID);

                    db.Delete(toDelete);
                }

                RefreshDisplay();
            }
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (FillupSelection != null)
            {
                FillupSelection.ItemBackground = new SolidColorBrush(Windows.UI.Colors.Transparent);
            }

            FillupSelection = (FillupView)e.ClickedItem;
            FillupSelection.ItemBackground = (Brush)App.Current.Resources["ButtonBackground"];
        }

        private void OnClickEdit(object sender, RoutedEventArgs e)
        {
            if (FillupSelection != null)
            {
                EditFillup.EditFillupPage.FillupID = FillupSelection.ID;
                Frame.Navigate(typeof(EditFillup.EditFillupPage));
            }
        }

        private void OnClickAdd(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddFillup.AddFillupPage));
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
