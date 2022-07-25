using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ViewModels.Vyber
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class CompareManualPageViewModel : BaseViewModel
    {
        public Command VyberCommand { get; }

        private Models.Polozka item;
        public Models.Polozka Item
        {
            get { return item; }
            set
            {
                SetProperty(ref item, value);
            }
        }

        public IDataStore<Models.Polozka> DataStore => DependencyService.Get<IDataStore<Models.Polozka>>();

        public ContentPage contentPage { get; set; }

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

        public CompareManualPageViewModel(ContentPage page)
        {
            contentPage = page;
            VyberCommand = new Command(OnVyber, ValidateVyber);
        }

        public void LoadItemId(int itemId)
        {
            try
            {
                Item = DataStore.GetItem(itemId);
                VyberCommand.ChangeCanExecute();
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
        private bool ValidateVyber()
        {
            //return !String.IsNullOrWhiteSpace(Item.Id.ToString())
            //	&& !String.IsNullOrWhiteSpace(Item.Nazov);

            //return !String.IsNullOrWhiteSpace(Item.Tovar);

            return Item != null;
        }

        private async void OnVyber()
        {

            bool answer = await contentPage.DisplayAlert(Resources.AppResources.vymazaniezaznamu, Resources.AppResources.naozajchcetevymazatzaznam, Resources.AppResources.ano, Resources.AppResources.nie);
            if (answer)
            {
                DataStore.DeleteItem(Item.Id);
                await Shell.Current.Navigation.PopToRootAsync();
                //await Shell.Current.GoToAsync("PreviewPage");
            }
        }
    }
}
