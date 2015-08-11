using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Porter.Util.ViewModels
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
            _message = Message;

            var saved = (bool?)Windows.Storage.ApplicationData.Current.LocalSettings.Values[Message];
            switch (saved)
            {
                case null:
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values[Message] = true;
                    break;
                case false:
                    Visibility = Visibility.Collapsed;
                    break;
                default:
                    Visibility = Visibility.Visible;
                    break;
            }

            Update();
        }

        public void Update()
        {
            using (var db = Util.Database.Connection())
            {
                var fillups = db.Table<Models.Fillup>();

                switch (fillups.Count())
                {
                    case 0:
                        Title.Text = "No fill-ups to show";
                        break;
                    case 1:
                        Title.Text = _message;
                        UpdateLatest();
                        break;
                    default:
                        Title.Text = _message;
                        switch (_numDays)
                        {
                            case -1: UpdateAll(); break;
                            case 0: UpdateLatest(); break;
                            default: UpdateSome(); break;
                        }
                        break;
                }
            }
        }

        private void UpdateLatest()
        {
            using (var db = Database.Connection())
            {
                var allFills = db.Table<Models.Fillup>().OrderByDescending(item => item.Odometer);

                Models.Fillup fill = allFills.First();
                TotalGallons.Text = Format.Gallons(fill.Volume);
                TotalCost.Text = Format.Currency(fill.Cost);

                if (allFills.Count() > 1)
                {
                    Models.Fillup second = allFills.ElementAt(1);

                    double _days = (fill.Date - second.Date).TotalDays;
                    double _miles = fill.Odometer - second.Odometer;

                    if (Util.Settings.PreferGPM)
                        Title.Text = _message + " " + Format.GPM(_miles, fill.Volume);
                    else
                        Title.Text = _message + " " + Format.MPG(_miles, fill.Volume);

                    GallonsPerDay.Text = Format.Gallons(fill.Volume / _days);
                    MilesPerDay.Text = Format.Miles(_miles / _days);
                    CostPerDay.Text = Format.Currency(fill.Cost / _days);
                    TotalMiles.Text = Format.Miles(_miles);
                }
            }
        }

        private void UpdateSome()
        {
            using (var db = Database.Connection())
            {
                var allFills = db.Table<Models.Fillup>().OrderByDescending(item => item.Odometer);

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
            using (var db = Database.Connection())
                UpdateFromList(db.Table<Models.Fillup>().OrderByDescending(item => item.Odometer).ToList());
        }

        private void UpdateFromList(List<Models.Fillup> src)
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

            TotalMiles.Text = Format.Miles(_miles);
            TotalGallons.Text = Format.Gallons(_gallons);
            TotalCost.Text = Format.Currency(_cost);

            MilesPerDay.Text = Format.Miles(_miles / _days);
            GallonsPerDay.Text = Format.Gallons(_gallons / _days);
            CostPerDay.Text = Format.Currency(_cost / _days);

            if (Util.Settings.PreferGPM)
            {
                Title.Text = _message + " " + Format.GPM(_miles, _gallons);
            }
            else
            {
                Title.Text = _message + " " + Format.MPG(_miles, _gallons);
            }
        }
    }
}
