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
	public class ZariadenieDataStore : IDataStore<Zariadenie>
	{
		SQLiteConnection db;


		readonly List<Zariadenie> items = new List<Zariadenie>();

		public ZariadenieDataStore()
		{
			var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

			db = new SQLiteConnection(databasePath);
			db.CreateTable<Zariadenie>();
		}

		public bool AddItem(Zariadenie item)
		{
			var id = db.Insert(item);
			return true;
		}

		public bool UpdateItem(Zariadenie item)
		{
			db.Update(item);
			return true;
		}

		public bool DeleteItem(int id)
		{
			db.Delete<Zariadenie>(id);
			return true;
		}

		public Zariadenie GetItem(int id)
		{
			return db.Table<Zariadenie>().FirstOrDefault(c => c.Id == id);
		}

		public IEnumerable<Zariadenie> GetItems(bool forceRefresh = false)
		{
			//return  Task.FromResult(items);
			return db.Table<Zariadenie>().ToList();
		}
	}
}
