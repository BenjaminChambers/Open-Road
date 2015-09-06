using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Porter.Util.Fillup;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Porter.Pages.Main.Views
{
    public sealed partial class FillupStatsView : UserControl
    {
        private int _numDays;
        private string _message;

        // -1 for all
        public FillupStatsView(int NumDays, string Message)
        {
            this.InitializeComponent();

            _numDays = NumDays;
            Title.Text = _message = Message;

            using (var db = Util.Database.Connection())
            {
                var set = db.Table<Fillup>().OrderByDescending(item => item.Odometer);

                if (set.Count() == 0)
                {
                    Title.Text = "No fill-ups to show";
                    Multi.Visibility = Visibility.Collapsed;
                    Single.Visibility = Visibility.Collapsed;
                }
                else
                {
                    DateTime cutoff = DateTime.Today - new TimeSpan(NumDays, 0, 0, 0);

                    int count = (NumDays == -1) ? set.Count() : set.Where(item => item.Date > cutoff).Count();

                    var work = set.Take(count).ToList();

                    if (work.Count > 1)
                    {
                        Single.Visibility = Visibility.Collapsed;
                        UpdateFromList(work);
                    }
                    else
                    {
                        Multi.Visibility = Visibility.Collapsed;
                        SingleGallons.Text = Util.Format.Gallons(work[0].Volume);
                        SingleCost.Text = Util.Format.Currency(work[0].Cost);
                    }
                }
            }
        }

        private void UpdateFromList(List<Fillup> src)
        {
            double _days = (src[0].Date - src[src.Count - 1].Date).TotalDays;
            double _miles = src[0].Odometer - src[src.Count - 1].Odometer;

            double _gallons = 0;
            double _cost = 0;

            for (int idx = 0; idx < src.Count - 1; idx++)
            {
                _gallons += src[idx].Volume;
                _cost += src[idx].Cost;
            }

            TotalMiles.Text = Util.Format.Miles(_miles);
            TotalGallons.Text = Util.Format.Gallons(_gallons);
            TotalCost.Text = Util.Format.Currency(_cost);

            MilesPerDay.Text = Util.Format.Miles(_miles / _days);
            GallonsPerDay.Text = Util.Format.Gallons(_gallons / _days);
            CostPerDay.Text = Util.Format.Currency(_cost / _days);

            if (Util.Settings.PreferGPM)
            {
                Title.Text = _message + " " + Util.Format.GPM(_miles, _gallons);
            }
            else
            {
                Title.Text = _message + " " + Util.Format.MPG(_miles, _gallons);
            }
        }
    }
}
