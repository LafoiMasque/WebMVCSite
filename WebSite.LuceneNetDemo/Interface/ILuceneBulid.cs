using System.Collections.Generic;
using WebSite.LuceneNetDemo.Model;

namespace WebSite.LuceneNetDemo.Interface
{
	public interface ILuceneBulid<T> where T : class, new()
	{
		/// <summary>
		/// 批量创建索引
		/// </summary>
		/// <param name="modelList"></param>
		/// <param name="fieldModelList"></param>
		/// <param name="pathSuffix">索引目录后缀，加在电商的路径后面，为空则为根目录.如sa\1</param>
		/// <param name="isCreate">默认为false 增量索引  true的时候删除原有索引</param>
		void BuildIndex(IEnumerable<T> modelList, IEnumerable<FieldDataModel> fieldModelList, string pathSuffix = "", bool isCreate = false);

		/// <summary>
		/// 将索引合并到上级目录
		/// </summary>
		/// <param name="sourceDir">子文件夹名</param>
		void MergeIndex(string[] sourceDirs);

		/// <summary>
		/// 新增一条数据的索引
		/// </summary>
		/// <param name="model"></param>
		/// <param name="fieldModelList"></param>
		void InsertIndex(T model, IEnumerable<FieldDataModel> fieldModelList);

		/// <summary>
		/// 批量新增数据的索引
		/// </summary>
		/// <param name="modelList">sourceflag统一的</param>
		/// <param name="fieldModelList"></param>
		void InsertIndexMultitude(IEnumerable<T> modelList, IEnumerable<FieldDataModel> fieldModelList);

		/// <summary>
		/// 删除一条数据的索引
		/// </summary>
		/// <param name="model"></param>
		/// <param name="propertyName"></param>
		void DeleteIndex(T model, string propertyName);

		/// <summary>
		/// 批量删除数据的索引
		/// </summary>
		/// <param name="modelList">sourceflag统一的</param>
		/// <param name="propertyName"></param>
		void DeleteIndexMultitude(IEnumerable<T> modelList, string propertyName);

		/// <summary>
		/// 更新一条数据的索引
		/// </summary>
		/// <param name="model"></param>
		/// <param name="propertyName"></param>
		/// <param name="fieldModelList"></param>
		void UpdateIndex(T model, string propertyName, IEnumerable<FieldDataModel> fieldModelList);

		/// <summary>
		/// 批量更新数据的索引
		/// </summary>
		/// <param name="modelList">sourceflag统一的</param>
		/// <param name="propertyName"></param>
		/// <param name="fieldModelList"></param>
		void UpdateIndexMultitude(IEnumerable<T> modelList, string propertyName, IEnumerable<FieldDataModel> fieldModelList);
	}
}