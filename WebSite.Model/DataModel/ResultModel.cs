using System;
using WebSite.Model.EnumExtention;
using WebSite.Model.EnumType;

namespace WebSite.Model.DataModel
{
	[Serializable]
	public class ResultModel<T>
	{
		public int StatusCode { get; private set; }
		public string Message { get; private set; }
		public T Data { get; private set; }

		public ResultModel() : this(ResultCodeEnum.Success)
		{
		}

		public ResultModel(ResultCodeEnum resultCodeEnum) : this(resultCodeEnum, resultCodeEnum.Display())
		{
		}

		public ResultModel(ResultCodeEnum resultCodeEnum, string message) : this((int)resultCodeEnum, message, default(T))
		{
		}

		public ResultModel(T data) : this(ResultCodeEnum.Success.Display(), data)
		{
		}

		//public ResultModel(ResultCodeEnum resultCodeEnum, T data) : this((int)resultCodeEnum, resultCodeEnum.Display(), data)
		//{
		//}

		public ResultModel(string message, T data) : this((int)ResultCodeEnum.Success, message, data)
		{
		}

		public ResultModel(ResultCodeEnum resultCodeEnum, string message, T data) : this((int)resultCodeEnum, message, data)
		{
		}

		public ResultModel(int statusCode, string message, T data)
		{
			StatusCode = statusCode;
			Message = message;
			Data = data;
		}

	}
}
