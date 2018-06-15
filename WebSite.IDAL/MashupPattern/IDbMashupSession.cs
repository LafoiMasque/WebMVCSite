using System.Data.Entity;

namespace WebSite.IDAL.MashupPattern
{
	/// <summary>
	/// 业务层调用的是数据会话层的接口。
	/// </summary>
	public interface IDbMashupSession<T> where T : class, IBaseMashupDal
	{
		DbContext DbContext { get; }
		T CreateInstanceDal { get; }
		bool SaveChanged();
	}
}
