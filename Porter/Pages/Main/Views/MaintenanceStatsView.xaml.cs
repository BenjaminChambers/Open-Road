using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Porter.Pages.Main.Views
{
    public sealed partial class MaintenanceStatsView : UserControl
    {
        // -1 for all, 0 for latest
        public MaintenanceStatsView(int _days, string _title)
        {
            this.InitializeComponent();

            DateTime cutoff = DateTime.Today - new TimeSpan(_days, 0, 0, 0);

            using (var db = Util.Database.Connection())
            {
                var set = (_days > 0 ?
                    db.Table<Util.Models.Maintenance>().Where(item => item.Date > cutoff)
                    : db.Table<Util.Models.Maintenance>()
                    );

                if (set.Count() == 0)
                    Message.Text = "No maintenance to show";
                else
                {
                    Title.Text = _title;

                    Message.Text = set.Count().ToString() + " Jobs done for " + Util.Format.Currency(set.Sum(item => item.Cost));
                }
            }
        }

    }
}
