using Memcached.ClientLibrary;
using System;
using WebSite.Common.UtilityClass;

namespace WebSite.Core
{
	public class MemcacheHelper
	{
		private static readonly MemcachedClient m_memcachedClient = null;

		static MemcacheHelper()
		{
			//最好放在配置文件中
			string localIP = ComputerHelper.GetLocalIP();
			string[] serverList = { localIP + ":11211", "10.0.0.137:11211" };

			//初始化池
			SockIOPool pool = SockIOPool.GetInstance();
			pool.SetServers(serverList);

			pool.InitConnections = 3;
			pool.MinConnections = 3;
			pool.MaxConnections = 5;

			pool.SocketConnectTimeout = 1000;
			pool.SocketTimeout = 3000;

			pool.MaintenanceSleep = 30;
			pool.Failover = true;

			pool.Nagle = false;
			pool.Initialize();

			//获得客户端实例
			m_memcachedClient = new MemcachedClient();
			m_memcachedClient.EnableCompression = false;

		}

		/// <summary>
		/// 存储数据
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool Set(string key, object value)
		{
			return m_memcachedClient.Set(key, value);
		}

		/// <summary>
		/// 存储数据
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		public static bool Set(string key, object value, DateTime time)
		{
			return m_memcachedClient.Set(key, value, time);
		}

		/// <summary>
		/// 获取数据
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static object Get(string key)
		{
			return m_memcachedClient.Get(key);
		}

		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool Delete(string key)
		{
			bool isOK = false;
			if (m_memcachedClient.KeyExists(key))
			{
				isOK = m_memcachedClient.Delete(key);
			}
			return isOK;
		}
	}
}
