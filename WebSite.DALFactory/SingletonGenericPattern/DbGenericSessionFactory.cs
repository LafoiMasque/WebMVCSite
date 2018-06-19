using System.Runtime.Remoting.Messaging;
using WebSite.IDAL.SingletonGenericPattern;
using WebSite.IDAL.SingletonPattern;

namespace WebSite.DALFactory.SingletonGenericPattern
{
	public class DbGenericSessionFactory
	{
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
