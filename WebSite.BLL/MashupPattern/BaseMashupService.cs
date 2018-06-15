using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebSite.DALFactory;
using WebSite.DALFactory.MashupPattern;
using WebSite.IBLL.MashupPattern;
using WebSite.IDAL.MashupPattern;

namespace WebSite.BLL.MashupPattern
{
	public abstract class BaseMashupService<T> : IBaseMashupService where T : class, IBaseMashupDal
	{
		protected IDbMashupSession<T> CurrentDbSession
		{
			get { return DbMashupSessionFactory.CreateDbMashupSession<T>(); }
		}

		protected T CurrentDal { get { return CurrentDbSession.CreateInstanceDal; } }

		public IQueryable<M> LoadEntities<M>(Expression<Func<M, bool>> whereLambda) where M : class, new()
		{
			return CurrentDal.LoadEntities(whereLambda);
		}

		public IQueryable<M> LoadPageEntities<M, S>(int pageIndex, int pageSize, out int totalCount, Expression<Func<M, bool>> whereLambda, Expression<Func<M, S>> orderByLambda, bool isAsc) where M : class, new()
		{
			return CurrentDal.LoadPageEntities<M, S>(pageIndex, pageSize, out totalCount, whereLambda, orderByLambda, isAsc);
		}

		/// <summary>
		/// 添加数据
		/// </summary>
		/// <typeparam name="M"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool AddEntity<M>(M entity) where M : class, new()
		{
			CurrentDal.AddEntity(entity);
			return CurrentDbSession.SaveChanged();
		}

		/// <summary>
		/// 删除
		/// </summary>
		/// <typeparam name="M"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool DeleteEntity<M>(M entity) where M : class, new()
		{
			CurrentDal.DeleteEntity(entity);
			return CurrentDbSession.SaveChanged();
		}

		/// <summary>
		/// 更新
		/// </summary>
		/// <typeparam name="M"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool EditEntity<M>(M entity) where M : class, new()
		{
			CurrentDal.EditEntity(entity);
			return CurrentDbSession.SaveChanged();
		}
	}
}
