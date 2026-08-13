using LLMLib.Repositories.Implements;
using LLMLib.Repositories.Interfaces;
using Microsoft.Extensions.AI;

namespace LLMWinFormsAP.Tests
{
    public class ChatHistoryRepositoryTests
    {
        private readonly IChatHistoryRepository _repository;

        public ChatHistoryRepositoryTests()
        {
            _repository = new ChatHistoryRepository();
        }

        [Fact]
        public void GetChatHistory_EmptyToken_ReturnsEmptyList()
        {
            // Arrange
            string token = "non-existent-token";

            // Act
            var result = _repository.GetChatHistory(token);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void AddUserMessage_SingleMessage_CanBeRetrieved()
        {
            // Arrange
            string token = "test-token-1";
            string message = "Hello, AI!";

            // Act
            _repository.AddUserMessage(token, message);
            var history = _repository.GetChatHistory(token);

            // Assert
            Assert.Single(history);
            Assert.Equal(ChatRole.User, history[0].Role);
            Assert.Equal(message, history[0].Text);
        }

        [Fact]
        public void AddAssistantMessage_SingleMessage_CanBeRetrieved()
        {
            // Arrange
            string token = "test-token-2";
            string message = "Hi there!";

            // Act
            _repository.AddAssistantMessage(token, message);
            var history = _repository.GetChatHistory(token);

            // Assert
            Assert.Single(history);
            Assert.Equal(ChatRole.Assistant, history[0].Role);
            Assert.Equal(message, history[0].Text);
        }

        [Fact]
        public void AddMessages_MultipleMessages_PreservesOrder()
        {
            // Arrange
            string token = "test-token-3";

            // Act
            _repository.AddUserMessage(token, "第一個問題");
            _repository.AddAssistantMessage(token, "第一個回答");
            _repository.AddUserMessage(token, "第二個問題");
            _repository.AddAssistantMessage(token, "第二個回答");

            var history = _repository.GetChatHistory(token);

            // Assert
            Assert.Equal(4, history.Count);
            Assert.Equal(ChatRole.User, history[0].Role);
            Assert.Equal("第一個問題", history[0].Text);
            Assert.Equal(ChatRole.Assistant, history[1].Role);
            Assert.Equal("第一個回答", history[1].Text);
            Assert.Equal(ChatRole.User, history[2].Role);
            Assert.Equal("第二個問題", history[2].Text);
            Assert.Equal(ChatRole.Assistant, history[3].Role);
            Assert.Equal("第二個回答", history[3].Text);
        }

        [Fact]
        public void ClearChatHistory_RemovesAllMessages()
        {
            // Arrange
            string token = "test-token-4";
            _repository.AddUserMessage(token, "message 1");
            _repository.AddAssistantMessage(token, "response 1");

            // Act
            _repository.ClearChatHistory(token);
            var history = _repository.GetChatHistory(token);

            // Assert
            Assert.Empty(history);
        }

        [Fact]
        public void ClearChatHistory_NonExistentToken_DoesNotThrow()
        {
            // Act & Assert — should not throw
            var exception = Record.Exception(() => _repository.ClearChatHistory("non-existent"));
            Assert.Null(exception);
        }

        [Fact]
        public void DifferentTokens_HaveIsolatedHistories()
        {
            // Arrange
            string token1 = "user-A";
            string token2 = "user-B";

            // Act
            _repository.AddUserMessage(token1, "Hello from A");
            _repository.AddUserMessage(token2, "Hello from B");

            var historyA = _repository.GetChatHistory(token1);
            var historyB = _repository.GetChatHistory(token2);

            // Assert
            Assert.Single(historyA);
            Assert.Equal("Hello from A", historyA[0].Text);

            Assert.Single(historyB);
            Assert.Equal("Hello from B", historyB[0].Text);
        }

        [Fact]
        public void ClearChatHistory_OnlyAffectsSpecifiedToken()
        {
            // Arrange
            string token1 = "keep-me";
            string token2 = "clear-me";
            _repository.AddUserMessage(token1, "message A");
            _repository.AddUserMessage(token2, "message B");

            // Act
            _repository.ClearChatHistory(token2);

            // Assert
            Assert.Single(_repository.GetChatHistory(token1));
            Assert.Empty(_repository.GetChatHistory(token2));
        }

        [Fact]
        public void ConcurrentAccess_MultipleThreads_DoesNotThrow()
        {
            // Arrange & Act
            var tasks = Enumerable.Range(0, 100).Select(i =>
                Task.Run(() =>
                {
                    string token = $"concurrent-token-{i % 10}";
                    _repository.AddUserMessage(token, $"Message {i}");
                    _repository.AddAssistantMessage(token, $"Response {i}");
                    _repository.GetChatHistory(token);
                })
            ).ToArray();

            // Assert — should complete without exceptions
            var exception = Record.Exception(() => Task.WaitAll(tasks));
            Assert.Null(exception);
        }
    }
}
