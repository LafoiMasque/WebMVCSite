using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.Model.DataModel
{
	public enum ResultCodeEnum
	{
		Success = 200,
		Failure = 201,
		Server_Exception = 500100,
		Parameter_IsNull = 500101,
		User_Not_Exsist = 500102,
		Online_User_Over = 500103,
		Session_Not_Exsist = 500104,
		Not_Find_Data = 500105,
	}
}
