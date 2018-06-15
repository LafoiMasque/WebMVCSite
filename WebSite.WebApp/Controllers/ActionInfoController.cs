using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.IBLL.SingletonPattern;
using WebSite.Model.DataBaseModel;
using WebSite.Model.DataModel;
using WebSite.Model.EnumType;
using WebSite.WebApp.CustomAttribute;

namespace WebSite.WebApp.Controllers
{
	[AuthenLogin]
	public class ActionInfoController : Controller
	{
		public IActionInfoService ActionInfoService { get; set; }

		// GET: ActionInfo
		public ActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// 获取权限信息
		/// </summary>
		/// <returns></returns>
		public ActionResult GetActionInfoList()
		{
			string value = Request["page"];
			int pageIndex = value != null ? int.Parse(value) : 1;
			int pageSize = value != null ? int.Parse(value) : 5;
			int totalCount;
			byte stateFlag = (byte)DeleteTypeEnum.Normarl;
			var actionInfoList = ActionInfoService.LoadPageEntities(pageIndex, pageSize, out totalCount, o => o.StateFlag == stateFlag, o => o.Id, true);
			var temp = from r in actionInfoList
					   select new
					   {
						   r.Id,
						   r.ActionName,
						   r.Sort,
						   r.CreateTime,
						   r.Remark,
						   r.Url,
						   r.ActionTypeEnum,
						   r.HttpMethod
					   };
			return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// 获取上传的文件
		/// </summary>
		/// <returns></returns>
		public ActionResult GetFileUp()
		{
			ResultModel<string> resultModel = null;
			HttpPostedFileBase file = Request.Files["fileUp"];
			string fileName = Path.GetFileName(file.FileName);
			string fileExt = Path.GetExtension(fileName);
			if (fileExt == ".jpg")
			{
				string dir = "/ImageIcon/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";
				Directory.CreateDirectory(Path.GetDirectoryName(Request.MapPath(dir)));
				string newfileName = Guid.NewGuid().ToString();
				string fullDir = dir + newfileName + fileExt;
				file.SaveAs(Request.MapPath(fullDir));
				//自己加上图片的缩略图
				resultModel = new ResultModel<string>(new CodeMessage(ResultCodeEnum.Success, fullDir));
			}
			else
			{
				resultModel = new ResultModel<string>(new CodeMessage(ResultCodeEnum.Failure, "文件类型错误!!"));
			}
			return Json(resultModel, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// 完成权限添加
		/// </summary>
		/// <param name="actionInfo"></param>
		/// <returns></returns>
		public ActionResult AddActionInfo(ActionInfo actionInfo)
		{
			actionInfo.StateFlag = 0;
			actionInfo.CreateTime = actionInfo.LastModifyTime = DateTime.Now;
			actionInfo.Url = actionInfo.Url.ToLower();
			ActionInfoService.AddEntity(actionInfo);
			ResultModel<string> resultModel = new ResultModel<string>();
			return Json(resultModel);
		}
	}
}