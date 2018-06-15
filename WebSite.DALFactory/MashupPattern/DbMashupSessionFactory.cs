using System.Runtime.Remoting.Messaging;
using WebSite.IDAL.MashupPattern;

namespace WebSite.DALFactory.MashupPattern
{
	public class DbMashupSessionFactory
	{
		public static IDbMashupSession<T> CreateDbMashupSession<T>() where T : class, IBaseMashupDal
		{
			IDbMashupSession<T> dbSession = (IDbMashupSession<T>)CallContext.GetData("dbSession");
			if (dbSession == null)
			{
				dbSession = new DbMashupSession<T>();
				CallContext.SetData("dbSession", dbSession);
			}
			return dbSession;
		}
	}
}
