using Mraznicka.ViewModels;
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
	public partial class PullPage : ContentPage
	{
		public PullPage()
		{
			InitializeComponent();
			this.BindingContext = new PullPageViewModel();
		}
	}
}