using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Essentials;
using System.ComponentModel;
using System.Diagnostics;

namespace Mraznicka.Models
{

    public class Polozka : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        private int miestnost;
        private int zariadenie;
        private int pozicia;
        private int tovar;
        private int typ;
        private string tagID = "";
        private string tagIDPrecitany = "";
        private string popis;
        private DateTime expiracia;
        private DateTime datumvytvorenia;
        private decimal hmotnost;
        private string poznamka;

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
        
        //[Unique]
        public string TagID
        {
            get { return tagID; }
            set { SetAndNotify(ref tagID, value, () => TagID); }
        }

        public string TagIDPrecitany
        {
            get { return tagIDPrecitany; }
            set { SetAndNotify(ref tagIDPrecitany, value, () => TagIDPrecitany); }
        }

        public string Popis
        {
            get { return popis; }
            set {
                Debug.WriteLine($"Set Popis :{ value }");
                SetAndNotify(ref popis, value, () => Popis); 
            }
        }

        public DateTime Expiracia
        {
            get { return expiracia; }
            set { SetAndNotify(ref expiracia, value, () => Expiracia); }
        }
        public decimal Hmotnost
        {
            get { return hmotnost; }
            set { SetAndNotify(ref hmotnost, value, () => Hmotnost); }
        }

        public string Poznamka
        {
            get { return poznamka; }
            set { SetAndNotify(ref poznamka, value, () => Poznamka); }
        }

        public int Typ
        {
            get { return typ; }
            set { SetAndNotify(ref typ, value, () => Typ); }
        }

        public DateTime DatumVytvorenia
        {
            get { return datumvytvorenia; }
            set { SetAndNotify(ref datumvytvorenia, value, () => DatumVytvorenia); }
        }
    }
}
