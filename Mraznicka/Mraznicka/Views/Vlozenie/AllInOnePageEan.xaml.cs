using Mraznicka.Models;
using Mraznicka.Services;
using Plugin.NFC;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using Plugin.Toast;
using Xamarin.CommunityToolkit.Extensions;

namespace Mraznicka.Views.Vlozenie
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllInOnePageEan : ContentPage
    {
        public IDataStore<Models.Polozka> DataStore => DependencyService.Get<IDataStore<Models.Polozka>>();
        public Command SaveCommand { get; }
        public Command<ZXing.Result> ScanResultCommand { get; private set; }
        public Models.Polozka Item { get; set; }

        public bool IsScanned { get; set; } = false;

        public AllInOnePageEan()
        {
            InitializeComponent();


            Item = ((App)Application.Current).AllInOneItem;
            SaveCommand = new Command(OnSave);
            ScanResultCommand = new Command<ZXing.Result>((x) => OnScanResultCommand(x), (x) => true);

            this.BindingContext = this;
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(Item.TagID);
        }

        private async void OnSave()
        {
            DataStore.AddItem(Item);
            // This will pop the current page off the navigation stack
            //await Shell.Current.GoToAsync("MainPage");
            await Shell.Current.Navigation.PopToRootAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    //CrossToastPopUp.Current.ShowToastSuccess(Mraznicka.Resources.AppResources.vlozenie_prebehlo_uspesne);
                    DMToast dt = new DMToast();
                    dt.ToastSuccess(Mraznicka.Resources.AppResources.vlozenie_prebehlo_uspesne);
                }
                catch (Exception ex)
                {

                }
                // Shell.Current.GoToAsync("VlozenieAllInOnePage");
                // Shell.Current.Navigation.PopToRootAsync();
                /*
                DisplayAlert("Chytrá Mraznička", "Zápis prebehol úspešne", "Ok").ContinueWith(t =>
                {
                    Shell.Current.Navigation.PopToRootAsync();
                });
                */
            });
        }

        private async void OnScanResultCommand(ZXing.Result param)
        {
            if (!IsScanned)
            {
                //var duration = TimeSpan.FromSeconds(1);
                //Vibration.Vibrate(duration);

                ISimpleAudioPlayer _simpleAudioPlayer;
                _simpleAudioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                System.IO.Stream beepStream = GetType().Assembly.GetManifestResourceStream("Mraznicka.beep-02.mp3");

                try
                {
                    bool isSuccess = _simpleAudioPlayer.Load(beepStream);
                    Vibration.Vibrate();
                    _simpleAudioPlayer.Play();
                    IsScanned = true;
                }
                catch (FeatureNotSupportedException ex)
                {
                    // Feature not supported on device
                }


                Item.TagID = param.Text;
                //await DisplayAlert("Uspesne naskenovany EAN", param.Text, "OK");
                //SaveCommand.ChangeCanExecute();
                SaveCommand.Execute(null);
            }
        }
    }
}