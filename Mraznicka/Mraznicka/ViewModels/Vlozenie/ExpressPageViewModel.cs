using Mraznicka.Models;
using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.NFC;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Mraznicka.ViewModels.Vlozenie
{
    public class ExpressPageViewModel : BaseViewModel
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Models.Polozka Item { get; set; } 
        public IDataStore<Models.Polozka> DataStore => DependencyService.Get<IDataStore<Models.Polozka>>();
        public IDataStore<Models.PoslednePouzite> DataStorePosledne => DependencyService.Get<IDataStore<Models.PoslednePouzite>>();
        public IDataStore<Models.Tovar> DataStoreTovar => DependencyService.Get<IDataStore<Models.Tovar>>();

        public IDataStore<Models.PoslednePouzite> PoslednePouziteDataStore => DependencyService.Get<IDataStore<Models.PoslednePouzite>>();

        public ObservableCollection<Mraznicka.Models.Miestnost> Miestnosti { get; set; } = new ObservableCollection<Mraznicka.Models.Miestnost>();
        public ObservableCollection<Mraznicka.Models.Zariadenie> Zariadenia { get; set; } = new ObservableCollection<Mraznicka.Models.Zariadenie>();
        public ObservableCollection<Mraznicka.Models.Pozicia> Pozicie { get; set; } = new ObservableCollection<Mraznicka.Models.Pozicia>();
        public ObservableCollection<Mraznicka.Models.Tovar> Tovary { get; set; } = new ObservableCollection<Mraznicka.Models.Tovar>();
        public SelectedData SelectedData { get; set; } = new SelectedData();
        public ContentPage contentPage { get; set; }

        public const string ALERT_TITLE = "NFC";
        public const string MIME_TYPE = "application/sk.qsoft.iqfridge";

        NFCNdefTypeFormat _type;
        bool _makeReadOnly = false;
        bool _eventsAlreadySubscribed = false;
        bool _isDeviceiOS = false;

        public ITagInfo TagInfo;

        /// <summary>
        /// Property that tracks whether the Android device is still listening,
        /// so it can indicate that to the user.
        /// </summary>
        public bool DeviceIsListening
        {
            get => _deviceIsListening;
            set
            {
                _deviceIsListening = value;
                OnPropertyChanged(nameof(DeviceIsListening));
            }
        }
        private bool _deviceIsListening;

        private bool _nfcIsEnabled;
        public bool NfcIsEnabled
        {
            get => _nfcIsEnabled;
            set
            {
                _nfcIsEnabled = value;
                OnPropertyChanged(nameof(NfcIsEnabled));
                OnPropertyChanged(nameof(NfcIsDisabled));
            }
        }

        public bool NfcIsDisabled => !NfcIsEnabled;

        private void SelectedData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var pp = ((App)Application.Current).PoslednePouzite;
            pp.Tovar = SelectedData.Tovar.Id;
            pp.Zariadenie = SelectedData.Zariadenie.Id;
            pp.Pozicia = SelectedData.Pozicia.Id;

            PoslednePouziteDataStore.UpdateItem(pp);
        }

        public ExpressPageViewModel(ContentPage page)
        {

            contentPage = page;

            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);

            var pp = ((App)Application.Current).PoslednePouzite;
            var tovar = DataStoreTovar.GetItem(pp.Tovar);

            Item = new Models.Polozka();
            Item.PropertyChanged += (o, e) => SaveCommand.ChangeCanExecute();
            Item.TagID = String.Empty;
            Item.Expiracia = DateTime.Now.AddDays(tovar.Expiracia);
            Item.Miestnost = pp.Miestnost;
            Item.Pozicia = pp.Pozicia;
            Item.Tovar = pp.Tovar;
            Item.Zariadenie = pp.Zariadenie;
            Item.Typ = 1;
            Item.DatumVytvorenia = DateTime.Now;


            var items = DataStore.GetItems(true).ToList();

            if (CrossNFC.IsSupported)
            {
                if (!CrossNFC.Current.IsAvailable)
                {
                    //await ShowAlert(Mraznicka.Resources.AppResources.nfcisnotavailable);
                    contentPage.DisplayAlert(Mraznicka.Resources.AppResources.chytra_mraznicka, Mraznicka.Resources.AppResources.nfcisnotavailable, Mraznicka.Resources.AppResources.zrusit);
                }


                NfcIsEnabled = CrossNFC.Current.IsEnabled;
                if (!NfcIsEnabled)
                {
                    contentPage.DisplayAlert(Mraznicka.Resources.AppResources.chytra_mraznicka, Mraznicka.Resources.AppResources.nfcisdissabled, Mraznicka.Resources.AppResources.zrusit);
                }


                if (Device.RuntimePlatform == Device.iOS)
                    _isDeviceiOS = true;


                SubscribeEvents();
                CrossNFC.Current.StartListening();
                CrossNFC.Current.StartPublishing(true);
            }


        }

        private bool ValidateSave()
        {
            return true; // !String.IsNullOrWhiteSpace(Item.Popis);
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            try
            {
                var exist = DataStore.GetItems(false).FirstOrDefault(o => o.TagID == Item.TagID);
                if (exist == null)
                {
                   
                    DataStore.AddItem(Item);

                    //TODO:
                    Item = new Models.Polozka();

                    await contentPage.DisplayAlert(Mraznicka.Resources.AppResources.chytra_mraznicka, Mraznicka.Resources.AppResources.writingtagoperationsuccessful, Mraznicka.Resources.AppResources.zrusit);
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await contentPage.DisplayAlert(Mraznicka.Resources.AppResources.chytra_mraznicka, "Dany tag uz je v databaze", Mraznicka.Resources.AppResources.zrusit);
                }

            }
            catch (Exception ex)
            {
                await contentPage.DisplayAlert(Mraznicka.Resources.AppResources.chytra_mraznicka, ex.Message, Mraznicka.Resources.AppResources.zrusit);
            }
        }

        public void SubscribeEvents()
        {
            if (_eventsAlreadySubscribed)
                return;

            _eventsAlreadySubscribed = true;

            //CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
            CrossNFC.Current.OnMessagePublished += Current_OnMessagePublished;
            CrossNFC.Current.OnTagDiscovered += Current_OnTagDiscovered;
            CrossNFC.Current.OnNfcStatusChanged += Current_OnNfcStatusChanged;
            CrossNFC.Current.OnTagListeningStatusChanged += Current_OnTagListeningStatusChanged;

            if (_isDeviceiOS)
                CrossNFC.Current.OniOSReadingSessionCancelled += Current_OniOSReadingSessionCancelled;
        }

        public void UnsubscribeEvents()
        {
            //CrossNFC.Current.OnMessageReceived -= Current_OnMessageReceived;
            CrossNFC.Current.OnMessagePublished -= Current_OnMessagePublished;
            CrossNFC.Current.OnTagDiscovered -= Current_OnTagDiscovered;
            CrossNFC.Current.OnNfcStatusChanged -= Current_OnNfcStatusChanged;
            CrossNFC.Current.OnTagListeningStatusChanged -= Current_OnTagListeningStatusChanged;

            if (_isDeviceiOS)
                CrossNFC.Current.OniOSReadingSessionCancelled -= Current_OniOSReadingSessionCancelled;
        }

        void Current_OnTagListeningStatusChanged(bool isListening)
        {
            DeviceIsListening = isListening;
        }

        async void Current_OnNfcStatusChanged(bool isEnabled)
        {
            NfcIsEnabled = isEnabled;
            await contentPage.DisplayAlert(Mraznicka.Resources.AppResources.chytra_mraznicka, $"NFC has been {(isEnabled ? "enabled" : "disabled")}", Mraznicka.Resources.AppResources.zrusit);
        }


        void Current_OniOSReadingSessionCancelled(object sender, EventArgs e)
        {

        }

        async void Current_OnMessagePublished(ITagInfo tagInfo)
        {
            //CrossNFC.Current.StopPublishing();


        }

        async void Current_OnTagDiscovered(ITagInfo tagInfo, bool format)
        {
            TagInfo = tagInfo;

            if (!CrossNFC.Current.IsWritingTagSupported)
            {
                await contentPage.DisplayAlert(Mraznicka.Resources.AppResources.chytra_mraznicka, Mraznicka.Resources.AppResources.writingtagisnotsupportedonthisdevice, Mraznicka.Resources.AppResources.zrusit);
                return;
            }

            var identifier = tagInfo.Identifier;
            var serialNumber = NFCUtils.ByteArrayToHexString(identifier, ":");
            //ctx.Item.TagID = serialNumber;

            try
            {
                var writeToTag = $@"V:{Item.DatumVytvorenia },E:{Item.Expiracia },K: { DataStoreTovar.GetItem(Item.Tovar).Nazov },P: {Item.Popis },H: {Item.Hmotnost }";
                NFCNdefRecord record = new NFCNdefRecord
                {
                    TypeFormat = NFCNdefTypeFormat.WellKnown,
                    MimeType = MIME_TYPE,

                    Payload = NFCUtils.EncodeToByteArray(writeToTag),
                    LanguageCode = "en"
                };

                tagInfo.Records = new[] { record };
                CrossNFC.Current.PublishMessage(tagInfo, false);

                Item.TagID = TagInfo.SerialNumber;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                await contentPage.DisplayAlert(Mraznicka.Resources.AppResources.chytra_mraznicka, ex.Message, Mraznicka.Resources.AppResources.zrusit);

                //if (ex.HResult != -2146233088)
                //{
                //    await ShowAlert(ex.Message);
                //}
            }

            //ctx.SaveCommand.Execute(null);
        }

        public void OnAppearing()
        {
            SelectedData.PropertyChanged += SelectedData_PropertyChanged;
        }
    }
}
