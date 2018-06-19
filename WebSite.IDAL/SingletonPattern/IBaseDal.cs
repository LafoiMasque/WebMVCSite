using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace WebSite.IDAL.SingletonPattern
{
	public interface IBaseDal<T> where T : class, new()
	{
		/// <summary>
		/// 查询过滤
		/// </summary>
		/// <param name="whereLambda"></param>
		/// <returns></returns>
		IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);

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
		IQueryable<T> LoadPageEntities<S>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda, bool isAsc);

		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		bool DeleteEntity(T entity);

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		bool EditEntity(T entity);

		/// <summary>
		/// 添加数据
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		bool AddEntity(T entity);

		/// <summary>
		/// 执行sql语句
		/// </summary>
		/// <param name="sql">sql语句</param>
		/// <param name="pars">参数</param>
		/// <returns></returns>
		bool ExecuteSql(string sql, params object[] pars);

		/// <summary>
		/// 执行sql语句查询，返回给定类型的元素
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="pars"></param>
		/// <returns></returns>
		M ExecuteQuery<M>(string sql, params SqlParameter[] pars);

		/// <summary>
		/// 执行sql语句查询，返回给定类型的元素集合
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="pars"></param>
		/// <returns></returns>
		IQueryable<M> ExecuteQueryList<M>(string sql, params SqlParameter[] pars);
	}
}
