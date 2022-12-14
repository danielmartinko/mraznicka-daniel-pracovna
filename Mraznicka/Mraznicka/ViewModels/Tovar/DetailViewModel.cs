using Mraznicka.Models;
using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ViewModels.Tovar
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class DetailViewModel : BaseViewModel
    {
        public IDataStore<Models.Tovar> DataStore => DependencyService.Get<IDataStore<Models.Tovar>>();
        public IDataStore<Models.Polozka> DataStorePolozka => DependencyService.Get<IDataStore<Models.Polozka>>();
        public List<Models.Tovar> Items { get; set; }
        public Models.Tovar Item { get; set; } = new Models.Tovar();

        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }
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

        public DetailViewModel(ContentPage page)
        {
            Title = "Tovar";
            contentPage = page;
            SaveCommand = new Command(OnSave, ValidateSave);
            DeleteCommand = new Command(OnDelete, ValidateDelete);

            Items = DataStore.GetItems(true).ToList();
            Item.PropertyChanged += (o, e) => SaveCommand.ChangeCanExecute();
        }

        public async void LoadItemId(int itemId)
        {
            try
            {
                var item = DataStore.GetItem(itemId);
                Item.Id = item.Id;
                Item.Nazov = item.Nazov;
                Item.Expiracia = item.Expiracia;

                DeleteCommand.ChangeCanExecute();
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        private bool ValidateSave()
        {
            //return !String.IsNullOrWhiteSpace(Item.Id.ToString())
            //	&& !String.IsNullOrWhiteSpace(Item.Nazov);

            return !String.IsNullOrWhiteSpace(Item.Nazov);
        }

        private bool ValidateDelete()
        {
            if(Item.Id < 17)
                return false;
            
            return DataStorePolozka.GetItems().Where(o => o.Tovar == Item.Id).Count() == 0;
        }

        private async void OnDelete()
        {
            if (Items.Count > 1)
            {
                //bool answer = await contentPage.DisplayAlert(Resources.AppResources.vymazaniezaznamu, Resources.AppResources.naozajchcetevymazatzaznam, Resources.AppResources.ano, Resources.AppResources.nie);
                /////
                //if (answer)
                //{
                    DataStore.DeleteItem(Item.Id);
                DMToast dt = new DMToast();
                dt.ToastMessage(Mraznicka.Resources.AppResources.polozka_bola_vymazana);
                // This will pop the current page off the navigation stack
                await Shell.Current.GoToAsync("..");
                //}
            }
            else
            {
                await contentPage.DisplayAlert(Resources.AppResources.niejemoznevymazatzaznam, Resources.AppResources.musiexistovataspomjedenzaznam, Resources.AppResources.zrusit);
            }
        }

        private void OnSave()
        {
            DataStore.UpdateItem(Item);
            // This will pop the current page off the navigation stack
            Shell.Current.GoToAsync("..");
        }
    }
}
