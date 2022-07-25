using Mraznicka.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Mraznicka.Services
{
	public class PoziciaDataStore : IDataStore<Pozicia>
	{
		SQLiteConnection db;


		readonly List<Pozicia> items = new List<Pozicia>();

		public PoziciaDataStore()
		{
			var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");
			
			db = new SQLiteConnection(databasePath);
			db.CreateTable<Pozicia>();
		}

		public  bool AddItem(Pozicia item)
		{
			var id =  db.Insert(item);
			return  true;
		}

		public  bool UpdateItem(Pozicia item)
		{
			 db.Update(item);
			return  true;
		}

		public  bool DeleteItem(int id)
		{
			 db.Delete<Pozicia>(id);
			return  true;
		}

		public  Pozicia GetItem(int id)
		{
			return  db.Table<Pozicia>().FirstOrDefault(c => c.Id == id);
		}

		public  IEnumerable<Pozicia> GetItems(bool forceRefresh = false)
		{
			//return  Task.FromResult(items);
			return  db.Table<Pozicia>().ToList();
		}
	}
}
