using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSite.IDAL;
using WebSite.Model.DataBaseModel;

namespace WebSite.DAL
{
	public partial class KeyWordsRankDal : BaseDal<KeyWordsRank>, IKeyWordsRankDal
	{
		/// <summary>
		/// 将统计的明细表的数据插入。
		/// </summary>
		/// <returns></returns>
		public bool InsertKeyWordsRank()
		{
			string sql = "insert into KeyWordsRank(Id,KeyWords,SearchCount) select newid(),KeyWords,count(*)  from SearchDetails where DateDiff(day,SearchDetails.SearchTime,getdate())<=7 group by SearchDetails.KeyWords";
			return ExecuteSql(sql);
		}

		/// <summary>
		/// 删除汇总中的数据。
		/// </summary>
		/// <returns></returns>
		public bool DeleteAllKeyWordsRank()
		{
			string sql = "truncate table KeyWordsRank";
			return ExecuteSql(sql) ;
		}

		public List<string> GetSearchMsg(string term)
		{
			string sql = "select KeyWords from KeyWordsRank where KeyWords like @term";
			return ExecuteQueryList<string>(sql, new SqlParameter("@term", term + "%"));
		}
	}
}
