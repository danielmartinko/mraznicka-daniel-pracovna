using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SQLite;

namespace Mraznicka.Models
{
    public class Umiestnenie : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        private string nazov;

        public string Nazov
        {
            get => nazov;
            set
            {
                nazov = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nazov)));
            }
        }
        public DateTime Date { get; set; }
        public bool IsDone { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
