using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebSite.LuceneNetDemo.Interface;
using WebSite.LuceneNetDemo.Model;
using WebSite.LuceneNetDemo.Utility;

namespace WebSite.LuceneNetDemo.Service
{
	public class LuceneBulid<T> : ILuceneBulid<T> where T : class, new()
	{
		#region Identity

		private CustomLogger m_logger = new CustomLogger(typeof(LuceneBulid<T>));

		#endregion Identity

		#region 批量BuildIndex 索引合并

		/// <summary>
		/// 批量创建索引(要求是统一的sourceflag，即目录是一致的)
		/// </summary>
		/// <param name="modelList">sourceflag统一的</param>
		/// <param name="fieldModelList"></param>
		/// <param name="pathSuffix">索引目录后缀，加在电商的路径后面，为空则为根目录.如sa\1</param>
		/// <param name="isCreate">默认为false 增量索引  true的时候删除原有索引</param>
		public void BuildIndex(IEnumerable<T> modelList, IEnumerable<FieldDataModel> fieldModelList, string pathSuffix = "", bool isCreate = false)
		{
			IndexWriter writer = null;
			try
			{
				if (modelList == null || modelList.Count() == 0)
					return;

				string rootIndexPath = StaticConstant.IndexPath;
				string indexPath = string.IsNullOrWhiteSpace(pathSuffix) ? rootIndexPath : string.Format("{0}\\{1}", rootIndexPath, pathSuffix);

				System.IO.DirectoryInfo dirInfo = System.IO.Directory.CreateDirectory(indexPath);
				Directory directory = FSDirectory.Open(dirInfo);
				writer = new IndexWriter(directory, new PanGuAnalyzer(), isCreate, IndexWriter.MaxFieldLength.LIMITED);
				writer.SetMaxBufferedDocs(100);//控制写入一个新的segent前内存中保存的doc的数量 默认10  
				writer.MergeFactor = 100;//控制多个segment合并的频率，默认10
				writer.UseCompoundFile = true;//创建符合文件 减少索引文件数量

				foreach (var model in modelList)
				{
					CreateModelIndex(writer, model, fieldModelList);
				}
			}
			finally
			{
				if (writer != null)
				{
					//writer.Optimize(); 创建索引的时候不做合并  merge的时候处理
					writer.Dispose();
				}
			}
		}

		/// <summary>
		/// 将索引合并到上级目录
		/// </summary>
		/// <param name="sourceDir">子文件夹名</param>
		public void MergeIndex(string[] childDirs)
		{
			IndexWriter writer = null;
			try
			{
				if (childDirs == null || childDirs.Length == 0)
					return;
				Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
				string rootPath = StaticConstant.IndexPath;
				System.IO.DirectoryInfo dirInfo = System.IO.Directory.CreateDirectory(rootPath);
				Directory directory = FSDirectory.Open(dirInfo);
				writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED);//删除原有的
				Directory[] dirNo = childDirs.Select(dir => FSDirectory.Open(System.IO.Directory.CreateDirectory(string.Format("{0}\\{1}", rootPath, dir)))).ToArray();
				writer.MergeFactor = 100;//控制多个segment合并的频率，默认10
				writer.UseCompoundFile = true;//创建符合文件 减少索引文件数量
				writer.AddIndexesNoOptimize(dirNo);
			}
			finally
			{
				if (writer != null)
				{
					writer.Optimize();
					writer.Dispose();
				}
			}
		}

		#region FieldInfo

		//Field.Store.YES:存储字段值（未分词前的字段值）        
		//Field.Store.NO:不存储,存储与索引没有关系         
		//Field.Store.COMPRESS:压缩存储,用于长文本或二进制，但性能受损

		//Field.Index.ANALYZED:分词建索引         
		//Field.Index.ANALYZED_NO_NORMS:分词建索引，但是Field的值不像通常那样被保存，而是只取一个byte，这样节约存储空间         
		//Field.Index.NOT_ANALYZED:不分词且索引         
		//Field.Index.NOT_ANALYZED_NO_NORMS:不分词建索引，Field的值去一个byte保存

		//TermVector表示文档的条目（由一个Document和Field定位）和它们在当前文档中所出现的次数         
		//Field.TermVector.YES:为每个文档（Document）存储该字段的TermVector         
		//Field.TermVector.NO:不存储TermVector         
		//Field.TermVector.WITH_POSITIONS:存储位置        
		//Field.TermVector.WITH_OFFSETS:存储偏移量         
		//Field.TermVector.WITH_POSITIONS_OFFSETS:存储位置和偏移量

