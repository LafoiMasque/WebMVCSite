using System;
using System.Linq;
using System.Linq.Expressions;
using WebSite.DALFactory;
using WebSite.IDAL;

namespace WebSite.BLL
{
	public abstract class BaseService<T> where T : class, new()
	{
		protected IDbSession CurrentDbSession
		{
			get
			{
				return DbSessionFactory.CreateDbSession();
			}
		}

		protected IBaseDal<T> CurrentDal { get; set; }

		public BaseService()
		{
			SetCurrentDal();
		}

		public abstract void SetCurrentDal();

		public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
		{
			return CurrentDal.LoadEntities(whereLambda);
		}

		public IQueryable<T> LoadPageEntities<S>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda, bool isAsc)
		{
			return CurrentDal.LoadPageEntities<S>(pageIndex, pageSize, out totalCount, whereLambda, orderByLambda, isAsc);
		}

		/// <summary>
		/// 添加数据
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool AddEntity(T entity)
		{
			CurrentDal.AddEntity(entity);
			return CurrentDbSession.SaveChanged();
		}

		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool DeleteEntity(T entity)
		{
			CurrentDal.DeleteEntity(entity);
			return CurrentDbSession.SaveChanged();
		}

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool EditEntity(T entity)
		{
			CurrentDal.EditEntity(entity);
			return CurrentDbSession.SaveChanged();
		}

	}
}
