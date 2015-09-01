using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Porter.Pages.Main.Views
{
    public sealed partial class CarStatsView : UserControl
    {
        public CarStatsView()
        {
            this.InitializeComponent();

            using (var db = Util.Database.Connection())
            {
                var fills = db.Table<Util.Models.Fillup>();
                var maint = db.Table<Util.Models.Maintenance>();

                int maxOdom = 0;

                if (fills.Count() > 0)
                    maxOdom = fills.Max(item => item.Odometer);

                if (maint.Count() > 0)
                {
                    int maxMaint = maint.Max(item => item.Odometer);
                    if (maxMaint > maxOdom)
                        maxOdom = maxMaint;
                }

                Odometer.Text = Util.Format.Miles(maxOdom);
            }
        }
    }
}
