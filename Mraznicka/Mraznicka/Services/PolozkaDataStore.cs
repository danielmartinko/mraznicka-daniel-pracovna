using Mraznicka.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Mraznicka.Services
{
	public class PolozkaDataStore : IDataStore<Polozka>
	{
		SQLiteConnection db;


		readonly List<Polozka> items = new List<Polozka>();

		public PolozkaDataStore()
		{
			var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");
			
			db = new SQLiteConnection(databasePath);
			db.CreateTable<Polozka>();
		}

		public  bool AddItem(Polozka item)
		{
			Debug.WriteLine($"Add Item Id:{item.Popis}");
			var id =  db.Insert(item);
			return true;
		}

		public  bool UpdateItem(Polozka item)
		{
			Debug.WriteLine($"Update Item Id:{item.Popis}");
			db.Update(item);
			return true;
		}

		public  bool DeleteItem(int id)
		{
			Debug.WriteLine(id);
			db.Delete<Polozka>(id);
			return true;
		}

		public Polozka GetItem(int id)
		{
			Debug.WriteLine($"Get Item Id:{id}");
			return  db.Table<Polozka>().FirstOrDefault(c => c.Id == id);
		}

		public IEnumerable<Polozka> GetItems(bool forceRefresh = false)
		{
			//return  Task.FromResult(items);
			var list = db.Table<Polozka>().ToList();
			Debug.WriteLine(list);
			return list;
		}
	}
}
