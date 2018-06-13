using ServiceStack.Redis;
using System;
using WebSite.Core.Redis.Init;

namespace WebSite.Core.Redis.Service
{
	/// <summary>
	/// RedisBase类，是redis操作的基类，继承自IDisposable接口，主要用于释放内存
	/// </summary>
	public abstract class RedisBase : IDisposable
	{
		private bool m_disposed = false;

		public static IRedisClient RedisClient { get; private set; }

        static RedisBase()
        {
            RedisClient = RedisManager.GetClient();
        }

        public virtual void FlushAll()
        {
            RedisClient.FlushAll();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.m_disposed)
            {
                if (disposing)
                {
                    RedisClient.Dispose();
                    RedisClient = null;
                }
            }
            this.m_disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 保存数据DB文件到硬盘
        /// </summary>
        public void Save()
        {
            RedisClient.Save();
        }

        /// <summary>
        /// 异步保存数据DB文件到硬盘
        /// </summary>
        public void SaveAsync()
        {
            RedisClient.SaveAsync();
        }
    }
}
