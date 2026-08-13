using SearchEngineManager;

namespace LLMWinFormsAP.Tests
{
    public class LBSearchManagerTests : IDisposable
    {
        private readonly LBSearchManager _searchManager;

        public LBSearchManagerTests()
        {
            _searchManager = new LBSearchManager();
        }

        [Fact]
        public void CreateIndex_WithValidData_DoesNotThrow()
        {
            // Arrange
            var testData = new TestData
            {
                TextField1 = "測試資料",
                NumField1 = 1,
                LongTextField = "這是一筆測試用的長文字欄位資料"
            };

            // Act & Assert
            var exception = Record.Exception(() => _searchManager.CreateIndex(testData));
            Assert.Null(exception);
        }

        [Fact]
        public void Search_AfterCreateIndex_ReturnsMatchingResults()
        {
            // Arrange
            var testData = new TestData
            {
                TextField1 = "C#入門教材",
                NumField1 = 1,
                LongTextField = "這是一個C#的入門教材"
            };
            _searchManager.CreateIndex(testData);

            // Act
            var results = _searchManager.Search<TestData>("C#");

            // Assert
            Assert.NotNull(results);
            Assert.NotEmpty(results);
        }

        [Fact]
        public void Search_WithNonMatchingKeyword_ReturnsEmptyList()
        {
            // Arrange
            var testData = new TestData
            {
                TextField1 = "C#入門教材",
                NumField1 = 1,
                LongTextField = "這是一個C#的入門教材"
            };
            _searchManager.CreateIndex(testData);

            // Act
            var results = _searchManager.Search<TestData>("ZZZZNOTFOUND12345");

            // Assert
            Assert.NotNull(results);
            Assert.Empty(results);
        }

        [Fact]
        public void Search_BuiltInTestData_ContainsCSharpCourses()
        {
            // Arrange — CreateIndex 內部也會加入 CreateTestDatas() 的資料
            var testData = new TestData
            {
                TextField1 = "觸發索引建立",
                NumField1 = 0,
                LongTextField = "觸發"
            };
            _searchManager.CreateIndex(testData);

            // Act
            var results = _searchManager.Search<TestData>("Java");

            // Assert
            Assert.NotNull(results);
            Assert.Contains(results, r => r.Contains("Java"));
        }

        [Fact]
        public void Search_BuiltInTestData_FindsPython()
        {
            // Arrange
            var testData = new TestData
            {
                TextField1 = "init",
                NumField1 = 0,
                LongTextField = "init"
            };
            _searchManager.CreateIndex(testData);

            // Act
            var results = _searchManager.Search<TestData>("Python");

            // Assert
            Assert.NotNull(results);
            Assert.Contains(results, r => r.Contains("Python"));
        }

        public void Dispose()
        {
            _searchManager?.Dispose();
        }
    }
}
