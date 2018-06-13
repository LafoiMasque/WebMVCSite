using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WebSite.Common.UtilityClass
{
	public class ExpressionClone<TIn, TOut>
	{
		private static Func<TIn, TOut> _func = null;

		static ExpressionClone()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
			List<MemberBinding> memberBindingList = new List<MemberBinding>();
			foreach (var item in typeof(TOut).GetProperties())
			{
				if (item.CanWrite)
				{
					MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
					MemberBinding memberBinding = Expression.Bind(item, property);
					memberBindingList.Add(memberBinding);
				}
			}
			foreach (var item in typeof(TOut).GetFields())
			{
				MemberExpression field = Expression.Field(parameterExpression, typeof(TIn).GetField(item.Name));
				MemberBinding memberBinding = Expression.Bind(item, field);
				memberBindingList.Add(memberBinding);
			}
			MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList);
			Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });
			_func = lambda.Compile();
		}

		public static TOut Trans(TIn t)
		{
			return _func(t);
		}
	}
}
