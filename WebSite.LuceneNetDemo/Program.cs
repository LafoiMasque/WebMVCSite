using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WebSite.LuceneNetDemo.Interface;
using WebSite.LuceneNetDemo.Service;

namespace WebSite.LuceneNetDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			do
			{
				Console.WriteLine("======================================================");
				Console.WriteLine("分词列名：");
				string fieldName = Console.ReadLine();
				Console.WriteLine("查询的词：");
				string keyword = Console.ReadLine();
				Console.WriteLine();

				CommodityRepository repository = new CommodityRepository();
				//IList<EntryDataModel<Commodity>> entryDataModelList = repository.GetEntryDataModelList();
				//IndexBuilderThread.BuildIndexThread<Commodity>(entryDataModelList);

				ILuceneQuery<Commodity> luceneQuery = new LuceneQuery<Commodity>();
				List<Commodity> modelList = luceneQuery.QueryIndex(keyword, fieldName, repository.GetFieldModelList());
				ShowDataList(modelList);
			} while (true);
		}

		private static void ShowDataList<T>(IEnumerable<T> modelList) where T : class
		{
			Type type = typeof(T);
			PropertyInfo[] propertyInfos = type.GetProperties();
			foreach (var model in modelList)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (var property in propertyInfos)
				{
					var propertyValue = property.GetValue(model);
					if (propertyValue != null)
						stringBuilder.AppendFormat("{0}={1} ", property.Name, propertyValue.ToString());
				}
				Console.WriteLine(stringBuilder.ToString());
			}
		}
	}
}
