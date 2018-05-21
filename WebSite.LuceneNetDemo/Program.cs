using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Standard;
using NSharp.SearchEngine.Lucene.Analysis.Cjk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.LuceneNetDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			do
			{
				Console.WriteLine("分词类型：");
				ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
				Console.WriteLine();
				Analyzer analyzer = null;
				TokenStream tokenStream = null;
				Token token = null;
				switch (consoleKeyInfo.Key)
				{
					case ConsoleKey.NumPad1:
						 analyzer = new StandardAnalyzer();
						 tokenStream = analyzer.TokenStream("", new StringReader("北京，Hi欢迎你们大家"));
						while ((token = tokenStream.Next()) != null)
						{
							Console.WriteLine(token.TermText());
						}
						break;
					case ConsoleKey.NumPad2:
						 analyzer = new CJKAnalyzer();
						 tokenStream = analyzer.TokenStream("", new StringReader("北京，Hi欢迎你们大家"));
						while ((token = tokenStream.Next()) != null)
						{
							Console.WriteLine(token.TermText());
						}
						break;
					case ConsoleKey.P:
						analyzer = new PanGuAnalyzer();
						tokenStream = analyzer.TokenStream("", new StringReader("北京，Hi欢迎你们大家"));
						while ((token = tokenStream.Next()) != null)
						{
							Console.WriteLine(token.TermText());
						}
						break;
					case ConsoleKey.O:
						return;
					default:
						break;
				}
			} while (true);
		}
	}
}
