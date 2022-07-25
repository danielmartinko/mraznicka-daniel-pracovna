using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Essentials;
using System.ComponentModel;
using System.Diagnostics;


namespace Mraznicka.Models
{
    public class Setting : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        private string key;
        private string val;

        public string Key
        {
            get { return key; }
            set { SetAndNotify(ref key, value, () => Key); }
        }
        public string Val
        {
            get { return val; }
            set { SetAndNotify(ref val, value, () => Val); }
        }
    }
}
