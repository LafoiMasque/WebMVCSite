using WebSite.LuceneNetDemo.Interface;
using WebSite.LuceneNetDemo.Model;
using WebSite.LuceneNetDemo.Service;
using WebSite.LuceneNetDemo.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSite.LuceneNetDemo.Processor
{
	public class IndexBuilderThread
	{
		private static CustomLogger m_logger = new CustomLogger(typeof(IndexBuilderThread));
		private static List<string> PathSuffixList = new List<string>();
		private static CancellationTokenSource CTS = null;

		public static void BuildIndexThread<T>(IList<EntryDataModel<T>> entryDataModelList) where T : class, new()
		{
			try
			{
				int count = entryDataModelList != null ? entryDataModelList.Count : 0;
				if (count > 0)
				{
					m_logger.Debug(string.Format("{0} BuildIndex开始", DateTime.Now));

					List<Task> taskList = new List<Task>();
					TaskFactory taskFactory = new TaskFactory();
					CTS = new CancellationTokenSource();

					for (int i = 1; i <= count; i++)
					{
						IndexBuilderPerThread<T> prerThread = null;
						var entryDataModel = entryDataModelList[i - 1];
						if (entryDataModel.DataListFunc != null)
						{
							prerThread = new IndexBuilderPerThread<T>(entryDataModel.DataListFunc, entryDataModel.FieldModelList, i.ToString("000"), CTS);
						}
						else 
						{
							prerThread = new IndexBuilderPerThread<T>(entryDataModel.DataList, entryDataModel.FieldModelList, i.ToString("000"), CTS);
						}
						PathSuffixList.Add(i.ToString("000"));
						taskList.Add(taskFactory.StartNew(prerThread.Process));//开启一个线程   里面创建索引
					}
					taskList.Add(taskFactory.ContinueWhenAll(taskList.ToArray(), MergeIndex<T>));
					Task.WaitAll(taskList.ToArray());
					m_logger.Debug(string.Format("BuildIndex{0}", CTS.IsCancellationRequested ? "失败" : "成功"));
				}
			}
			catch (Exception ex)
			{
				m_logger.Error("BuildIndex出现异常", ex);
			}
			finally
			{
				m_logger.Debug(string.Format("{0} BuildIndex结束", DateTime.Now));
			}
		}

		private static void MergeIndex<T>(Task[] tasks) where T : class, new()
		{
			try
			{
				if (CTS.IsCancellationRequested)
					return;
				ILuceneBulid<T> builder = new LuceneBulid<T>();
				builder.MergeIndex(PathSuffixList.ToArray());
			}
			catch (Exception ex)
			{
				CTS.Cancel();
				m_logger.Error("MergeIndex出现异常", ex);
			}
		}
	}
}

