using System.Runtime.Remoting.Messaging;
using WebSite.IDAL.SingletonPattern;

namespace WebSite.DALFactory.SingletonPattern
{
	public class DbSessionFactory
	{
		public static IDbSession CreateDbSession()
		{
			IDbSession dbSession = (IDbSession)CallContext.GetData("dbSession");
			if (dbSession == null)
			{
				dbSession = new DbSession();
				CallContext.SetData("dbSession", dbSession);
			}
			return dbSession;
		}
	}
}
