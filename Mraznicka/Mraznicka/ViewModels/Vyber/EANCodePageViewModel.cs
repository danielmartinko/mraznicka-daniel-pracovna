using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Mraznicka.ViewModels.Vyber
{
    public class EANCodePageViewModel : BaseViewModel
    {
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
        }


        public async void ExecuteLoadItemsCommand(string ean)
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

            if (Items.Count == 0)
                await contentPage.DisplayAlert(Resources.AppResources.oznam, Resources.AppResources.eansanepouziva, Resources.AppResources.ok);
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
                DataStore.DeleteItem(item.Id);
                Shell.Current.GoToAsync("..");
            }

        }

        async void OnItemSelected(Models.Polozka item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"PolozkaDetailPage?{nameof(ViewModels.Polozka.DetailViewModel.ItemId)}={item.Id}");
        }
    }
}
