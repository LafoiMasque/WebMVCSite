using System.Configuration;

namespace WebSite.Core.LuceneNet.Utility
{
	public class StaticConstant
	{
		public static readonly string IndexPath = ConfigurationManager.AppSettings["TestIndexPath"];
	}
}