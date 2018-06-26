using log4net;
using log4net.Config;
using System;
using System.IO;

namespace WebSite.LuceneNetDemo.Utility
{
	public class CustomLogger
	{
		static CustomLogger()
		{
			XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CfgFiles\\log4net.cfg.xml")));
			ILog Log = LogManager.GetLogger(typeof(CustomLogger));
			Log.Info("系统初始化Logger模块");
		}

		private ILog loger = null;

		public CustomLogger(Type type)
		{
			loger = LogManager.GetLogger(type);
		}

		/// <summary>
		/// Log4日志
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="ex"></param>
		public void Error(string msg = "出现异常", Exception ex = null)
		{
			loger.Error(msg, ex);
		}

		/// <summary>
		/// Log4日志
		/// </summary>
		/// <param name="msg"></param>
		public void Warn(string msg)
		{
			loger.Warn(msg);
		}

		/// <summary>
		/// Log4日志
		/// </summary>
		/// <param name="msg"></param>
		public void Info(string msg)
		{
			loger.Info(msg);
		}

		/// <summary>
		/// Log4日志
		/// </summary>
		/// <param name="msg"></param>
		public void Debug(string msg)
		{
			loger.Debug(msg);
		}
	}
}