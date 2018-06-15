using System.Data.Entity;

namespace WebSite.IDAL.SingletonPattern
{
	/// <summary>
	/// 业务层调用的是数据会话层的接口。
	/// </summary>
	public interface IDbGenericSession<T, M> where T : class, IBaseDal<M> where M : class, new()
	{
		DbContext DbContext { get; }
		T CreateInstanceDal { get; }
		bool SaveChanged();
	}
}
