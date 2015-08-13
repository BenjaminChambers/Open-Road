using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Porter.Util.ViewModels
{
    public class MaintenanceView : Util.NotificationBase
    {
        public MaintenanceView(Models.Maintenance Item)
        {
            ID = Item.ID;
            ItemBackground = null;

            Date = Format.Date(Item.Date);
            Description = Item.Description;
            Miles = Format.Miles(Item.Odometer);
            Cost = Format.Currency(Item.Cost);

            switch(Item.Reminder)
            {
                case Models.Maintenance.ReminderType.Date:
                    Reminder = "Next on " + Format.Date(Item.NextDate);
                    break;
                case Models.Maintenance.ReminderType.Mileage:
                    Reminder = "Next at " + Format.Miles(Item.NextMileage) + " miles";
                    break;
                case Models.Maintenance.ReminderType.Both:
                    Reminder = "Next at " + Format.Miles(Item.NextMileage) + " miles or on " + Format.Date(Item.NextDate);
                    break;
            }
        }

        private Brush _itemBackground;
        public Brush ItemBackground { get { return _itemBackground; } set { SetField(ref _itemBackground, value); } }

        public int ID { get; private set; }

        public string Date { get; private set; }
        public string Description { get; private set; }
        public string Miles { get; private set; }
        public string Cost { get; private set; }
        public string Reminder { get; private set; }
    }
}
