using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WebSite.IDAL.MashupPattern
{
	public interface IBaseMashupDal
	{
		/// <summary>
		/// 查询过滤
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="whereLambda"></param>
		/// <returns></returns>
		IQueryable<T> LoadEntities<T>(Expression<Func<T, bool>> whereLambda) where T : class, new();

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
		IQueryable<T> LoadPageEntities<T, S>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda, bool isAsc) where T : class, new();

		/// <summary>
		/// 删除
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		bool DeleteEntity<T>(T entity) where T : class, new();

		/// <summary>
		/// 更新
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		bool EditEntity<T>(T entity) where T : class, new();

		/// <summary>
		/// 添加数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		bool AddEntity<T>(T entity) where T : class, new();

		/// <summary>
		/// 执行sql语句
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sql">sql语句</param>
		/// <param name="pars">参数</param>
		/// <returns></returns>
		bool ExecuteSql<T>(string sql, params object[] pars) where T : class, new();

		/// <summary>
		/// 执行sql语句查询，返回给定类型的元素
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="M"></typeparam>
		/// <param name="sql"></param>
		/// <param name="pars"></param>
		/// <returns></returns>
		M ExecuteQuery<T, M>(string sql, params object[] pars) where T : class, new();

		/// <summary>
		/// 执行sql语句查询，返回给定类型的元素集合
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="M"></typeparam>
		/// <param name="sql"></param>
		/// <param name="pars"></param>
		/// <returns></returns>
		List<M> ExecuteQueryList<T, M>(string sql, params object[] pars) where T : class, new();
	}
}
