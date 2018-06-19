using WebSite.IBLL.SingletonPattern;
using WebSite.Model.DataBaseModel;

namespace WebSite.IBLL.SingletonGenericPattern
{
	public partial interface IUserInfoGenericService : IBaseService<UserInfo>
	{
		void DoSomething();
	}
}
