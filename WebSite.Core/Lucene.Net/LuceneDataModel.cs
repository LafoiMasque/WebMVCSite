using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lucene.Net.Documents.Field;

namespace WebSite.Core.Lucene.Net
{
	public class LuceneDataModel
	{
		public string FieldName { get; set; }
		public string PropertyName { get; set; }
		public Store Store { get; set; } = Store.YES;
		public Index Index { get; set; } = Index.NOT_ANALYZED;
		public TermVector TermVector { get; set; } = TermVector.NO;
	}
}
