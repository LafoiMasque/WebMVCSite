using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.Core.LuceneNet.Model
{
	public class EntryDataModel<T> where T : class, new()
	{
		public Func<IEnumerable<T>> DataListFunc { get;private set; }
		public IEnumerable<T> DataList { get; private set; }
		public IEnumerable<FieldDataModel> FieldModelList { get; private set; }

		public EntryDataModel(Func<IEnumerable<T>> dataListFunc, IEnumerable<FieldDataModel> fieldModelList)
		{
			DataListFunc = dataListFunc;
			FieldModelList = fieldModelList;
		}

		public EntryDataModel(IEnumerable<T> dataList, IEnumerable<FieldDataModel> fieldModelList)
		{
			DataList = dataList;
			FieldModelList = fieldModelList;
		}
	}
}
