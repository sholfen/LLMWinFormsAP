using LLMLib;
using LLMLib.Models;
using RAGLib.Models;

namespace LLMWinFormsAP.Tests
{
    public class ModelTests
    {
        #region LLMConfigModel

        [Fact]
        public void LLMConfigModel_DefaultValues_AreEmptyStrings()
        {
            var model = new LLMConfigModel();

            Assert.Equal(string.Empty, model.Token);
            Assert.Equal(string.Empty, model.EndPoint);
        }

        [Fact]
        public void LLMConfigModel_SetProperties_RetainsValues()
        {
            var model = new LLMConfigModel
            {
                Token = "my-secret-token",
                EndPoint = "https://api.example.com"
            };

            Assert.Equal("my-secret-token", model.Token);
            Assert.Equal("https://api.example.com", model.EndPoint);
        }

        #endregion

        #region ImageResult

        [Fact]
        public void ImageResult_DefaultValues_HasEmptyArray()
        {
            var result = new ImageResult();

            Assert.NotNull(result.data);
            Assert.Empty(result.data);
        }

        [Fact]
        public void ImageResultData_DefaultValues_AreEmptyStrings()
        {
            var data = new ImageResultData();

            Assert.Equal(string.Empty, data.revised_prompt);
            Assert.Equal(string.Empty, data.url);
        }

        [Fact]
        public void ImageResult_WithData_CanBeAccessed()
        {
            var result = new ImageResult
            {
                data = new[]
                {
                    new ImageResultData
                    {
                        revised_prompt = "A cat on a sofa",
                        url = "https://example.com/cat.png"
                    }
                }
            };

            Assert.Single(result.data);
            Assert.Equal("A cat on a sofa", result.data[0].revised_prompt);
            Assert.Equal("https://example.com/cat.png", result.data[0].url);
        }

        #endregion

        #region AzureConfigModel

        [Fact]
        public void AzureConfigModel_DefaultValues()
        {
            var model = new AzureConfigModel();

            Assert.Equal(string.Empty, model.Host);
            Assert.Equal(string.Empty, model.ApiKey);
        }

        [Fact]
        public void AzureConfigModel_SetProperties()
        {
            var model = new AzureConfigModel
            {
                Host = "https://my-azure.openai.azure.com",
                ApiKey = "azure-key-123"
            };

            Assert.Equal("https://my-azure.openai.azure.com", model.Host);
            Assert.Equal("azure-key-123", model.ApiKey);
        }

        #endregion

        #region EmbeddingResult

        [Fact]
        public void EmbeddingResult_DefaultValues()
        {
            var result = new EmbeddingResult();

            Assert.Equal(string.Empty, result.model);
            Assert.NotNull(result.embeddings);
            Assert.Empty(result.embeddings);
        }

        [Fact]
        public void EmbeddingResult_WithEmbeddings()
        {
            var result = new EmbeddingResult
            {
                model = "text-embedding-ada-002",
                embeddings = new[] { new float[] { 0.1f, 0.2f, 0.3f } }
            };

            Assert.Equal("text-embedding-ada-002", result.model);
            Assert.Single(result.embeddings);
            Assert.Equal(3, result.embeddings[0].Length);
            Assert.Equal(0.1f, result.embeddings[0][0]);
        }

        #endregion

        #region TextData

        [Fact]
        public void TextData_DefaultValues()
        {
            var data = new TextData();

            Assert.Equal(string.Empty, data.catg);
            Assert.Equal(string.Empty, data.text);
        }

        [Fact]
        public void TextData_SetProperties()
        {
            var data = new TextData
            {
                catg = "詩詞",
                text = "床前明月光"
            };

            Assert.Equal("詩詞", data.catg);
            Assert.Equal("床前明月光", data.text);
        }

        #endregion

        #region QdrantDbConfigModel

        [Fact]
        public void QdrantDbConfigModel_DefaultValues()
        {
            var model = new QdrantDbConfigModel();

            Assert.Equal("localhost", model.Host);
            Assert.Equal(0, model.Port);
            Assert.Equal(string.Empty, model.DeploymentName);
            Assert.Equal(string.Empty, model.CollectionName);
            Assert.Equal(0UL, model.VectorSize);
            Assert.Equal(string.Empty, model.ApiKey);
        }

        [Fact]
        public void QdrantDbConfigModel_SetProperties()
        {
            var model = new QdrantDbConfigModel
            {
                Host = "qdrant-server",
                Port = 6334,
                DeploymentName = "text-embedding",
                CollectionName = "my_collection",
                VectorSize = 1536,
                ApiKey = "qdrant-key"
            };

            Assert.Equal("qdrant-server", model.Host);
            Assert.Equal(6334, model.Port);
            Assert.Equal("text-embedding", model.DeploymentName);
            Assert.Equal("my_collection", model.CollectionName);
            Assert.Equal(1536UL, model.VectorSize);
            Assert.Equal("qdrant-key", model.ApiKey);
        }

        #endregion

        #region GirlItem (ConfigReader models)

        [Fact]
        public void GirlItem_DefaultValues()
        {
            var item = new GirlItem();

            Assert.Equal(string.Empty, item.Name);
            Assert.NotNull(item.Systems);
            Assert.Empty(item.Systems);
        }

        #endregion
    }
}
