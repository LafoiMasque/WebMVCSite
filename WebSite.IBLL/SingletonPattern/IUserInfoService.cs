using System.Collections.Generic;
using System.Linq;
using WebSite.Model.DataBaseModel;
using WebSite.Model.Search;

namespace WebSite.IBLL.SingletonPattern
{
	public partial interface IUserInfoService : IBaseService<UserInfo>
	{
		bool DeleteEntities(List<int> list);
		IQueryable<UserInfo> LoadSearchEntities(UserInfoSearch userInfoSearch, byte state);
		bool SetUserRoleInfo(int userId, List<int> roleIdList);
		bool SetUserActionInfo(int actionId, int userId, bool isPass);
	}
}
