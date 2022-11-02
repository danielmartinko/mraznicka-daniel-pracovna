using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.SimpleAudioPlayer;
using Plugin.Toast;
using Xamarin.CommunityToolkit.Extensions;

namespace Mraznicka.ViewModels.Vyber
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class CompareEANPageViewModel : BaseViewModel
    {


        private Models.Polozka item;
        private string staryEAN;
        private Label lNajdenaVlozena;
        private Label lNajdenaDatumVytvorenia;
        private Label lTextNajdenaPolozka;
        private Label lTextEAN;
        private Label lVyber;
        private ImageButton iTovar;
        private Button btnSubmit;

        public Models.Polozka Item
        {
            get { return item; }
            set
            {
                SetProperty(ref item, value);
            }
        }

        private Models.Polozka tagItem;
        public Models.Polozka TagItem
        {
            get { return tagItem; }
            set
            {
                SetProperty(ref tagItem, value);
            }
        }

        public IDataStore<Models.Polozka> DataStore => DependencyService.Get<IDataStore<Models.Polozka>>();

        public Command<Models.Polozka> ItemTapped { get; }
        public Command<Models.Polozka> ItemPull { get; }
        public Command VyberCommand { get; }

        public ContentPage contentPage { get; set; }

        public CompareEANPageViewModel(ContentPage page)
        {
            staryEAN = "";
            contentPage = page;
            ItemTapped = new Command<Models.Polozka>(OnItemSelected);
            ItemPull = new Command<Models.Polozka>(OnItemPull);
            VyberCommand = new Command(OnVyber, ValidateVyber);
            lNajdenaVlozena = contentPage.FindByName<Label>("label_najdena_vlozene");
            lNajdenaDatumVytvorenia = contentPage.FindByName<Label>("label_najdena_datum_vytvorenia");
            lTextNajdenaPolozka = contentPage.FindByName<Label>("text_najdena_polozka");
            lTextEAN = contentPage.FindByName<Label>("text_ean");
            lVyber = contentPage.FindByName<Label>("vyber");
            iTovar = contentPage.FindByName<ImageButton>("image_tovar");
            btnSubmit = contentPage.FindByName<Button>("btnSubmit");

            lNajdenaVlozena.IsVisible = false;
            lNajdenaDatumVytvorenia.IsVisible = false;
            lTextNajdenaPolozka.IsVisible = false;
            lTextEAN.IsVisible = false;
            lVyber.IsVisible = false;
            iTovar.IsVisible = false;
            btnSubmit.IsVisible = false;
        }

        private int itemId;

        public int ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public void OnAppearing()
        {
        }
        public void LoadItemId(int itemId)
        {
            try
            {
                Item = DataStore.GetItem(itemId);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        private async void OnPreviewClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            await Shell.Current.GoToAsync("PreviewPage");
        }

        private bool ValidateVyber()
        {
            if (Item == null || TagItem == null)
                return false;

            return Item.TagID == TagItem.TagID;
        }

        public void Compare(string tagId)
        {
            var duration = TimeSpan.FromSeconds(1);

            ISimpleAudioPlayer _simpleAudioPlayer;
            _simpleAudioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            System.IO.Stream beepStream = GetType().Assembly.GetManifestResourceStream("Mraznicka.beep-02.mp3");

            try
            {
                bool isSuccess = _simpleAudioPlayer.Load(beepStream);
                _simpleAudioPlayer.Play();
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            if (staryEAN == tagId)
                return;
            staryEAN = tagId;
            Models.Polozka polozka = new Models.Polozka();
            polozka.TagIDPrecitany = "";
            item.TagIDPrecitany = tagId;
            TagItem = DataStore.GetItems().OrderBy(o=>o.Expiracia).FirstOrDefault(o => o.TagID == tagId);
            if (TagItem != null)
            {
                lNajdenaVlozena.IsVisible = true;
                lNajdenaDatumVytvorenia.IsVisible = true;
                lTextNajdenaPolozka.IsVisible = true;
                lTextEAN.IsVisible = true;
                lVyber.IsVisible = true;
                iTovar.IsVisible = true;
                try
                {
                    Vibration.Vibrate(duration);
                }
                catch (FeatureNotSupportedException ex)
                {
                    // Feature not supported on device
                }
                if(item.TagID == tagItem.TagID)
                {
                    beepStream = GetType().Assembly.GetManifestResourceStream("Mraznicka.Pubg - ok.mp3");
                    DMToast dt = new DMToast();
                    dt.ToastMessage(Mraznicka.Resources.AppResources.spravny_ean);
                }
                else
                {
                    beepStream = GetType().Assembly.GetManifestResourceStream("Mraznicka.No No.mp3");
                    DMToast dt = new DMToast();
                    dt.ToastError(Mraznicka.Resources.AppResources.nespravny_ean);
                }
            }
            else
            {
                lNajdenaVlozena.IsVisible = false;
                lNajdenaDatumVytvorenia.IsVisible = false;
                lTextNajdenaPolozka.IsVisible = false;
                lTextEAN.IsVisible = false;
                lVyber.IsVisible = false;
                iTovar.IsVisible = false;

                beepStream = GetType().Assembly.GetManifestResourceStream("Mraznicka.No No.mp3");
                DMToast dt = new DMToast();
                dt.ToastError(Mraznicka.Resources.AppResources.eansanepouziva);
            }

            try
            {
                bool isSuccess = _simpleAudioPlayer.Load(beepStream);
                _simpleAudioPlayer.Play();
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }

            VyberCommand.ChangeCanExecute();
        }

        private async void OnVyber()
        {
            var item = DataStore.GetItem(Item.Id);
            if (item != null)
            {
                bool answer = await contentPage.DisplayAlert(Resources.AppResources.vymazaniezaznamu, Resources.AppResources.naozajchcetevymazatzaznam, Resources.AppResources.ano, Resources.AppResources.nie);
                if (answer)
                {
                    DataStore.DeleteItem(item.Id);
                    DMToast dt = new DMToast();
                    dt.ToastMessage(Mraznicka.Resources.AppResources.polozka_bola_vymazana);
                    await Shell.Current.Navigation.PopToRootAsync();
                    //await Shell.Current.GoToAsync("PreviewPage");
                }
            }
            // This will pop the current page off the navigation stack
        }
        async void OnItemSelected(Models.Polozka item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"PolozkaDetailPage?{nameof(ViewModels.Polozka.DetailViewModel.ItemId)}={item.Id}");
        }
        async void OnItemPull(Models.Polozka item)
        {
            if (item == null)
                return;

            bool answer = await contentPage.DisplayAlert(Resources.AppResources.vymazaniezaznamu, Resources.AppResources.naozajchcetevymazatzaznam, Resources.AppResources.ano, Resources.AppResources.nie);
            if (answer)
            {
                //CrossToastPopUp.Current.ShowToastSuccess(Mraznicka.Resources.AppResources.polozka_ean_vymazana, Plugin.Toast.Abstractions.ToastLength.Long);
                DataStore.DeleteItem(item.Id);
                DMToast dt = new DMToast();
                dt.ToastSuccess(Mraznicka.Resources.AppResources.polozka_ean_vymazana);
                await Shell.Current.Navigation.PopToRootAsync();

                // DataStore.DeleteItem(item.Id);
                // ExecuteLoadItemsCommand(item.TagID);
                // Shell.Current.GoToAsync("..");
            }

        }
    }
}
