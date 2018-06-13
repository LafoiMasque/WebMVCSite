using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.IDAL
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
