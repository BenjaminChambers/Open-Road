using Porter.Common;
using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Porter.Pages.FillupList.EditFillup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditFillupPage : Page
    {
        public static int FillupID = -1;
        Util.ViewModels.FillupForm FormData = null;
        Util.Models.Fillup Current = null;

        public EditFillupPage()
        {
            InitializeComponent();
            InitializeNavigation();

            using (var db = Util.Database.Connection())
            {
                Current = db.Get<Util.Models.Fillup>(FillupID);
                FormData = new Util.ViewModels.FillupForm(Current);
            }
            FillupForm.DataContext = FormData;
        }

        private void OnClickSave(object sender, RoutedEventArgs e)
        {
            if (FormData != null)
            {
                FormData.Update(Current);
                using (var db = Util.Database.Connection())
                {
                    db.Update(Current);
                }
            }
            Frame.GoBack();
        }

        private void OnClickCancel(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void OnRevert(object sender, RoutedEventArgs e)
        {
            FormData = new Util.ViewModels.FillupForm(Current);
            FillupForm.DataContext = FormData;
        }


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
        protected override void OnNavigatedTo(NavigationEventArgs e) { this.navigationHelper.OnNavigatedTo(e); }
        protected override void OnNavigatedFrom(NavigationEventArgs e) { this.navigationHelper.OnNavigatedFrom(e); }
    }
}
