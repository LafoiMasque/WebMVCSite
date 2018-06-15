using WebSite.IBLL.MashupPattern;
using WebSite.IDAL.MashupPattern;
using WebSite.Model.DataBaseModel;

namespace WebSite.BLL.MashupPattern
{
	public partial class UserInfoRoleInfoService : BaseMashupService<IUserInfoRoleInfoDal>, IUserInfoRoleInfoService
	{
		public void DoSomething()
		{
			CurrentDbSession.CreateInstanceDal.DoSomething();
			this.CurrentDal.DeleteEntity(new UserInfo());
			this.CurrentDal.DeleteEntity(new RoleInfo());
		}
	}
}
