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
	public class IndexBuilderPerThread<T> where T : class, new()
	{
		private CustomLogger m_logger = new CustomLogger(typeof(IndexBuilderPerThread<T>));
		private int m_currentThreadNum = 0;
		private string m_pathSuffix = "";
		private List<T> m_dataList = null;
		private Func<List<T>> m_getDataListFunc = null;
		private IList<FieldDataModel> m_fieldModelList = null;
		private CancellationTokenSource m_cts = null;

		public IndexBuilderPerThread(Func<List<T>> getDataListFunc, IList<FieldDataModel> fieldModelList, string pathSuffix, CancellationTokenSource cts)
		{
			m_getDataListFunc = getDataListFunc;
			m_fieldModelList = fieldModelList;
			m_pathSuffix = pathSuffix;
			m_cts = cts;
		}

		public IndexBuilderPerThread(List<T> dataList, IList<FieldDataModel> fieldModelList, string pathSuffix, CancellationTokenSource cts)
		{
			m_dataList = dataList;
			m_fieldModelList = fieldModelList;
			m_pathSuffix = pathSuffix;
			m_cts = cts;
		}

		public void Process()
		{
			try
			{
				m_currentThreadNum = Thread.CurrentThread.ManagedThreadId;
				m_logger.Debug(string.Format("ThreadNum={0}开始创建", m_currentThreadNum));
				ILuceneBulid<T> builder = new LuceneBulid<T>();
				bool isFirst = true;
				if (!m_cts.IsCancellationRequested)
				{
					List<T> modelList = m_getDataListFunc != null ? m_getDataListFunc() : null;
					if (modelList == null)
						modelList = m_dataList;
					if (modelList != null && modelList.Count > 0)
					{
						builder.BuildIndex(modelList, m_fieldModelList, m_pathSuffix, isFirst);
						m_logger.Debug(string.Format("ThreadNum={0}完成{1}条的创建", m_currentThreadNum, modelList.Count));
						isFirst = false;
					}
				}
			}
			catch (Exception ex)
			{
				m_cts.Cancel();
				m_logger.Error(string.Format("ThreadNum={0}出现异常", m_currentThreadNum), ex);
			}
			finally
			{
				m_logger.Debug(string.Format("ThreadNum={0}完成创建", m_currentThreadNum));
			}
		}
	}
}
