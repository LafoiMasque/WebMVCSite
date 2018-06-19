using System.Collections.Generic;
using System.Linq;
using WebSite.IBLL.SingletonPattern;
using WebSite.Model.DataBaseModel;
using WebSite.Model.Search;

namespace WebSite.BLL.SingletonPattern
{
	public partial class UserInfoService : BaseService<UserInfo>, IUserInfoService
	{
		//public override void SetCurrentDal()
		//{
		//	CurrentDal = CurrentDbSession.UserInfoDal;
		//}

		/// <summary>
		/// 批量删除多条用户数据
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public bool DeleteEntities(List<int> list)
		{
			var userInfoList = CurrentDbSession.UserInfoDal.LoadEntities(o => list.Contains(o.Id));
			foreach (var userInfo in userInfoList)
			{
				userInfo.StateFlag = 1;
				//CurrentDbSession.UserInfoDal.DeleteEntity(userInfo);
			}
			return CurrentDbSession.SaveChanged();
		}

		/// <summary>
		/// 完成用户信息的搜索
		/// </summary>
		/// <param name="userInfoSearch">封装的搜索条件数据</param>
		/// <returns></returns>
		public IQueryable<UserInfo> LoadSearchEntities(UserInfoSearch userInfoSearch, byte state)
		{
			var temp = CurrentDbSession.UserInfoDal.LoadEntities(o => o.StateFlag == state);
			//根据用户名来搜索
			if (!string.IsNullOrEmpty(userInfoSearch.UserName))
			{
				temp = temp.Where(o => o.UserName.Contains(userInfoSearch.UserName));
			}
			else if (!string.IsNullOrEmpty(userInfoSearch.Remark))
			{
				temp = temp.Where(o => o.Remark.Contains(userInfoSearch.Remark));
			}
			return temp.OrderBy(o => o.Id).Skip((userInfoSearch.PageIndex - 1) * userInfoSearch.PageSize).Take(userInfoSearch.PageSize);
		}

		/// <summary>
		/// 为用户分配角色
		/// </summary>
		/// <param name="userId">用户编号</param>
		/// <param name="roleIdList">要分配的角色的编号</param>
		/// <returns></returns>
		public bool SetUserRoleInfo(int userId, List<int> roleIdList)
		{
			bool isSuccess = false;
			var userInfo = CurrentDbSession.UserInfoDal.LoadEntities(o => o.Id == userId).FirstOrDefault();
			if (userInfo != null)
			{
				userInfo.RoleInfo_UserInfo.Clear();
				foreach (int roleId in roleIdList)
				{
					var roleInfo = CurrentDbSession.RoleInfoDal.LoadEntities(o => o.Id == roleId).FirstOrDefault();
					RoleInfo_UserInfo roleInfo_UserInfo = new RoleInfo_UserInfo();
					roleInfo_UserInfo.RoleInfoId = roleInfo.Id;
					roleInfo_UserInfo.UserInfoId = userId;
					userInfo.RoleInfo_UserInfo.Add(roleInfo_UserInfo);
				}
				isSuccess = CurrentDbSession.SaveChanged();
			}
			return isSuccess;
		}

		/// <summary>
		/// 完成用户权限的分配
		/// </summary>
		/// <param name="actionId"></param>
		/// <param name="userId"></param>
		/// <param name="isPass"></param>
		/// <returns></returns>
		public bool SetUserActionInfo(int actionId, int userId, bool isPass)
		{
			//判断userId以前是否有了该actionId,如果有了只需要修改isPass状态，否则插入。
			var UserInfo_ActionInfo = CurrentDbSession.UserInfo_ActionInfoDal.LoadEntities(o => o.ActionInfoId == actionId && o.UserInfoId == userId).FirstOrDefault();
			if (UserInfo_ActionInfo == null)
			{
				UserInfo_ActionInfo userInfo_ActionInfo = new UserInfo_ActionInfo();
				userInfo_ActionInfo.ActionInfoId = actionId;
				userInfo_ActionInfo.UserInfoId = userId;
				userInfo_ActionInfo.IsPass = isPass;
				CurrentDbSession.UserInfo_ActionInfoDal.AddEntity(userInfo_ActionInfo);
			}
			else
			{
				UserInfo_ActionInfo.IsPass = isPass;
				CurrentDbSession.UserInfo_ActionInfoDal.EditEntity(UserInfo_ActionInfo);
			}
			return CurrentDbSession.SaveChanged();
		}

	}
}
