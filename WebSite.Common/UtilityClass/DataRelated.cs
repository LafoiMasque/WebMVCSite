using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.Common.UtilityClass
{
	public static class DataRelated
	{
		public static string ObjectToString(this object obj)
		{
			return (null == obj || obj == DBNull.Value) ? "" : obj.ToString().Trim();
		}

		public static bool OjectEqualString(this string str, string compareString)
		{
			return str.Equals(compareString, StringComparison.OrdinalIgnoreCase);
		}

		public static int ToInt<T>(this T obj)
		{
			int res = 0;
			int.TryParse(obj.ObjectToString(), out res);
			return res;
		}

		public static long ToLong<T>(this T obj)
		{
			long res = 0;
			long.TryParse(obj.ObjectToString(), out res);
			return res;
		}

		public static double ToDouble<T>(this T obj)
		{
			double res = 0;
			double.TryParse(obj.ObjectToString(), out res);
			return res;
		}

		public static bool ToBool<T>(this T obj)
		{
			bool res = false;
			bool.TryParse(obj.ObjectToString(), out res);
			return res;
		}

		public static bool IsCustomType(Type type)
		{
			return (type != typeof(object) && Type.GetTypeCode(type) == TypeCode.Object);
		}

		public static bool IsEnumerableType(Type type)
		{
			return (type.GetInterface("IEnumerable", false) != null);
		}

		public static bool IsCollectionType(Type type)
		{
			return (type.GetInterface("ICollection", false) != null);
		}
	}
}
