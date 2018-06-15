using Quartz;
using Spring.Context;
using Spring.Context.Support;
using WebSite.IBLL.SingletonPattern;

namespace WebSite.QuartzNet
{
	/// <summary>
	/// 完成工作任务的定义。
	/// </summary>
	public class IndexJob : IJob
	{
		public IKeyWordsRankService KeyWordsRankService { get; set; }

		public IndexJob()
		{
			IApplicationContext ctx = ContextRegistry.GetContext();
			KeyWordsRankService = (IKeyWordsRankService)ctx.GetObject("KeyWordsRankService");
		}

		/// <summary>
		/// 将明细表中的数据插入到汇总表中。
		/// </summary>
		/// <param name="context"></param>
		public void Execute(JobExecutionContext context)
		{
			KeyWordsRankService.DeleteAllKeyWordsRank();
			KeyWordsRankService.InsertKeyWordsRank();
		}
	}
}
