using System;

namespace WebSite.Model.CustomAttribute
{
	[AttributeUsage(AttributeTargets.Field)]
	public class EnumDisplayAttribute : Attribute
	{
		public EnumDisplayAttribute(string displayStr)
		{
			Display = displayStr;
		}
		public string Display
		{
			get;
			private set;
		}
	}
}
