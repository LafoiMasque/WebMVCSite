using WebSite.Model.CustomAttribute;

namespace WebSite.Model.EnumType
{
	public enum ResultCodeEnum
	{
		/// <summary>
		/// 操作成功
		/// </summary>
		[EnumDisplay("操作成功")]
		Success = 200,
		/// <summary>
		/// 操作失败
		/// </summary>
		[EnumDisplay("操作失败")]
		Failure = 201,
		/// <summary>
		/// 服务端异常
		/// </summary>
		[EnumDisplay("服务端异常")]
		Server_Exception = 500100,
		/// <summary>
		/// 输入参数为空
		/// </summary>
		[EnumDisplay("输入参数为空")]
		Parameter_IsNull = 500101,

		// 业务异常  
		/// <summary>
		/// 用户不存在
		/// </summary>
		[EnumDisplay("用户不存在")]
		User_Not_Exsist = 500102,
		/// <summary>
		/// 在线用户数超出允许登录的最大用户限制
		/// </summary>
		[EnumDisplay("在线用户数超出允许登录的最大用户限制")]
		Online_User_Over = 500103,
		/// <summary>
		/// 不存在离线session数据
		/// </summary>
		[EnumDisplay("不存在离线session数据")]
		Session_Not_Exsist = 500104,
		/// <summary>
		/// 查找不到对应数据
		/// </summary>
		[EnumDisplay("查找不到对应数据")]
		Not_Find_Data = 500105,
	}
}