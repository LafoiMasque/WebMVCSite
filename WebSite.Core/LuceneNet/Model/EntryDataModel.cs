using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.Core.LuceneNet.Model
{
	public class EntryDataModel<T> where T : class, new()
	{
		public Func<List<T>> DataListFunc { get;private set; }
		public List<T> DataList { get; private set; }
		public IList<FieldDataModel> FieldModelList { get; private set; }

		public EntryDataModel(Func<List<T>> dataListFunc, IList<FieldDataModel> fieldModelList)
		{
			DataListFunc = dataListFunc;
			FieldModelList = fieldModelList;
		}

		public EntryDataModel(List<T> dataList, IList<FieldDataModel> fieldModelList)
		{
			DataList = dataList;
			FieldModelList = fieldModelList;
		}
	}
}
