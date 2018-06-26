using System.Configuration;

namespace WebSite.LuceneNetDemo.Utility
{
	public class StaticConstant
	{
		public static readonly string IndexPath = ConfigurationManager.AppSettings["TestIndexPath"];
	}
}