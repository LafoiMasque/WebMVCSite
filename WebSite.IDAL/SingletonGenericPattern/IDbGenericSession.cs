using System;
using System.Data.Entity;
using WebSite.IDAL.SingletonPattern;

namespace WebSite.IDAL.SingletonGenericPattern
{
	/// <summary>
	/// 业务层调用的是数据会话层的接口。
	/// </summary>
	public interface IDbGenericSession<T, M> : IDisposable where T : class, IBaseDal<M> where M : class, new()
	{
		DbContext DbContext { get; }
		T CreateInstanceDal { get; }
		bool SaveChanged();
	}
}
