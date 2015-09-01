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

                if (allFills.Count()==0)
                {
                    Title.Text = "No fill-ups to show";
                    Multi.Visibility = Visibility.Collapsed;
                    Single.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Util.Models.Fillup fill = allFills.First();
                    TotalGallons.Text = SingleGallons.Text = Util.Format.Gallons(fill.Volume);
                    TotalCost.Text = SingleCost.Text = Util.Format.Currency(fill.Cost);

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

                        Single.Visibility = Visibility.Collapsed;
                    }
                    else
                        Multi.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
