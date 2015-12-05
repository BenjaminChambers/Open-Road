using Microsoft.OneDrive.Sdk.WinStore;
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

namespace Porter.Pages.Settings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RestoreDataPage : Page
    {
        public RestoreDataPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        private async void GetBackupList()
        {
            RingOfProgress.IsActive = true;
            RingOfProgress.Visibility = Visibility.Visible;

            var OneDriveClient = OneDriveClientExtensions.GetUniversalClient(Util.Database.OneDriveScopes);
            await OneDriveClient.AuthenticateAsync();

            var backups = await OneDriveClient.Drive.Special.AppRoot.Children.Request().GetAsync();

            RingOfProgress.Visibility = Visibility.Collapsed;
            RingOfProgress.IsActive = false;

            if (backups.Count < 1)
            {
                FileList.Items.Clear();
                FileList.Items.Add("No files found.");
            } else
            {
                foreach (var file in backups)
                {
                    FileList.Items.Add(file.Name);
                }
            }
        }

        private async void OnSelectDataFile(object sender, ItemClickEventArgs e)
        {
            string fName = (string)e.ClickedItem;

            RingOfProgress.IsActive = true;
            RingOfProgress.Visibility = Visibility.Visible;
            await Util.Database.DownloadAsync(fName);
            RingOfProgress.Visibility = Visibility.Collapsed;
            RingOfProgress.IsActive = false;

            this.Frame.Navigate(typeof(Pages.Main.MainPage));
        }

        // Navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetBackupList();

            this.navigationHelper.OnNavigatedTo(e);
        }
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
