using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Essentials;
using System.ComponentModel;

namespace Mraznicka.Models
{
    public class Zariadenie : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        private string nazov;

        [Unique]
        public string Nazov
        {
            get => nazov;
            set
            {
                nazov = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nazov)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
