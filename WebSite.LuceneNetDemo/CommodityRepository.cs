using Lucene.Net.Documents;
using WebSite.LuceneNetDemo.DataService;
using WebSite.LuceneNetDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.LuceneNetDemo
{
	public class CommodityRepository
	{
		private string GetSqlString(int pageIndex, int pageSize)
		{
			return string.Format("SELECT top {1} * FROM JD_Commodity_001 WHERE id>{0};", pageSize * Math.Max(0, pageIndex - 1), pageSize);
		}

		/// <summary>
		/// 分页获取商品数据
		/// </summary>
		/// <param name="tableNum"></param>
		/// <param name="pageIndex">从1开始</param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		private List<Commodity> QueryList(int pageIndex, int pageSize)
		{
			string sqlString = GetSqlString(pageIndex, pageSize);
			return SqlHelper.QueryList<Commodity>(sqlString);
		}

		public IList<FieldDataModel> GetFieldModelList()
		{
			#region fieldModelList

			List<FieldDataModel> fieldModelList = new List<FieldDataModel>()
			{
				new FieldDataModel()
				{
					FieldName = "Id",
					PropertyName = "Id",
					FieldType = TypeCode.Int32,
					Index = Field.Index.NOT_ANALYZED,
					Store = Field.Store.NO,
				},
				new FieldDataModel()
				{
					FieldName = "ProductId",
					PropertyName = "ProductId",
					FieldType = TypeCode.Int64,
					Index = Field.Index.NOT_ANALYZED,
					Store = Field.Store.YES,
				},
				new FieldDataModel()
				{
					FieldName = "CategoryId",
					PropertyName = "CategoryId",
					FieldType = TypeCode.Int64,
					Index = Field.Index.NOT_ANALYZED,
					Store = Field.Store.YES,
				},
				new FieldDataModel()
				{
					FieldName = "Title",
					PropertyName = "Title",
					FieldType = TypeCode.String,
					Index = Field.Index.ANALYZED,
					Store = Field.Store.YES,
				},
				new FieldDataModel()
				{
					FieldName = "Price",
					PropertyName = "Price",
					FieldType = TypeCode.Double,
					Index = Field.Index.NOT_ANALYZED,
					Store = Field.Store.YES,
				},
			};

			#endregion

			return fieldModelList;
		}

		public IList<EntryDataModel<Commodity>> GetEntryDataModelList()
		{
			IList<EntryDataModel<Commodity>> entryDataModelList = new List<EntryDataModel<Commodity>>();

			int pageSize = 50000;
			for (int i = 1; i < 3; i++)
			{
				int index = i;
				KeyValuePair<int, int> keyValue = new KeyValuePair<int, int>(index, pageSize);
				Func<List<Commodity>> dataListFunc = () =>
				{
					return QueryList(index, pageSize);
				};
				EntryDataModel<Commodity> entryModel = new EntryDataModel<Commodity>(dataListFunc, GetFieldModelList());
				entryDataModelList.Add(entryModel);
			}

			return entryDataModelList;
		}
	}
}
