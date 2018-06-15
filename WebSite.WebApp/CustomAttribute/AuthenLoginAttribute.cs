using Spring.Context;
using Spring.Context.Support;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using WebSite.Common.UtilityClass;
using WebSite.Core;
using WebSite.IBLL.SingletonPattern;
using WebSite.Model.DataBaseModel;

namespace WebSite.WebApp.CustomAttribute
{
	public class AuthenLoginAttribute : FilterAttribute, IAuthenticationFilter
	{
		public UserInfo LoginUser { get; set; }

		/// <summary>
		/// 这个方法是在Action执行之前调用
		/// </summary>
		/// <param name="filterContext"></param>
		public void OnAuthentication(AuthenticationContext filterContext)
		{
			//if (filterContext.HttpContext.Session["userInfo"] == null)
			//{
			//	//var Url = new UrlHelper(filterContext.RequestContext);
			//	//var url = Url.Action("Logon", "Account", new { area = "" });
			//	//filterContext.Result = new RedirectResult(url);
			//	filterContext.Result = new RedirectResult("/Login/Index");
			//}
			bool isSucess = false;
			if (filterContext.HttpContext.Request.Cookies["sessionId"] != null)
			{
				string sessionId = filterContext.HttpContext.Request.Cookies["sessionId"].Value;
				//根据该值查Memcache.
				object obj = MemcacheHelper.Get(sessionId);
				if (obj != null)
				{
					UserInfo userInfo = SerializeHelper.DeserializeToObject<UserInfo>(obj.ToString());
					LoginUser = userInfo;
					isSucess = true;
					MemcacheHelper.Set(sessionId, obj, DateTime.Now.AddMinutes(20));//模拟出滑动过期时间.
					//留一个后门，测试方便。发布的时候一定要删除该代码。
					if (LoginUser.UserName == "admin")
					{
						return;
					}

					//完成权限校验。
					//获取用户请求的URL地址.
					string url = filterContext.HttpContext.Request.Url.AbsolutePath.ToLower();
					//获取请求的方式.
					string httpMehotd = filterContext.HttpContext.Request.HttpMethod;
					//根据获取的URL地址与请求的方式查询权限表。
					IApplicationContext ctx = ContextRegistry.GetContext();
					IActionInfoService ActionInfoService = (IActionInfoService)ctx.GetObject("ActionInfoService");
					var actionInfo = ActionInfoService.LoadEntities(a => a.Url == url && a.HttpMethod == httpMehotd).FirstOrDefault();

					//判断用户是否具有所访问的地址对应的权限
					IUserInfoService UserInfoService = (IUserInfoService)ctx.GetObject("UserInfoService");
					var loginUserInfo = UserInfoService.LoadEntities(o => o.Id == LoginUser.Id).FirstOrDefault();
					//1:可以先按照用户权限这条线进行过滤。
					var isExt = (from a in loginUserInfo.UserInfo_ActionInfo
								 where a.ActionInfoId == actionInfo.Id
								 select a).FirstOrDefault();
					if (isExt != null)
					{
						if (isExt.IsPass)
						{
							return;
						}
						else
						{
							filterContext.Result = new RedirectResult("/Error.html");
							return;
						}
					}
					//2：按照用户角色权限这条线进行过滤。
					var loginUserRole = loginUserInfo.RoleInfo_UserInfo;
					var count = (from r in loginUserRole
								 from a in r.RoleInfo.RoleInfo_ActionInfo
								 where a.ActionInfo.Id == actionInfo.Id
								 select a).Count();
					if (count < 1)
					{
						filterContext.Result = new RedirectResult("/Error.html");
						return;
					}
				}
			}
			if (!isSucess)
			{
				filterContext.Result = new RedirectResult("/Login/Index");//注意.
			}
		}

		/// <summary>
		/// 这个方法是在Action执行之后调用
		/// </summary>
		/// <param name="filterContext"></param>
		public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
		{

		}
	}
}