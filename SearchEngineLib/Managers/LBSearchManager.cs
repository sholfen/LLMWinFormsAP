using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Microsoft.VisualBasic.FileIO;
using System.Reflection;
using static Lucene.Net.Util.Packed.PackedInt32s;

namespace SearchEngineManager
{
	public class TestData
	{
		public string TextField1 { get; set; } = string.Empty;
		public long NumField1 { get; set; } = 0;
		public string LongTextField { get; set; } = string.Empty;
	}

	public class LBSearchManager : ILBSearchManager, IDisposable
	{
		private IndexWriter _writet;
		private RAMDirectory _directory;
		private const LuceneVersion lv = LuceneVersion.LUCENE_48;

		public LBSearchManager()
		{
            Analyzer a = new StandardAnalyzer(lv);
            _directory = new RAMDirectory();
            var config = new IndexWriterConfig(lv, a);
            _writet = new IndexWriter(_directory, config);
        }

		public void CreateIndex<T>(T contents) where T : class
		{
			Type type = typeof(T);
			var testD = new Document();
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				bool isNum = propertyInfo.PropertyType.IsNumericType();
				if(isNum)
				{
					long num = long.Parse(propertyInfo.GetValue(contents).ToString());
					var field = new NumericDocValuesField(nameof(propertyInfo.Name), num);
					testD.Add(field);

                    continue;
				}
				var field2 = new TextField(nameof(propertyInfo.Name), propertyInfo.GetValue(contents).ToString(), Field.Store.YES);
				testD.Add(field2);
			}
			Lucene.Net.Documents.FieldType fieldType = new Lucene.Net.Documents.FieldType();
			fieldType.NumericType = NumericType.INT32;
			fieldType.IsStored = true;
			//Field field1 = new Field("aaa3", "values", fieldType);

			var list = CreateTestDatas();
			foreach (var testData in list)
			{
				var d = new Document()
				{
					//new StringField("GUID", PersonGuidToBeUpdated, Field.Store.YES),
					new TextField(nameof(testData.TextField1), testData.TextField1, Field.Store.YES),
					new NumericDocValuesField(nameof(testData.NumField1), testData.NumField1),
					new TextField(nameof(testData.LongTextField), testData.LongTextField, Field.Store.YES),
				};
			}

			//_writet.AddDocument(testD);
			_writet.AddDocuments(list.Select(testData => new Document()
			{
				//new StringField("GUID", PersonGuidToBeUpdated, Field.Store.YES),
				new TextField(nameof(testData.TextField1), testData.TextField1, Field.Store.YES),
				new NumericDocValuesField(nameof(testData.NumField1), testData.NumField1),
				new TextField(nameof(testData.LongTextField), testData.LongTextField, Field.Store.YES),
			}));
            //_writet.UpdateDocument(new Term("GUID", PersonGuidToBeUpdated), d);
            _writet.Commit();
		}

		private List<TestData> CreateTestDatas()
		{
			return new List<TestData>
			{
				new TestData { TextField1 = "C#課程", NumField1 = 1, LongTextField = "C#課程" },
				new TestData { TextField1 = "從零開始認識C#", NumField1 = 2, LongTextField = "從零開始認識C#" },
				new TestData { TextField1 = "進階C#教材", NumField1 = 3, LongTextField = "進階C#教材" },
				new TestData { TextField1 = "21天學會C#程式語言", NumField1 = 4, LongTextField = "21天學會C#程式語言" },
                new TestData { TextField1 = "21天學會Java程式語言", NumField1 = 4, LongTextField = "21天學會Java程式語言" },
                new TestData { TextField1 = "Python入門", NumField1 = 4, LongTextField = "Python入門" }
            };
        }

        public List<string> Search<T>(string keyword) where T : class
		{
            Type type = typeof(T);
            T doc = default(T);
			Analyzer a = new StandardAnalyzer(lv);
			using var dirReader = DirectoryReader.Open(_directory);
			var searcher = new IndexSearcher(dirReader);

			string[] fnames = { "TextField1" };
			var multiFieldQP = new MultiFieldQueryParser(lv, fnames, a);
			Query query = multiFieldQP.Parse(keyword.Trim());
			ScoreDoc[] docs = searcher.Search(query, null, 1000).ScoreDocs;

			var results = new List<string>();
			for (int i = 0; i < docs.Length; i++)
			{
				Document d = searcher.Doc(docs[i].Doc);
				//string guid = d.Get("GUID");
				//string firstname = d.Get("FirstName");
				//string middlename = d.Get("MiddleName");
				//string lastname = d.Get("LastName");
				//string description = d.Get("Description");
				string text = d.Get("TextField1");
				//results.Add($"{guid} {firstname} {middlename} {lastname} {description}");
				results.Add(text);
			}
			//foreach (var item in results)
			//{
			//	Console.WriteLine(item);
			//}

			return results;
        }

		public void Dispose()
		{
			_writet?.Dispose();
			_directory?.Dispose();
		}
	}
}