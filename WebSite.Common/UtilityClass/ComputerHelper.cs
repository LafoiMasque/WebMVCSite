using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace WebSite.Common.UtilityClass
{
	/// 获取本机用户名、MAC地址、内网IP地址、公网IP地址、硬盘ID、CPU序列号、系统名称、物理内存。
	/// </summary>
	public class ComputerHelper
	{
		///// <summary>
		///// 操作系统的登录用户名
		///// </summary>
		///// <returns>系统的登录用户名</returns>
		//public static string GetUserName()
		//{
		//	try
		//	{
		//		string strUserName = string.Empty;
		//		ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
		//		ManagementObjectCollection moc = mc.GetInstances();
		//		foreach (ManagementObject mo in moc)
		//		{
		//			strUserName = mo["UserName"].ToString();
		//		}
		//		moc = null;
		//		mc = null;
		//		return strUserName;
		//	}
		//	catch
		//	{
		//		return "unknown";
		//	}
		//}
		///// <summary>
		///// 获取本机MAC地址
		///// </summary>
		///// <returns>本机MAC地址</returns>
		//public static string GetMacAddress()
		//{
		//	try
		//	{
		//		string strMac = string.Empty;
		//		ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
		//		ManagementObjectCollection moc = mc.GetInstances();
		//		foreach (ManagementObject mo in moc)
		//		{
		//			if ((bool)mo["IPEnabled"] == true)
		//			{
		//				strMac = mo["MacAddress"].ToString();
		//			}
		//		}
		//		moc = null;
		//		mc = null;
		//		return strMac;
		//	}
		//	catch
		//	{
		//		return "unknown";
		//	}
		//}
		///// <summary>
		///// 获取本机的物理地址
		///// </summary>
		///// <returns></returns>
		//public static string getMacAddr_Local()
		//{
		//	string madAddr = null;
		//	try
		//	{
		//		ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
		//		ManagementObjectCollection moc2 = mc.GetInstances();
		//		foreach (ManagementObject mo in moc2)
		//		{
		//			if (Convert.ToBoolean(mo["IPEnabled"]) == true)
		//			{
		//				madAddr = mo["MacAddress"].ToString();
		//				madAddr = madAddr.Replace(':', '-');
		//			}
		//			mo.Dispose();
		//		}
		//		if (madAddr == null)
		//		{
		//			return "unknown";
		//		}
		//		else
		//		{
		//			return madAddr;
		//		}
		//	}
		//	catch (Exception)
		//	{
		//		return "unknown";
		//	}
		//}
		///// <summary>
		///// 获取客户端内网IPv6地址
		///// </summary>
		///// <returns>客户端内网IPv6地址</returns>
		//public static string GetClientLocalIPv6Address()
		//{
		//	string strLocalIP = string.Empty;
		//	try
		//	{
		//		IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
		//		IPAddress ipAddress = ipHost.AddressList[0];
		//		strLocalIP = ipAddress.ToString();
		//		return strLocalIP;
		//	}
		//	catch
		//	{
		//		return "unknown";
		//	}
		//}
		///// <summary>
		///// 获取客户端内网IPv4地址
		///// </summary>
		///// <returns>客户端内网IPv4地址</returns>
		//public static string GetClientLocalIPv4Address()
		//{
		//	string strLocalIP = string.Empty;
		//	try
		//	{
		//		IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
		//		IPAddress ipAddress = ipHost.AddressList[0];
		//		strLocalIP = ipAddress.ToString();
		//		return strLocalIP;
		//	}
		//	catch
		//	{
		//		return "unknown";
		//	}
		//}
		///// <summary>
		///// 获取客户端内网IPv4地址集合
		///// </summary>
		///// <returns>返回客户端内网IPv4地址集合</returns>
		//public static List<string> GetClientLocalIPv4AddressList()
		//{
		//	List<string> ipAddressList = new List<string>();
		//	try
		//	{
		//		IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
		//		foreach (IPAddress ipAddress in ipHost.AddressList)
		//		{
		//			if (!ipAddressList.Contains(ipAddress.ToString()))
		//			{
		//				ipAddressList.Add(ipAddress.ToString());
		//			}
		//		}
		//	}
		//	catch
		//	{

		//	}
		//	return ipAddressList;
		//}

		///// <summary>
		///// 获取客户端外网IP地址
		///// </summary>
		///// <returns>客户端外网IP地址</returns>
		//public static string GetClientInternetIPAddress()
		//{
		//	string strInternetIPAddress = string.Empty;
		//	try
		//	{
		//		using (WebClient webClient = new WebClient())
		//		{
		//			strInternetIPAddress = webClient.DownloadString("http://www.coridc.com/ip");
		//			Regex r = new Regex("[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}");
		//			Match mth = r.Match(strInternetIPAddress);
		//			if (!mth.Success)
		//			{
		//				strInternetIPAddress = GetClientInternetIPAddress2();
		//				mth = r.Match(strInternetIPAddress);
		//				if (!mth.Success)
		//				{
		//					strInternetIPAddress = "unknown";
		//				}
		//			}
		//			return strInternetIPAddress;
		//		}
		//	}
		//	catch
		//	{
		//		return "unknown";
		//	}
		//}
		///// <summary>
		///// 获取本机公网IP地址
		///// </summary>
		///// <returns>本机公网IP地址</returns>
		//private static string GetClientInternetIPAddress2()
		//{
		//	string tempip = "";
		//	try
		//	{
		//		//http://iframe.ip138.com/ic.asp 返回的是：您的IP是：[220.231.17.99] 来自：北京市 光环新网
		//		WebRequest wr = WebRequest.Create("http://iframe.ip138.com/ic.asp");
		//		Stream s = wr.GetResponse().GetResponseStream();
		//		StreamReader sr = new StreamReader(s, Encoding.Default);
		//		string all = sr.ReadToEnd(); //读取网站的数据

		//		int start = all.IndexOf("[") + 1;
		//		int end = all.IndexOf("]", start);
		//		tempip = all.Substring(start, end - start);
		//		sr.Close();
		//		sr.Dispose();
		//		s.Close();
		//		s.Dispose();
		//		return tempip;
		//	}
		//	catch
		//	{
		//		return "unknown";
		//	}
		//}
		///// <summary>
		///// 获取硬盘序号
		///// </summary>
		///// <returns>硬盘序号</returns>
		//public static string GetDiskID()
		//{
		//	try
		//	{
		//		string strDiskID = string.Empty;
		//		ManagementClass mc = new ManagementClass("Win32_DiskDrive");
		//		ManagementObjectCollection moc = mc.GetInstances();
		//		foreach (ManagementObject mo in moc)
		//		{
		//			strDiskID = mo.Properties["Model"].Value.ToString();
		//		}
		//		moc = null;
		//		mc = null;
		//		return strDiskID;
		//	}
		//	catch
		//	{
		//		return "unknown";
		//	}
		//}
		///// <summary>
		///// 获取CpuID
		///// </summary>
		///// <returns>CpuID</returns>
		//public static string GetCpuID()
		//{
		//	try
		//	{
		//		string strCpuID = string.Empty;
		//		ManagementClass mc = new ManagementClass("Win32_Processor");
		//		ManagementObjectCollection moc = mc.GetInstances();
		//		foreach (ManagementObject mo in moc)
		//		{
		//			strCpuID = mo.Properties["ProcessorId"].Value.ToString();
		//		}
		//		moc = null;
		//		mc = null;
		//		return strCpuID;
		//	}
		//	catch
		//	{
		//		return "unknown";
		//	}
		//}
		///// <summary>
		///// 获取操作系统类型
		///// </summary>
		///// <returns>操作系统类型</returns>
		//public static string GetSystemType()
		//{
		//	try
		//	{
		//		string strSystemType = string.Empty;
		//		ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
		//		ManagementObjectCollection moc = mc.GetInstances();
		//		foreach (ManagementObject mo in moc)
		//		{
		//			strSystemType = mo["SystemType"].ToString();
		//		}
		//		moc = null;
		//		mc = null;
		//		return strSystemType;
		//	}
		//	catch
		//	{
		//		return "unknown";
		//	}
		//}
		///// <summary>
		///// 获取操作系统名称
		///// </summary>
		///// <returns>操作系统名称</returns>
		//public static string GetSystemName()
		//{
		//	try
		//	{
		//		string strSystemName = string.Empty;
		//		ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT PartComponent FROM Win32_SystemOperatingSystem");
		//		foreach (ManagementObject mo in mos.Get())
		//		{
		//			strSystemName = mo["PartComponent"].ToString();
		//		}
		//		mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT Caption FROM Win32_OperatingSystem");
		//		foreach (ManagementObject mo in mos.Get())
		//		{
		//			strSystemName = mo["Caption"].ToString();
		//		}
		//		return strSystemName;
		//	}
		//	catch
		//	{
		//		return "unknown";
		//	}
		//}
		///// <summary>
		///// 获取物理内存信息
		///// </summary>
		///// <returns>物理内存信息</returns>
		//public static string GetTotalPhysicalMemory()
		//{
		//	try
		//	{
		//		string strTotalPhysicalMemory = string.Empty;
		//		ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
		//		ManagementObjectCollection moc = mc.GetInstances();
		//		foreach (ManagementObject mo in moc)
		//		{
		//			strTotalPhysicalMemory = mo["TotalPhysicalMemory"].ToString();
		//		}
		//		moc = null;
		//		mc = null;
		//		return strTotalPhysicalMemory;
		//	}
		//	catch
		//	{
		//		return "unknown";
		//	}
		//}

		///// <summary>
		///// 获取主板id
		///// </summary>
		///// <returns></returns>
		//public static string GetMotherBoardID()
		//{
		//	try
		//	{
		//		ManagementClass mc = new ManagementClass("Win32_BaseBoard");
		//		ManagementObjectCollection moc = mc.GetInstances();
		//		string strID = null;
		//		foreach (ManagementObject mo in moc)
		//		{
		//			strID = mo.Properties["SerialNumber"].Value.ToString();
		//			break;
		//		}
		//		return strID;
		//	}
		//	catch
		//	{
		//		return "unknown";
		//	}
		//}

		///// <summary>
		///// 获取公用桌面路径         
		///// </summary>  
		//public static string GetAllUsersDesktopFolderPath()
		//{
		//	RegistryKey folders;
		//	folders = OpenRegistryPath(Registry.LocalMachine, @"/software/microsoft/windows/currentversion/explorer/shell folders");
		//	string desktopPath = folders.GetValue("Common Desktop").ToString();
		//	return desktopPath;
		//}
		///// <summary>  
		///// 获取公用启动项路径  
		///// </summary>  
		//public static string GetAllUsersStartupFolderPath()
		//{
		//	RegistryKey folders;
		//	folders = OpenRegistryPath(Registry.LocalMachine, @"/software/microsoft/windows/currentversion/explorer/shell folders");
		//	string Startup = folders.GetValue("Common Startup").ToString();
		//	return Startup;
		//}
		//private static RegistryKey OpenRegistryPath(RegistryKey root, string s)
		//{
		//	s = s.Remove(0, 1) + @"/";
		//	while (s.IndexOf(@"/") != -1)
		//	{
		//		root = root.OpenSubKey(s.Substring(0, s.IndexOf(@"/")));
		//		s = s.Remove(0, s.IndexOf(@"/") + 1);
		//	}
		//	return root;
		//}

		/// <summary>  
		/// 获取当前使用的IP  
		/// </summary>  
		/// <returns></returns>  
		public static string GetLocalIP()
		{
			string result = RunApp("route", "print", true);
			Match m = Regex.Match(result, @"0.0.0.0\s+0.0.0.0\s+(\d+.\d+.\d+.\d+)\s+(\d+.\d+.\d+.\d+)");
			if (m.Success)
			{
				return m.Groups[2].Value;
			}
			else
			{
				try
				{
					System.Net.Sockets.TcpClient c = new System.Net.Sockets.TcpClient();
					c.Connect("www.baidu.com", 80);
					string ip = ((System.Net.IPEndPoint)c.Client.LocalEndPoint).Address.ToString();
					c.Close();
					return ip;
				}
				catch (Exception)
				{
					return null;
				}
			}
		}

		/// <summary>  
		/// 获取本机主DNS  
		/// </summary>  
		/// <returns></returns>  
		public static string GetPrimaryDNS()
		{
			string result = RunApp("nslookup", "", true);
			Match m = Regex.Match(result, @"\d+\.\d+\.\d+\.\d+");
			if (m.Success)
			{
				return m.Value;
			}
			else
			{
				return null;
			}
		}

		/// <summary>  
		/// 运行一个控制台程序并返回其输出参数。  
		/// </summary>  
		/// <param name="filename">程序名</param>  
		/// <param name="arguments">输入参数</param>  
		/// <returns></returns>  
		public static string RunApp(string filename, string arguments, bool recordLog)
		{
			try
			{
				if (recordLog)
				{
					Trace.WriteLine(filename + " " + arguments);
				}
				Process proc = new Process();
				proc.StartInfo.FileName = filename;
				proc.StartInfo.CreateNoWindow = true;
				proc.StartInfo.Arguments = arguments;
				proc.StartInfo.RedirectStandardOutput = true;
				proc.StartInfo.UseShellExecute = false;
				proc.Start();

				using (System.IO.StreamReader sr = new System.IO.StreamReader(proc.StandardOutput.BaseStream, Encoding.Default))
				{
					//string txt = sr.ReadToEnd();  
					//sr.Close();  
					//if (recordLog)  
					//{  
					//    Trace.WriteLine(txt);  
					//}  
					//if (!proc.HasExited)  
					//{  
					//    proc.Kill();  
					//}  
					//上面标记的是原文，下面是我自己调试错误后自行修改的  
					Thread.Sleep(100);           //貌似调用系统的nslookup还未返回数据或者数据未编码完成，程序就已经跳过直接执行  
												 //txt = sr.ReadToEnd()了，导致返回的数据为空，故睡眠令硬件反应  
					if (!proc.HasExited)         //在无参数调用nslookup后，可以继续输入命令继续操作，如果进程未停止就直接执行  
					{                            //txt = sr.ReadToEnd()程序就在等待输入，而且又无法输入，直接掐住无法继续运行  
						proc.Kill();
					}
					string txt = sr.ReadToEnd();
					sr.Close();
					if (recordLog)
						Trace.WriteLine(txt);
					return txt;
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex);
				return ex.Message;
			}
		}
	}
}
