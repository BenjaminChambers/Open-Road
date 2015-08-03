using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Porter.Util.ViewModels
{
    public class FillupView : Util.NotificationBase
    {
        public FillupView(Util.Models.Fillup Single)
        {
            ItemBackground = null;
            ID = Single.ID;

            Miles = "-";

            Date = Util.Format.Date(Single.Date);
            Gallons = Util.Format.Gallons(Single.Volume);
            Cost = Util.Format.Currency(Single.Cost);
        }

        public FillupView(Util.Models.Fillup Item, Util.Models.Fillup Previous)
        {
            ItemBackground = null;
            ID = Item.ID;

            double mileage = Item.Odometer - Previous.Odometer;

            Date = Util.Format.Date(Item.Date);

            Gallons = Util.Format.Gallons(Item.Volume);
            Miles = Util.Format.Miles(mileage);
            Cost = Util.Format.Currency(Item.Cost);

            if (Util.Settings.PreferGPM)
                Efficiency = Util.Format.GPM(mileage, Item.Volume);
            else
                Efficiency = Util.Format.MPG(mileage, Item.Volume);
        }

        private Brush itemBackground;
        public Brush ItemBackground { get { return itemBackground; } set { SetField(ref itemBackground, value); } }

        public int ID { get; private set; }

        public string Date { get; private set; }

        public string Gallons { get; private set; }
        public string Miles { get; private set; }
        public string Cost { get; private set; }
        public string Efficiency { get; private set; }
    }
}
