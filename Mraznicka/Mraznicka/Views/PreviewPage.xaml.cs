using Mraznicka.ViewModels;
using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PreviewPage : ContentPage
	{
		ViewModels.PreviewPageViewModel _viewModel;

		public PreviewPage()
		{
			InitializeComponent();

			BindingContext = _viewModel = new ViewModels.PreviewPageViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_viewModel.OnAppearing();

			pckTovar.SelectedIndex = 0;
			pckZariadenie.SelectedIndex = 0;
            try
            {
                CrossNFC.Current.StartListening();
                CrossNFC.Current.StartPublishing(true);
            }
            catch (Exception ex)
            {
            }

        }


        private void Handle_SelectedTovarIndexChanged(object sender, System.EventArgs e)
		{
			//Models.Tovar selectedOption = (Models.Tovar)(sender as Picker).SelectedItem;
			_viewModel.SelectedTovar = ((Models.Tovar)(sender as Picker).SelectedItem).Id;
			_viewModel.LoadItemsCommand.Execute(null);
		}

		private void Handle_SelectedZariadenieIndexChanged(object sender, System.EventArgs e)
		{
			_viewModel.SelectedZariadenie = ((Models.Zariadenie)(sender as Picker).SelectedItem).Id;
			_viewModel.LoadItemsCommand.Execute(null);
		}

        private void Entry_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
    }
}