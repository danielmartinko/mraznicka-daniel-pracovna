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
	public class TovarDataStore : IDataStore<Tovar>
	{
		SQLiteConnection db;


		readonly List<Tovar> items = new List<Tovar>();

		public TovarDataStore()
		{
			var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

			db = new SQLiteConnection(databasePath);
			db.CreateTable<Tovar>();
		}

		public bool AddItem(Tovar item)
		{
			var id = db.Insert(item);
			return true;
		}

		public bool UpdateItem(Tovar item)
		{
			db.Update(item);
			return true;
		}

		public bool DeleteItem(int id)
		{
			db.Delete<Tovar>(id);
			return true;
		}

		public Tovar GetItem(int id)
		{
			return db.Table<Tovar>().FirstOrDefault(c => c.Id == id);
		}

		public IEnumerable<Tovar> GetItems(bool forceRefresh = false)
		{
			//return  Task.FromResult(items);
			return db.Table<Tovar>().ToList();
		}
	}
}
