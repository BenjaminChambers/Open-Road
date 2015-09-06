using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Porter.Util.Maintenance;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Porter.Pages.Main.Views
{
    public sealed partial class RecentMaintenanceView : UserControl
    {
        public RecentMaintenanceView()
        {
            this.InitializeComponent();

            Maintenance work = null;

            using (var db = Util.Database.Connection())
            {
                var table = db.Table<Maintenance>();
                if (table.Count() > 0)
                    work = table.OrderByDescending(item => item.Odometer).First();
            }

            if (work == null)
            {
                MaintenanceName.Text = "No maintenance to show";
            }
            else
            {
                MaintenanceName.Text = work.Description;
                Cost.Text = Util.Format.Currency(work.Cost);
                Mileage.Text = Util.Format.Miles(work.Odometer);
            }
        }

    }
}
