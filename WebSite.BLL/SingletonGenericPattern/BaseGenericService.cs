using System;
using System.Linq;
using System.Linq.Expressions;
using WebSite.DALFactory.SingletonGenericPattern;
using WebSite.IDAL.SingletonGenericPattern;
using WebSite.IDAL.SingletonPattern;

namespace WebSite.BLL.SingletonGenericPattern
{
	public abstract class BaseGenericService<T, M> where T : class, IBaseDal<M> where M : class, new()
	{
		protected IDbGenericSession<T, M> CurrentDbSession
		{
			get { return DbGenericSessionFactory.CreateDbGenericSession<T, M>(); }
		}

		protected T CurrentDal { get { return CurrentDbSession.CreateInstanceDal; } }

		public IQueryable<M> LoadEntities(Expression<Func<M, bool>> whereLambda)
		{
			return CurrentDal.LoadEntities(whereLambda);
		}

		public IQueryable<M> LoadPageEntities<S>(int pageIndex, int pageSize, out int totalCount, Expression<Func<M, bool>> whereLambda, Expression<Func<M, S>> orderByLambda, bool isAsc)
		{
			return CurrentDal.LoadPageEntities<S>(pageIndex, pageSize, out totalCount, whereLambda, orderByLambda, isAsc);
		}

		/// <summary>
		/// 添加数据
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool AddEntity(M entity)
		{
			CurrentDal.AddEntity(entity);
			return CurrentDbSession.SaveChanged();
		}

		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool DeleteEntity(M entity)
		{
			CurrentDal.DeleteEntity(entity);
			return CurrentDbSession.SaveChanged();
		}

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool EditEntity(M entity)
		{
			CurrentDal.EditEntity(entity);
			return CurrentDbSession.SaveChanged();
		}

		public void Dispose()
		{
			CurrentDbSession.Dispose();
		}
	}
}
