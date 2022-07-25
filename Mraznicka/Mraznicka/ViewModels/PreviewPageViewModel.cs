using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mraznicka.ViewModels
{
    public class PreviewPageViewModel : BaseViewModel
    {
        public IDataStore<Models.Polozka> DataStore => DependencyService.Get<IDataStore<Models.Polozka>>();
        public IDataStore<Models.Tovar> TovarDataStore => DependencyService.Get<IDataStore<Models.Tovar>>();
        public IDataStore<Models.Zariadenie> ZariadenieDataStore => DependencyService.Get<IDataStore<Models.Zariadenie>>();

        private Models.Polozka _selectedItem;

        public ObservableCollection<Models.Polozka> Items { get; } = new ObservableCollection<Models.Polozka>();
        public ObservableCollection<Models.Tovar> Tovary { get; } = new ObservableCollection<Models.Tovar>();
        public ObservableCollection<Models.Zariadenie> Zariadenia { get; } = new ObservableCollection<Models.Zariadenie>();
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }

        //public Command ChangeOptionCommand { get; }

        public int SelectedTovar { get; set; }
        public int SelectedZariadenie { get; set; }

        private string filter;

        public string Filter
        {
            get
            {
                return filter;
            }
            set
            {
                filter = value;
                ExecuteLoadItemsCommand();
            }
        }

        public Command<Models.Tovar> ItemFilter { get; }
        public Command<Models.Polozka> ItemTapped { get; }
        public Command<Models.Polozka> ItemPull { get; }

        public PreviewPageViewModel()
        {
            Title = "Číselník Poloziek";
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
            ItemTapped = new Command<Models.Polozka>(OnItemSelected);
            ItemPull = new Command<Models.Polozka>(OnItemPull);
            ItemFilter = new Command<Models.Tovar>(OnFilter);

            //ChangeOptionCommand = new Command<int>(ExecuteChangeOptionCommand);

            AddItemCommand = new Command(OnAddItem);

            Tovary.Clear();
            Tovary.Add(new Models.Tovar() { Id = -1, Nazov = "Vyberte tovar - všetko" });
            foreach (var item in TovarDataStore.GetItems(true))
            {
                Tovary.Add(item);
            }

            Zariadenia.Clear();
            Zariadenia.Add(new Models.Zariadenie() { Id = -1, Nazov = "Vyberte zariadenie - všetky" });
            foreach (var item in ZariadenieDataStore.GetItems(true))
            {
                Zariadenia.Add(item);
            }

            

        }

        //private void ExecuteChangeOptionCommand(int id)
        //{
        //    ExecuteLoadItemsCommand();
        //}

        private void ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                IQueryable<Models.Polozka> items = DataStore.GetItems(true).OrderBy(o => o.Expiracia).AsQueryable();
               
                if (SelectedTovar > 0)
                    items = items.Where(o => o.Tovar == SelectedTovar);
                
                if (SelectedZariadenie > 0)
                    items = items.Where(o => o.Zariadenie == SelectedZariadenie);

                if (!String.IsNullOrEmpty(Filter))
                    items = items.Where(o => o.Popis.ToLowerInvariant().Contains(Filter.ToLowerInvariant()));

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
        }



        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Models.Polozka SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync("PolozkaCreatePage");
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

            switch (item.Typ)
            {
                case 1:
                    await Shell.Current.GoToAsync($"CompareTagPage?{nameof(ViewModels.Polozka.DetailViewModel.ItemId)}={item.Id}");
                    break;
                case 2:
                    await Shell.Current.GoToAsync($"CompareEanPage?{nameof(ViewModels.Polozka.DetailViewModel.ItemId)}={item.Id}");
                    break;
                case 3:
                    await Shell.Current.GoToAsync($"CompareManualPage?{nameof(ViewModels.Polozka.DetailViewModel.ItemId)}={item.Id}");
                    break;
                default:
                    await Shell.Current.GoToAsync($"VyberManualPage?{nameof(ViewModels.Polozka.DetailViewModel.ItemId)}={item.Id}");
                    break;
            }
        }

        private void OnFilter(Models.Tovar tovar)
        {
            if (tovar == null)
                return;


            Items.Clear();
            var items = DataStore.GetItems(true).OrderBy(o => o.Expiracia);
            foreach (var item in items.Where(o => o.Tovar == tovar.Id))
            {
                Items.Add(item);
            }
        }
    }
}
