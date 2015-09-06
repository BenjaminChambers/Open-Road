using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Porter.Util.Maintenance
{
    public class MaintenanceView : NotificationBase
    {
        public MaintenanceView(int MaintenanceID)
        {
            using (var db = Database.Connection())
            {
                Maintenance work = db.Get<Maintenance>(MaintenanceID);
                ID = work.ID;
                ItemBackground = null;
                Date = Format.Date(work.Date);
                Description = work.Description;
                Miles = Format.Miles(work.Odometer);
                Cost = Format.Currency(work.Cost);
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
