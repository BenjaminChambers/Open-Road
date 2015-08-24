using Porter.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Porter.Pages.MaintenanceList.AddMaintenance
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditReminderPage : Page
    {
        public static int MaintenanceID = -1;
        public static Util.ViewModels.ReminderForm Work = new Util.ViewModels.ReminderForm();

        public EditReminderPage()
        {
            this.InitializeComponent();
            InitializeNavigation();
        }

        /// <summary>
        // Navigation stuff
        private void InitializeNavigation()
        {
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public NavigationHelper NavigationHelper { get { return this.navigationHelper; } }
        public ObservableDictionary DefaultViewModel { get { return this.defaultViewModel; } }
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e) { }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e) { }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);

            if (MaintenanceID != -1)
            {
                using (var db = Util.Database.Connection())
                {
                    Util.Models.Maintenance item = db.Get<Util.Models.Maintenance>(MaintenanceID);
                    Work.Update(item);
                    db.Update(item);
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);

            if (MaintenanceID != -1)
            {
                using (var db = Util.Database.Connection())
                {
                    Work = new Util.ViewModels.ReminderForm(db.Get<Util.Models.Maintenance>(MaintenanceID));
                }
            }
            ReminderForm.DataContext = Work;
        }
    }
}
