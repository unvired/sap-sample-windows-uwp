using Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unvired.Kernel.Sync;
using Unvired.Kernel.UWP.Constants;
using Unvired.Kernel.UWP.Core;
using Unvired.Kernel.UWP.Database;
using Unvired.Kernel.UWP.Log;
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
    public sealed partial class AddPerson : Page
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        List<E_MAIL> EmailListCollection = new List<E_MAIL>();


        private ObservableCollection<E_MAIL> _EmailHeader;
        public ObservableCollection<E_MAIL> EmailHeader
        {
            get => _EmailHeader;
            set
            {
                _EmailHeader = value;
                RaisePropertyChanged("EmailHeader");
            }
        }

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

        private E_MAIL _EmailInput;

        public E_MAIL EmailInput
        {
            get => _EmailInput;
            set
            {
                _EmailInput = value;
                RaisePropertyChanged("EmailInput");
            }
        }

        private E_MAIL _EmailResponse;

        public E_MAIL EmailResponse
        {
            get => _EmailResponse;
            set
            {
                _EmailResponse= value;
                RaisePropertyChanged("EmailResponse");
            }
        }

        internal static readonly DataManagerImpl AppDataManager = ApplicationManager.Instance.GetDataManager();
        public AddPerson()
        {
            this.InitializeComponent();

            if (EmailHeader == null)
            {
                EmailHeader = new ObservableCollection<E_MAIL>();

            }
            if (PersonHeaderInput == null) PersonHeaderInput = new PERSON_HEADER();
            PersonHeaderResponse = new PERSON_HEADER();

            if (EmailInput == null) EmailInput = new E_MAIL();
            EmailResponse = new E_MAIL();
            // displayEmail.Visibility = Visibility.Collapsed;

            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
           
            if (EmailHeader.Any()) EmailHeader.Clear();

            //EmailList.ItemsSource = EmailListCollection;

            //EmailList.ItemsSource  = AppDataManager.Get<E_MAIL>().ToList();

            //     EmailHeader = new ObservableCollection<E_MAIL>(pDetails);
        }

        private async void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            Util.ShowProgressDialog("Please wait getting the Person Save");
            try
            {
                //string emailAddresses = (string)value;
                //string[] addresses = emailAddresses.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);
                //ObservableCollection<string> addressCollection = new ObservableCollection<string>();
                //foreach (string address in addresses) addressCollection.Add(address.Trim());


                int i = 0;
                if (PersonHeaderInput != null && EmailInput != null)
                {
                    PersonHeaderInput.WEIGHT = double.Parse(Weighttxt.Text);
                    PersonHeaderInput.HEIGHT = double.Parse(Heighttxt.Text);


                    PersonHeaderInput.PERSNUMBER =new  Random().Next(1,1000);
                    PersonHeaderInput.MANDT="800";//TODO: Need to check
                    PersonHeaderInput.OBJECT_STATUS = Unvired.Kernel.UWP.Model.DataStructure.OBJECT_STATUS_ENUM.ADD;

                 
                    try
                    {
                       
                        AppDataManager.InsertOrUpdateBasedOnGid(PersonHeaderInput);
                        //---Email---//
                        if (EmailHeader.Any())

                        {
                            try
                            {
                                foreach (var item in EmailHeader)
                                {
                                    item.OBJECT_STATUS = Unvired.Kernel.UWP.Model.DataStructure.OBJECT_STATUS_ENUM.ADD;
                                    AppDataManager.InsertOrUpdateBasedOnGid(item);
                                }
                            }
                            catch (Exception ex)
                            {

                                Logger.E($"");
                            }
                        }
                    }



                    //string emailAddresses = (string)EmailInput.E_ADDR_TEXT;
                    //string[] addresses = emailAddresses.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    //ObservableCollection<string> addressCollection = new ObservableCollection<string>();
                    //foreach (string address in addresses)
                    //{
                    //    addressCollection.Add(address.Trim());

                    //        //AppDataManager.InsertOrUpdateBasedOnGid(EmailInput);
                    //        var res = addressCollection;

                    //        AppDataManager.InsertOrUpdateBasedOnGid(EmailInput);

                    //        i++;
                    //    }
                    //}
                    //---------------//

                    //try
                    //{ 
                    //    AppDataManager.InsertOrUpdateBasedOnGid(PersonHeaderInput);
                    //    //---Email---//
                    //    AppDataManager.InsertOrUpdateBasedOnGid(EmailInput);
                    //}
                    catch (Exception ex)
                    {
                        Logger.E($"Exception caught while saving Person in database. Message:{ex.Message}");
                        Logger.E($"StackTrace - {ex.StackTrace}");
                    }
                    var iSyncResponse = await SyncEngine.Instance.SubmitInForeground(MessageRequestType.RQST,
                  PersonHeaderInput, null, AppConstants.PA_CREATE_PERSON, true);
                    var response = (SyncBEResponse)iSyncResponse;
                    var infoMessages = response.InfoMessages;

                    //---Email---//
                    var iSyncResponseEmail = await SyncEngine.Instance.SubmitInForeground(MessageRequestType.RQST,
                                           EmailInput, null, AppConstants.PA_CREATE_PERSON, true);
                    var responseEmail = (SyncBEResponse)iSyncResponseEmail;
                    var infoMsgEmail = responseEmail.InfoMessages;
                   

                    if (infoMessages != null && infoMsgEmail!= null && infoMessages.Any())
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

                    if (response.ResponseStatus == ISyncResponse.RESPONSE_STATUS.SUCCESS  && responseEmail.ResponseStatus == ISyncResponse.RESPONSE_STATUS.SUCCESS)
                    {
                        var successMesg = response.InfoMessages[0].Message;
                           
                            try
                            {
                                var indexOfEquals = successMesg.IndexOf("=") + 1;
                                PersonHeaderInput.PERSNUMBER = int.Parse(successMesg.Substring(indexOfEquals, successMesg.Length - indexOfEquals - 1));
                                //AppDataManager.InsertOrUpdate(PersonHeaderInput);
                               
                               

                            try
                            {
                                foreach (var item in EmailHeader)
                                {
                                    item.OBJECT_STATUS = Unvired.Kernel.UWP.Model.DataStructure.OBJECT_STATUS_ENUM.GLOBAL;
                                    item.PERSNUMBER = PersonHeaderInput.PERSNUMBER;
                                    AppDataManager.InsertOrUpdateBasedOnGid(item);
                                }
                            }
                            catch (Exception ex)
                            {

                                Logger.E($"");
                            }
                            }
                            catch (Exception ex)
                            {
                                Logger.E($"Exception caught while saving Person in database. Message:{ex.Message}");
                                Logger.E($"StackTrace - {ex.StackTrace}");
                            }
                       
                        Util.HideProgressDialog();
                        var msg = Util.SucessAlert("Sucess", response.InfoMessages[0].Message,"OK");
                        await msg.ShowAsync(); 
                        

                    }
                }
                else
                {
                    await Util.InformationAlert("Alert..!", $"Please enter City Name", "OK").ShowAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.E($"Exception caught while getting document {ex.Message}");
            }
            Util.HideProgressDialog();

            FNametxt.Text = string.Empty;
            LNametxt.Text = string.Empty;
            Gendertxt.Text = string.Empty;
            Professiontxt.Text = string.Empty;
            Weighttxt.Text = string.Empty;
            Heighttxt.Text = string.Empty;
            Emailtxt.Text = string.Empty;
           // PersonHeaderInput = new PERSON_HEADER();
        }

        private void AddEmail_Click(object sender, RoutedEventArgs e)
        {
           
            displayEmail.Visibility = Visibility.Visible;

            if (LNametxt.Text == string.Empty)
            { 
                ValidationTextBlock.Visibility = Visibility.Visible;
            }
            //displayGrid.Visibility = Visibility.Collapsed;
            //displayButton.Visibility = Visibility.Collapsed;
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    displayGrid.Visibility = Visibility.Visible;
        //    displayEmail.Visibility =Visibility.Collapsed;
        //    displayButton.Visibility = Visibility.Visible;
        //}

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //displayEmailID.Visibility = Visibility.Visible;

            if (EmailHeader.Any()) EmailHeader.Clear();
            var pDetails = AppDataManager.Get<E_MAIL>().ToList();

            //EmailHeader = new ObservableCollection<E_MAIL>(pDetails);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddEmailBtn_Click(object sender, RoutedEventArgs e)
        {
            displayEmailIDList.Visibility = Visibility.Visible;

            if (!string.IsNullOrEmpty(Emailtxt.Text))
            {
                if(EmailListCollection == null)
                {
                    EmailListCollection = new List<E_MAIL>();


                }
                int i = EmailHeader.Count();
                EmailHeader.Add(new E_MAIL { E_ADDR = Emailtxt.Text, MANDT = "800", PERSNUMBER = 0, SEQNO_E_MAIL = i + 1, FID = PersonHeaderInput.LID });
                Emailtxt.Text = string.Empty;
               
            }
        }

        //private async void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    Util.ShowProgressDialog("Please wait getting the Email ID Save");
        //    try
        //    {
        //        string[] Emailarray = new string[4];

        //        if (EmailInput != null )
        //        {
        //            for (int i = 0; i < Emailarray.Length; i++)
        //            {
        //                EmailInput.FID = "3F8648B7096A4F2293992879B3625E39";
        //                EmailInput.SEQNO_E_MAIL = new Random().Next(1, i + 1);
        //                EmailInput.PERSNUMBER = 1389;
        //                EmailInput.MANDT = "800";//TODO: Need to check
        //                EmailInput.OBJECT_STATUS = Unvired.Kernel.UWP.Model.DataStructure.OBJECT_STATUS_ENUM.ADD;



        //                try
        //                {
        //                    AppDataManager.InsertOrUpdateBasedOnGid(EmailInput);
        //                }
        //                catch (Exception ex)
        //                {
        //                    Logger.E($"Exception caught while saving Person in database. Message:{ex.Message}");
        //                    Logger.E($"StackTrace - {ex.StackTrace}");
        //                }
        //                break;

        //                var iSyncResponse = await SyncEngine.Instance.SubmitInForeground(MessageRequestType.RQST,
        //                                    EmailInput, null, AppConstants.PA_CREATE_PERSON, true);

        //                var response = (SyncBEResponse)iSyncResponse;
        //                var infoMessages = response.InfoMessages;

        //                if (infoMessages != null && infoMessages.Any())
        //                {
        //                    foreach (var infoMessage in infoMessages)
        //                    {
        //                        if (!infoMessage.Category.Equals((ISyncResponse.RESPONSE_STATUS.FAILURE).ToString())) continue;
        //                        Util.HideProgressDialog();
        //                        var result = Util.FailureAlert("Error", "Failed get SAP Respose", "OK");
        //                        await result.ShowAsync();
        //                        Logger.E($"Error occur while receiving the response {infoMessage.Message}");
        //                        return;
        //                    }
        //                }


        //                if (response.ResponseStatus == ISyncResponse.RESPONSE_STATUS.SUCCESS)
        //                {
        //                    var successMesg = response.InfoMessages[0].Message;

        //                    try
        //                    {
        //                        var indexOfEquals = successMesg.IndexOf("=") + 1;
        //                        PersonHeaderInput.PERSNUMBER = int.Parse(successMesg.Substring(indexOfEquals, successMesg.Length - indexOfEquals - 1));
        //                        AppDataManager.InsertOrUpdate(PersonHeaderInput);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Logger.E($"Exception caught while saving Person in database. Message:{ex.Message}");
        //                        Logger.E($"StackTrace - {ex.StackTrace}");
        //                    }

        //                    Util.HideProgressDialog();
        //                    var msg = Util.SucessAlert("Sucess", response.InfoMessages[0].Message, "OK");
        //                    await msg.ShowAsync();


        //                }
        //            }
        //        }
        //        else
        //        {
        //            await Util.InformationAlert("Alert..!", $"Please enter City Name", "OK").ShowAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.E($"Exception caught while getting document {ex.Message}");
        //    }
        //    Util.HideProgressDialog();
        //}
    }
}
