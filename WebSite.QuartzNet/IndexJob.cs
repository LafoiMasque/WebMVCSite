using Quartz;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSite.IBLL;

namespace WebSite.QuartzNet
{
	/// <summary>
	/// 完成工作任务的定义。
	/// </summary>
	public class IndexJob : IJob
	{
		IKeyWordsRankService bll = null;

		public IndexJob()
		{
			IApplicationContext ctx = ContextRegistry.GetContext();
			bll = (IKeyWordsRankService)ctx.GetObject("KeyWordsRankService");
		}

		/// <summary>
		/// 将明细表中的数据插入到汇总表中。
		/// </summary>
		/// <param name="context"></param>
		public void Execute(JobExecutionContext context)
		{
			bll.DeleteAllKeyWordsRank();
			bll.InsertKeyWordsRank();
		}
	}
}
