using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Mraznicka.Models
{
	public class SelectedData : ObservableObject
	{
		private Tovar tovar;
		private Miestnost miestnost;
		private Pozicia pozicia;
		private Zariadenie zariadenie;

		public Tovar Tovar
		{
			get { return tovar; }
			set { SetAndNotify(ref tovar, value, () => Tovar); }
		}
		public Miestnost Miestnost
		{
			get { return miestnost; }
			set { SetAndNotify(ref miestnost, value, () => Miestnost); }
		}
		public Pozicia Pozicia
		{
			get { return pozicia; }
			set { SetAndNotify(ref pozicia, value, () => Pozicia); }
		}

		public Zariadenie Zariadenie
		{
			get { return zariadenie; }
			set { SetAndNotify(ref zariadenie, value, () => Zariadenie); }
		}

	}
}
