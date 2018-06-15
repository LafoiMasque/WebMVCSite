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

		public static IDbGenericSession<T, M> CreateDbGenericSession<T, M>() where T : class, IBaseDal<M> where M : class, new()
		{
			IDbGenericSession<T, M> dbSession = (IDbGenericSession<T, M>)CallContext.GetData("dbSession");
			if (dbSession == null)
			{
				dbSession = new DbGenericSession<T, M>();
				CallContext.SetData("dbSession", dbSession);
			}
			return dbSession;
		}
	}
}
