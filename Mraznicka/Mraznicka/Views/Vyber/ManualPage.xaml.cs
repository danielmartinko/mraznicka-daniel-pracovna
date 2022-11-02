using Mraznicka.ViewModels;
using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Views.Vyber
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManualPage : ContentPage
	{
		private ViewModels.Vyber.ManualPageViewModel ctx { get; set; }
		public ManualPage()
		{
			InitializeComponent();
			this.BindingContext = this.ctx = new ViewModels.Vyber.ManualPageViewModel(this);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			this.ctx.ExecuteLoadItemsCommand();

        }


        private void Handle_SelectedZariadenieIndexChanged(object sender, System.EventArgs e)
		{
			this.ctx.SelectedZariadenie = ((Models.Zariadenie)(sender as Picker).SelectedItem).Id;
			this.ctx.LoadItemsCommand.Execute(null);
		}
	}
}