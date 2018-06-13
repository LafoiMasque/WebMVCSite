using System;
using System.Collections;
using System.Reflection;

namespace WebSite.Common.UtilityClass
{
	public class ReflectionClone
	{
		// 利用反射实现深拷贝
		public static T DeepCopyWithReflection<T>(T obj)
		{
			Type type = obj.GetType();

			// 如果是字符串或值类型则直接返回
			if (obj is string || type.IsValueType) return obj;

			//if (type.IsArray)
			//{
			//	string typeFullName = type.FullName.Replace("[]", string.Empty);
			//	Type elementType = Type.GetType(typeFullName);
			//	if (elementType == null)
			//	{
			//		elementType = type.Assembly.GetType(typeFullName);
			//	}
			//	var array = obj as Array;
			//	Array copied = Array.CreateInstance(elementType, array.Length);
			//	for (int i = 0; i < array.Length; i++)
			//	{
			//		copied.SetValue(DeepCopyWithReflection(array.GetValue(i)), i);
			//	}
			//	return (T)Convert.ChangeType(copied, type);
			//}

			var list = obj as IList;
			if (list != null)
			{
				Array copied = null;
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					if (i == 0)
					{
						copied = Array.CreateInstance(list[0].GetType(), count);
					}
					copied.SetValue(DeepCopyWithReflection(list[0]), i);
				}
				return (T)Convert.ChangeType(copied, type);
			}

			object retval = Activator.CreateInstance(obj.GetType());

			PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
			foreach (var property in properties)
			{
				var propertyValue = property.GetValue(obj, null);
				if (propertyValue == null)
					continue;
				property.SetValue(retval, DeepCopyWithReflection(propertyValue), null);
			}

			return (T)retval;
		}
	}
}
