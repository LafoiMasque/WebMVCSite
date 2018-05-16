using System;
using System.Linq;
using System.Linq.Expressions;

namespace WebSite.IDAL
{
	public interface IBaseDal<T> where T : class, new()
	{
		IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);

		IQueryable<T> LoadPageEntities<S>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda, bool isAsc);

		bool DeleteEntity(T entity);

		bool EditEntity(T entity);

		bool AddEntity(T entity);
	}
}