		#endregion

		#endregion 批量BuildIndex 索引合并

		#region 单个/批量索引增删改

		/// <summary>
		/// 新增一条数据的索引
		/// </summary>
		/// <param name="model"></param>
		/// <param name="fieldModelList"></param>
		public void InsertIndex(T model, IEnumerable<FieldDataModel> fieldModelList)
		{
			IndexWriter writer = null;
			try
			{
				if (model == null)
					return;
				string rootIndexPath = StaticConstant.IndexPath;
				System.IO.DirectoryInfo dirInfo = System.IO.Directory.CreateDirectory(rootIndexPath);

				bool isCreate = dirInfo.GetFiles().Count() == 0;//下面没有文件则为新建索引 
				Directory directory = FSDirectory.Open(dirInfo);
				writer = new IndexWriter(directory, CreateAnalyzerWrapper(fieldModelList), isCreate, IndexWriter.MaxFieldLength.LIMITED);
				writer.MergeFactor = 100;//控制多个segment合并的频率，默认10
				writer.UseCompoundFile = true;//创建符合文件 减少索引文件数量
				CreateModelIndex(writer, model, fieldModelList);
			}
			catch (Exception ex)
			{
				m_logger.Error("InsertIndex异常", ex);
			}
			finally
			{
				if (writer != null)
				{
					//if (fileNum > 50)
					//    writer.Optimize();
					writer.Dispose();
				}
			}
		}

		/// <summary>
		/// 批量新增数据的索引
		/// </summary>
		/// <param name="modelList"></param>
		/// <param name="fieldModelList"></param>
		public void InsertIndexMultitude(IEnumerable<T> modelList, IEnumerable<FieldDataModel> fieldModelList)
		{
			BuildIndex(modelList, fieldModelList, "", false);
		}

		/// <summary>
		/// 删除多条数据的索引
		/// </summary>
		/// <param name="model"></param>
		/// <param name="modelList"></param>
		public void DeleteIndex(T model, string propertyName)
		{
			IList<T> modelList = model != null ? new List<T>() { model } : null;
			DeleteIndexMultitude(modelList, propertyName);
		}

		/// <summary>
		/// 批量删除数据的索引
		/// </summary>
		/// <param name="modelList"></param>
		public void DeleteIndexMultitude(IEnumerable<T> modelList, string propertyName)
		{
			IndexReader reader = null;
			try
			{
				if (modelList == null || modelList.Count() == 0)
					return;
				Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
				string rootIndexPath = StaticConstant.IndexPath;
				System.IO.DirectoryInfo dirInfo = System.IO.Directory.CreateDirectory(rootIndexPath);
				Directory directory = FSDirectory.Open(dirInfo);
				reader = IndexReader.Open(directory, false);
				Type type = typeof(T);
				PropertyInfo propertyInfo = type.GetProperty(propertyName);
				foreach (T model in modelList)
				{
					var propertyValue = propertyInfo.GetValue(model);
					if (propertyValue != null)
						reader.DeleteDocuments(new Term(propertyName, propertyValue.ToString()));
				}
			}
			catch (Exception ex)
			{
				m_logger.Error("DeleteIndex异常", ex);
			}
			finally
			{
				if (reader != null)
				{
					reader.Dispose();
				}
			}
		}

		///// <summary>
		///// 更新一条数据的索引
		///// </summary>
		//public void UpdateIndex(T model)
		//{
		//    DeleteIndex(model);
		//    InsertIndex(model);
		//}

		/// <summary>
		/// 更新一条数据的索引
		/// </summary>
		/// <param name="model"></param>
		/// <param name="propertyName"></param>
		/// <param name="fieldModelList"></param>
		public void UpdateIndex(T model, string propertyName, IEnumerable<FieldDataModel> fieldModelList)
		{
			IList<T> modelList = model != null ? new List<T>() { model } : null;
			UpdateIndexMultitude(modelList, propertyName, fieldModelList);
		}

