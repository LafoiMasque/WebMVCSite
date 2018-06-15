using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace WebSite.DAL.MashupPattern
{
	public abstract class BaseMashupDal
	{
		DbContext m_dBContext = DbContextFactory.CreateDbContext();

		/// <summary>
		/// 添加数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool AddEntity<T>(T entity) where T : class, new()
		{
			m_dBContext.Set<T>().Add(entity);
			return true;
		}

		/// <summary>
		/// 删除
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool DeleteEntity<T>(T entity) where T : class, new()
		{
			m_dBContext.Entry(entity).State = EntityState.Deleted;
			return true;
		}

		/// <summary>
		/// 更新
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool EditEntity<T>(T entity) where T : class, new()
		{
			m_dBContext.Entry(entity).State = EntityState.Modified;
			return true;
		}

		/// <summary>
		/// 查询过滤
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="whereLambda"></param>
		/// <returns></returns>
		public IQueryable<T> LoadEntities<T>(Expression<Func<T, bool>> whereLambda) where T : class, new()
		{
			return m_dBContext.Set<T>().Where(whereLambda);
		}

		/// <summary>
		/// 分页
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="S"></typeparam>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="totalCount"></param>
		/// <param name="whereLambda"></param>
		/// <param name="orderByLambda"></param>
		/// <param name="isAsc"></param>
		/// <returns></returns>
		public IQueryable<T> LoadPageEntities<T,S>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda, bool isAsc) where T : class, new()
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
		/// <typeparam name="T"></typeparam>
		/// <param name="sql">sql语句</param>
		/// <param name="pars">参数</param>
		/// <returns></returns>
		public bool ExecuteSql<T>(string sql, params object[] pars) where T : class, new()
		{
			return m_dBContext.Database.ExecuteSqlCommand(sql, pars) > 0;
		}

		/// <summary>
		/// 执行sql语句查询，返回给定类型的元素
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="M"></typeparam>
		/// <param name="sql"></param>
		/// <param name="pars"></param>
		/// <returns></returns>
		public M ExecuteQuery<T,M>(string sql, params object[] pars) where T : class, new()
		{
			M result = default(M);
			var dbRawSqlQuery = m_dBContext.Database.SqlQuery(typeof(M), sql, pars);
			foreach (var item in dbRawSqlQuery)
			{
				result = (M)item;
				break;
			}
			return result;
		}

		/// <summary>
		/// 执行sql语句查询，返回给定类型的元素集合
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="M"></typeparam>
		/// <param name="sql"></param>
		/// <param name="pars"></param>
		/// <returns></returns>
		public List<M> ExecuteQueryList<T,M>(string sql, params object[] pars) where T : class, new()
		{
			return m_dBContext.Database.SqlQuery<M>(sql, pars).ToList();
		}
	}
}
