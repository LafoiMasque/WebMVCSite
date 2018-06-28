using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.Core.LuceneNet.Interface
{
	public interface ILuceneAnalyze
	{
		/// <summary>
		/// 根据查询的field将keyword分词
		/// </summary>
		/// <param name="fieldName"></param>
		/// <param name="keyword"></param>
		/// <returns></returns>
		List<string> AnalyzerKey(string fieldName, string keyword);
	}
}