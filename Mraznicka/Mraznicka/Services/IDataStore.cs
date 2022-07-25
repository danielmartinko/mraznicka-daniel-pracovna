using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mraznicka.Services
{
	public interface IDataStore<T>
	{
		bool AddItem(T item);
		bool UpdateItem(T item);
		bool DeleteItem(int id);
		T GetItem(int id);
		IEnumerable<T> GetItems(bool forceRefresh = false);
	}
}
