using Windows.UI.Xaml.Media;

namespace Porter.Util.ViewModels
{
    public class FillupView : NotificationBase
    {
        public FillupView(Models.Fillup Single)
        {
            ItemBackground = null;
            ID = Single.ID;

            Miles = "-";

            Date = Format.Date(Single.Date);
            Gallons = Format.Gallons(Single.Volume);
            Cost = Format.Currency(Single.Cost);
        }

        public FillupView(Models.Fillup Item, Models.Fillup Previous)
        {
            ItemBackground = null;
            ID = Item.ID;

            double mileage = Item.Odometer - Previous.Odometer;

            Date = Format.Date(Item.Date);

            Gallons = Format.Gallons(Item.Volume);
            Miles = Format.Miles(mileage);
            Cost = Format.Currency(Item.Cost);

            if (Settings.PreferGPM)
                Efficiency = Format.GPM(mileage, Item.Volume);
            else
                Efficiency = Format.MPG(mileage, Item.Volume);
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
