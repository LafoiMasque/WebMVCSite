using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebSite.Common.UtilityClass
{
	public class SqlHelper
	{
		#region 返回连接字符串

		/// <summary>
		/// 返回连接字符串
		/// </summary>
		/// <returns>连接字符串</returns>
		public static string GetSqlConnectionString()
		{
			return ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
		}

		#endregion

		#region 封装一个执行sql 返回受影响的行数

		/// <summary>
		/// 执行一个sql，返回影响行数
		/// </summary>
		/// <param name="sqlText">执行的sql脚本</param>
		/// <param name="parameters">参数集合</param>
		/// <returns>受影响的行数</returns>
		public static int ExecuteNonQuery(string sqlText, params SqlParameter[] parameters)
		{
			using (SqlConnection conn = new SqlConnection(GetSqlConnectionString()))
			{
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandText = sqlText;
					cmd.Parameters.AddRange(parameters);//把参数添加到cmd命令中。
					return cmd.ExecuteNonQuery();
				}
			}
		}

		#endregion

		#region 执行sql。返回 查询结果中的 第一行第一列的值

		/// <summary>
		/// 执行sql。返回 查询结果中的 第一行第一列的值
		/// </summary>
		/// <param name="sqlText"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static object ExecuteScalar(string sqlText, params SqlParameter[] parameters)
		{
			using (SqlConnection conn = new SqlConnection(GetSqlConnectionString()))
			{
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandText = sqlText;
					cmd.Parameters.AddRange(parameters);
					return cmd.ExecuteScalar();
				}
			}
		}

		/// <summary>
		/// 执行sql。返回 查询结果中的 第一行第一列的值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sqlText"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static T ExecuteScalar<T>(string sqlText, params SqlParameter[] parameters)
		{
			return (T)ExecuteScalar(sqlText, parameters);
		}

		#endregion

		#region 执行sql 返回一个DataTable

		/// <summary>
		/// 执行sql 返回一个DataTable
		/// </summary>
		/// <param name="sqlText"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static DataTable ExecuteDataTable(string sqlText, params SqlParameter[] parameters)
		{
			return ExecuteDataTable(sqlText, CommandType.Text, parameters);
		}

		#endregion

		#region 执行sql 脚本，返回一个SqlDataReader

		/// <summary>
		/// 执行sql 脚本，返回一个SqlDataReader
		/// </summary>
		/// <param name="sqlText"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static SqlDataReader ExecuteReader(string sqlText, params SqlParameter[] parameters)
		{
			//SqlDataReader要求，它读取数据的时候，它哟啊独占 它的SqlConnection对象，而且SqlConnection必须是Open状态。
			SqlConnection conn = new SqlConnection(GetSqlConnectionString());//不要释放连接，因为后面还要需要连接打开状态。
			SqlCommand cmd = conn.CreateCommand();
			conn.Open();
			cmd.CommandText = sqlText;
			cmd.Parameters.AddRange(parameters);
			//CommandBehavior.CloseConnection:代表，当SqlDataReader释放的时候，顺便把SqlConnection对象也释放掉。
			return cmd.ExecuteReader(CommandBehavior.CloseConnection);
		}

		#endregion

		#region 提供一个  可以 执行存储过程的方法。

		/// <summary>
		/// 提供一个  可以 执行存储过程的方法。
		/// </summary>
		/// <param name="sqlText"></param>
		/// <param name="commandType"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static DataTable ExecuteDataTable(string sqlText, CommandType commandType, params SqlParameter[] parameters)
		{
			using (SqlDataAdapter adapter = new SqlDataAdapter(sqlText, GetSqlConnectionString()))
			{
				DataTable dt = new DataTable();
				adapter.SelectCommand.Parameters.AddRange(parameters);
				//兼容存储过程
				adapter.SelectCommand.CommandType = commandType;
				adapter.Fill(dt);
				return dt;
			}
		}

		#endregion
	}
}
