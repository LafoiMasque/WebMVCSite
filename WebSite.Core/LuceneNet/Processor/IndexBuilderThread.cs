using WebSite.Core.LuceneNet.Interface;
using WebSite.Core.LuceneNet.Model;
using WebSite.Core.LuceneNet.Service;
using WebSite.Core.LuceneNet.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSite.Core.LuceneNet.Processor
{
	public class IndexBuilderThread
	{
		//private static CustomLogger m_logger = new CustomLogger(typeof(IndexBuilderThread));
		private static List<string> m_pathSuffixList = new List<string>();
		private static CancellationTokenSource m_cts = null;

		public static void BuildIndexThread<T>(IEnumerable<EntryDataModel<T>> entryDataModelList) where T : class, new()
		{
			try
			{
				if (entryDataModelList != null && entryDataModelList.Count() > 0)
				{
					//m_logger.Debug(string.Format("{0} BuildIndex开始", DateTime.Now));

					List<Task> taskList = new List<Task>();
					TaskFactory taskFactory = new TaskFactory();
					m_cts = new CancellationTokenSource();
					int i = 1;
					foreach (var entryDataModel in entryDataModelList)
					{
						IndexBuilderPerThread<T> prerThread = null;
						if (entryDataModel.DataListFunc != null)
						{
							prerThread = new IndexBuilderPerThread<T>(entryDataModel.DataListFunc, entryDataModel.FieldModelList, i.ToString("000"), m_cts);
						}
						else
						{
							prerThread = new IndexBuilderPerThread<T>(entryDataModel.DataList, entryDataModel.FieldModelList, i.ToString("000"), m_cts);
						}
						m_pathSuffixList.Add(i.ToString("000"));
						taskList.Add(taskFactory.StartNew(prerThread.Process));//开启一个线程   里面创建索引
						i++;
					}
					taskList.Add(taskFactory.ContinueWhenAll(taskList.ToArray(), MergeIndex<T>));
					Task.WaitAll(taskList.ToArray());
					//m_logger.Debug(string.Format("BuildIndex{0}", CTS.IsCancellationRequested ? "失败" : "成功"));
				}
			}
			catch (Exception ex)
			{
				//m_logger.Error("BuildIndex出现异常", ex);
			}
			finally
			{
				//m_logger.Debug(string.Format("{0} BuildIndex结束", DateTime.Now));
			}
		}

		private static void MergeIndex<T>(Task[] tasks) where T : class, new()
		{
			try
			{
				if (m_cts.IsCancellationRequested)
					return;
				ILuceneBulid<T> builder = new LuceneBulid<T>();
				builder.MergeIndex(m_pathSuffixList.ToArray());
			}
			catch (Exception ex)
			{
				m_cts.Cancel();
				//m_logger.Error("MergeIndex出现异常", ex);
			}
		}
	}
}

