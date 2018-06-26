using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using WebSite.Core.LuceneNet;
using WebSite.IBLL.SingletonPattern;
using WebSite.Model.DataBaseModel;
using WebSite.WebApp.Models;

namespace WebSite.WebApp.Controllers
{
	public class SearchController : Controller
	{
		public IJD_Commodity_001Service JD_Commodity_001Service { get; set; }
		public IKeyWordsRankService KeyWordsRankService { get; set; }
		public ISearchDetailsService SearchDetailsService { get; set; }

		// GET: Search
		public ActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// 表单中有多个Submit，单击每个Submit都会提交表单，但是只会将用户所单击的表单元素的value值提交到服务端。
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult SearchContent()
		{
			List<ContentViewModel> list = ShowSearchContent();
			ViewData["list"] = list;
			return View("Index");
		}

		private List<ContentViewModel> ShowSearchContent()
		{
			string indexPath = System.Configuration.ConfigurationManager.AppSettings["LuceneNetDir"];
			string searchString = Request["txtSearch"];
			List<string> list = LuceneCommon.PanGuSplitWord(searchString);//对用户输入的搜索条件进行拆分。
			FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
			IndexReader reader = IndexReader.Open(directory, true);
			IndexSearcher searcher = new IndexSearcher(reader);
			//搜索条件
			PhraseQuery queryBody = new PhraseQuery();
			////多部分查询
			//PhraseQuery queryTitle = new PhraseQuery();
			//先用空格，让用户去分词，空格分隔的就是词“计算机   专业”
			foreach (string word in list)
			{
				queryBody.Add(new Term("Price", word));
				//queryTitle.Add(new Term("Title", word));
			}
			//多个查询条件的词之间的最大距离.在文章中相隔太远 也就无意义.（例如 “大学生”这个查询条件和"简历"这个查询条件之间如果间隔的词太多也就没有意义了。）
			queryBody.Slop = 100;
			//queryTitle.SetSlop(100);

			#region 多部分查询

			//BooleanQuery query = new BooleanQuery();
			//query.Add(queryTitle, BooleanClause.Occur.SHOULD);
			//query.Add(queryBody, BooleanClause.Occur.SHOULD);

			#endregion

			//TopScoreDocCollector是盛放查询结果的容器
			TopScoreDocCollector collector = TopScoreDocCollector.Create(1000, true);
			//根据query查询条件进行查询，查询结果放入collector容器
			searcher.Search(queryBody, null, collector);
			//得到所有查询结果中的文档,GetTotalHits():表示总条数   TopDocs(300, 20);//表示得到300（从300开始），到320（结束）的文档内容.
			ScoreDoc[] docs = collector.TopDocs(0, collector.TotalHits).ScoreDocs;
			//可以用来实现分页功能
			List<ContentViewModel> viewModelList = new List<ContentViewModel>();
			for (int i = 0; i < docs.Length; i++)
			{
				//搜索ScoreDoc[]只能获得文档的id,这样不会把查询结果的Document一次性加载到内存中。降低了内存压力，需要获得文档的详细内容的时候通过searcher.Doc来根据文档id来获得文档的详细内容对象Document.
				ContentViewModel viewModel = new ContentViewModel();
				int docId = docs[i].Doc;//得到查询结果文档的id（Lucene内部分配的id）
				Document doc = searcher.Doc(docId);//找到文档id对应的文档详细信息
				viewModel.Id = Convert.ToInt32(doc.Get("Id"));// 取出放进字段的值
				viewModel.Title = doc.Get("Title");
				viewModel.Content = LuceneCommon.CreateHightLight(searchString, doc.Get("Price"));//将搜索的关键字高亮显示。
				viewModelList.Add(viewModel);
			}
			//先将搜索的词插入到明细表。
			SearchDetails searchDetail = new SearchDetails();
			searchDetail.Id = Guid.NewGuid();
			searchDetail.KeyWords = Request["txtSearch"];
			searchDetail.SearchTime = DateTime.Now;
			SearchDetailsService.AddEntity(searchDetail);

			return viewModelList;
		}

		public ActionResult AutoComplete()
		{
			//Thread.Sleep(5000);
			string term = Request["term"];
			List<string> list = KeyWordsRankService.GetSearchMsg(term);
			return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
		}
	}
}