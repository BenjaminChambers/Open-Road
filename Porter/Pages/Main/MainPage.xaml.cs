using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.Data.Xml.Dom;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Porter.Pages.Main
{
    public sealed partial class MainPage : Page
    {
        Util.ViewModels.FillupForm FormData = new Util.ViewModels.FillupForm();
        ObservableCollection<Util.ViewModels.FillupStatsView> fillupStats = new ObservableCollection<Util.ViewModels.FillupStatsView>();

        public MainPage()
        {
            this.InitializeComponent();

            SetupForms();
            SetupAnimations();
            SetupFillupStats();

            RefreshDisplay();
        }

        private void SetupForms()
        {
            FormData = new Util.ViewModels.FillupForm();
            QuickFillupForm.DataContext = FormData;
        }

        private void SetupAnimations()
        {
            Timeline fade = (Timeline)Resources["NotificationFade"];
            Storyboard.SetTarget(fade, NotificationPanel);
            Timeline slide = (Timeline)Resources["NotificationSlide"];
            Storyboard.SetTarget(slide, NotificationPanelTransform);
        }

        private void RefreshDisplay()
        {
            foreach (Util.ViewModels.FillupStatsView item in fillupStats)
                    item.Update();

            Util.LiveTile.Render();
        }

        private void OnTapHamburger(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void OnClickCustomize(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Customization.CustomizationPage));
        }

        private void OnClickPreferences(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings.PreferencesPage));
        }
    }
}
