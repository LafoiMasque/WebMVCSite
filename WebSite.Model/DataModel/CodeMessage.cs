using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.Model.DataModel
{
	public class CodeMessage
	{
		public int StatusCode { get; private set; }
		public string Message { get; private set; }

		private static readonly List<KeyValuePair<int, string>> m_keyValuePair = null;

		static CodeMessage()
		{
			m_keyValuePair = new List<KeyValuePair<int, string>>()
			{
				// 通用异常  
				new KeyValuePair<int, string>(200,"操作成功"),
				new KeyValuePair<int, string>(201,"操作失败"),
				new KeyValuePair<int, string>(500100, "服务端异常"),
				new KeyValuePair<int, string>(500101, "输入参数为空"),
				// 业务异常  
				new KeyValuePair<int, string>(500102, "用户不存在"),
				new KeyValuePair<int, string>(500103, "在线用户数超出允许登录的最大用户限制。"),
				new KeyValuePair<int, string>(500104, "不存在离线session数据"),
				new KeyValuePair<int, string>(500105, "查找不到对应数据"),
			};
		}

		//public CodeMessage(ResultCodeEnum resultCodeEnum)
		//{
		//	KeyValuePair<int, string> keyValuePair = m_keyValuePair.FirstOrDefault(o => o.Key == (int)resultCodeEnum);
		//	Code = keyValuePair.Key;
		//	Message = keyValuePair.Value;
		//}

		public CodeMessage(int code, string message)
		{
			StatusCode = code;
			Message = message;
		}

		public CodeMessage(ResultCodeEnum resultCodeEnum)
		{
			StatusCode = GetStatusCode(resultCodeEnum);
			Message = GetMessage(resultCodeEnum);
		}

		public CodeMessage(ResultCodeEnum resultCodeEnum, string message) : this((int)resultCodeEnum, message)
		{
		}

		public static int GetStatusCode(ResultCodeEnum resultCodeEnum)
		{
			return (int)resultCodeEnum;
		}

		public static string GetMessage(ResultCodeEnum resultCodeEnum)
		{
			return m_keyValuePair.FirstOrDefault(o => o.Key == (int)resultCodeEnum).Value;
		}
	}
}
