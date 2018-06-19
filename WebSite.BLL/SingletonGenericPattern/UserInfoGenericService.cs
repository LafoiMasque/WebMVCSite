using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSite.BLL.SingletonPattern;
using WebSite.IBLL.SingletonGenericPattern;
using WebSite.IDAL.SingletonPattern;
using WebSite.Model.DataBaseModel;

namespace WebSite.BLL.SingletonGenericPattern
{

	public partial class UserInfoGenericService : BaseGenericService<IUserInfoDal, UserInfo>, IUserInfoGenericService
	{
		public void DoSomething()
		{

		}
	}
}
