using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Porter.Pages.Main.Views
{
    public sealed partial class RemindersView : UserControl
    {
        public RemindersView()
        {
            this.InitializeComponent();

            using (var db = Util.Database.Connection())
            {
                var workSet = db.Table<Util.Models.Maintenance>().OrderByDescending(item => item.Odometer);

                List<Util.Models.Maintenance> reminderSet = new List<Util.Models.Maintenance>();

                foreach (Util.Models.Maintenance work in workSet)
                {
                    if (work.Reminder != Util.Models.Maintenance.ReminderType.None)
                    {
                        switch (work.Reminder)
                        {
                            case Util.Models.Maintenance.ReminderType.Date:
                                if (work.NextDate > DateTime.Today)
                                    AddReminder(work);
                                break;
                            case Util.Models.Maintenance.ReminderType.Mileage:
//                                if (work.NextMileage > currentMileage)
//                                    AddReminder(work);
                                break;
                            case Util.Models.Maintenance.ReminderType.Both:
                                break;
                        }
                    }
                }
            }
        }

        void AddReminder(Util.Models.Maintenance work)
        {
            TextBlock tb = new TextBlock();
            tb.Text = work.ReminderDescription;

            ReminderPanel.Children.Add(tb);
        }
    }
}
