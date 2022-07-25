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
	public class MiestnostDataStore : IDataStore<Miestnost>
	{
		SQLiteConnection db;


		readonly List<Miestnost> items = new List<Miestnost>();

		public MiestnostDataStore()
		{
			//string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp.txt");

			var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

			db = new SQLiteConnection(databasePath);
			db.CreateTable<Miestnost>();
		}

		public bool AddItem(Miestnost item)
		{
			db.Insert(item);
			return true;
		}

		public bool UpdateItem(Miestnost item)
		{
			db.Update(item);
			return true;
		}

		public bool DeleteItem(int id)
		{
			db.Delete<Miestnost>(id);
			return true;
		}

		public Miestnost GetItem(int id)
		{
			return db.Table<Miestnost>().FirstOrDefault(c => c.Id == id);
		}

		public IEnumerable<Miestnost> GetItems(bool forceRefresh = false)
		{
			//return  Task.FromResult(items);
			return db.Table<Miestnost>().ToList();
		}
	}
}
