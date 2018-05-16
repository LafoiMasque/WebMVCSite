using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.WebApp.CustomAttribute
{
	public class WebSiteExceptionAttribute : HandleErrorAttribute
	{
		public static Queue<Exception> ExceptionQueue { get; set; } = new Queue<Exception>();

		/// <summary>
		/// 可以捕获异常数据
		/// </summary>
		/// <param name="filterContext"></param>
		public override void OnException(ExceptionContext filterContext)
		{
			base.OnException(filterContext);
			Exception ex = filterContext.Exception;
			//写到队列
			ExceptionQueue.Enqueue(ex);
			//跳转到错误页面.
			filterContext.HttpContext.Response.Redirect("/Error.html");
		}
	}
}