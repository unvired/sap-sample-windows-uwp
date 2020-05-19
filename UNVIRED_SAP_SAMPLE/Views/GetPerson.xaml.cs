using Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Unvired.Kernel.Sync;
using Unvired.Kernel.UWP.Constants;
using Unvired.Kernel.UWP.Core;
using Unvired.Kernel.UWP.Database;
using Unvired.Kernel.UWP.Log;
using Unvired.Kernel.UWP.Model;
using Unvired.Kernel.UWP.Sync;
using UNVIRED_REST_SAMPLE.Utility;
using Utils;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UNVIRED_SAP_SAMPLE.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GetPerson : Page
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //private ObservableCollection<PERSON_HEADER> _personHeader;
        //public ObservableCollection<PERSON_HEADER> PersonHeader
        //{
        //    get => _personHeader;
        //    set
        //    {
        //        _personHeader = value;
        //        RaisePropertyChanged("PersonHeader");
        //    }
        //}

        private PERSON_HEADER _personHeaderInput;

        public PERSON_HEADER PersonHeaderInput
        {
            get => _personHeaderInput;
            set
            {
                _personHeaderInput = value;
                RaisePropertyChanged("PersonHeaderInput");
            }
        }

        private PERSON_HEADER _personHeaderResponse;

        public PERSON_HEADER PersonHeaderResponse
        {
            get => _personHeaderResponse;
            set
            {
                _personHeaderResponse = value;
                RaisePropertyChanged("PersonHeaderResponse");
            }
        }


        public GetPerson()
        {
            this.InitializeComponent();
            if (PersonHeaderInput == null) PersonHeaderInput = new PERSON_HEADER();
            PersonHeaderResponse = new PERSON_HEADER();
            ValidationTextBlock.Visibility = Visibility.Collapsed;
            displayGrid.Visibility = Visibility.Collapsed;
            gridview.Visibility = Visibility.Collapsed;
        }

        private void FirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (ValidationTextBlock.Visibility == Visibility.Visible)
                    ValidationTextBlock.Visibility = Visibility.Collapsed;
                displayGrid.Visibility = Visibility.Collapsed;
                gridview.Visibility = Visibility.Collapsed;
                if (!string.IsNullOrEmpty(FirstName.Text) && FirstName.Text.Length > 0)
                {
                    bool isNumeric = Util.IsNumeric(FirstName.Text);
                    if (!isNumeric)
                    {
                        FirstName.Text = FirstName.Text.Substring(0, FirstName.Text.Length - 1);
                        FirstName.Select(FirstName.Text.Length, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.E($"Exception caught while last focus command executing {ex.Message}");
            }


        }




        //internal static readonly DataManagerImpl AppDataManager = ApplicationManager.Instance.GetDataManager();
        private async void GetPersonClick(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(FirstName.Text))
            {
                ValidationTextBlock.Visibility = Visibility.Visible; return;
            }
            PersonHeaderInput.PERSNUMBER = int.Parse(FirstName.Text);

            if (!ConnectionHelper.HasAnyInternetConnection())
            {
                var connectionDialog = Util.AlertContentDialog("No Internet", "No Internet ConnectionPlease Try Again Later", "OK");
                await connectionDialog.ShowAsync();
                return;
            }
            await CallPAForPerson(PersonHeaderInput);
        }
        private async Task CallPAForPerson(PERSON_HEADER personHeaderInput)
        {
            Util.ShowProgressDialog("Please wait getting the Person No.");
            try
            {
                if (personHeaderInput.PERSNUMBER != null)
                {
                    var iSyncResponse = await SyncEngine.Instance.SubmitInForeground(MessageRequestType.RQST,
                  personHeaderInput, null, AppConstants.PA_GET_PERSON, false);
                    var response = (SyncBEResponse)iSyncResponse;
                    var infoMessages = response.InfoMessages;

                    if (infoMessages != null && infoMessages.Any())
                    {
                        foreach (var infoMessage in infoMessages)
                        {
                            if (!infoMessage.Category.Equals((ISyncResponse.RESPONSE_STATUS.FAILURE).ToString())) continue;
                            Util.HideProgressDialog();
                            var result = Util.FailureAlert("Error", "Failed get SAP Respose", "OK");
                            await result.ShowAsync();
                            Logger.E($"Error occur while receiving the response {infoMessage.Message}");
                            return;
                        }
                    }

                    if (response.ResponseStatus == ISyncResponse.RESPONSE_STATUS.SUCCESS)
                    {
                        BindResponseFromUMP(response.DataBEs);
                        displayGrid.Visibility = Visibility.Visible;
                        gridview.Visibility = Visibility.Visible;

                    }
                }
                else
                {
                    await Util.InformationAlert("Alert..!", $"Please enter Person No.", "OK").ShowAsync();
                }
            }
            catch (Exception e)
            {
                Logger.E($"Exception caught while getting document {e.Message}");
            }
            Util.HideProgressDialog();
        }

        private void BindResponseFromUMP(Dictionary<string, Dictionary<DataStructure, List<DataStructure>>> dataBEs)
        {
            try
            {


                for (int i = 0; i < dataBEs.Count; i++)
                {
                    var item = dataBEs.ElementAt(i);
                    if (item.Key.Equals("PERSON"))
                    {
                        for (int j = 0; j < item.Value.Count; j++)
                        {
                            var headerData = (item.Value).ElementAt(j);

                            PersonHeaderResponse = (PERSON_HEADER)headerData.Key;

                            IDtxt.Text = Convert.ToString(PersonHeaderResponse.PERSNUMBER);
                            Nametxt.Text = PersonHeaderResponse.FIRST_NAME;
                            Professiontxt.Text = PersonHeaderResponse.PROFESSION;
                            Gendertxt.Text = PersonHeaderResponse.SEX;
                            //dobtxt.Text = PersonHeaderResponse.BIRTHDAY ?? string.Empty;
                            weighttxt.Text = Convert.ToString(PersonHeaderResponse.WEIGHT);
                            heighttxt.Text = Convert.ToString(PersonHeaderResponse.HEIGHT);


                        }
                    }
                }


            }
            catch (Exception e)
            {
                Logger.E($"Exception caught while binding data to the physical inventory detail page {e.Message}");
            }
        }

        private void FirstName_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
