using System.Web.Mvc;
using WebSite.WebApp.CustomAttribute;

namespace WebSite.WebApp
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			//filters.Add(new HandleErrorAttribute());
			filters.Add(new WebSiteExceptionAttribute());
		}
	}
}
