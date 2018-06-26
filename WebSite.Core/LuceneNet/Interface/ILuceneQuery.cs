using Lucene.Net.Search;
using WebSite.Core.LuceneNet.Model;
using System.Collections.Generic;

namespace WebSite.Core.LuceneNet.Interface
{
	public interface ILuceneQuery<T> where T : class, new()
	{
		/// <summary>
		/// 获取商品信息数据
		/// </summary>
		/// <param name="queryString"></param>
		/// <param name="fieldName"></param>
		/// <param name="fieldModelList"></param>
		/// <param name="listCount"></param>
		/// <returns></returns>
		List<T> QueryIndex(string queryString, string fieldName, IEnumerable<FieldDataModel> fieldModelList, int listCount = 1000);

		/// <summary>
		/// 获取商品信息数据
		/// </summary>
		/// <param name="query"></param>
		/// <param name="fieldModelList"></param>
		/// <param name="listCount"></param>
		/// <returns></returns>
		List<T> QueryIndex(Query query, IEnumerable<FieldDataModel> fieldModelList, int listCount = 1000);

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
		List<T> QueryIndexPage(string queryString, string fieldName, int pageIndex, int pageSize, out int totalCount, Filter filter, Sort sort, IEnumerable<FieldDataModel> fieldModelList);

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
		List<T> QueryIndexPage(Query query, int pageIndex, int pageSize, out int totalCount, Filter filter, Sort sort, IEnumerable<FieldDataModel> fieldModelList);
	}
}