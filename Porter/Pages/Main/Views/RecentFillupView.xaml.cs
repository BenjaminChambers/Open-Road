using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Porter.Pages.Main.Views
{
    public sealed partial class RecentFillupView : UserControl
    {
        private int _numDays;
        private string _message;

        // -1 for all, 0 for latest
        public RecentFillupView()
        {
            this.InitializeComponent();

            Title.Text = "Most Recent fill-up";

            using (var db = Util.Database.Connection())
            {
                var allFills = db.Table<Util.Models.Fillup>().OrderByDescending(item => item.Odometer);

                Util.Models.Fillup fill = allFills.First();
                TotalGallons.Text = Util.Format.Gallons(fill.Volume);
                TotalCost.Text = Util.Format.Currency(fill.Cost);

                if (allFills.Count() > 1)
                {
                    Util.Models.Fillup second = allFills.ElementAt(1);

                    double _days = (fill.Date - second.Date).TotalDays;
                    double _miles = fill.Odometer - second.Odometer;

                    if (Util.Settings.PreferGPM)
                        Title.Text = _message + " " + Util.Format.GPM(_miles, fill.Volume);
                    else
                        Title.Text = _message + " " + Util.Format.MPG(_miles, fill.Volume);

                    GallonsPerDay.Text = Util.Format.Gallons(fill.Volume / _days);
                    MilesPerDay.Text = Util.Format.Miles(_miles / _days);
                    CostPerDay.Text = Util.Format.Currency(fill.Cost / _days);
                    TotalMiles.Text = Util.Format.Miles(_miles);
                }
                else
                {
                    GallonsPerDay.Visibility = Visibility.Collapsed;
                    CostPerDay.Visibility = Visibility.Collapsed;
                    MilesPerDay.Visibility = Visibility.Collapsed;
                    DailyBlock.Visibility = Visibility.Collapsed;
                    TotalBlock.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
