using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace WebSite.DAL
{
	public abstract class BaseDal<T> where T : class, new()
	{
		DbContext m_dBContext = DbContextFactory.CreateDbContext();

		/// <summary>
		/// 添加数据
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool AddEntity(T entity)
		{
			m_dBContext.Set<T>().Add(entity);
			return true;
		}

		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool DeleteEntity(T entity)
		{
			m_dBContext.Entry(entity).State = EntityState.Deleted;
			return true;
		}

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool EditEntity(T entity)
		{
			m_dBContext.Entry(entity).State = EntityState.Modified;
			return true;
		}

		/// <summary>
		/// 查询过滤
		/// </summary>
		/// <param name="whereLambda"></param>
		/// <returns></returns>
		public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
		{
			return m_dBContext.Set<T>().Where(whereLambda);
		}

		/// <summary>
		/// 分页
		/// </summary>
		/// <typeparam name="S"></typeparam>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="totalCount"></param>
		/// <param name="whereLambda"></param>
		/// <param name="orderByLambda"></param>
		/// <param name="isAsc"></param>
		/// <returns></returns>
		public IQueryable<T> LoadPageEntities<S>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda, bool isAsc)
		{
			IQueryable<T> temp = LoadEntities(whereLambda);
			totalCount = temp.Count();
			if (isAsc)//升序
			{
				temp = temp.OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
			}
			else
			{
				temp = temp.OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
			}
			return temp;
		}
	}
}
