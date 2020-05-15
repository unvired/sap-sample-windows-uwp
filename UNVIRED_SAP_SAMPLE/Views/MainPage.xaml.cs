using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Unvired.Kernel.Utils;
using Unvired.Kernel.UWP.Log;
using UNVIRED_REST_SAMPLE.Utility;
using UNVIRED_SAP_SAMPLE.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UNVIRED_SAP_SAMPLE
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
       
        public MainPage()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(ListPerson));
       
        }

       
        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if(args.IsSettingsSelected)
            {
                //ContentFrame.Navigate(typeof(ListPerson));
                SettingSplitView.IsPaneOpen = true;
            }
            else
            {
                NavigationViewItem item = args.SelectedItem as NavigationViewItem;

                switch(item.Tag.ToString())
                {
                    case "all":
                        ContentFrame.Navigate(typeof(ListPerson));
                        //NavView.Header = "House";
                        break;
                    case "add":
                        ContentFrame.Navigate(typeof(AddPerson));
                        break;
                    case "get":
                        ContentFrame.Navigate(typeof(GetPerson));
                        break;

                }
            }
        }

        private void EmailBtn_Click(object sender, RoutedEventArgs e)
        {
            Logger.SendLogViaEmail();
        }

        private void SendLogToServerBtn_Click(object sender, RoutedEventArgs e)
        {
            Logger.SendLogToServer();
        }

        private void ViewLogBtn_Click(object sender, RoutedEventArgs e)
        {
            Logger.GetLogViewer();
        }

        private async void ClearLogBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clearLogConfirmationDialog = Util.CommonContentDialog(Util.GetString("Alert"), "This will clear all the Logs associated with this application. Do you want to clear log", "Yes", "Cancel");
                var clearLogResult = await clearLogConfirmationDialog.ShowAsync();

                if (clearLogResult == ContentDialogResult.Primary)
                {
                    Logger.DeleteLogs();
                }
            }
            catch (Exception ex)
            {
                Logger.E($"Exception caught while clearing the application logs. Message {ex.Message}");
            }

        }

        private async void ClearDataBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clearDataConfirmationDialog = Util.CommonContentDialog(Util.GetString("Clear Application Data"), "This will clear all application related data. Are you sure you want to continue?", Util.GetString("Yes"), Util.GetString("Cancel"));
                var clearDataResult = await clearDataConfirmationDialog.ShowAsync();
                if (clearDataResult == ContentDialogResult.Primary)
                {
                    Task clearData = FrameworkHelper.ClearData();
                    await clearData;
                }
            }
            catch (Exception ex)
            {
                Logger.E($"Exception caught while clearing the application data. Message {ex.Message}");
            }
        }
    }

}
