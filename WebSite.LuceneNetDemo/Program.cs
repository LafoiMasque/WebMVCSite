using Lucene.Net.Analysis;
using System;

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
				//Token token = null;
				switch (consoleKeyInfo.Key)
				{
					case ConsoleKey.NumPad1:
						break;
					case ConsoleKey.NumPad2:
						break;
					case ConsoleKey.P:
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
