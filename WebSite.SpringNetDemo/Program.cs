using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.SpringNetDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			IApplicationContext ctx = ContextRegistry.GetContext();
			IUserInfoService lister = (IUserInfoService)ctx.GetObject("KeyWordsRankService");
			Console.WriteLine(lister.ShowMsg());
			Console.ReadKey();
		}
	}
}
