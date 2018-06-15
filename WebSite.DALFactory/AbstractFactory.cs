using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebSite.Common.UtilityClass;
using WebSite.DALFactory.XmlModel;

namespace WebSite.DALFactory
{
	/// <summary>
	/// 通过反射的形式创建类的实例
	/// </summary>
	public class AbstractFactory
	{
		private static readonly List<EntityModel> m_entityModelList = null;

		static AbstractFactory()
		{
			string filePath = AppDomain.CurrentDomain.BaseDirectory + @"Config\EntityInfo.xml";
			m_entityModelList = XmlUtils.GetXmlElements<EntityModel>(filePath);
		}

		private static object CreateInstanceObject(string typeName, string assemblyPath)
		{
			Assembly assembly = Assembly.Load(assemblyPath);
			return assembly.CreateInstance(typeName);
		}

		public static T CreateInstanceDal<T>() where T : class
		{
			T result = null;
			if (m_entityModelList != null)
			{
				string dalKey = typeof(T).Name;
				EntityModel entityModel = m_entityModelList.FirstOrDefault(o => o.Key == dalKey);
				if (entityModel != null)
				{
					result = CreateInstanceObject(entityModel.FullName, entityModel.AssemblyPath) as T;
				}
			}
			return result;
		}
	}
}
