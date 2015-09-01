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
            }
        }

        void AddReminder(Util.Models.Reminder rem)
        {
            TextBlock tb = new TextBlock();
            tb.Text = rem.ReminderDescription;

            ReminderPanel.Children.Add(tb);
        }
    }
}
