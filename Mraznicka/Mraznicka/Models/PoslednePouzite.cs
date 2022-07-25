using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Essentials;
using System.ComponentModel;

namespace Mraznicka.Models
{
    public class PoslednePouzite : ObservableObject
    {
        [PrimaryKey, AutoIncrement]

        public int Id { get; set; }
        private int miestnost = 0;
        private int zariadenie = 0;
        private int pozicia = 0;
        private int tovar = 0;

        public int Miestnost
        {
            get { return miestnost; }
            set { SetAndNotify(ref miestnost, value, () => Miestnost); }
        }
        public int Zariadenie
        {
            get { return zariadenie; }
            set { SetAndNotify(ref zariadenie, value, () => Zariadenie); }
        }
        public int Pozicia
        {
            get { return pozicia; }
            set { SetAndNotify(ref pozicia, value, () => Pozicia); }
        }
        public int Tovar
        {
            get { return tovar; }
            set { SetAndNotify(ref tovar, value, () => Tovar); }
        }

    }
}
