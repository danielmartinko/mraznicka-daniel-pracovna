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
            get
            {
                switch (Id)
                {
                    case 1:
                        return Resources.AppResources.bravcove;
                    case 2:
                        return Resources.AppResources.kuracie;
                    case 3:
                        return Resources.AppResources.hovadzie;
                    case 4:
                        return Resources.AppResources.hydina;
                    case 5:
                        return Resources.AppResources.zverina;
                    case 6:
                        return Resources.AppResources.masovevyrobky;
                    case 7:
                        return Resources.AppResources.hotovejedlo;
                    case 8:
                        return Resources.AppResources.polotovar;
                    case 9:
                        return Resources.AppResources.zelenina;
                    case 10:
                        return Resources.AppResources.ovocie;
                    case 11:
                        return Resources.AppResources.bylinky;
                    case 12:
                        return Resources.AppResources.pecivo;
                    case 13:
                        return Resources.AppResources.cukrovinky;
                    case 14:
                        return Resources.AppResources.mliecnevyrobky;
                    case 15:
                        return Resources.AppResources.ryby;
                    case 16:
                        return Resources.AppResources.ine;
                    default:
                        return nazov;
                }
            }
            set { SetAndNotify(ref nazov, value, () => Nazov); }
        }

        public int Expiracia
        {
            get { return expiracia; }
            set { SetAndNotify(ref expiracia, value, () => Expiracia); }
        }
    }
}
