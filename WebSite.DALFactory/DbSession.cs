using System.Data.Entity;
using WebSite.DAL;
using WebSite.IDAL;

namespace WebSite.DALFactory
{
	/// <summary>
	/// 数据会话层：就是一个工厂类，负责完成所有数据操作类实例的创建，然后业务层通过数据会话层来获取要操作数据类的实例。所以数据会话层将业务层与数据层解耦。
	/// 在数据会话层中提供一个方法：完成所有数据的保存。
	/// </summary>
	public partial class DbSession : IDbSession//<T> where T : class, new()
	{
		public DbContext DbContext { get { return DbContextFactory.CreateDbContext(); } }

		//private IUserInfoDal m_userInfoDal = null;

		//public IUserInfoDal UserInfoDal
		//{
		//	get
		//	{
		//		if (m_userInfoDal == null)
		//		{
		//			//通过抽象工厂封装了类的实例的创建
		//			m_userInfoDal = AbstractFactory.CreateInstanceDal<IUserInfoDal>();
		//		}
		//		return m_userInfoDal;
		//	}
		//	set => m_userInfoDal = value;
		//}

		////private T m_createInstanceDal = null;

		////public T CreateInstanceDal
		////{
		////	get
		////	{
		////		if (m_createInstanceDal == null)
		////		{
		////			m_createInstanceDal = AbstractFactory.CreateInstanceDal<T>("userInfo");
		////		}
		////		return m_createInstanceDal;
		////	}
		////	set => m_createInstanceDal = value;
		////}

		/// <summary>
		/// 一个业务中经常涉及到对多张操作，我们希望链接一次数据库，完成对张表数据的操作。提高性能。 工作单元模式。
		/// </summary>
		/// <returns></returns>
		public bool SaveChanged()
		{
			return DbContext.SaveChanges() > 0;
		}

		//public int ExecuteSql(string sql, params object[] pars)
		//{
		//	return DbContext.Database.ExecuteSqlCommand(sql, pars);
		//}

	}
}
