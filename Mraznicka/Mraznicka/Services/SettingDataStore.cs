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
	public class SettingDataStore : IDataStore<Setting>
	{
		SQLiteConnection db;


		readonly List<Setting> items = new List<Setting>();

		public SettingDataStore()
		{
			var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

			db = new SQLiteConnection(databasePath);
			db.CreateTable<Setting>();
		}

		public bool AddItem(Setting item)
		{
			var id = db.Insert(item);
			return true;
		}

		public bool UpdateItem(Setting item)
		{
			db.Update(item);
			return true;
		}

		public bool DeleteItem(int id)
		{
			db.Delete<Setting>(id);
			return true;
		}

		public Setting GetItem(int id)
		{
			return db.Table<Setting>().FirstOrDefault(c => c.Id == id);
		}

		public IEnumerable<Setting> GetItems(bool forceRefresh = false)
		{
			//return  Task.FromResult(items);
			return db.Table<Setting>().ToList();
		}
	}
}
