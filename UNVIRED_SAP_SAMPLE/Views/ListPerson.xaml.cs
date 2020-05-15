using Entity;
using System;
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

        public ListPerson()
        {
            this.InitializeComponent();

            if (PersonHeader == null)
            {
                PersonHeader = new ObservableCollection<PERSON_HEADER>();
            }

            if (PersonHeaderInput == null) PersonHeaderInput = new PERSON_HEADER();
            PersonHeaderResponse = new PERSON_HEADER();

            GetPersonDetails();
        }

        internal static readonly DataManagerImpl AppDataManager = ApplicationManager.Instance.GetDataManager();

        private void GetPersonDetails()
        {
            try
            {
                if (PersonHeader.Any()) PersonHeader.Clear();
                var pDetails = AppDataManager.Get<PERSON_HEADER>().ToList();
                PersonHeader = new ObservableCollection<PERSON_HEADER>(pDetails);
            }
            catch (Exception e)
            {
                Logger.E($"Exception caught while getting the data from data base {e.Message}");
            }
        }
    }
}