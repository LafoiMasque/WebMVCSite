using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using WebSite.LuceneNetDemo.Interface;
using WebSite.LuceneNetDemo.Utility;
using System.Collections.Generic;
using System.Linq;

namespace WebSite.LuceneNetDemo.Service
{
	public class LuceneAnalyze : ILuceneAnalyze
	{
		private CustomLogger m_logger = new CustomLogger(typeof(LuceneAnalyze));

		#region AnalyzerKey

		/// <summary>
		/// 将搜索的keyword分词
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		public string[] AnalyzerKey(string keyword, string fieldName)
		{
			Analyzer analyzer = new PanGuAnalyzer();
			QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, fieldName, analyzer);
			Query query = parser.Parse(CleanKeyword(keyword));
			if (query is TermQuery)
			{
				Term term = ((TermQuery)query).Term;
				return new string[] { term.Text };
			}
			else if (query is PhraseQuery)
			{
				Term[] term = ((PhraseQuery)query).GetTerms();
				return term.Select(t => t.Text).ToArray();
			}
			else if (query is BooleanQuery)
			{
				BooleanClause[] clauses = ((BooleanQuery)query).GetClauses();
				List<string> analyzerWords = new List<string>();
				foreach (BooleanClause clause in clauses)
				{
					Query childQuery = clause.Query;
					if (childQuery is TermQuery)
					{
						Term term = ((TermQuery)childQuery).Term;
						analyzerWords.Add(term.Text);
					}
					else if (childQuery is PhraseQuery)
					{
						Term[] term = ((PhraseQuery)childQuery).GetTerms();
						analyzerWords.AddRange(term.Select(t => t.Text));
					}
				}
				return analyzerWords.ToArray();
			}
			else
			{
				m_logger.Debug(string.Format("AnalyzerKey在解析keyword={0}的结果为new string[] { keyword } ", keyword));
				return new string[] { keyword };
			}
		}

		/// <summary>
		/// 清理头尾and or 关键字
		/// </summary>
		/// <param name="keyword"></param>
		/// <returns></returns>
		private string CleanKeyword(string keyword)
		{
			if (!string.IsNullOrWhiteSpace(keyword))
			{
				bool isClean = false;
				while (!isClean)
				{
					keyword = keyword.Trim();
					if (keyword.EndsWith(" AND"))
					{
						keyword = string.Format("{0}and", keyword.Remove(keyword.Length - 3, 3));
					}
					else if (keyword.EndsWith(" OR"))
					{
						keyword = string.Format("{0}or", keyword.Remove(keyword.Length - 2, 2));
					}
					else if (keyword.StartsWith("AND "))
					{
						keyword = string.Format("and{0}", keyword.Substring(3));
					}
					else if (keyword.StartsWith("OR "))
					{
						keyword = string.Format("or{0}", keyword.Substring(2));
					}
					else if (keyword.Contains(" OR "))
					{
						keyword = keyword.Replace(" OR ", " or ");
					}
					else if (keyword.Contains(" AND "))
					{
						keyword = keyword.Replace(" AND ", " and ");
					}
					else
					{
						isClean = true;
					}
				}
			}
			return QueryParser.Escape(keyword);
		}

		#endregion AnalyzerKey
	}
}