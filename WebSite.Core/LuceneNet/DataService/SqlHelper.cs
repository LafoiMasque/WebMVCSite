﻿using WebSite.Core.LuceneNet.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.Core.LuceneNet.DataService
{
	public class SqlHelper
	{
		//private static CustomLogger m_logger = new CustomLogger(typeof(SqlHelper));
		private static string ConnStr = ConfigurationManager.ConnectionStrings["mvc5"].ConnectionString;

		public static bool ExecuteNonQuery(string sql)
		{
			//try
			//{
			using (SqlConnection sqlConn = new SqlConnection(ConnStr))
			{
				sqlConn.Open();
				SqlCommand cmd = new SqlCommand(sql, sqlConn);
				return cmd.ExecuteNonQuery() > 0;//.ExecuteNonQueryAsync();//
			}
			//}
			//catch (Exception ex)
			//{
			//}
			//finally
			//{
			//}
		}

		public static List<T> QueryList<T>(string sql) where T : new()
		{
			using (SqlConnection sqlConn = new SqlConnection(ConnStr))
			{
				sqlConn.Open();
				SqlCommand cmd = new SqlCommand(sql, sqlConn);
				cmd.CommandTimeout = 120;
				return TransList<T>(cmd.ExecuteReader());
			}
		}

		public static void Insert<T>(T model, string tableName) where T : new()
		{
			string sql = GetInsertSql<T>(model, tableName);
			ExecuteNonQuery(sql);
		}

		public static void InsertList<T>(List<T> list, string tableName) where T : new()
		{
			string sql = string.Join(" ", list.Select(t => GetInsertSql<T>(t, tableName)));
			ExecuteNonQuery(sql);
		}

		#region Private

		private static string GetInsertSql<T>(T model, string tableName)
		{
			StringBuilder sbSql = new StringBuilder();

			StringBuilder sbFields = new StringBuilder();
			StringBuilder sbValues = new StringBuilder();

			Type type = model.GetType();
			var properties = type.GetProperties();
			foreach (PropertyInfo property in properties)
			{
				string name = property.Name;
				if (!name.Equals("id", StringComparison.OrdinalIgnoreCase))
				{
					sbFields.AppendFormat("[{0}],", name);
					sbValues.AppendFormat("'{0}',", property.GetValue(model));
				}
			}
			sbSql.AppendFormat("INSERT INTO {0} ({1}) VALUES ({2});", tableName, sbFields.ToString().TrimEnd(','), sbValues.ToString().TrimEnd(','));
			return sbSql.ToString();
		}

		private static List<T> TransList<T>(SqlDataReader reader) where T : new()
		{
			List<T> tList = new List<T>();
			Type type = typeof(T);
			var properties = type.GetProperties();
			if (reader.Read())
			{
				do
				{
					T t = new T();
					foreach (PropertyInfo property in properties)
					{
						property.SetValue(t, Convert.ChangeType(reader[property.Name], property.PropertyType));
					}
					tList.Add(t);
				}
				while (reader.Read());
			}
			return tList;
		}

		private static T TransModel<T>(SqlDataReader reader) where T : new()
		{
			T t = new T();
			if (reader.Read())
			{
				do
				{
					Type type = typeof(T);
					var properties = type.GetProperties();
					foreach (PropertyInfo p in properties)
					{
						p.SetValue(t, Convert.ChangeType(reader[p.Name], p.PropertyType));
					}
				}
				while (reader.Read());
			}
			return t;
		}

		#endregion Private
	}
}
