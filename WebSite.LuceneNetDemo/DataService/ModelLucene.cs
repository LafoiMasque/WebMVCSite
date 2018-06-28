using Lucene.Net.Search;
using WebSite.LuceneNetDemo.Interface;
using WebSite.LuceneNetDemo.Model;
using WebSite.LuceneNetDemo.Service;
using WebSite.LuceneNetDemo.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.LuceneNetDemo.DataService
{
	public class ModelLucene
	{
		private static CustomLogger m_logger = new CustomLogger(typeof(ModelLucene));

		#region QueryModel

		/// <summary>
		/// 用lucene进行商品查询
		/// </summary>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="totalCount"></param>
		/// <param name="keyword"></param>
		/// <param name="fieldName"></param>
		/// <param name="categoryIdList"></param>
		/// <param name="priceFilter">[13,50]  13,50且包含13到50   {13,50}  13,50且不包含13到50</param>
		/// <param name="priceOrderBy">price desc   price asc</param>
		/// <param name="fieldModelList"></param>
		/// <returns></returns>
		public static List<T> QueryCommodity<T>(int pageIndex, int pageSize, out int totalCount, string keyword, string fieldName, List<int> categoryIdList, Filter filter, Sort sort, IList<FieldDataModel> fieldModelList) where T : class, new()
		{
			totalCount = 0;
			List<T> modelList = null;
			try
			{
				if (string.IsNullOrWhiteSpace(keyword) && (categoryIdList == null || categoryIdList.Count == 0)) return null;
				ILuceneQuery<T> luceneQuery = new LuceneQuery<T>();
				string analyzerKeyword = string.IsNullOrWhiteSpace(keyword) ? "" : string.Format(" +{0}", AnalyzerKeyword(keyword, fieldName));
				string queryString = string.Format(" {0} ", analyzerKeyword);
				modelList = luceneQuery.QueryIndexPage(queryString, fieldName, pageIndex, pageSize, out totalCount, filter, sort, fieldModelList);
			}
			catch (Exception ex)
			{
				m_logger.Error(string.Format("QueryCommodity参数为{0}出现异常", keyword), ex);
			}
			return modelList;
		}

		#endregion

		/// <summary>
		/// 为keyword做盘古分词
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		private static string AnalyzerKeyword(string keyword, string fieldName)
		{
			StringBuilder queryStringBuilder = new StringBuilder();
			ILuceneAnalyze analyzer = new LuceneAnalyze();
			List<string> words = analyzer.AnalyzerKey(keyword, fieldName);
			if (words.Count == 1)
			{
				queryStringBuilder.AppendFormat("{0}:{1}* ", fieldName, words[0]);
			}
			else
			{
				StringBuilder fieldQueryStringBuilder = new StringBuilder();
				foreach (string word in words)
				{
					queryStringBuilder.AppendFormat("{0}:{1} ", fieldName, word);
				}
			}
			string result = queryStringBuilder.ToString().TrimEnd();
			m_logger.Info(string.Format("AnalyzerKeyword 将 keyword={0}转换为{1}", keyword, result));

			return result;
		}

		///// <summary>
		///// 为类别做custom分词
		///// </summary>
		///// <param name="categoryIdList"></param>
		///// <returns></returns>
		//private static string AnalyzerCategory(List<int> categoryIdList)
		//{
		//	return string.Join(" ", categoryIdList.Select(c => string.Format("{0}:{1}", "categoryid", c)));
		//}
	}
}
