using System.Data.Entity;
using System.Runtime.Remoting.Messaging;
using WebSite.Model.DataBaseModel;

namespace WebSite.DAL
{
	/// <summary>
	/// 负责创建EF数据操作上下文实例，必须保证线程内唯一
	/// </summary>
	public class DbContextFactory
	{
		//线程槽
		public static DbContext CreateDbContext()
		{
			DbContext dbContext = (DbContext)CallContext.GetData("dbContext");
			if (dbContext == null)
			{
				dbContext = new WebSiteDbContext();
				CallContext.SetData("dbContext", dbContext);
			}
			return dbContext;
		}
	}
}
