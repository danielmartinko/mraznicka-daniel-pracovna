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
	public class PoslednePouziteDataStore : IDataStore<PoslednePouzite>
	{
		SQLiteConnection db;


		public PoslednePouziteDataStore()
		{
			var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

			db = new SQLiteConnection(databasePath);
			db.CreateTable<PoslednePouzite>();
		}

		public bool AddItem(PoslednePouzite item)
		{

			db.Insert(item);
			return true;
		}

		public bool DeleteItem(int id)
		{
			throw new NotImplementedException();
		}

		public PoslednePouzite GetItem(int id)
		{
			return db.Table<PoslednePouzite>().FirstOrDefault(c => c.Id == id);
		}

		public IEnumerable<PoslednePouzite> GetItems(bool forceRefresh = false)
		{
			return db.Table<PoslednePouzite>().ToList();
		}

		public bool UpdateItem(PoslednePouzite item)
		{
			//var items = GetItems(true).Configure(true).Geter().GetResult();
			//if (items.Count() > 0)
			//{
			//	string str_sql = string.Format("update PoslednePouzite set Id=1, miestnost='{0}', zariadenie='{1}', pozicia='{2}', tovar='{3}'",
			//		item.Miestnost, item.Zariadenie, item.Pozicia, item.Tovar);
			//	db.Execute(str_sql);
			//	return true;

			//}
			//else
			//{
			//	AddItem(item);
			//	return true;
			//}

			db.Update(item);
			return true;

		}
	}
}
