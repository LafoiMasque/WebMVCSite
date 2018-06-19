using System;
using System.Linq;
using System.Linq.Expressions;

namespace WebSite.IBLL.MashupPattern
{
	public interface IBaseMashupService : IDisposable
	{
		IQueryable<T> LoadEntities<T>(Expression<Func<T, bool>> whereLambda) where T : class, new();

		IQueryable<T> LoadPageEntities<T, S>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda, bool isAsc) where T : class, new();

		bool DeleteEntity<T>(T entity) where T : class, new();

		bool EditEntity<T>(T entity) where T : class, new();

		bool AddEntity<T>(T entity) where T : class, new();
	}
}
