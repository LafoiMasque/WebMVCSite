using Lucene.Net.Documents;
using System;

namespace WebSite.Core.LuceneNet.Model
{
	public class FieldDataModel
	{
		/*
		Field.Store.YES:存储字段值（未分词前的字段值）        
		Field.Store.NO:不存储,存储与索引没有关系
		Field.Store.COMPRESS:压缩存储, 用于长文本或二进制，但性能受损

		Field.Index.ANALYZED:分词建索引
		Field.Index.ANALYZED_NO_NORMS:分词建索引，但是Field的值不像通常那样被保存，而是只取一个byte，这样节约存储空间
		Field.Index.NOT_ANALYZED:不分词且索引
		Field.Index.NOT_ANALYZED_NO_NORMS:不分词建索引，Field的值去一个byte保存

		TermVector表示文档的条目（由一个Document和Field定位）和它们在当前文档中所出现的次数
		Field.TermVector.YES:为每个文档（Document）存储该字段的TermVector
		Field.TermVector.NO:不存储TermVector
		Field.TermVector.WITH_POSITIONS:存储位置
		Field.TermVector.WITH_OFFSETS:存储偏移量
		Field.TermVector.WITH_POSITIONS_OFFSETS:存储位置和偏移量
		*/

		public string FieldName { get; set; }
		public string PropertyName { get; set; }
		public TypeCode FieldType { get; set; } = TypeCode.String;
		public Field.Store Store { get; set; } = Field.Store.YES;
		public Field.Index Index { get; set; } = Field.Index.NOT_ANALYZED;
		public Field.TermVector TermVector { get; set; } = Field.TermVector.NO;
	}

	//public enum FieldTypeEnum
	//{
	//	Field,
	//	NumericField,
	//	//DateTools,
	//}
}