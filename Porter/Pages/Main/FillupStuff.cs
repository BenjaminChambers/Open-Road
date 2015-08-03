using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Porter.Pages.Main
{
    public sealed partial class MainPage
    {
        private string sLatest = "Latest Fill-up";
        private string sMonthly = "Monthly Gas Usage";
        private string sAnnual = "Annual Gas Usage";
        private string sTotal = "Total Gas Usage";

        private void OnViewFillups(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Pages.FillupList.FillupListPage));
        }

        private void OnAddFillup(object sender, RoutedEventArgs e)
        {
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

        private void SetupFillupStats()
        {
            fillupStats.Clear();
            FillupStatListView.Items.Clear();

            fillupStats.Add(new Util.ViewModels.FillupStatsView(0, sLatest));
            fillupStats.Add(new Util.ViewModels.FillupStatsView(31, sMonthly));
            fillupStats.Add(new Util.ViewModels.FillupStatsView(366, sAnnual));
            fillupStats.Add(new Util.ViewModels.FillupStatsView(-1, sTotal));

            foreach (Util.ViewModels.FillupStatsView view in fillupStats)
                FillupStatListView.Items.Add(view);
        }

        private void FillupToggle(object sender, RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var menu = (MenuFlyoutItem)sender;
            string str;
            switch (menu.Name)
            {
                case "MenuLatestFillupToggle": str = sLatest; break;
                case "MenuMonthlyFillupToggle": str = sMonthly; break;
                case "MenuAnnualFillupToggle": str = sAnnual; break;
                case "MenuAllFillupToggle": str = sTotal; break;
                default:
                    str = "";
                    break;
            }

            var view = fillupStats.Single(item => item.Name == str);

            switch (view.Visibility)
            {
                case Visibility.Visible:
                    view.Visibility = Visibility.Collapsed;
                    menu.Text = "Show " + str;
                    localSettings.Values["Toggle " + str] = false;
                    break;
                case Visibility.Collapsed:
                    view.Visibility = Visibility.Visible;
                    menu.Text = "Hide " + str;
                    localSettings.Values["Toggle " + str] = true;
                    break;
            }

            RefreshDisplay();
        }
    }
}
