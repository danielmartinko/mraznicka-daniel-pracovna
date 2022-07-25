using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Essentials;
using System.ComponentModel;

namespace Mraznicka.Models
{
    public class Tovar : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        private string nazov;
        private int expiracia;

        public string Nazov
        {
            get { return nazov; }
            set { SetAndNotify(ref nazov, value, () => Nazov); }
        }

        public int Expiracia
        {
            get { return expiracia; }
            set { SetAndNotify(ref expiracia, value, () => Expiracia); }
        }
    }
}