		/// <summary>
		/// 批量更新数据的索引
		/// </summary>
		/// <param name="modelList">sourceflag统一的</param>
		/// <param name="propertyName"></param>
		/// <param name="fieldModelList"></param>
		public void UpdateIndexMultitude(IEnumerable<T> modelList, string propertyName, IEnumerable<FieldDataModel> fieldModelList)
		{
			IndexWriter writer = null;
			try
			{
				if (modelList == null || modelList.Count() == 0)
					return;
				string rootIndexPath = StaticConstant.IndexPath;
				System.IO.DirectoryInfo dirInfo = System.IO.Directory.CreateDirectory(rootIndexPath);

				bool isCreate = dirInfo.GetFiles().Count() == 0;//下面没有文件则为新建索引 
				Directory directory = FSDirectory.Open(dirInfo);
				writer = new IndexWriter(directory, CreateAnalyzerWrapper(fieldModelList), isCreate, IndexWriter.MaxFieldLength.LIMITED);
				writer.MergeFactor = 50;//控制多个segment合并的频率，默认10
				writer.UseCompoundFile = true;//创建符合文件 减少索引文件数量
				Type type = typeof(T);
				PropertyInfo propertyInfo = type.GetProperty(propertyName);
				foreach (T model in modelList)
				{
					var propertyValue = propertyInfo.GetValue(model);
					if (propertyValue != null)
						writer.UpdateDocument(new Term(propertyName, propertyValue.ToString()), ParseModeltoDoc(model, fieldModelList));
				}
			}
			catch (Exception ex)
			{
				m_logger.Error("InsertIndex异常", ex);
			}
			finally
			{
				if (writer != null)
				{
					//if (fileNum > 50)
					//    writer.Optimize();
					writer.Dispose();
				}
			}
		}

		#endregion 单个索引增删改

		#region PrivateMethod

		/// <summary>
		/// 创建分析器
		/// </summary>
		/// <returns></returns>
		private PerFieldAnalyzerWrapper CreateAnalyzerWrapper(IEnumerable<FieldDataModel> fieldModelList)
		{
			Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

			PerFieldAnalyzerWrapper analyzerWrapper = new PerFieldAnalyzerWrapper(analyzer);
			foreach (var fieldModel in fieldModelList)
			{
				if (fieldModel.FieldType == TypeCode.String && (fieldModel.Index == Field.Index.ANALYZED || fieldModel.Index == Field.Index.ANALYZED_NO_NORMS))
				{
					analyzerWrapper.AddAnalyzer(fieldModel.FieldName, new PanGuAnalyzer());
				}
				else if ((fieldModel.FieldType == TypeCode.Single
					|| fieldModel.FieldType == TypeCode.Int32
					|| fieldModel.FieldType == TypeCode.Int64
					|| fieldModel.FieldType == TypeCode.Double)
					&& (fieldModel.Index == Field.Index.ANALYZED
					|| fieldModel.Index == Field.Index.ANALYZED_NO_NORMS))
				{
					analyzerWrapper.AddAnalyzer(fieldModel.FieldName, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30));
				}
			}
			return analyzerWrapper;
		}

		/// <summary>
		/// 创建索引
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="model"></param>
		/// <param name="fieldModelList"></param>
		private void CreateModelIndex(IndexWriter writer, T model, IEnumerable<FieldDataModel> fieldModelList)
		{
			try
			{
				writer.AddDocument(ParseModeltoDoc(model, fieldModelList));
			}
			catch (Exception ex)
			{
				m_logger.Error("CreateCIIndex异常", ex);
			}
		}

		/// <summary>
		/// 将T转换成doc
		/// </summary>
		/// <param name="model"></param>
		/// <param name="fieldModelList"></param>
		/// <returns></returns>
		private Document ParseModeltoDoc(T model, IEnumerable<FieldDataModel> fieldModelList)
		{
			Document document = new Document();
			Type type = model.GetType();
			foreach (var item in fieldModelList)
			{
				PropertyInfo propertyInfo = type.GetProperty(item.PropertyName);
				var propertyValue = propertyInfo.GetValue(model);
				if (propertyValue != null)
				{
					string valueString = propertyValue.ToString();
					IFieldable fieldable = null;
					if (item.FieldType == TypeCode.String)
					{
						fieldable = new Field(item.FieldName, valueString, item.Store, item.Index, item.TermVector);
					}
					else
					{
						NumericField numericField = new NumericField(item.FieldName, item.Store, item.Index == Field.Index.ANALYZED_NO_NORMS);
						switch (item.FieldType)
						{
							case TypeCode.Double:
								numericField.SetDoubleValue(Convert.ToDouble(valueString));
								break;
							case TypeCode.Single:
								numericField.SetFloatValue(Convert.ToSingle(valueString));
								break;
							case TypeCode.Int32:
								numericField.SetIntValue(Convert.ToInt32(valueString));
								break;
							case TypeCode.Int64:
								numericField.SetLongValue(Convert.ToInt64(valueString));
								break;
							default:
								break;
						}
						fieldable = numericField;
					}
					document.Add(fieldable);
				}
			}
			return document;
		}

		#endregion PrivateMethod
	}
}