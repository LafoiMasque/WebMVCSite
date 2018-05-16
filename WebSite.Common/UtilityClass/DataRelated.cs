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

		public static bool OjectEqualString(this string str,string compareString)
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

		//public static T DeepClone<T>(this T a)
		//{
		//    using (MemoryStream stream = new MemoryStream())
		//    {
		//        BinaryFormatter formatter = new BinaryFormatter();
		//        formatter.Serialize(stream, a);
		//        stream.Position = 0;
		//        return (T)formatter.Deserialize(stream);
		//    }
		//}

		private static Dictionary<string, object> m_dic = new Dictionary<string, object>();
		public static TOut DeepClone<TIn, TOut>(this TIn tIn)
		{
			string key = string.Format("trans_exp_{0}_{1}", typeof(TIn).FullName, typeof(TOut).FullName);
			if (!m_dic.ContainsKey(key))
			{
				ParameterExpression paramExpr = Expression.Parameter(typeof(TIn), "p");
				List<MemberBinding> ltMemberBinding = new List<MemberBinding>();
				foreach (var item in typeof(TOut).GetProperties())
				{
					if (!item.CanWrite)
					{
						continue;
					}
					MemberExpression property = Expression.Property(paramExpr, typeof(TIn).GetProperty(item.Name));
					MemberBinding memberBinding = Expression.Bind(item, property);
					ltMemberBinding.Add(memberBinding);
				}
				MemberInitExpression memberInitExpr = Expression.MemberInit(Expression.New(typeof(TOut)), ltMemberBinding.ToArray());
				Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpr, new ParameterExpression[] { paramExpr });
				Func<TIn, TOut> func = lambda.Compile();
				m_dic[key] = func;
			}
			return tIn != null ? ((Func<TIn, TOut>)m_dic[key])(tIn) : default(TOut);
		}

		//public static void AddRange<T>(this ObservableCollection<T> source, IEnumerable<T> element)
		//{
		//	ICollection<T> collection = element as ICollection<T>;
		//	if (null != collection)
		//	{
		//		foreach (var item in collection)
		//		{
		//			T temp = item.DeepClone<T, T>();
		//			source.Add(temp);
		//		}
		//	}
		//}
	}
}
