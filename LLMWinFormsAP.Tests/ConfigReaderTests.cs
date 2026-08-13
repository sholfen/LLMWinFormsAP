using LLMWinFormsAP;

namespace LLMWinFormsAP.Tests
{
    public class ConfigReaderTests : IDisposable
    {
        private readonly List<string> _tempFiles = new();

        private string CreateTempJsonFile(string jsonContent)
        {
            string path = Path.GetTempFileName();
            File.WriteAllText(path, jsonContent);
            _tempFiles.Add(path);
            return path;
        }

        [Fact]
        public void Constructor_ValidJson_DeserializesCorrectly()
        {
            // Arrange
            string json = """
            {
                "Girls": [
                    {
                        "Name": "Alice",
                        "Systems": ["System1", "System2"]
                    },
                    {
                        "Name": "Bob",
                        "Systems": ["System3"]
                    }
                ]
            }
            """;
            string path = CreateTempJsonFile(json);

            // Act
            var reader = new ConfigReader(path);
            var girls = reader.GetGirls();

            // Assert
            Assert.NotNull(girls);
            Assert.Equal(2, girls.Length);
            Assert.Equal("Alice", girls[0].Name);
            Assert.Equal(new[] { "System1", "System2" }, girls[0].Systems);
            Assert.Equal("Bob", girls[1].Name);
            Assert.Equal(new[] { "System3" }, girls[1].Systems);
        }

        [Fact]
        public void Constructor_EmptyGirlsArray_ReturnsEmptyArray()
        {
            // Arrange
            string json = """{ "Girls": [] }""";
            string path = CreateTempJsonFile(json);

            // Act
            var reader = new ConfigReader(path);
            var girls = reader.GetGirls();

            // Assert
            Assert.NotNull(girls);
            Assert.Empty(girls);
        }

        [Fact]
        public void Constructor_NonExistentFile_ThrowsException()
        {
            // Arrange
            string fakePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".json");

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => new ConfigReader(fakePath));
        }

        [Fact]
        public void Constructor_InvalidJson_ThrowsException()
        {
            // Arrange
            string path = CreateTempJsonFile("THIS IS NOT JSON");

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => new ConfigReader(path));
        }

        [Fact]
        public void GetGirls_SingleEntry_WithEmptySystems()
        {
            // Arrange
            string json = """
            {
                "Girls": [
                    {
                        "Name": "Carol",
                        "Systems": []
                    }
                ]
            }
            """;
            string path = CreateTempJsonFile(json);

            // Act
            var reader = new ConfigReader(path);
            var girls = reader.GetGirls();

            // Assert
            Assert.Single(girls);
            Assert.Equal("Carol", girls[0].Name);
            Assert.Empty(girls[0].Systems);
        }

        public void Dispose()
        {
            foreach (var file in _tempFiles)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
        }
    }
}
