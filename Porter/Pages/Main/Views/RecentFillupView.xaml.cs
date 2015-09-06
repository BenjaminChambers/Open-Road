using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Porter.Util.Fillup;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Porter.Pages.Main.Views
{
    public sealed partial class RecentFillupView : UserControl
    {
        public RecentFillupView()
        {
            this.InitializeComponent();

            Title.Text = "Most Recent fill-up";

            using (var db = Util.Database.Connection())
            {
                var allFills = db.Table<Fillup>().OrderByDescending(item => item.Odometer);

                if (allFills.Count()==0)
                {
                    Title.Text = "No fill-ups to show";
                    Multi.Visibility = Visibility.Collapsed;
                    Single.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Fillup fill = allFills.First();
                    TotalGallons.Text = SingleGallons.Text = Util.Format.Gallons(fill.Volume);
                    TotalCost.Text = SingleCost.Text = Util.Format.Currency(fill.Cost);

                    if (allFills.Count() > 1)
                    {
                        Fillup second = allFills.ElementAt(1);

                        double _days = (fill.Date - second.Date).TotalDays;
                        double _miles = fill.Odometer - second.Odometer;

                        if (Util.Settings.PreferGPM)
                            Title.Text = Title.Text + " " + Util.Format.GPM(_miles, fill.Volume);
                        else
                            Title.Text = Title.Text + " " + Util.Format.MPG(_miles, fill.Volume);

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
