using System.Linq;
using System.Web.Mvc;
using WebSite.Common.UtilityClass;
using WebSite.Core;
using WebSite.IBLL.SingletonPattern;
using WebSite.Model.DataBaseModel;
using WebSite.Model.DataModel;
using WebSite.Model.EnumType;
using WebSite.WebApp.CustomAttribute;

namespace WebSite.WebApp.Controllers
{
	[AuthenLogin]
	public class HomeController : Controller
	{
		IUserInfoService UserInfoService { get; set; }

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public ActionResult HomePage()
		{
			return View();
		}

		/// <summary>
		/// 过滤登录用户的菜单权限
		/// 1: 可以按照用户---角色---权限这条线找出登录用户的权限，放在一个集合中。
		/// 2：可以按照用户---权限这条线找出用户的权限，放在一个集合中。
		/// 3：将这两个集合合并成一个集合。
		/// 4：把禁止的权限从总的集合中清除。
		/// 5：将总的集合中的重复权限清除。
		/// 6：把过滤好的菜单权限生成JSON返回。
		/// </summary>
		/// <returns></returns>
		public ActionResult GetMenus()
		{
			//1: 可以按照用户---角色---权限这条线找出登录用户的权限，放在一个集合中。
			//获取登录用户的信息
			string sessionId = HttpContext.Request.Cookies["sessionId"].Value;
			object obj = MemcacheHelper.Get(sessionId);
			UserInfo loginUser = SerializeHelper.DeserializeToObject<UserInfo>(obj.ToString());
			var userInfo = UserInfoService.LoadEntities(o => o.Id == loginUser.Id).FirstOrDefault();
			var userRoleInfo = userInfo.RoleInfo_UserInfo;
			byte actionTypeEnum = (byte)ActionTypeEnum.MenumActionType;
			var loginUserMenuActions = (from r in userRoleInfo
										from a in r.RoleInfo.RoleInfo_ActionInfo
										where a.ActionInfo.ActionTypeEnum == actionTypeEnum
										select a.ActionInfo).ToList();
			//2：可以按照用户---权限这条线找出用户的权限，放在一个集合中。
			var userActions = from a in userInfo.UserInfo_ActionInfo
							  select a.ActionInfo;
			var userMenuActions = (from a in userActions
								   where a.ActionTypeEnum == actionTypeEnum
								   select a).ToList();
			//3：将这两个集合合并成一个集合。
			loginUserMenuActions.AddRange(userMenuActions);
			//4：把禁止的权限从总的集合中清除。
			var forbidActions = (from a in userInfo.UserInfo_ActionInfo
								 where !a.IsPass
								 select a.ActionInfoId).ToList();
			var loginUserAllowActions = loginUserMenuActions.Where(o => !forbidActions.Contains(o.Id));
			//5：将总的集合中的重复权限清除。
			var lastLoginUserActions = loginUserMenuActions.Distinct(new WebSite.Model.EqualityComparer());
			//6：把过滤好的菜单权限生成JSON返回。
			var temp = from a in lastLoginUserActions
					   select new
					   {
						   icon = a.MenuIcon,
						   title = a.ActionName,
						   url = a.Url
					   };

			ResultModel<object> resultModel = new ResultModel<object>(temp);
			return Json(resultModel, JsonRequestBehavior.AllowGet);
		}
	}
}