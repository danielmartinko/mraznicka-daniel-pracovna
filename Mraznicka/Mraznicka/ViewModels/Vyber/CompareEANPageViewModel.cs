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

namespace Mraznicka.ViewModels.Vyber
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class CompareEANPageViewModel : BaseViewModel
    {


        private Models.Polozka item;
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

        public Command VyberCommand { get; }

        public ContentPage contentPage { get; set; }

        public CompareEANPageViewModel(ContentPage page)
        {
            contentPage = page;
            VyberCommand = new Command(OnVyber, ValidateVyber);
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
            TagItem = DataStore.GetItems().OrderBy(o=>o.Expiracia).FirstOrDefault(o => o.TagID == tagId);
            if (TagItem != null)
            {
                

                ISimpleAudioPlayer _simpleAudioPlayer;
                _simpleAudioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                System.IO.Stream beepStream = GetType().Assembly.GetManifestResourceStream("Mraznicka.beep-02.mp3");

                try
                {
                    bool isSuccess = _simpleAudioPlayer.Load(beepStream);
                    _simpleAudioPlayer.Play();

                    var duration = TimeSpan.FromSeconds(1);
                    Vibration.Vibrate(duration);
                }
                catch (FeatureNotSupportedException ex)
                {
                    // Feature not supported on device
                }
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
                    await Shell.Current.Navigation.PopToRootAsync();
                    //await Shell.Current.GoToAsync("PreviewPage");
                }
            }
            // This will pop the current page off the navigation stack
        }
    }
}
