using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSite.IBLL;
using WebSite.Model.DataBaseModel;

namespace WebSite.BLL
{
	public partial class RoleInfoService : BaseService<RoleInfo>, IRoleInfoService
	{
		/// <summary>
		/// 为角色分配权限
		/// </summary>
		/// <param name="roleId">角色编号</param>
		/// <param name="actionIdList">权限编号列表</param>
		/// <returns></returns>
		public bool SetRoleActionInfo(int roleId, List<int> actionIdList)
		{
			bool isSuccess = false;
			//获取角色信息.
			var roleInfo = CurrentDbSession.RoleInfoDal.LoadEntities(o => o.Id == roleId).FirstOrDefault();
			if (roleInfo != null)
			{
				roleInfo.RoleInfo_ActionInfo.Clear();
				foreach (int actionId in actionIdList)
				{
					var actionInfo = CurrentDbSession.ActionInfoDal.LoadEntities(o => o.Id == actionId).FirstOrDefault();
					RoleInfo_ActionInfo roleInfo_ActionInfo = new RoleInfo_ActionInfo();
					roleInfo_ActionInfo.ActionInfoId = actionInfo.Id;
					roleInfo_ActionInfo.RoleInfoId = roleId;
					roleInfo.RoleInfo_ActionInfo.Add(roleInfo_ActionInfo);
				}
				isSuccess = CurrentDbSession.SaveChanged();
			}
			return isSuccess;
		}
	}
}
