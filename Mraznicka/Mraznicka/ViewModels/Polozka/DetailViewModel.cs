using Mraznicka.Models;
using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Plugin.Toast;
using Xamarin.Essentials;
using Xamarin.CommunityToolkit.Extensions;

namespace Mraznicka.ViewModels.Polozka
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class DetailViewModel : BaseViewModel
    {
        public IDataStore<Models.Polozka> DataStore => DependencyService.Get<IDataStore<Models.Polozka>>();
        public IDataStore<Models.Tovar> TovarDataStore => DependencyService.Get<IDataStore<Models.Tovar>>();
        public IDataStore<Models.Zariadenie> ZariadenieDataStore => DependencyService.Get<IDataStore<Models.Zariadenie>>();
        public IDataStore<Models.Pozicia> PoziciaDataStore => DependencyService.Get<IDataStore<Models.Pozicia>>();
        public Models.Polozka Item { get; set; } = new Models.Polozka();
        public ObservableCollection<Mraznicka.Models.Tovar> Tovary { get; set; }
        public ObservableCollection<Mraznicka.Models.Zariadenie> Zariadenia { get; set; }
        public ObservableCollection<Mraznicka.Models.Pozicia> Pozicie { get; set; }


        private Models.Tovar selectedTovar;
        public Models.Tovar SelectedTovar
        {
            get { return selectedTovar; }
            set
            {
                if( value != null)
                {
                    SetProperty(ref selectedTovar, value);
                    Item.Tovar = value.Id;
                }
            }
        }

        private int selectedTovarIndex;
        public int SelectedTovarIndex
        {
            get { return selectedTovarIndex; }
            set
            {
                SetProperty(ref selectedTovarIndex, value);
            }
        }

        private Models.Pozicia selectedPozicia;
        public Models.Pozicia SelectedPozicia
        {
            get { return selectedPozicia; }
            set
            {
                if( value != null)
                {
                    SetProperty(ref selectedPozicia, value);
                    Item.Pozicia = value.Id;
                }
            }
        }

        private int selectedPoziciaIndex;
        public int SelectedPoziciaIndex
        {
            get { return selectedPoziciaIndex; }
            set
            {
                SetProperty(ref selectedPoziciaIndex, value);
            }
        }

        private Models.Zariadenie selectedZariadenie;
        public Models.Zariadenie SelectedZariadenie
        {
            get { return selectedZariadenie; }
            set
            {
                if( value != null )
                {
                    SetProperty(ref selectedZariadenie, value);
                    Item.Zariadenie = value.Id;
                }
            }
        }

        private int selectedZariadenieIndex;
        public int SelectedZariadenieIndex
        {
            get { return selectedZariadenieIndex; }
            set
            {
                SetProperty(ref selectedZariadenieIndex, value);
            }
        }
        public Command ItemPullCommand { get; }

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
            Title = "Polozka";
            contentPage = page;
            SaveCommand = new Command(OnSave, ValidateSave);
            DeleteCommand = new Command(OnDelete);
            ItemPullCommand = new Command(OnItemPull);

            Item.PropertyChanged += (o, e) => SaveCommand.ChangeCanExecute();

            Tovary = new ObservableCollection<Models.Tovar>();
            Pozicie = new ObservableCollection<Models.Pozicia>();
            Zariadenia = new ObservableCollection<Models.Zariadenie>();

            Populate();       

        }

        public void LoadItemId(int itemId)
        {
            try
            {
                var item = DataStore.GetItem(itemId);
                Item.Id = item.Id;
                Item.Popis = item.Popis;
                Item.Expiracia = item.Expiracia;
                Item.Poznamka = item.Poznamka;
                Item.Hmotnost = item.Hmotnost;
                Item.TagID = item.TagID;
                Item.Zariadenie = item.Zariadenie;
                Item.Tovar = item.Tovar;
                Item.Pozicia = item.Pozicia;
                Item.TagID = item.TagID;
                Item.Typ = item.Typ;
                Item.Miestnost = item.Miestnost;
                Item.DatumVytvorenia = item.DatumVytvorenia;

                

                for(int i = 0; i < Tovary.Count; i++)
                {
                    if(Tovary[i].Id == Item.Tovar)
                    { 
                        SelectedTovarIndex = i++;
                        break;
                    }
                }

                for (int i = 0; i < Pozicie.Count; i++)
                {
                    if (Tovary[i].Id == Item.Pozicia)
                    {
                        SelectedPoziciaIndex = i++;
                        break;
                    }
                }

                for (int i = 0; i < Zariadenia.Count; i++)
                {
                    if (Tovary[i].Id == Item.Zariadenie)
                    {
                        SelectedZariadenieIndex = i++;
                        break;
                    }
                }
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

            return !String.IsNullOrWhiteSpace(Item.Popis);
        }

        private async void OnDelete()
        {
            //CrossToastPopUp.Current.ShowToastSuccess(Mraznicka.Resources.AppResources.polozka_ean_vymazana, Plugin.Toast.Abstractions.ToastLength.Long);

            DataStore.DeleteItem(Item.Id);
            DMToast dt = new DMToast();
            dt.ToastSuccess(Mraznicka.Resources.AppResources.polozka_ean_vymazana);
            await Shell.Current.Navigation.PopToRootAsync();
        }

        private void OnSave()
        {
            DataStore.UpdateItem(Item);
            Populate();
            // LoadItemId(Item.Id);
            // This will pop the current page off the navigation stack
            Shell.Current.GoToAsync("..");
        }


        async void OnItemPull()
        {
            switch (Item.Typ)
            {
                case 1:
                    await Shell.Current.GoToAsync($"CompareTagPage?{nameof(ViewModels.Polozka.DetailViewModel.ItemId)}={Item.Id}");
                    break;
                case 2:
                    await Shell.Current.GoToAsync($"CompareEanPage?{nameof(ViewModels.Polozka.DetailViewModel.ItemId)}={Item.Id}");
                    break;
                case 3:
                    // await Shell.Current.GoToAsync($"CompareManualPage?{nameof(ViewModels.Polozka.DetailViewModel.ItemId)}={Item.Id}");
                    bool answer = await contentPage.DisplayAlert(Resources.AppResources.vymazaniezaznamu, Resources.AppResources.naozajchcetevymazatzaznam, Resources.AppResources.ano, Resources.AppResources.nie);
                    if (answer)
                    {
                        DataStore.DeleteItem(Item.Id);
                        DMToast dt = new DMToast();
                        dt.ToastMessage(Mraznicka.Resources.AppResources.polozka_bola_vymazana);
                        await Shell.Current.GoToAsync("..");
                    }
                    break;
                default:
                    await Shell.Current.GoToAsync($"VyberManualPage?{nameof(ViewModels.Polozka.DetailViewModel.ItemId)}={Item.Id}");
                    break;
            }
        }

        public void Populate()
        {
            try
            {

                Tovary.Clear();
                Zariadenia.Clear();
                Pozicie.Clear();

                foreach (var item in TovarDataStore.GetItems(true))
                {
                    Tovary.Add(item);
                }

                foreach (var item in ZariadenieDataStore.GetItems(true))
                {
                    Zariadenia.Add(item);
                }

                foreach (var item in PoziciaDataStore.GetItems(true))
                {
                    Pozicie.Add(item);
                }
            }
            catch (Exception ex)
            { 
            }

        }
    }
}
