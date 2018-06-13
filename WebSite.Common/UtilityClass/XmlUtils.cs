using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace WebSite.Common.UtilityClass
{
	/// <summary>
	/// 操作Xml文件相关方法
	/// </summary>
	public class XmlUtils
	{
		//private XmlUtils()
		//{
		//}

		//private static XmlUtils _instance;

		//public static XmlUtils Instance
		//{
		//	get
		//	{
		//		if (_instance == null)
		//		{
		//			_instance = new XmlUtils();
		//		}
		//		return _instance;
		//	}
		//}

		#region 实例部分

		private XElement m_rootXElement = null;

		private string m_filePath = string.Empty;

		private bool m_authorityFile = false;

		public void BeginInitialize<T>(string filePath) where T : class, new()
		{
			try
			{
				m_filePath = filePath;
				if (File.Exists(filePath))
				{
					m_rootXElement = XElement.Load(filePath);
				}
				else
				{
					m_rootXElement = new XElement(typeof(T).Name + "s");
				}
				m_authorityFile = true;
			}
			catch
			{
			}
		}

		public void EndInitialize()
		{
			if (m_authorityFile)
				m_rootXElement.Save(m_filePath);
		}

		#region 查询

		/// <summary>
		/// xml文件中是否存在匹配项的元素
		/// </summary>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public bool ExistXmlElementInstance(Predicate<XElement> match)
		{
			bool isExist = false;
			if (m_authorityFile)
			{
				IEnumerable<XElement> elements = from ele in m_rootXElement.Elements()
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
		/// <returns>实例集合</returns>
		public List<T> GetXmlElementsInstance<T>() where T : class, new()
		{
			return GetXmlElementsInstance<T>(typeof(T).Name);
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
		public List<T> GetXmlElementsInstance<T>(string elementName, Predicate<XElement> match) where T : class, new()
		{
			List<T> ltTarget = null;
			if (m_authorityFile)
			{
				IEnumerable<XElement> elements = m_rootXElement.Elements(elementName);
				ltTarget = GetXmlElementsInstanceList<T>(elements, match);
			}
			return ltTarget;
		}

		#endregion

		#region 添加

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public bool AddXmlElementInstance<T>(string elementName, T model) where T : class, new()
		{
			return AddXmlElementsInstance<T>(elementName, new List<T>() { model });
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public bool AddXmlElementInstance<T>(T model) where T : class, new()
		{
			return AddXmlElementsInstance<T>(typeof(T).Name, new List<T>() { model });
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public bool AddXmlElementsInstance<T>(List<T> dataList) where T : class, new()
		{
			return AddXmlElementsInstance<T>(typeof(T).Name, dataList);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public bool AddXmlElementsInstance<T>(string elementName, List<T> dataList) where T : class, new()
		{
			bool isOK = false;
			if (m_authorityFile && dataList != null && dataList.Count > 0)
			{
				XElement parentElement = m_rootXElement.Element(elementName);
				XElement xElement = parentElement != null ? parentElement.Parent : m_rootXElement;
				if (AddXmlElementsDataList(xElement, dataList))
				{
					isOK = true;
				}
			}
			return isOK;
		}

		#endregion

		#region 修改

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public bool EditXmlElementInstance<T>(Predicate<XElement> match, T model) where T : class, new()
		{
			return EditXmlElementsInstance<T>(match, new List<T>() { model });
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public bool EditXmlElementsInstance<T>(Predicate<XElement> match, List<T> dataList) where T : class, new()
		{
			return EditXmlElementsByDataInstance<T>(match, dataList, false);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public bool ReplaceXmlElementInstance<T>(Predicate<XElement> match, T model) where T : class, new()
		{
			return ReplaceXmlElementsInstance<T>(match, new List<T>() { model });
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public bool ReplaceXmlElementsInstance<T>(Predicate<XElement> match, List<T> dataList) where T : class, new()
		{
			return EditXmlElementsByDataInstance<T>(match, dataList, true);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="dataList">要修改成的数据的集合</param>
		/// <param name="isReplace">是否替换</param>
		/// <returns></returns>
		private bool EditXmlElementsByDataInstance<T>(Predicate<XElement> match, List<T> dataList, bool isReplace) where T : class, new()
		{
			bool isOK = false;
			int dataCount = dataList != null ? dataList.Count : 0;
			if (m_authorityFile && dataCount > 0)
			{
				List<XElement> elements = (from ele in m_rootXElement.Elements()
										   where match == null || match(ele)
										   select ele).ToList();
				if (EditXmlElementsList<T>(elements, dataList, isReplace))
				{
					isOK = true;
				}
			}
			return isOK;
		}

		#endregion

		#region 删除

		/// <summary>
		/// 删除xml文件中匹配项的数据
		/// </summary>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public bool DeleteXmlElementsInstance(Predicate<XElement> match)
		{
			bool isOK = false;
			if (m_authorityFile)
			{
				IEnumerable<XElement> elements = from ele in m_rootXElement.Elements()
												 where match == null || match(ele)
												 select ele;
				if (elements.Count() > 0)
				{
					elements.Remove();
					isOK = true;
				}
			}
			return isOK;
		}

		#endregion

		#endregion

		#region 静态部分

		#region 查询

		/// <summary>
		/// xml文件中是否存在匹配项的元素
		/// </summary>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool ExistXmlElement(string filePath, Predicate<XElement> match)
		{
			bool isExist = false;
			if (File.Exists(filePath))
			{
				XElement xe = XElement.Load(filePath);
				IEnumerable<XElement> elements = from ele in xe.Elements()
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
		/// <param name="filePath">xml文件的路径</param>
		/// <returns>实例集合</returns>
		public static List<T> GetXmlElements<T>(string filePath) where T : class, new()
		{
			return GetXmlElements<T>(filePath, null);
		}

		/// <summary>
		/// 获取xml文件中的元素指定类型的实例集合
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns>实例集合</returns>
		public static List<T> GetXmlElements<T>(string filePath, Predicate<XElement> match) where T : class, new()
		{
			return GetXmlElements<T>(filePath, typeof(T).Name, match);
		}

		/// <summary>
		/// 获取xml文件中的元素指定类型的实例集合
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns>实例集合</returns>
		public static List<T> GetXmlElements<T>(string filePath, string elementName, Predicate<XElement> match) where T : class, new()
		{
			List<T> ltTarget = null;
			try
			{
				if (File.Exists(filePath))
				{
					XElement xe = XElement.Load(filePath);
					IEnumerable<XElement> elements = xe.Elements(elementName);
					ltTarget = GetXmlElementsInstanceList<T>(elements, match);
				}
			}
			catch
			{
			}
			return ltTarget;
		}

		#endregion

		#region 添加

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
			return AddXmlElements<T>(filePath, elementName, new List<T>() { model });
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public static bool AddXmlElement<T>(string filePath, T model) where T : class, new()
		{
			return AddXmlElements<T>(filePath, typeof(T).Name, new List<T>() { model });
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public static bool AddXmlElements<T>(string filePath, List<T> dataList) where T : class, new()
		{
			return AddXmlElements<T>(filePath, typeof(T).Name, dataList);
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="elementName">元素名</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public static bool AddXmlElements<T>(string filePath, string elementName, List<T> dataList) where T : class, new()
		{
			bool isOK = false;
			try
			{
				XElement rootElement = null;
				if (File.Exists(filePath))
				{
					rootElement = XElement.Load(filePath);
				}
				else
				{
					rootElement = new XElement(elementName + "s");
				}
				if (dataList != null && dataList.Count > 0)
				{
					XElement parentElement = rootElement.Element(elementName);
					XElement xElement = parentElement != null ? parentElement.Parent : rootElement;
					if (AddXmlElementsDataList(xElement, dataList))
					{
						rootElement.Save(filePath);
						isOK = true;
					}
				}
			}
			catch
			{
			}
			return isOK;
		}

		#endregion

		#region 修改

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public static bool EditXmlElement<T>(string filePath, Predicate<XElement> match, T model) where T : class, new()
		{
			return EditXmlElementsByDataList<T>(filePath, match, new List<T>() { model }, false);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public static bool EditXmlElements<T>(string filePath, Predicate<XElement> match, List<T> dataList) where T : class, new()
		{
			return EditXmlElementsByDataList<T>(filePath, match, dataList, false);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="model">要修改成的数据</param>
		/// <returns></returns>
		public static bool ReplaceXmlElement<T>(string filePath, Predicate<XElement> match, T model) where T : class, new()
		{
			return EditXmlElementsByDataList<T>(filePath, match, new List<T>() { model }, true);
		}

		/// <summary>
		/// 替换xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		public static bool ReplaceXmlElements<T>(string filePath, Predicate<XElement> match, List<T> dataList) where T : class, new()
		{
			return EditXmlElementsByDataList<T>(filePath, match, dataList, true);
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <param name="dataList">要修改成的数据的集合</param>
		/// <param name="isReplace">是否替换</param>
		/// <returns></returns>
		private static bool EditXmlElementsByDataList<T>(string filePath, Predicate<XElement> match, List<T> dataList, bool isReplace) where T : class, new()
		{
			bool isOK = false;
			try
			{
				int dataCount = dataList != null ? dataList.Count : 0;
				if (File.Exists(filePath) && dataCount > 0)
				{
					XElement xe = XElement.Load(filePath);
					List<XElement> elements = (from ele in xe.Elements()
											   where match == null || match(ele)
											   select ele).ToList();
					if (EditXmlElementsList<T>(elements, dataList, isReplace))
					{
						xe.Save(filePath);
						isOK = true;
					}
				}
			}
			catch
			{ }
			return isOK;
		}

		#endregion

		#region 删除

		/// <summary>
		/// 删除xml文件中匹配项的数据
		/// </summary>
		/// <param name="filePath">xml文件的路径</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns></returns>
		public static bool DeleteXmlElements(string filePath, Predicate<XElement> match)
		{
			bool isOK = false;
			try
			{
				if (File.Exists(filePath))
				{
					XElement xe = XElement.Load(filePath);
					IEnumerable<XElement> elements = from ele in xe.Elements()
													 where match == null || match(ele)
													 select ele;
					if (elements.Count() > 0)
					{
						elements.Remove();
						xe.Save(filePath);
						isOK = true;
					}
				}
			}
			catch
			{ }
			return isOK;
		}

		#endregion

		#region 公用方法

		/// <summary>
		/// 获取元素集合中指定类型的实例集合
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elements">元素集合</param>
		/// <param name="match">定义要搜索的元素的条件</param>
		/// <returns>实例集合</returns>
		private static List<T> GetXmlElementsInstanceList<T>(IEnumerable<XElement> elements, Predicate<XElement> match) where T : class, new()
		{
			List<T> ltTarget = null;
			if (elements != null && elements.Count() > 0)
			{
				ltTarget = new List<T>();
				foreach (var ele in elements)
				{
					if (match == null || match(ele))
					{
						T target = Activator.CreateInstance<T>();
						PropertyInfo[] propertys = target.GetType().GetProperties();
						foreach (PropertyInfo property in propertys)
						{
							string value = string.Empty;
							XAttribute tempAttribute = ele.Attribute(property.Name);
							if (tempAttribute != null)
							{
								value = tempAttribute.Value;
							}
							else
							{
								XElement xElement = ele.Element(property.Name);
								if (xElement != null)
								{
									value = xElement.Value;
								}
							}
							if (!string.IsNullOrEmpty(value))
							{
								if (!property.PropertyType.IsGenericType)
								{
									//非泛型
									property.SetValue(target, Convert.ChangeType(value, property.PropertyType));
								}
								else
								{
									//泛型Nullable<>
									Type genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();
									if (genericTypeDefinition == typeof(Nullable<>))
									{
										property.SetValue(target, Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType)));
									}
								}
							}
						}
						ltTarget.Add(target);
					}
				}
			}
			return ltTarget;
		}

		/// <summary>
		/// 添加xml文件中指定类型的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="parentElement">父元素</param>
		/// <param name="dataList">要修改成的数据集合</param>
		/// <returns></returns>
		private static bool AddXmlElementsDataList<T>(XElement parentElement, List<T> dataList) where T : class, new()
		{
			bool isOK = false;
			if (parentElement != null && dataList != null && dataList.Count > 0)
			{
				foreach (T item in dataList)
				{
					Type type = item.GetType();
					XElement element = new XElement(type.Name);
					PropertyInfo[] propertyInfos = type.GetProperties();
					foreach (PropertyInfo propertyInfo in propertyInfos)
					{
						var attribute = propertyInfo.GetCustomAttribute<System.Xml.Serialization.XmlAttributeAttribute>();
						if (attribute != null)
						{
							element.SetAttributeValue(propertyInfo.Name, propertyInfo.GetValue(item));
						}
						else
						{
							element.SetElementValue(propertyInfo.Name, propertyInfo.GetValue(item));
						}
					}
					parentElement.Add(element);
				}
				isOK = true;
			}
			return isOK;
		}

		/// <summary>
		/// 修改xml文件中指定类型匹配项的数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="elements">元素集合</param>
		/// <param name="dataList">要修改成的数据的集合</param>
		/// <param name="isReplace">是否替换</param>
		/// <returns></returns>
		private static bool EditXmlElementsList<T>(List<XElement> elements, List<T> dataList, bool isReplace) where T : class, new()
		{
			bool isOK = false;
			int dataCount = dataList != null ? dataList.Count : 0;
			if (dataCount > 0 && elements != null)
			{
				int elementCount = elements.Count;
				if (elementCount > 0)
				{
					for (int i = 0; i < elementCount && i < dataCount; i++)
					{
						Type type = dataList[i].GetType();
						XElement xElement = null;
						if (isReplace)
						{
							xElement = elements[i];
						}
						else
						{
							xElement = new XElement(type.Name);
						}
						PropertyInfo[] propertyInfos = type.GetProperties();
						foreach (PropertyInfo propertyInfo in propertyInfos)
						{
							var attribute = propertyInfo.GetCustomAttribute<System.Xml.Serialization.XmlAttributeAttribute>();
							if (attribute != null)
							{
								xElement.SetAttributeValue(propertyInfo.Name, propertyInfo.GetValue(dataList[i]));
							}
							else
							{
								xElement.SetElementValue(propertyInfo.Name, propertyInfo.GetValue(dataList[i]));
							}
						}
						elements[i] = xElement;
					}
					isOK = true;
				}
			}
			return isOK;
		}

		#endregion

		#endregion

	}
}
