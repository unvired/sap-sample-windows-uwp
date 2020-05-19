using Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Unvired.Kernel.UWP.Core;
using Unvired.Kernel.UWP.Database;
using Unvired.Kernel.UWP.Log;
using Windows.UI.Xaml.Controls;


namespace UNVIRED_SAP_SAMPLE.Views
{

    public sealed partial class ListPerson : Page
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<PERSON_HEADER> _personHeader;

        public ObservableCollection<PERSON_HEADER> PersonHeader
        {
            get => _personHeader;
            set
            {
                _personHeader = value;
                RaisePropertyChanged("PersonHeader");
            }
        }

        public void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //private string _searchString;
        //public string SearchString
        //{
        //    get => _searchString;
        //    set
        //    {
        //        _searchString = value;
        //        RaisePropertyChanged(_searchString);
        //        SearchedResult(_searchString);
        //    }

        //}
        public ListPerson()
        {
            this.InitializeComponent();

            if (PersonHeader == null)
            {
                PersonHeader = new ObservableCollection<PERSON_HEADER>();
            }

            personDetailGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            GetPersonDetails();
        }

        internal static readonly DataManagerImpl AppDataManager = ApplicationManager.Instance.GetDataManager();

        private void GetPersonDetails()
        {
            try
            {
                if (PersonHeader.Any()) PersonHeader.Clear();
                var pDetails = AppDataManager.Get<PERSON_HEADER>().ToList().OrderBy(x => x.FIRST_NAME);
                displayGrid.ItemsSource = pDetails;
                //PersonHeader = new ObservableCollection<PERSON_HEADER>(pDetails);
            }
            catch (Exception e)
            {
                Logger.E($"Exception caught while getting the data from data base {e.Message}");
            }
        }

        private void displayGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                personDetailGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                emailListView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                var itemClicked = e.ClickedItem as PERSON_HEADER;
                Nametext.Text = itemClicked.FIRST_NAME + " " + itemClicked.LAST_NAME;
                GenderText.Text = itemClicked.SEX ?? string.Empty;
                ProfessionText.Text = itemClicked.PROFESSION ?? string.Empty;
                NumberText.Text = itemClicked.PERSNUMBER.ToString();
                BirthDayText.Text = itemClicked.BIRTHDAY ?? string.Empty;
                HeightText.Text = itemClicked.HEIGHT.ToString();
                WeightText.Text = itemClicked.WEIGHT.ToString();

                if (!string.IsNullOrEmpty(NumberText.Text))
                {
                    var emailsList = AppDataManager.Get<E_MAIL>($"PERSNUMBER = '{NumberText.Text}'").ToList();
                    if (emailsList.Any())
                    {
                        emailListView.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        emailListView.ItemsSource = emailsList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.E($"Exception caught while clicking the element {ex.Message}");

            }
        }
        private void SearchedResult(string searchString)
        {
            try
            {
                if (PersonHeader.Any()) PersonHeader.Clear();
                var query = $"SELECT * FROM {nameof(PERSON_HEADER)} WHERE FIRST_NAME LIKE '%{searchString}%' OR PERSNUMBER LIKE '%{searchString}%'";
                var pDetails = AppDataManager.ExecuteDeferredQuery(nameof(PERSON_HEADER), query);
                displayGrid.ItemsSource = pDetails.ToList();
                //     PersonHeader = new ObservableCollection<PERSON_HEADER>(pDetails);
            }
            catch (Exception e)
            {
                Logger.E($"Exception caught while getting the data from data base {e.Message}");
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchedResult((sender as TextBox).Text);
        }
    }
}