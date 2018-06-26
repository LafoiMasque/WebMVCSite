using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using WebSite.Core.LuceneNet.Interface;
using WebSite.Core.LuceneNet.Model;
using WebSite.Core.LuceneNet.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.Core.LuceneNet.Service
{
	public class LuceneQuery<T> : ILuceneQuery<T> where T : class, new()
	{
		#region Identity

		//private CustomLogger m_logger = new CustomLogger(typeof(LuceneQuery<T>));

		#endregion Identity

		#region QueryIndex

		/// <summary>
		/// 获取商品信息数据
		/// </summary>
		/// <param name="queryString"></param>
		/// <param name="fieldName"></param>
		/// <param name="fieldModelList"></param>
		/// <param name="listCount"></param>
		/// <returns></returns>
		public List<T> QueryIndex(string queryString, string fieldName, IEnumerable<FieldDataModel> fieldModelList, int listCount = 1000)
		{
			List<T> modelList = null;
			using (Analyzer analyzer = new PanGuAnalyzer())
			{
				//--------------------------------------这里配置搜索条件
				QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, fieldName, analyzer);
				Query query = parser.Parse(queryString);
				modelList = QueryIndex(query, fieldModelList, listCount);
			}
			return modelList;
		}

		/// <summary>
		/// 获取商品信息数据
		/// </summary>
		/// <param name="query"></param>
		/// <param name="fieldModelList"></param>
		/// <param name="listCount"></param>
		/// <returns></returns>
		public List<T> QueryIndex(Query query, IEnumerable<FieldDataModel> fieldModelList, int listCount)
		{
			List<T> modelList = null;
			IndexSearcher searcher = null;
			try
			{
				modelList = new List<T>();
				Directory dir = FSDirectory.Open(StaticConstant.IndexPath);
				searcher = new IndexSearcher(dir);
				TopDocs docs = searcher.Search(query, listCount);
				foreach (ScoreDoc sd in docs.ScoreDocs)
				{
					Document doc = searcher.Doc(sd.Doc);
					modelList.Add(DocumentToTInfo(doc, fieldModelList));
				}
			}
			finally
			{
				searcher?.Dispose();
			}
			return modelList;
		}

		/// <summary>
		/// 分页获取商品信息数据
		/// </summary>
		/// <param name="queryString"></param>
		/// <param name="fieldName"></param>
		/// <param name="pageIndex">第一页为1</param>
		/// <param name="pageSize"></param>
		/// <param name="totalCount"></param>
		/// <param name="filter"></param>
		/// <param name="sort"></param>
		/// <param name="fieldModelList"></param>
		/// <returns></returns>
		public List<T> QueryIndexPage(string queryString, string fieldName, int pageIndex, int pageSize, out int totalCount, Filter filter, Sort sort, IEnumerable<FieldDataModel> fieldModelList)
		{
			List<T> modelList = null;
			using (Analyzer analyzer = new PanGuAnalyzer())
			{
				//--------------------------------------这里配置搜索条件
				QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, fieldName, analyzer);
				Query query = parser.Parse(queryString);
				modelList = QueryIndexPage(query, pageIndex, pageSize, out totalCount, filter, sort, fieldModelList);
			}
			return modelList;
		}

		/// <summary>
		/// 分页获取商品信息数据
		/// </summary>
		/// <param name="query"></param>
		/// <param name="pageIndex">第一页为1</param>
		/// <param name="pageSize"></param>
		/// <param name="totalCount"></param>
		/// <param name="filter"></param>
		/// <param name="sort"></param>
		/// <param name="fieldModelList"></param>
		/// <returns></returns>
		public List<T> QueryIndexPage(Query query, int pageIndex, int pageSize, out int totalCount, Filter filter, Sort sort, IEnumerable<FieldDataModel> fieldModelList)
		{
			totalCount = 0;
			List<T> modelList = null;
			IndexSearcher searcher = null;
			try
			{
				modelList = new List<T>();
				FSDirectory dir = FSDirectory.Open(StaticConstant.IndexPath);
				searcher = new IndexSearcher(dir);
				pageIndex = Math.Max(1, pageIndex);//索引从1开始
				int startIndex = (pageIndex - 1) * pageSize;
				int endIndex = pageIndex * pageSize;

				#region Filter

				//NumericRangeFilter<float> numPriceFilter = null;
				//if (!string.IsNullOrWhiteSpace(priceFilter))
				//{
				//	bool isContainStart = priceFilter.StartsWith("[");
				//	bool isContainEnd = priceFilter.EndsWith("]");
				//	string[] floatArray = priceFilter.Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "").Split(',');
				//	float start = 0;
				//	float end = 0;
				//	if (!float.TryParse(floatArray[0], out start) || !float.TryParse(floatArray[1], out end))
				//	{
				//		throw new Exception("Wrong priceFilter");
				//	}
				//	numPriceFilter = NumericRangeFilter.NewFloatRange("price", start, end, isContainStart, isContainEnd);
				//}

				#endregion

				#region Sort

				//Sort sort = new Sort();
				//if (!string.IsNullOrWhiteSpace(priceOrderBy))
				//{
				//	SortField sortField = new SortField("price", SortField.FLOAT, priceOrderBy.EndsWith("asc", StringComparison.CurrentCultureIgnoreCase));
				//	sort.SetSort(sortField);
				//}

				#endregion

				TopDocs docs = searcher.Search(query, filter, 10000, sort);

				totalCount = docs.TotalHits;
				//PrintScores(docs, startIndex, endIndex, searcher);
				for (int i = startIndex; i < endIndex && i < totalCount; i++)
				{
					Document doc = searcher.Doc(docs.ScoreDocs[i].Doc);
					modelList.Add(DocumentToTInfo(doc, fieldModelList));
				}
			}
			finally
			{
				searcher?.Dispose();
			}

			return modelList;
		}

		#endregion QueryIndex

		#region private

		//private void PrintScores(TopDocs docs, int startIndex, int endIndex, MultiSearcher searcher)
		//{
		//	ScoreDoc[] scoreDocs = docs.ScoreDocs;
		//	for (int i = startIndex; i < endIndex && i < scoreDocs.Count(); i++)
		//	{
		//		int docId = scoreDocs[i].Doc;
		//		Document doc = searcher.Doc(docId);
		//		//m_logger.Info(string.Format("{0}的分值为{1}", doc.Get("productid"), scoreDocs[i].Score));
		//	}
		//}

		private T DocumentToTInfo(Document doc, IEnumerable<FieldDataModel> fieldModelList)
		{
			Type modelType = typeof(T);
			var modelInstance = Activator.CreateInstance(modelType);
			PropertyInfo[] properties = modelType.GetProperties();
			foreach (var prop in properties)
			{
				if (prop.CanWrite)
				{
					bool hasModel = fieldModelList.Any(o => o.FieldName == prop.Name);
					if (hasModel)
					{
						string fieldValue = doc.Get(prop.Name);
						if (fieldValue != null)
							prop.SetValue(modelInstance, Convert.ChangeType(fieldValue, prop.PropertyType));
					}
				}
			}
			return modelInstance as T;
		}

		#endregion private
	}
}
