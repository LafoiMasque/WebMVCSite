using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.BLL;
using WebSite.IBLL;
using WebSite.Model.DataBaseModel;
using WebSite.Model.DataModel;
using WebSite.Model.EnumType;
using WebSite.Model.Search;
using WebSite.WebApp.CustomAttribute;

namespace WebSite.WebApp.Controllers
{
	[AuthenLogin]
	public class UserInfoController : Controller
	{
		// GET: UserInfo
		public IUserInfoService UserInfoService { get; set; }
		public IRoleInfoService RoleInfoService { get; set; }
		public IActionInfoService ActionInfoService { get; set; }
		public IUserInfo_ActionInfoService UserInfo_ActionInfoService { get; set; }

		public ActionResult Index()
		{
			//int a = 1, b = 0;
			//int c = a / b;
			return View();
		}

		/// <summary>
		/// 获取用户列表数据
		/// </summary>
		/// <returns></returns>
		public ActionResult GetUserInfoList()
		{
			string value = Request["page"];
			int pageIndex = value != null ? int.Parse(value) : 1;
			value = Request["rows"];
			int pageSize = value != null ? int.Parse(value) : 5;
			string userName = Request["name"];
			string remark = Request["remark"];
			int totalCount = 0;
			//构建搜索条件.
			UserInfoSearch userInfoSearch = new UserInfoSearch()
			{
				UserName = userName,
				Remark = remark,
				PageIndex = pageIndex,
				PageSize = pageSize,
				TotalCount = totalCount
			};
			byte delFlag = (byte)DeleteTypeEnum.Normarl;
			//根据构建好的搜索条件完成搜索
			var userInfoList = UserInfoService.LoadSearchEntities(userInfoSearch, delFlag);
			////获取分页数据。
			//var userInfoList = UserInfoService.LoadPageEntities(pageIndex, pageSize, out totalCount, o => o.State == (int)DeleteEnumType.Normarl, o => o.Sort, true);
			var temp = from u in userInfoList
					   select new
					   {
						   u.Id,
						   u.UserName,
						   u.UserPassword,
						   u.Remark,
						   u.CreateTime,
					   };
			return Json(new { rows = temp, total = totalCount });
		}

		/// <summary>
		/// 删除用户列表数据
		/// </summary>
		/// <returns></returns>
		public ActionResult DeleteUserInfo()
		{
			string strId = Request["strId"];
			string[] strIds = strId.Split(',');
			List<int> list = new List<int>();
			foreach (string id in strIds)
			{
				list.Add(Convert.ToInt32(id));
			}
			//将list集合存储要删除的记录编号发送到服务端
			bool isOK = UserInfoService.DeleteEntities(list);
			ResultCodeEnum resultCodeEnum = isOK ? ResultCodeEnum.Success : ResultCodeEnum.Failure;
			ResultModel<string> resultModel = new ResultModel<string>(new CodeMessage(resultCodeEnum));
			return Json(resultModel);
		}

		/// <summary>
		/// 添加用户数据
		/// </summary>
		/// <returns></returns>
		public ActionResult AddUserInfo(UserInfo userInfo)
		{
			userInfo.StateFlag = 0;
			userInfo.CreateTime = DateTime.Now;
			userInfo.CreaterId = 1;
			userInfo.Account = userInfo.UserName;
			bool isOK = UserInfoService.AddEntity(userInfo);
			ResultCodeEnum resultCodeEnum = isOK ? ResultCodeEnum.Success : ResultCodeEnum.Failure;
			ResultModel<string> resultModel = new ResultModel<string>(new CodeMessage(resultCodeEnum));
			return Json(resultModel);
		}

