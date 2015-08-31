using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Porter.Pages.Main.Views
{
    public sealed partial class FillupStatsView : UserControl
    {
        private int _numDays;
        private string _message;

        // -1 for all, 0 for latest
        public FillupStatsView(int NumDays, string Message)
        {
            this.InitializeComponent();

            Name = Message;

            _numDays = NumDays;
            Title.Text = _message = Message;

            int count = 0;
            using (var db = Util.Database.Connection())
                count = db.Table<Util.Models.Fillup>().Count();

            switch (count)
            {
                case 0: Title.Text = "No fill-ups to show"; break;
                case 1: UpdateLatest(); break;
                default:
                    switch (_numDays)
                    {
                        case -1: UpdateAll(); break;
                        case 0: UpdateLatest(); break;
                        default: UpdateSome(); break;
                    }
                    break;
            }
        }

        private void UpdateLatest()
        {
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
            }
        }

        private void UpdateSome()
        {
            using (var db = Util.Database.Connection())
            {
                var allFills = db.Table<Util.Models.Fillup>().OrderByDescending(item => item.Odometer);

                DateTime cutoff = DateTime.Now - new TimeSpan(_numDays, 0, 0, 0);

                var selected = allFills.Where(item => item.Date >= cutoff);

                if (selected.Count() == allFills.Count())
                    UpdateFromList(allFills.ToList());
                else
                    UpdateFromList(allFills.Take(selected.Count() + 1).ToList());
            }
        }

        private void UpdateAll()
        {
            using (var db = Util.Database.Connection())
                UpdateFromList(db.Table<Util.Models.Fillup>().OrderByDescending(item => item.Odometer).ToList());
        }

        private void UpdateFromList(List<Util.Models.Fillup> src)
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
