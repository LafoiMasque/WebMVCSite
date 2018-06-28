using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Util;
using PanGu;
using PanGu.HighLight;
using System.Collections.Generic;
using System.IO;
using WebSite.Core.LuceneNet.Interface;
using WebSite.Core.LuceneNet.Model;
using WebSite.Core.LuceneNet.Service;

namespace WebSite.Core.LuceneNet
{
	public class LuceneCommon
	{
		public static List<string> PanGuSplitWord(string fieldName, string keyword)
		{
			ILuceneAnalyze luceneAnalyze = new LuceneAnalyze();
			return luceneAnalyze.AnalyzerKey(fieldName, keyword);
		}

		/// <summary>
		/// 创建HTMLFormatter,参数为高亮单词的前后缀
		/// </summary>
		/// <param name="keywords"></param>
		/// <param name="Content"></param>
		/// <returns></returns>
		public static string CreateHightLight(string keywords, string Content)
		{
			SimpleHTMLFormatter simpleHTMLFormatter = new SimpleHTMLFormatter("<font color=\"red\">", "</font>");
			//创建Highlighter ，输入HTMLFormatter 和盘古分词对象Semgent
			Highlighter highlighter = new Highlighter(simpleHTMLFormatter, new Segment())
			{
				//设置每个摘要段的字符数
				FragmentSize = 150
			};
			//获取最匹配的摘要段
			return highlighter.GetBestFragment(keywords, Content);
		}
	}
}
