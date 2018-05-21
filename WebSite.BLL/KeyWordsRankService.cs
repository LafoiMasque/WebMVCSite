using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSite.IBLL;
using WebSite.Model.DataBaseModel;

namespace WebSite.BLL
{
	public partial class KeyWordsRankService : BaseService<KeyWordsRank>, IKeyWordsRankService
	{
		/// <summary>
		/// 将统计的明细表的数据插入。
		/// </summary>
		/// <returns></returns>
		public bool InsertKeyWordsRank()
		{
			return CurrentDbSession.KeyWordsRankDal.InsertKeyWordsRank();
		}

		/// <summary>
		/// 删除汇总中的数据。
		/// </summary>
		/// <returns></returns>
		public bool DeleteAllKeyWordsRank()
		{
			return CurrentDbSession.KeyWordsRankDal.DeleteAllKeyWordsRank();
		}

		public List<string> GetSearchMsg(string term)
		{
			return CurrentDbSession.KeyWordsRankDal.GetSearchMsg(term);
		}
	}
}
