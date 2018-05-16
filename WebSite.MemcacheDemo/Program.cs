using Memcached.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.MemcacheDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			string[] serverList = { "192.168.1.120:11211", "10.0.0.137:11211" };

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
			MemcachedClient mc = new MemcachedClient();
			mc.EnableCompression = false;

			Console.WriteLine("------------测  试-----------");
			string key = "test";
			mc.Set(key, "my value");  //存储数据到缓存服务器，这里将字符串"my value"缓存，key 是"test"

			if (mc.KeyExists(key))
			{
				Console.WriteLine("test is Exists");
				Console.WriteLine(mc.Get("test").ToString());  //在缓存中获取key为test的项目
			}
			else
			{
				Console.WriteLine("test not Exists");
			}
			Console.ReadLine();

			//移除缓存中key为test的项目
			mc.Delete(key);
			if (mc.KeyExists(key))
			{
				Console.WriteLine("test is Exists");
				Console.WriteLine(mc.Get("test").ToString());
			}
			else
			{
				Console.WriteLine("test not Exists");
			}
			Console.ReadLine();

			//关闭池， 关闭sockets
			SockIOPool.GetInstance().Shutdown();
		}
	}
}
