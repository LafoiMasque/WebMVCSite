using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace WebSite.Common.UtilityClass
{
	/// <summary>
	/// 操作Xml文件相关方法
	/// </summary>
		public class XmlUtils
	{
		#region 实例部分

		private XElement m_rootXElement = null;

		private string m_filePath = string.Empty;

		private bool m_isAuthorityFile = false;

		/// <summary>
		/// 开始初始化
		/// </summary>
		/// <param name="filePath">文件路径</param>
		public void BeginInitialize(string filePath)
		{
			m_filePath = filePath;
			KeyValuePair<XElement, bool> keyValuePair = GetRootElement(filePath);
			m_rootXElement = keyValuePair.Key;
			m_isAuthorityFile = keyValuePair.Value;
		}

		/// <summary>
		/// 结束初始化
		/// </summary>
		public void EndInitialize()
		{
			if (m_isAuthorityFile)
			{
				m_rootXElement.Save(m_filePath);
			}
		}

		#region 查询

		/// <summary>
		/// xml文件中是否存在匹配项的元素
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public bool ExistXmlElementInstance<T>(Predicate<XElement> match) where T : class, new()
		{
			return ExistXmlElementInstance(typeof(T).Name, match);
		}

		/// <summary>
		/// xml文件中是否存在匹配项的元素
		/// </summary>
		/// <param name="elementName">元素名</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public bool ExistXmlElementInstance(string elementName, Predicate<XElement> match)
		{
			return m_isAuthorityFile && ExistElementInXml(m_rootXElement, elementName, match);
		}

		/// <summary>
		/// 获取xml文件中的元素指定类型的实例集合
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <returns>实例集合</returns>
		public List<T> GetXmlElementsInstance<T>() where T : class, new()
		{
			return GetXmlElementsInstance<T>(typeof(T).Name);
		}

		/// <summary>
		/// 获取xml文件中的元素指定类型的实例集合
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns>实例集合</returns>
		public List<T> GetXmlElementsInstance<T>(Predicate<string> match) where T : class, new()
		{
			return GetXmlElementsInstance<T>(typeof(T).Name, match);
		}

		/// <summary>
		/// 获取xml文件中的元素指定类型的实例集合
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <returns>实例集合</returns>
		public List<T> GetXmlElementsInstance<T>(string elementName) where T : class, new()
		{
			return GetXmlElementsInstance<T>(elementName, null);
		}

		/// <summary>
		/// 获取xml文件中的元素指定类型的实例集合
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns>实例集合</returns>
		public List<T> GetXmlElementsInstance<T>(string elementName, Predicate<string> match) where T : class, new()
		{
			List<T> result = null;
			if (m_isAuthorityFile)
			{
				result = GetXmlElementList<T>(m_rootXElement, elementName, match);
			}
			return result;
		}

		#endregion

		#region 添加

		#region 公用

		/// <summary>
		/// 保存xml文件
		/// </summary>
		/// <param name="rootElement">根元素</param>
		/// <param name="filePath">文件路径</param>
		/// <returns></returns>
		private bool CreateDirectoryBefromSave()
		{
			bool isOK = m_rootXElement != null;
			if (isOK)
				CreateXmlDirectory(m_filePath);
			return true;
		}

		#endregion

		#region 集合类型

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public bool AddXmlElementInstance<T>(T model) where T : class, new()
		{
			return AddXmlElementInstance<T>(model, null);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="model">要修改成的数据</param>
		/// <param name="parentElementFunc">获取要写入的父元素</param>
		/// <returns></returns>
		public bool AddXmlElementInstance<T>(T model, Func<XElement, XElement> parentElementFunc) where T : class, new()
		{
			return AddXmlElementInstance<T>(typeof(T).Name, model, parentElementFunc);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public bool AddXmlElementInstance<T>(string elementName, T model) where T : class, new()
		{
			return AddXmlElementInstance<T>(elementName, model, null);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <param name="model">要修改成的数据</param>
		/// <param name="parentElementFunc">获取要写入的父元素</param>
		/// <returns></returns>
		public bool AddXmlElementInstance<T>(string elementName, T model, Func<XElement, XElement> parentElementFunc) where T : class, new()
		{
			return AddXmlElementsInstance<T>(elementName, new List<T>() { model }, parentElementFunc);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public bool AddXmlElementsInstance<T>(IEnumerable<T> dataList) where T : class, new()
		{
			return AddXmlElementsInstance<T>(dataList, null);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <param name="parentElementFunc">获取要写入的父元素</param>
		/// <returns></returns>
		public bool AddXmlElementsInstance<T>(IEnumerable<T> dataList, Func<XElement, XElement> parentElementFunc) where T : class, new()
		{
			return AddXmlElementsInstance<T>(typeof(T).Name, dataList, parentElementFunc);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public bool AddXmlElementsInstance<T>(string elementName, IEnumerable<T> dataList) where T : class, new()
		{
			return AddXmlElementsInstance(elementName, dataList, null);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <param name="parentElementFunc">获取要写入的父元素</param>
		/// <returns></returns>
		public bool AddXmlElementsInstance<T>(string elementName, IEnumerable<T> dataList, Func<XElement, XElement> parentElementFunc) where T : class, new()
		{
			m_isAuthorityFile = true;
			m_rootXElement = AddXmlElementsBytDataCollection(m_rootXElement, elementName, dataList, parentElementFunc);
			return CreateDirectoryBefromSave();
		}

		#endregion

		#region Json对象

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <param name="elementName">元素名</param>
		/// <param name="jsonString">要修改成的Json数据</param>
		/// <returns></returns>
		public bool AddXmlElementInstance(string elementName, string jsonString)
		{
			return AddXmlElementInstance(elementName, jsonString, null);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <param name="elementName">元素名</param>
		/// <param name="jsonString">要修改成的Json数据</param>
		/// <param name="parentElementFunc">获取要写入的父元素</param>
		/// <returns></returns>
		public bool AddXmlElementInstance(string elementName, string jsonString, Func<XElement, XElement> parentElementFunc)
		{
			m_isAuthorityFile = true;
			m_rootXElement = AddXmlElementsBytJsonData(m_rootXElement, elementName, jsonString, parentElementFunc);
			return CreateDirectoryBefromSave();
		}

		#endregion

		#endregion

		#region 修改

		#region 集合类型

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public bool EditXmlElementInstance<T>(T model) where T : class, new()
		{
			return EditXmlElementInstance<T>(model, null);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public bool EditXmlElementInstance<T>(T model, Predicate<XElement> match) where T : class, new()
		{
			return EditXmlElementInstance<T>(typeof(T).Name, model, match);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public bool EditXmlElementInstance<T>(string elementName, T model) where T : class, new()
		{
			return EditXmlElementInstance<T>(elementName, model, null);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <param name="model">要修改成的数据</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public bool EditXmlElementInstance<T>(string elementName, T model, Predicate<XElement> match) where T : class, new()
		{
			return EditXmlElementsInstance<T>(elementName, new List<T>() { model }, match);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public bool EditXmlElementsInstance<T>(IEnumerable<T> dataList) where T : class, new()
		{
			return EditXmlElementsInstance<T>(dataList, null);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public bool EditXmlElementsInstance<T>(IEnumerable<T> dataList, Predicate<XElement> match) where T : class, new()
		{
			return EditXmlElementsInstance<T>(typeof(T).Name, dataList, match);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public bool EditXmlElementsInstance<T>(string elementName, IEnumerable<T> dataList) where T : class, new()
		{
			return EditXmlElementsInstance<T>(elementName, dataList, null);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public bool EditXmlElementsInstance<T>(string elementName, IEnumerable<T> dataList, Predicate<XElement> match) where T : class, new()
		{
			return EditXmlElementsByDataInstance<T>(elementName, dataList, match, false);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public bool ReplaceXmlElementInstance<T>(T model) where T : class, new()
		{
			return ReplaceXmlElementInstance<T>(model, null);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="model">要修改成的数据</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public bool ReplaceXmlElementInstance<T>(T model, Predicate<XElement> match) where T : class, new()
		{
			return ReplaceXmlElementInstance<T>(typeof(T).Name, model, match);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public bool ReplaceXmlElementInstance<T>(string elementName, T model) where T : class, new()
		{
			return ReplaceXmlElementInstance<T>(elementName, model, null);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <param name="model">要修改成的数据</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public bool ReplaceXmlElementInstance<T>(string elementName, T model, Predicate<XElement> match) where T : class, new()
		{
			return ReplaceXmlElementsInstance<T>(elementName, new List<T>() { model }, match);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public bool ReplaceXmlElementsInstance<T>(IEnumerable<T> dataList) where T : class, new()
		{
			return ReplaceXmlElementsInstance<T>(dataList, null);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public bool ReplaceXmlElementsInstance<T>(IEnumerable<T> dataList, Predicate<XElement> match) where T : class, new()
		{
			return ReplaceXmlElementsInstance<T>(typeof(T).Name, dataList, match);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public bool ReplaceXmlElementsInstance<T>(string elementName, IEnumerable<T> dataList) where T : class, new()
		{
			return ReplaceXmlElementsInstance<T>(elementName, dataList, null);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public bool ReplaceXmlElementsInstance<T>(string elementName, IEnumerable<T> dataList, Predicate<XElement> match) where T : class, new()
		{
			return EditXmlElementsByDataInstance<T>(elementName, dataList, match, true);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="dataList">要修改成的数据的集合</param>
		/// <param name="isReplace">是否替换</param>
		/// <returns></returns>
		private bool EditXmlElementsByDataInstance<T>(string elementName, IEnumerable<T> dataList, Predicate<XElement> match, bool isReplace) where T : class, new()
		{
			return m_isAuthorityFile && EditXmlElementsByDataCollection(m_rootXElement, elementName, dataList, match, isReplace);
		}

		#endregion

		#region Json对象

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <param name="elementName">元素名</param>
		/// <param name="jsonString">要修改成的Json数据</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public bool EditXmlElementInstance(string elementName, string jsonString, Predicate<XElement> match)
		{
			return EditXmlElementsByDataInstance(elementName, jsonString, match, false);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <param name="elementName">元素名</param>
		/// <param name="jsonString">要修改成的Json数据</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public bool ReplaceXmlElementInstance(string elementName, string jsonString, Predicate<XElement> match)
		{
			return EditXmlElementsByDataInstance(elementName, jsonString, match, true);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <param name="elementName">元素名</param>
		/// <param name="jsonString">要修改成的Json数据</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="isReplace">是否替换</param>
		/// <returns></returns>
		private bool EditXmlElementsByDataInstance(string elementName, string jsonString, Predicate<XElement> match, bool isReplace)
		{
			return m_isAuthorityFile && EditXmlElementsByJsonData(m_rootXElement, elementName, jsonString, match, isReplace);
		}

		#endregion

		#endregion

		#region 删除

		/// <summary>
		/// 删除xml文件中匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <returns></returns>
		public bool DeleteXmlElementsInstance<T>() where T : class, new()
		{
			return DeleteXmlElementsInstance<T>(null);
		}

		/// <summary>
		/// 删除xml文件中匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public bool DeleteXmlElementsInstance<T>(Predicate<XElement> match) where T : class, new()
		{
			return DeleteXmlElementsInstance(typeof(T).Name, match);
		}

		/// <summary>
		/// 删除xml文件中匹配项的数据
		/// </summary>
		/// <param name="elementName">元素名</param>
		/// <returns></returns>
		public bool DeleteXmlElementsInstance(string elementName)
		{
			return DeleteXmlElementsInstance(elementName, null);
		}

		/// <summary>
		/// 删除xml文件中匹配项的数据
		/// </summary>
		/// <param name="elementName">元素名</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public bool DeleteXmlElementsInstance(string elementName, Predicate<XElement> match)
		{
			return m_isAuthorityFile && DeleteElementsFromXml(m_rootXElement, elementName, match);
		}

		#endregion

		#endregion

		#region 静态部分

		#region 公用

		/// <summary>
		/// 保存xml文件
		/// </summary>
		/// <param name="rootElement">根元素</param>
		/// <param name="filePath">文件路径</param>
		/// <returns></returns>
		private static bool SaveXmlElement(XElement rootElement, string filePath)
		{
			rootElement.Save(filePath);
			return true;
		}

		#endregion

		#region 查询

		/// <summary>
		/// xml文件中是否存在匹配项的元素
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool ExistXmlElement<T>(string filePath, Predicate<XElement> match) where T : class, new()
		{
			return ExistXmlElement(filePath, typeof(T).Name, match);
		}

		/// <summary>
		/// xml文件中是否存在匹配项的元素
		/// </summary>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool ExistXmlElement(string filePath, string elementName, Predicate<XElement> match)
		{
			XElement xe = GetRootElement(filePath).Key;
			return ExistElementInXml(xe, elementName, match);
		}

		/// <summary>
		/// 获取xml文件中的元素指定类型的实例集合
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <returns>实例集合</returns>
		public static List<T> GetXmlElements<T>(string filePath) where T : class, new()
		{
			return GetXmlElements<T>(filePath, typeof(T).Name);
		}

		/// <summary>
		/// 获取xml文件中的元素指定类型的实例集合
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns>实例集合</returns>
		public static List<T> GetXmlElements<T>(string filePath, Predicate<string> match) where T : class, new()
		{
			return GetXmlElements<T>(filePath, typeof(T).Name, match);
		}

		/// <summary>
		/// 获取xml文件中的元素指定类型的实例集合
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <returns>实例集合</returns>
		public static List<T> GetXmlElements<T>(string filePath, string elementName) where T : class, new()
		{
			return GetXmlElements<T>(filePath, elementName, null);
		}

		/// <summary>
		/// 获取xml文件中的元素指定类型的实例集合
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns>实例集合</returns>
		public static List<T> GetXmlElements<T>(string filePath, string elementName, Predicate<string> match) where T : class, new()
		{
			XElement rootElement = GetRootElement(filePath).Key;
			return GetXmlElementList<T>(rootElement, elementName, match);
		}

		#endregion

		#region 添加

		#region 集合类型

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public static bool AddXmlElement<T>(string filePath, T model) where T : class, new()
		{
			return AddXmlElement<T>(filePath, model, null);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="model">要修改成的数据</param>
		/// <param name="parentElementFunc">获取要写入的父元素</param>
		/// <returns></returns>
		public static bool AddXmlElement<T>(string filePath, T model, Func<XElement, XElement> parentElementFunc) where T : class, new()
		{
			return AddXmlElement<T>(filePath, typeof(T).Name, model, parentElementFunc);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public static bool AddXmlElement<T>(string filePath, string elementName, T model) where T : class, new()
		{
			return AddXmlElement<T>(filePath, elementName, model, null);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="model">要修改成的数据</param>
		/// <param name="parentElementFunc">获取要写入的父元素</param>
		/// <returns></returns>
		public static bool AddXmlElement<T>(string filePath, string elementName, T model, Func<XElement, XElement> parentElementFunc) where T : class, new()
		{
			return AddXmlElements<T>(filePath, elementName, new List<T>() { model }, parentElementFunc);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public static bool AddXmlElements<T>(string filePath, ICollection<T> dataList) where T : class, new()
		{
			return AddXmlElements<T>(filePath, dataList, null);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <param name="parentElementFunc">获取要写入的父元素</param>
		/// <returns></returns>
		public static bool AddXmlElements<T>(string filePath, ICollection<T> dataList, Func<XElement, XElement> parentElementFunc) where T : class, new()
		{
			return AddXmlElements<T>(filePath, typeof(T).Name, dataList, parentElementFunc);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public static bool AddXmlElements<T>(string filePath, string elementName, ICollection<T> dataList) where T : class, new()
		{
			return AddXmlElements<T>(filePath, elementName, dataList, null);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <param name="parentElementFunc">获取要写入的父元素</param>
		/// <returns></returns>
		public static bool AddXmlElements<T>(string filePath, string elementName, ICollection<T> dataList, Func<XElement, XElement> parentElementFunc) where T : class, new()
		{
			bool isOK = false;
			XElement rootElement = GetRootElement(filePath).Key;
			rootElement = AddXmlElementsBytDataCollection(rootElement, elementName, dataList, parentElementFunc);
			if (rootElement != null)
			{
				CreateXmlDirectory(filePath);
				isOK = SaveXmlElement(rootElement, filePath);
			}
			return isOK;
		}

		#endregion

		#region Json对象

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="jsonString">要修改成的Json数据</param>
		/// <returns></returns>
		public static bool AddXmlElements(string filePath, string elementName, string jsonString)
		{
			return AddXmlElements(filePath, elementName, jsonString, null);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="jsonString">要修改成的Json数据</param>
		/// <param name="parentElementFunc">获取要写入的父元素</param>
		/// <returns></returns>
		public static bool AddXmlElements(string filePath, string elementName, string jsonString, Func<XElement, XElement> parentElementFunc)
		{
			bool isOK = false;
			XElement rootElement = GetRootElement(filePath).Key;
			rootElement = AddXmlElementsBytJsonData(rootElement, elementName, jsonString, parentElementFunc);
			if (rootElement != null)
			{
				CreateXmlDirectory(filePath);
				isOK = SaveXmlElement(rootElement, filePath);
			}
			return isOK;
		}

		#endregion

		#endregion

		#region 修改

		#region 集合类型

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public static bool EditXmlElement<T>(string filePath, T model) where T : class, new()
		{
			return EditXmlElement<T>(filePath, model, null);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="model">要修改成的数据</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool EditXmlElement<T>(string filePath, T model, Predicate<XElement> match) where T : class, new()
		{
			return EditXmlElement<T>(filePath, typeof(T).Name, model, match);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public static bool EditXmlElement<T>(string filePath, string elementName, T model) where T : class, new()
		{
			return EditXmlElement<T>(filePath, elementName, model, null);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="model">要修改成的数据</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool EditXmlElement<T>(string filePath, string elementName, T model, Predicate<XElement> match) where T : class, new()
		{
			return EditXmlElements<T>(filePath, elementName, new List<T>() { model }, match);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public static bool EditXmlElements<T>(string filePath, IEnumerable<T> dataList) where T : class, new()
		{
			return EditXmlElements<T>(filePath, dataList, null);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool EditXmlElements<T>(string filePath, IEnumerable<T> dataList, Predicate<XElement> match) where T : class, new()
		{
			return EditXmlElements<T>(filePath, typeof(T).Name, dataList, match);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public static bool EditXmlElements<T>(string filePath, string elementName, IEnumerable<T> dataList) where T : class, new()
		{
			return EditXmlElements<T>(filePath, elementName, dataList, null);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool EditXmlElements<T>(string filePath, string elementName, IEnumerable<T> dataList, Predicate<XElement> match) where T : class, new()
		{
			return EditXmlElementsByData<T>(filePath, elementName, dataList, match, false);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public static bool ReplaceXmlElement<T>(string filePath, T model) where T : class, new()
		{
			return ReplaceXmlElement<T>(filePath, model, null);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="model">要修改成的数据</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool ReplaceXmlElement<T>(string filePath, T model, Predicate<XElement> match) where T : class, new()
		{
			return ReplaceXmlElement<T>(filePath, typeof(T).Name, model, match);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="model">要修改成的数据</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool ReplaceXmlElement<T>(string filePath, string elementName, T model) where T : class, new()
		{
			return ReplaceXmlElement<T>(filePath, elementName, model, null);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="model">要修改成的数据</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool ReplaceXmlElement<T>(string filePath, string elementName, T model, Predicate<XElement> match) where T : class, new()
		{
			return ReplaceXmlElements<T>(filePath, elementName, new List<T>() { model }, match);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public static bool ReplaceXmlElements<T>(string filePath, IEnumerable<T> dataList) where T : class, new()
		{
			return ReplaceXmlElements<T>(filePath, dataList, null);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool ReplaceXmlElements<T>(string filePath, IEnumerable<T> dataList, Predicate<XElement> match) where T : class, new()
		{
			return ReplaceXmlElements<T>(filePath, typeof(T).Name, dataList, match);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public static bool ReplaceXmlElements<T>(string filePath, string elementName, IEnumerable<T> dataList) where T : class, new()
		{
			return ReplaceXmlElements<T>(filePath, elementName, dataList, null);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool ReplaceXmlElements<T>(string filePath, string elementName, IEnumerable<T> dataList, Predicate<XElement> match) where T : class, new()
		{
			return EditXmlElementsByData<T>(filePath, elementName, dataList, match, true);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="dataList">要修改成的数据的集合</param>
		/// <param name="isReplace">是否替换</param>
		/// <returns></returns>
		private static bool EditXmlElementsByData<T>(string filePath, string elementName, IEnumerable<T> dataList, Predicate<XElement> match, bool isReplace) where T : class, new()
		{
			bool isOK = false;
			XElement rootElement = GetRootElement(filePath).Key;
			if (rootElement != null && EditXmlElementsByDataCollection(rootElement, elementName, dataList, match, isReplace))
			{
				isOK = SaveXmlElement(rootElement, filePath);
			}
			return isOK;
		}

		#endregion

		#region Json对象

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="jsonString">要修改成的Json数据</param>
		/// <returns></returns>
		public static bool EditXmlElement(string filePath, string elementName, string jsonString)
		{
			return EditXmlElement(filePath, elementName, jsonString, null);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="jsonString">要修改成的Json数据</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool EditXmlElement(string filePath, string elementName, string jsonString, Predicate<XElement> match)
		{
			return EditXmlElementsByData(filePath, elementName, jsonString, match, false);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="jsonString">要修改成的Json数据</param>
		/// <returns></returns>
		public static bool ReplaceXmlElement(string filePath, string elementName, string jsonString)
		{
			return ReplaceXmlElement(filePath, elementName, jsonString, null);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="jsonString">要修改成的Json数据</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool ReplaceXmlElement(string filePath, string elementName, string jsonString, Predicate<XElement> match)
		{
			return EditXmlElementsByData(filePath, elementName, jsonString, match, true);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="jsonString">要修改成的Json数据</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="isReplace">是否替换</param>
		/// <returns></returns>
		private static bool EditXmlElementsByData(string filePath, string elementName, string jsonString, Predicate<XElement> match, bool isReplace)
		{
			bool isOK = false;
			XElement rootElement = GetRootElement(filePath).Key;
			if (rootElement != null && EditXmlElementsByJsonData(rootElement, elementName, jsonString, match, isReplace))
			{
				isOK = SaveXmlElement(rootElement, filePath);
			}
			return isOK;
		}

		#endregion

		#endregion

		#region 删除

		/// <summary>
		/// 删除xml文件中匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <returns></returns>
		public static bool DeleteXmlElements<T>(string filePath) where T : class, new()
		{
			return DeleteXmlElements<T>(filePath, null);
		}

		/// <summary>
		/// 删除xml文件中匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool DeleteXmlElements<T>(string filePath, Predicate<XElement> match) where T : class, new()
		{
			return DeleteXmlElements(filePath, typeof(T).Name, match);
		}

		/// <summary>
		/// 删除xml文件中匹配项的数据
		/// </summary>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <returns></returns>
		public static bool DeleteXmlElements(string filePath, string elementName)
		{
			return DeleteXmlElements(filePath, elementName, null);
		}

		/// <summary>
		/// 删除xml文件中匹配项的数据
		/// </summary>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool DeleteXmlElements(string filePath, string elementName, Predicate<XElement> match)
		{
			bool isOK = false;
			XElement rootElement = GetRootElement(filePath).Key;
			if (DeleteElementsFromXml(rootElement, elementName, match))
			{
				isOK = SaveXmlElement(rootElement, filePath);
			}
			return isOK;
		}

		#endregion

		#endregion

		#region 公用部分

		#region 集合类型

		/// <summary>
		/// 获取xml文件的根节点
		/// </summary>
		/// <param name="filePath">xml文件的路径</param>
		/// <returns></returns>
		private static KeyValuePair<XElement, bool> GetRootElement(string filePath)
		{
			KeyValuePair<XElement, bool> keyValuePair = new KeyValuePair<XElement, bool>();
			try
			{
				XElement rootElement = null;
				if (File.Exists(filePath))
				{
					rootElement = XElement.Load(filePath);
					keyValuePair = new KeyValuePair<XElement, bool>(rootElement, true);
				}
			}
			catch (Exception ex)
			{
			}
			return keyValuePair;
		}

		/// <summary>
		/// 在父元素下按文档顺序返回此元素或文档的经过筛选的子元素集合。集合中只包括具有匹配 System.Xml.Linq.XName 的元素。
		/// </summary>
		/// <param name="parentElement">父元素</param>
		/// <param name="elementName">元素名</param>
		/// <returns>实例集合</returns>
		private static IEnumerable<XElement> GetXmlElementsInParent(XElement parentElement, string elementName)
		{
			IEnumerable<XElement> elements = new List<XElement>();
			if (parentElement != null)
			{
				elements = parentElement.Elements(elementName);
				if (elements.Count() == 0 && parentElement.Name == elementName)
				{
					elements = new List<XElement>() { parentElement };
				}
			}
			return elements;
		}

		/// <summary>
		/// 在父元素下按文档顺序返回此元素或文档的经过筛选的子元素第一个集合。集合中只包括具有匹配 System.Xml.Linq.XName 的元素。
		/// </summary>
		/// <param name="parentElement">父元素</param>
		/// <param name="elementName">元素名</param>
		/// <returns>实例</returns>
		private static XElement GetXmlElementInParent(XElement parentElement, string elementName)
		{
			XElement element = null;
			if (parentElement != null)
			{
				element = parentElement.Element(elementName);
				if (element == null && parentElement.Name == elementName)
				{
					element = parentElement;
				}
			}
			return element;
		}

		#endregion

		#region Json对象

		/// <summary>
		/// 根据JToken对象生成JArray
		/// </summary>
		/// <param name="jToken"></param>
		/// <returns></returns>
		private static JArray GetJArrayByJToken(JToken jToken)
		{
			JArray jArray = null;
			if (jToken != null)
			{
				jArray = jToken as JArray;
				if (jArray == null)
				{
					jArray = new JArray(jToken);
				}
			}
			return jArray;
		}

		/// <summary>
		/// 获取匹配的元素集合
		/// </summary>
		/// <param name="parentElement">父元素</param>
		/// <param name="elementName">元素名</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns>元素集合</returns>
		private static IEnumerable<XElement> GetMatchElement(XElement parentElement, string elementName, Predicate<XElement> match)
		{
			IEnumerable<XElement> childElements = GetXmlElementsInParent(parentElement, elementName);
			var matchElements = (from ele in childElements
								 where match == null || match(ele)
								 select ele);
			return matchElements;
		}

		/// <summary>
		/// 获取对象的克隆
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="originObject">克隆对象</param>
		/// <returns>克隆后的对象</returns>
		private static T GetObjectClone<T>(T originObject)
		{
			string objectString = JsonConvert.SerializeObject(originObject);
			T cloneObject = JsonConvert.DeserializeObject<T>(objectString);
			return cloneObject;
		}

		private static object GetJsonObjectValue(JToken jToken)
		{
			JValue jValue = jToken as JValue;
			if (jValue == null)
			{
				return jToken;
			}
			return jValue.Value;
		}

		#endregion

		#region Get

		/// <summary>
		/// xml文件中是否存在匹配项的元素
		/// </summary>
		/// <param name="rootElement">根元素</param>
		/// <param name="elementName">元素名</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		private static bool ExistElementInXml(XElement rootElement, string elementName, Predicate<XElement> match)
		{
			bool isExist = false;
			if (rootElement != null)
			{
				IEnumerable<XElement> matchElements = GetXmlElementsInParent(rootElement, elementName);
				IEnumerable<XElement> elements = from ele in matchElements
												 where match == null || match(ele)
												 select ele;
				isExist = elements != null && elements.Count() > 0;
			}
			return isExist;
		}

		/// <summary>
		/// 获取xml文件中的元素指定类型的实例集合
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="parentElement">父元素</param>
		/// <param name="elementName">元素名</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns>实例集合</returns>
		private static List<T> GetXmlElementList<T>(XElement parentElement, string elementName, Predicate<string> match) where T : class, new()
		{
			List<T> result = new List<T>();
			try
			{
				if (parentElement != null)
				{
					Type type = typeof(T);
					IEnumerable<XElement> xElements = GetXmlElementsInParent(parentElement, elementName);
					var temp = GetXmlElementListByType(type, xElements, match);
					if (temp != null)
					{
						T element = temp as T;
						if (element != null)
						{
							result.Add(element);
						}
						else
						{
							Array array = temp as Array;
							foreach (var item in array)
							{
								element = item as T;
								if (element != null)
								{
									result.Add(element);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
			}
			return result;
		}

		/// <summary>
		/// 获取元素集合中指定类型的实例数据
		/// </summary>
		/// <param name="elementType">类型</param>
		/// <param name="elements">元素集合</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns>实例数据</returns>
		private static object GetXmlElementListByType(Type elementType, IEnumerable<XElement> elements, Predicate<string> match)
		{
			Array targets = null;
			object target = null;
			int count = elements.Count();
			if (elements != null && count > 0)
			{
				targets = Array.CreateInstance(elementType, count);
				int index = 0;
				foreach (var ele in elements)
				{
					target = Activator.CreateInstance(elementType);
					PropertyInfo[] propertys = target.GetType().GetProperties();
					//获取数组和集合的属性
					IEnumerable<PropertyInfo> propertyInfos = propertys.Where(p => p.PropertyType.GetInterface("ICollection", false) != null);
					foreach (PropertyInfo property in propertys)
					{
						if (!property.CanWrite)
						{
							continue;
						}
						string propertyName = property.Name;
						object value = null;
						Type propType = property.PropertyType;
						//是否是自定义类
						bool isCustomClass = propType != typeof(object) && Type.GetTypeCode(propType) == TypeCode.Object;
						bool isCollection = propertyInfos.Contains(property);
						if (match == null || match(propertyName))
						{
							if (isCollection || isCustomClass)
							{
								Type type = null;
								if (propType.IsArray)
								{
									type = propType.GetElementType();
								}
								else if (propType.IsGenericType)
								{
									//未实现反射赋值
									//type = propType.GetGenericArguments()[0];
								}
								else if (isCustomClass)
								{
									type = propType;
								}
								if (type != null)
								{
									IEnumerable<XElement> elementList = GetXmlElementsInParent(ele, type.Name);
									object temp = GetXmlElementListByType(type, elementList, match);
									if (temp != null && propType.IsArray)
									{
										value = GetArrayByData(temp, type);
									}
									else if (isCustomClass)
									{
										value = temp;
									}
								}
							}
							else
							{
								XAttribute tempAttribute = ele.Attribute(propertyName);
								if (tempAttribute != null)
								{
									value = tempAttribute.Value;
								}
								else
								{
									XElement xElement = ele.Element(propertyName);
									if (xElement != null)
									{
										value = xElement.Value;
									}
								}
							}
							if (value != null && !string.IsNullOrEmpty(value.ToString()))
							{
								try
								{
									if (!propType.IsGenericType)
									{
										//非泛型
										property.SetValue(target, Convert.ChangeType(value, propType));
									}
									else
									{
										//泛型Nullable<>
										Type genericTypeDefinition = propType.GetGenericTypeDefinition();
										if (genericTypeDefinition == typeof(Nullable<>))
										{
											property.SetValue(target, Convert.ChangeType(value, Nullable.GetUnderlyingType(propType)));
										}
									}
								}
								catch (Exception ex)
								{

								}
							}
						}
					}
					targets.SetValue(target, index);
					index++;
				}
			}
			return count > 1 ? targets : target;
		}

		/// <summary>
		/// 获取反射的属性值转换为数组
		/// </summary>
		/// <param name="obj">反射的属性值</param>
		/// <param name="type">数组类型</param>
		/// <returns>数组</returns>
		private static Array GetArrayByData(object obj, Type type)
		{
			Array array = null;
			ICollection collection = obj as ICollection;
			if (collection == null && obj != null)
			{
				collection = new object[] { obj };
			}
			if (collection != null)
			{
				int count = collection.Count;
				if (count > 0)
				{
					array = Array.CreateInstance(type, count);
					int index = 0;
					foreach (var item in collection)
					{
						array.SetValue(item, index);
						index++;
					}
				}
			}
			return array;
		}

		#endregion

		#region Add

		#region 公用

		/// <summary>
		/// 获取目标元素
		/// </summary>
		/// <param name="rootElement">根元素</param>
		/// <param name="elementName">元素名</param>
		/// <returns></returns>
		private static XElement GetTargetElement(XElement rootElement, string elementName, Func<XElement, XElement> parentElementFunc)
		{
			XElement xElement = null;
			if (!string.IsNullOrEmpty(elementName))
			{
				if (rootElement == null)
					xElement = new XElement(elementName);
				else
					xElement = GetXmlElementInParent(rootElement, elementName);
			}
			xElement = parentElementFunc != null ? parentElementFunc(xElement) : xElement;
			return xElement;
		}

		/// <summary>
		/// 创建xml文件父目录
		/// </summary>
		/// <param name="filePath">xml文件路径</param>
		private static void CreateXmlDirectory(string filePath)
		{
			FileInfo fileInfo = new FileInfo(filePath);
			DirectoryInfo directoryInfo = fileInfo.Directory;
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
		}

		#endregion

		#region 集合类型

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="rootElement">根元素</param>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <param name="parentElementFunc">要修改成的数据集合</param>
		/// <returns></returns>
		private static XElement AddXmlElementsBytDataCollection<T>(XElement rootElement, string elementName, IEnumerable<T> dataList, Func<XElement, XElement> parentElementFunc) where T : class, new()
		{
			try
			{
				if (dataList != null && dataList.Count() > 0)
				{
					XElement xElement = GetTargetElement(rootElement, elementName, parentElementFunc);
					rootElement = AddElementListToXml(xElement, dataList);
				}
			}
			catch
			{
			}
			return rootElement;
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <param name="parentElement">父元素</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		private static XElement AddElementListToXml(XElement parentElement, IEnumerable dataList)
		{
			if (dataList != null)
			{
				foreach (var item in dataList)
				{
					Type objectType = item.GetType();
					XElement element = new XElement(objectType.Name);
					PropertyInfo[] propertyInfos = objectType.GetProperties();
					//获取数组和集合的属性
					IEnumerable<PropertyInfo> propertyInfoList = propertyInfos.Where(p => p.PropertyType.GetInterface("ICollection", false) != null);
					foreach (PropertyInfo propertyInfo in propertyInfos)
					{
						object propertyValue = propertyInfo.GetValue(item);
						if (propertyValue == null)
							continue;
						Type propType = propertyInfo.PropertyType;
						//是否是自定义类
						bool isCustomClass = propType != typeof(object) && Type.GetTypeCode(propType) == TypeCode.Object;
						bool isCollection = propertyInfoList.Contains(propertyInfo);
						if ((isCollection || isCustomClass))
						{
							object temp = null;
							if (!isCollection)
							{
								temp = new object[] { propertyValue };
							}
							else
							{
								temp = propertyValue;
							}
							element = AddElementListToXml(element, temp as IEnumerable);
						}
						else
						{
							var attribute = propertyInfo.GetCustomAttribute<XmlAttributeAttribute>();
							if (attribute != null)
							{
								element.SetAttributeValue(propertyInfo.Name, propertyValue);
							}
							else
							{
								element.SetElementValue(propertyInfo.Name, propertyValue);
							}
						}
					}
					if (parentElement != null)
					{
						parentElement.Add(element);
					}
					else
					{
						parentElement = element;
					}
				}
			}
			return parentElement;
		}

		#endregion

		#region Json对象

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <param name="rootElement">根元素</param>
		/// <param name="elementName">元素名</param>
		/// <param name="jsonString">要修改成的Json数据</param>
		/// <param name="parentElementFunc">要修改成的数据集合</param>
		/// <returns></returns>
		private static XElement AddXmlElementsBytJsonData(XElement rootElement, string elementName, string jsonString, Func<XElement, XElement> parentElementFunc)
		{
			try
			{
				if (!string.IsNullOrEmpty(jsonString))
				{
					XElement xElement = GetTargetElement(rootElement, elementName, parentElementFunc);
					JToken jToken = JsonConvert.DeserializeObject(jsonString) as JToken;
					JArray jArray = GetJArrayByJToken(jToken);
					rootElement = AddElementListToXmlByJson(xElement, elementName, jArray);
				}
			}
			catch (Exception ex)
			{
			}
			return rootElement;
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <param name="parentElement">父元素</param>
		/// <param name="jArray">要修改成的数据集合</param>
		/// <returns></returns>
		private static XElement AddElementListToXmlByJson(XElement parentElement, string elementName, JArray jArray)
		{
			int count = jArray != null ? jArray.Count : 0;
			if (jArray != null && count > 0)
			{
				foreach (var item in jArray)
				{
					JObject jObject = item as JObject;
					if (jObject == null)
						continue;
					XElement element = null;
					//if (jObject.Count > 1)
					//	element = new XElement(elementName);
					foreach (var keyValue in jObject)
					{
						var jsonKey = keyValue.Key;
						var jsonValue = keyValue.Value;
						bool isObject = jsonValue.Type == JTokenType.Object;
						bool isArray = jsonValue.Type == JTokenType.Array;
						if ((isObject || isArray))
						{
							JArray JArrayValue = null;
							if (isObject)
							{
								JArrayValue = GetJArrayByJToken(jsonValue);
							}
							else
							{
								JArrayValue = jsonValue as JArray;
							}
							element = AddElementListToXmlByJson(element, jsonKey, JArrayValue);
						}
						else
						{
							if (element == null)
								element = new XElement(elementName);
							bool isAttribute = jsonKey.StartsWith("@");
							if (isAttribute)
							{
								jsonKey = jsonKey.TrimStart('@');
							}
							if (isAttribute)
							{
								element.SetAttributeValue(jsonKey, GetJsonObjectValue(jsonValue));
							}
							else
							{
								element.SetElementValue(jsonKey, GetJsonObjectValue(jsonValue));
							}
						}
					}
					if (parentElement != null)
					{
						parentElement.Add(element);
					}
					else
					{
						parentElement = element;
					}
				}
			}
			return parentElement;
		}

		#endregion

		#endregion

		#region Edit

		#region 集合类型

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="parentElement">父元素</param>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据的集合</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="isReplace">是否替换</param>
		/// <returns></returns>
		private static bool EditXmlElementsByDataCollection<T>(XElement parentElement, string elementName, IEnumerable<T> dataList, Predicate<XElement> match, bool isReplace) where T : class, new()
		{
			bool isOK = false;
			try
			{
				if (parentElement != null)
				{
					//string elementString = JsonConvert.SerializeObject(parentElement);
					//var matchCopyElement = JsonConvert.DeserializeObject<XElement>(elementString);
					int dataCount = dataList != null ? dataList.Count() : 0;
					isOK = dataCount > 0 && EditElementListToXml(parentElement, elementName, dataList.ToList(), match, isReplace);
				}
			}
			catch (Exception ex)
			{ }
			return isOK;
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="parentElement">父元素</param>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据的集合</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="isReplace">是否替换</param>
		/// <returns></returns>
		private static bool EditElementListToXml(XElement parentElement, string elementName, IList dataList, Predicate<XElement> match, bool isReplace)
		{
			bool isOK = false;
			int dataCount = dataList != null ? dataList.Count : 0;
			if (dataCount == 0)
				return isOK;
			var matchElements = GetMatchElement(parentElement, elementName, match);
			var matchCopyElements = GetObjectClone(matchElements);
			int elementCount = matchElements != null ? matchElements.Count() : 0;
			if (dataCount > 0 && elementCount > 0)
			{
				for (int i = 0; i < elementCount && i < dataCount; i++)
				{
					var dataItem = dataList[i];
					Type type = dataItem.GetType();
					XElement originElement = matchElements.ElementAt(i);
					XElement originCopyElement = matchCopyElements.ElementAt(i);
					PropertyInfo[] propertyInfos = type.GetProperties();
					//获取数组和集合的属性
					IEnumerable<PropertyInfo> propertyInfoList = propertyInfos.Where(p => p.PropertyType.GetInterface("ICollection", false) != null);
					foreach (PropertyInfo propertyInfo in propertyInfos)
					{
						object propertyValue = propertyInfo.GetValue(dataItem);
						Type propType = propertyInfo.PropertyType;
						//是否是自定义类
						bool isCustomClass = propType != typeof(object) && Type.GetTypeCode(propType) == TypeCode.Object;
						bool isCollection = propertyInfoList.Contains(propertyInfo);
						if ((isCollection || isCustomClass) && propertyValue != null)
						{
							object temp = null;
							if (!isCollection)
							{
								temp = new object[] { propertyValue };
							}
							else
							{
								temp = propertyValue;
							}
							EditElementListToXml(originElement, propertyInfo.Name, temp as IList, match, isReplace);
						}
						else
						{
							XmlAttributeAttribute attribute = propertyInfo.GetCustomAttribute<XmlAttributeAttribute>();
							if (attribute != null && (!isReplace || (isReplace && originCopyElement.Attribute(propertyInfo.Name) != null)))
							{
								originElement.SetAttributeValue(propertyInfo.Name, propertyValue);
							}
							else if (attribute == null && (!isReplace || (isReplace && originCopyElement.Element(propertyInfo.Name) != null)))
							{
								originElement.SetElementValue(propertyInfo.Name, propertyValue);
							}
						}
					}
				}
				isOK = true;
			}
			return isOK;
		}

		#endregion

		#region Json对象

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <param name="parentElement">父元素</param>
		/// <param name="elementName">元素名</param>
		/// <param name="jsonString">要修改成的Json数据</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="isReplace">是否替换</param>
		/// <returns></returns>
		private static bool EditXmlElementsByJsonData(XElement parentElement, string elementName, string jsonString, Predicate<XElement> match, bool isReplace)
		{
			bool isOK = false;
			try
			{
				if (!string.IsNullOrEmpty(jsonString))
				{
					JToken jToken = JsonConvert.DeserializeObject(jsonString) as JToken;
					JArray jArray = GetJArrayByJToken(jToken);
					int dataCount = jArray != null ? jArray.Count : 0;
					isOK = parentElement != null && dataCount > 0 && EditXmlElementListByJson(parentElement, elementName, jArray, match, isReplace);
				}
			}
			catch (Exception ex)
			{ }
			return isOK;
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <param name="parentElement">父元素</param>
		/// <param name="elementName">元素名</param>
		/// <param name="jArray">要修改成的数据的集合</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="isReplace">是否替换</param>
		/// <returns></returns>
		private static bool EditXmlElementListByJson(XElement parentElement, string elementName, JArray jArray, Predicate<XElement> match, bool isReplace)
		{
			bool isOK = false;
			int dataCount = jArray != null ? jArray.Count : 0;
			var matchElements = GetMatchElement(parentElement, elementName, match);
			var matchCopyElements = GetObjectClone(matchElements);
			int elementCount = matchElements != null ? matchElements.Count() : 0;
			if (dataCount > 0 && elementCount > 0)
			{
				for (int i = 0; i < elementCount && i < dataCount; i++)
				{
					XElement originElement = matchElements.ElementAt(i);
					XElement originCopyElement = matchCopyElements.ElementAt(i);
					JObject jObject = jArray[i] as JObject;
					if (jObject != null)
					{
						foreach (var item in jObject)
						{
							var jsonKey = item.Key;
							var jsonValue = item.Value;
							bool isObject = jsonValue.Type == JTokenType.Object;
							bool isArray = jsonValue.Type == JTokenType.Array;
							if ((isObject || isArray) && jsonValue.HasValues)
							{
								JArray JArrayValue = null;
								if (isObject)
								{
									JArrayValue = GetJArrayByJToken(jsonValue);
								}
								else
								{
									JArrayValue = jsonValue as JArray;
								}
								EditXmlElementListByJson(originElement, item.Key, JArrayValue, match, isReplace);
							}
							else
							{
								bool isAttribute = jsonKey.StartsWith("@");
								if (isAttribute)
								{
									jsonKey = jsonKey.TrimStart('@');
								}
								if (isAttribute && (!isReplace || (isReplace && originCopyElement.Attribute(jsonKey) != null)))
								{
									originElement.SetAttributeValue(jsonKey, GetJsonObjectValue(jsonValue));
								}
								else if (!isAttribute && (!isReplace || (isReplace && originCopyElement.Element(jsonKey) != null)))
								{
									originElement.SetElementValue(jsonKey, GetJsonObjectValue(jsonValue));
								}
							}
						}
					}
				}
				isOK = true;
			}
			return isOK;
		}

		#endregion

		#endregion

		#region Delete

		/// <summary>
		/// 删除xml文件中匹配项的数据
		/// </summary>
		/// <param name="rootElement">根元素</param>
		/// <param name="elementName">元素名</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		private static bool DeleteElementsFromXml(XElement rootElement, string elementName, Predicate<XElement> match)
		{
			bool isOK = false;
			try
			{
				if (rootElement != null)
				{
					IEnumerable<XElement> elements = from ele in rootElement.Elements(elementName)
													 where match == null || match(ele)
													 select ele;
					if (elements.Count() > 0)
					{
						elements.Remove();
						isOK = true;
					}
				}
			}
			catch
			{ }
			return isOK;
		}

		#endregion

		#endregion
	}
}