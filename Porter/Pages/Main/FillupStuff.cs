using Windows.UI.Xaml;
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

            if (!Util.Settings.HideFillupRecent) fillupStats.Add(new Util.ViewModels.FillupStatsView(0, sLatest));
            if (!Util.Settings.HideFillupMonthly) fillupStats.Add(new Util.ViewModels.FillupStatsView(31, sMonthly));
            if (!Util.Settings.HideFillupAnnual) fillupStats.Add(new Util.ViewModels.FillupStatsView(366, sAnnual));
            if (!Util.Settings.HideFillupTotal) fillupStats.Add(new Util.ViewModels.FillupStatsView(-1, sTotal));

            foreach (Util.ViewModels.FillupStatsView view in fillupStats)
                FillupStatListView.Items.Add(view);
        }
    }
}
