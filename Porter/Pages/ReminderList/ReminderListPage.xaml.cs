using Porter.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Porter.Util.Reminder;

namespace Porter.Pages.ReminderList
{
    public sealed partial class ReminderListPage : Page
    {
        ObservableCollection<ReminderView> Reminders = new ObservableCollection<ReminderView>();

        public ReminderListPage()
        {
            InitializeNavigation();
            this.InitializeComponent();
        }


        private void OnItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void OnClickAdd(object sender, RoutedEventArgs e)
        {

        }

        private void OnClickEdit(object sender, RoutedEventArgs e)
        {

        }

        private void OnClickDelete(object sender, RoutedEventArgs e)
        {

        }

        // Navigation
        protected override void OnNavigatedTo(NavigationEventArgs e) { this.navigationHelper.OnNavigatedTo(e); }
        protected override void OnNavigatedFrom(NavigationEventArgs e) { this.navigationHelper.OnNavigatedFrom(e); }

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
    }
}
