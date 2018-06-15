using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSite.IBLL.SingletonPattern;
using WebSite.Model.DataBaseModel;
using WebSite.Model.DataModel;
using WebSite.Model.EnumType;
using WebSite.WebApp.CustomAttribute;

namespace WebSite.WebApp.Controllers
{
	[AuthenLogin]
	public class RoleInfoController : Controller
	{
		public IRoleInfoService RoleInfoService { get; set; }
		public IActionInfoService ActionInfoService { get; set; }

		// GET: RoleInfo
		public ActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// 获取角色列表
		/// </summary>
		/// <returns></returns>
		public ActionResult GetRoleInfoList()
		{
			string pageIndexInfo = Request["page"];
			int pageIndex = pageIndexInfo != null ? int.Parse(pageIndexInfo) : 1;
			pageIndexInfo = Request["rows"];
			int pageSize = pageIndexInfo != null ? int.Parse(pageIndexInfo) : 1;
			int totalCount;
			byte delFlag = (byte)DeleteTypeEnum.Normarl;
			var roleInfoList = RoleInfoService.LoadPageEntities(pageIndex, pageSize, out totalCount, o => o.StateFlag == delFlag, o => o.Id, true);
			var temp = from r in roleInfoList
					   select new
					   {
						   r.Id,
						   r.RoleName,
						   r.Sort,
						   r.Remark,
						   r.CreateTime,
					   };
			return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// 展示添加表单
		/// </summary>
		/// <returns></returns>
		public ActionResult ShowAddInfo()
		{
			return View();
		}

		/// <summary>
		/// 完成角色信息添加
		/// </summary>
		/// <param name="roleInfo"></param>
		/// <returns></returns>
		public ActionResult AddRoleInfo(RoleInfo roleInfo)
		{
			roleInfo.CreateTime = roleInfo.LastModifyTime = DateTime.Now;
			roleInfo.StateFlag = 0;
			RoleInfoService.AddEntity(roleInfo);
			ResultModel<string> resultModel = new ResultModel<string>();
			return Json(resultModel);
		}

		/// <summary>
		/// 展示要分配的权限
		/// </summary>
		/// <returns></returns>
		public ActionResult ShowRoleAction()
		{
			int id = int.Parse(Request["id"]);
			var roleInfo = RoleInfoService.LoadEntities(o => o.Id == id).FirstOrDefault();//获取要分配的权限的角色信息。
			ViewBag.RoleInfo = roleInfo;
			//获取所有的权限。
			byte stateFlag = (byte)DeleteTypeEnum.Normarl;
			var actionInfoList = ActionInfoService.LoadEntities(o => o.StateFlag == stateFlag).ToList();
			//要分配权限的角色以前有哪些权限。
			var actionIdList = (from a in roleInfo.RoleInfo_ActionInfo
								select a.ActionInfo.Id).ToList();
			ViewBag.ActionInfoList = actionInfoList;
			ViewBag.ActionIdList = actionIdList;
			return View();
		}

		/// <summary>
		/// 完成角色权限的分配
		/// </summary>
		/// <returns></returns>
		public ActionResult SetRoleAction()
		{
			int roleId = int.Parse(Request["roleId"]);//获取角色编号
			string[] allKeys = Request.Form.AllKeys;//获取所有表单元素name属性的值。
			List<int> list = new List<int>();
			string markString = "cba_";
			foreach (string key in allKeys)
			{
				if (key.StartsWith(markString))
				{
					string k = key.Replace(markString, "");
					list.Add(Convert.ToInt32(k));
				}
			}
			bool isOK = RoleInfoService.SetRoleActionInfo(roleId, list);
			ResultCodeEnum resultCodeEnum = isOK ? ResultCodeEnum.Success : ResultCodeEnum.Failure;
			ResultModel<string> resultModel = new ResultModel<string>(new CodeMessage(resultCodeEnum));
			return Json(resultModel);
		}
	}
}