		/// <summary>
		/// 展示要修改的数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult ShowUserInfo()
		{
			int id = int.Parse(Request["id"]);
			var userInfo = UserInfoService.LoadEntities(o => o.Id == id).FirstOrDefault();
			ResultModel<UserInfo> resultModel = new ResultModel<UserInfo>(userInfo);
			return Json(resultModel, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// 完成用户数据的更新
		/// </summary>
		/// <param name="userInfo"></param>
		/// <returns></returns>
		public ActionResult EditUserInfo(UserInfo userInfo)
		{
			userInfo.LastModifyTime = DateTime.Now;
			bool isOK = UserInfoService.EditEntity(userInfo);
			ResultCodeEnum resultCodeEnum = isOK ? ResultCodeEnum.Success : ResultCodeEnum.Failure;
			ResultModel<string> resultModel = new ResultModel<string>(new CodeMessage(resultCodeEnum));
			return Json(resultModel);
		}

		/// <summary>
		/// 展示用户已经有的角色
		/// </summary>
		/// <returns></returns>
		public ActionResult ShowUserRoleInfo()
		{
			int id = int.Parse(Request["id"]);
			var userInfo = UserInfoService.LoadEntities(o => o.Id == id).FirstOrDefault();
			ViewBag.UserInfo = userInfo;
			//查询所有的角色.
			byte stateFlag = (byte)DeleteTypeEnum.Normarl;
			var allRoleList = RoleInfoService.LoadEntities(o => o.StateFlag == stateFlag).ToList();
			//查询一下要分配角色的用户以前具有了哪些角色编号。
			var allUserRoleIdList = (from r in userInfo.RoleInfo_UserInfo
									 select r.RoleInfo.Id).ToList();
			ViewBag.AllRoleList = allRoleList;
			ViewBag.AllUserRoleIdList = allUserRoleIdList;
			return View();
		}

		/// <summary>
		/// 完成用户角色的分配
		/// </summary>
		/// <returns></returns>
		public ActionResult SetUserRoleInfo()
		{
			int userId = int.Parse(Request["userId"]);
			//获取所有表单元素name属性值。
			string[] allKeys = Request.Form.AllKeys;
			List<int> roleIdList = new List<int>();
			string markString = "cba_";
			foreach (string key in allKeys)
			{
				if (key.StartsWith(markString))
				{
					string k = key.Replace(markString, "");
					roleIdList.Add(Convert.ToInt32(k));
				}
			}
			bool isOK = UserInfoService.SetUserRoleInfo(userId, roleIdList);
			ResultCodeEnum resultCodeEnum = isOK ? ResultCodeEnum.Success : ResultCodeEnum.Failure;
			ResultModel<string> resultModel = new ResultModel<string>(new CodeMessage(resultCodeEnum));
			return Json(resultModel);
		}

		/// <summary>
		/// 展示用户权限
		/// </summary>
		/// <returns></returns>
		public ActionResult ShowUserAction()
		{
			int userId = int.Parse(Request["userId"]);
			var userInfo = UserInfoService.LoadEntities(u => u.Id == userId).FirstOrDefault();
			ViewBag.UserInfo = userInfo;
			//获取所有的权限。
			byte stateFlag = (byte)DeleteTypeEnum.Normarl;
			List<ActionInfo> allActionList = ActionInfoService.LoadEntities(a => a.StateFlag == stateFlag).ToList();
			//获取要分配的用户已经有的权限。
			List<UserInfo_ActionInfo> allActionIdList = (from a in userInfo.UserInfo_ActionInfo
														 select a).ToList();
			ViewBag.AllActionList = allActionList;
			ViewBag.AllActionIdList = allActionIdList;
			return View();
		}

		/// <summary>
		/// 完成用户权限的分配
		/// </summary>
		/// <returns></returns>
		public ActionResult SetUserAction()
		{
			int actionId = int.Parse(Request["actionId"]);
			int userId = int.Parse(Request["userId"]);
			bool isPass = Request["isPass"] == "true" ? true : false;
			bool isOK = UserInfoService.SetUserActionInfo(actionId, userId, isPass);
			ResultCodeEnum resultCodeEnum = isOK ? ResultCodeEnum.Success : ResultCodeEnum.Failure;
			ResultModel<string> resultModel = new ResultModel<string>(new CodeMessage(resultCodeEnum));
			return Json(resultModel);
		}

		/// <summary>
		/// 完成权限删除
		/// </summary>
		/// <returns></returns>
		public ActionResult ClearUserAction()
		{
			string result = null;
			ResultCodeEnum resultCodeEnum = ResultCodeEnum.Failure;
			int actionId = int.Parse(Request["actionId"]);
			int userId = int.Parse(Request["userId"]);
			var roleInfo_userInfo_actionInfo = UserInfo_ActionInfoService.LoadEntities(r => r.ActionInfoId == actionId && r.UserInfoId == userId).FirstOrDefault();
			if (roleInfo_userInfo_actionInfo != null)
			{
				if (UserInfo_ActionInfoService.DeleteEntity(roleInfo_userInfo_actionInfo))
					result = "删除成功!!";
				else
					result = "删除失败!!";
				resultCodeEnum = ResultCodeEnum.Success;
			}
			else
			{
				result = "数据不存在!!";
			}
			ResultModel<string> resultModel = new ResultModel<string>(new CodeMessage(resultCodeEnum, result));
			return Json(resultModel);
		}
	}
}