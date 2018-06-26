using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.LuceneNetDemo.Interface
{
	public interface ILuceneAnalyze
	{
		/// <summary>
		/// 根据查询的field将keyword分词
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		string[] AnalyzerKey(string keyword, string fieldName);
	}
}