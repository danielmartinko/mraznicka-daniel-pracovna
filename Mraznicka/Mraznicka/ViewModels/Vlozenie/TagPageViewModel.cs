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

namespace Mraznicka.ViewModels.Vlozenie
{
    public class TagPageViewModel : BaseViewModel
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public Models.Polozka Item { get; set; } 
        public IDataStore<Models.Polozka> DataStore => DependencyService.Get<IDataStore<Models.Polozka>>();
        public IDataStore<Models.PoslednePouzite> DataStorePosledne => DependencyService.Get<IDataStore<Models.PoslednePouzite>>();
        public IDataStore<Models.Tovar> DataStoreTovar => DependencyService.Get<IDataStore<Models.Tovar>>();
        public ContentPage contentPage { get; set; }

        public const string ALERT_TITLE = "NFC";
        public const string MIME_TYPE = "application/sk.qsoft.iqfridge";

        NFCNdefTypeFormat _type;
        bool _makeReadOnly = false;
        bool _eventsAlreadySubscribed = false;
        bool _isDeviceiOS = false;


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


        public TagPageViewModel(ContentPage page)
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
                    contentPage.DisplayAlert("Chytra Mraznicka", Mraznicka.Resources.AppResources.nfcisnotavailable, "Zrusit");
                }


                NfcIsEnabled = CrossNFC.Current.IsEnabled;
                if (!NfcIsEnabled)
                {
                    contentPage.DisplayAlert("Chytra Mraznicka", Mraznicka.Resources.AppResources.nfcisdissabled, "Zrusit");
                }


                if (Device.RuntimePlatform == Device.iOS)
                    _isDeviceiOS = true;


            }
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(Item.Popis);
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Button b = contentPage.FindByName<Button>("btnSubmit");
            b.BackgroundColor = Color.Gray;


            SubscribeEvents();
            CrossNFC.Current.StartListening();
            CrossNFC.Current.StartPublishing(true);
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
            await contentPage.DisplayAlert("Chytra Mraznicka", $"NFC has been {(isEnabled ? "enabled" : "disabled")}", "Zrusit");
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
            if (!CrossNFC.Current.IsWritingTagSupported)
            {
                await contentPage.DisplayAlert("Chytra Mraznicka", Mraznicka.Resources.AppResources.writingtagisnotsupportedonthisdevice, "Zrusit");
                return;
            }

            var identifier = tagInfo.Identifier;
            var serialNumber = NFCUtils.ByteArrayToHexString(identifier, ":");
            //ctx.Item.TagID = serialNumber;

            Debug.WriteLine(serialNumber);

            try
            {
                try
                {
                    var exist = DataStore.GetItems(false).FirstOrDefault(o => o.TagID == tagInfo.SerialNumber);
                    if (exist == null)
                    {
                        var writeToTag = $@"Bravcove koleno
Vlozene:{Item.DatumVytvorenia.ToString("dd.MM.yyyy")}
Expiracia:{Item.Expiracia.ToString("dd.MM.yyyy")}
{Item.Hmotnost} g";
                        
                        NFCNdefRecord record = new NFCNdefRecord
                        {
                            TypeFormat = NFCNdefTypeFormat.WellKnown,
                            MimeType = MIME_TYPE,

                            Payload = NFCUtils.EncodeToByteArray(writeToTag),
                            LanguageCode = "en"
                        };

                        tagInfo.Records = new[] { record };

                        //CrossNFC.Current.ClearMessage(tagInfo);
                        CrossNFC.Current.PublishMessage(tagInfo, false);



                        Item.TagID = tagInfo.SerialNumber;
                        DataStore.AddItem(Item);

                        //TODO:
                        Item = new Models.Polozka();

                        await contentPage.DisplayAlert("Chytra Mraznicka", Mraznicka.Resources.AppResources.writingtagoperationsuccessful, "Zrusit");
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        //Item = new Models.Polozka();
                        await contentPage.DisplayAlert("Chytra Mraznicka", "Dany tag uz je v databaze", "Zrusit");
                    }

                }
                catch (Exception ex)
                {
                    await contentPage.DisplayAlert("Chytra Mraznicka", ex.Message, "Zrusit");
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                await contentPage.DisplayAlert("Chytra Mraznicka", ex.Message, "Zrusit");

                //if (ex.HResult != -2146233088)
                //{
                //    await ShowAlert(ex.Message);
                //}
            }

            //ctx.SaveCommand.Execute(null);
        }
    }
}
