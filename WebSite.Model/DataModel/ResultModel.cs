using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.Model.DataModel
{
	[Serializable]
	public class ResultModel<T>
	{
		public int StatusCode { get; private set; }
		public string Message { get; private set; }
		public T Data { get; private set; }

		public ResultModel()
		{
			StatusCode = CodeMessage.GetStatusCode(ResultCodeEnum.Success);
			Message = CodeMessage.GetMessage(ResultCodeEnum.Success);
		}

		public ResultModel(CodeMessage codeMessage)
		{
			if (codeMessage != null)
			{
				StatusCode = codeMessage.StatusCode;
				Message = codeMessage.Message;
			}
		}

		public ResultModel(T data) : this()
		{
			Data = data;
		}

		public ResultModel(CodeMessage codeMessage, T data) : this(codeMessage)
		{
			Data = data;
		}

	}
}
