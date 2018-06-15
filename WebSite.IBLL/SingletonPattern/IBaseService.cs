using System;
using System.Linq;
using System.Linq.Expressions;

namespace WebSite.IBLL.SingletonPattern
{
	public interface IBaseService<T> where T : class, new()
	{
		//IDbSession CurrentDbSession { get; }

		IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);

		IQueryable<T> LoadPageEntities<S>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda, bool isAsc);

		bool DeleteEntity(T entity);

		bool EditEntity(T entity);

		bool AddEntity(T entity);
	}
}
