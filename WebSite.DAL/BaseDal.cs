using System;
using System.Collections.Generic;
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

		/// <summary>
		/// 执行sql语句
		/// </summary>
		/// <param name="sql">sql语句</param>
		/// <param name="pars">参数</param>
		/// <returns></returns>
		public bool ExecuteSql(string sql, params object[] pars)
		{
			return m_dBContext.Database.ExecuteSqlCommand(sql, pars) > 0;
		}

		/// <summary>
		/// 执行sql语句查询，返回给定类型的元素
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="pars"></param>
		/// <returns></returns>
		public M ExecuteQuery<M>(string sql, params object[] pars)
		{
			Type type = typeof(M);
			var result = m_dBContext.Database.SqlQuery(type, sql, pars);
			return default(M);
		}

		/// <summary>
		/// 执行sql语句查询，返回给定类型的元素集合
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="pars"></param>
		/// <returns></returns>
		public List<M> ExecuteQueryList<M>(string sql, params object[] pars)
		{
			return m_dBContext.Database.SqlQuery<M>(sql, pars).ToList();
		}
	}
}
