using System.Collections.Generic;
using WebSite.Model.DataBaseModel;

namespace WebSite.IBLL.SingletonPattern
{
	public partial interface IRoleInfoService : IBaseService<RoleInfo>
	{
		/// <summary>
		/// 为角色分配权限
		/// </summary>
		/// <param name="roleId">角色编号</param>
		/// <param name="actionIdList">权限编号列表</param>
		/// <returns></returns>
		bool SetRoleActionInfo(int roleId, List<int> actionIdList);
	}
}
