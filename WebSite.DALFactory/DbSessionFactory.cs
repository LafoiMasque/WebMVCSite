using System.Runtime.Remoting.Messaging;
using WebSite.IDAL;

namespace WebSite.DALFactory
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
