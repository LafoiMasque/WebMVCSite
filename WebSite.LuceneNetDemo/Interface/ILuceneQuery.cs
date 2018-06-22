using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.LuceneNetDemo.Interface
{
	public interface ILuceneQuery<T> where T : class, new()
	{
		/// <summary>
		/// 获取商品信息数据
		/// </summary>
		/// <param name="queryString"></param>
		/// <returns></returns>
		List<T> QueryIndex(string queryString);

		/// <summary>
		/// 分页获取商品信息数据
		/// </summary>
		/// <param name="queryString"></param>
		/// <param name="pageIndex">第一页为1</param>
		/// <param name="pageSize"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		List<T> QueryIndexPage(string queryString, int pageIndex, int pageSize, out int totalCount, string priceFilter, string priceOrderBy);
	}
}
