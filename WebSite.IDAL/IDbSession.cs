using System.Data.Entity;

namespace WebSite.IDAL
{
	/// <summary>
	/// 业务层调用的是数据会话层的接口。
	/// </summary>
	public partial interface IDbSession//<T> where T : class, new()
	{
		DbContext DbContext { get; }
		//IUserInfoDal UserInfoDal { get; set; }
		////T CreateInstanceDal { get; set; }
		bool SaveChanged();
		//int ExecuteSql(string sql, params object[] pars);
	}
}
