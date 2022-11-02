using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.Toast;
using Xamarin.CommunityToolkit.Extensions;

namespace Mraznicka.ViewModels.Vyber
{
    public class EANCodePageViewModel : BaseViewModel
    {
        private Button btnSubmit;
        private Label lNajdenaPolozka;
        public Command VyberCommand { get; }

        public Command<Models.Polozka> ItemTapped { get; }
        public Command<Models.Polozka> ItemPull { get; }

        public ObservableCollection<Models.Polozka> Items { get; }

        public IDataStore<Models.Polozka> DataStore => DependencyService.Get<IDataStore<Models.Polozka>>();

        public ContentPage contentPage { get; set; }

        public EANCodePageViewModel(ContentPage page)
        {
            contentPage = page;
            Items = new ObservableCollection<Models.Polozka>();
            ItemTapped = new Command<Models.Polozka>(OnItemSelected);
            ItemPull = new Command<Models.Polozka>(OnItemPull);
            VyberCommand = new Command(OnVyber, ValidateVyber);
            btnSubmit = contentPage.FindByName<Button>("btnSubmit");
            lNajdenaPolozka = contentPage.FindByName<Label>("najdena_polozka");
            btnSubmit.IsVisible = false;
            lNajdenaPolozka.IsVisible = false;
        }

        private bool ValidateVyber()
        {
            return true;
        }


        public void ExecuteLoadItemsCommand(string ean)
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = DataStore.GetItems(true).Where(o => o.TagID == ean);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

            lNajdenaPolozka.IsVisible = true;
            if (Items.Count == 0)
            {
                //CrossToastPopUp.Current.ShowToastError(Resources.AppResources.eansanepouziva);
                DMToast dt = new DMToast();
                dt.ToastError(Mraznicka.Resources.AppResources.eansanepouziva);

                /*
                contentPage.DisplayAlert(Resources.AppResources.oznam, Resources.AppResources.eansanepouziva, Resources.AppResources.ok).ContinueWith(t =>
                {
                    Shell.Current.Navigation.PopToRootAsync();
                });
                */
            }
            if (Items.Count == 1)
            {
                // btnSubmit.IsVisible = true;
            }
            else
            {
                btnSubmit.IsVisible = false;
            }
            IsBusy = false;
        }

        private async void OnPreviewClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            await Shell.Current.GoToAsync("PreviewPage");
        }


        async void OnItemPull(Models.Polozka item)
        {
            if (item == null)
                return;

            bool answer = await contentPage.DisplayAlert(Resources.AppResources.vymazaniezaznamu, Resources.AppResources.naozajchcetevymazatzaznam, Resources.AppResources.ano, Resources.AppResources.nie);
            if (answer)
            {
                //CrossToastPopUp.Current.ShowToastSuccess(Mraznicka.Resources.AppResources.polozka_ean_vymazana, Plugin.Toast.Abstractions.ToastLength.Long);
                DataStore.DeleteItem(Items[0].Id);
                DMToast dt = new DMToast();
                dt.ToastSuccess(Mraznicka.Resources.AppResources.polozka_ean_vymazana);
                await Shell.Current.GoToAsync("..");

                // DataStore.DeleteItem(item.Id);
                // ExecuteLoadItemsCommand(item.TagID);
                // Shell.Current.GoToAsync("..");
            }

        }

        async void OnItemSelected(Models.Polozka item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"PolozkaDetailPage?{nameof(ViewModels.Polozka.DetailViewModel.ItemId)}={item.Id}");
        }
        private async void OnVyber()
        {
            if (Items[0] != null)
            {
                bool answer = await contentPage.DisplayAlert(Resources.AppResources.vymazaniezaznamu, Resources.AppResources.naozajchcetevymazatzaznam, Resources.AppResources.ano, Resources.AppResources.nie);
                if (answer)
                {
                    //CrossToastPopUp.Current.ShowToastSuccess(Mraznicka.Resources.AppResources.polozka_ean_vymazana, Plugin.Toast.Abstractions.ToastLength.Long);
                    DataStore.DeleteItem(Items[0].Id);
                    DMToast dt = new DMToast();
                    dt.ToastSuccess(Mraznicka.Resources.AppResources.polozka_ean_vymazana);
                    await Shell.Current.GoToAsync("..");
                }
            }
            else
            {
                //CrossToastPopUp.Current.ShowToastSuccess(Resources.AppResources.eansanepouziva, Plugin.Toast.Abstractions.ToastLength.Long);
                DMToast dt = new DMToast();
                dt.ToastSuccess(Mraznicka.Resources.AppResources.eansanepouziva);

            }
            // This will pop the current page off the navigation stack
        }
    }
}
