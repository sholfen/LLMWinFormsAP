using LLMLib;

namespace LLMWinFormsAP.Tests
{
    public class TaskRouterTests
    {
        [Fact]
        public void RouteTask_WithValidInput_DoesNotThrow()
        {
            // Arrange
            var router = new TaskRouter();

            // Act & Assert
            var exception = Record.Exception(() => router.RouteTask("TestTask", "A test task description"));
            Assert.Null(exception);
        }

        [Fact]
        public void RouteTask_WritesExpectedOutput()
        {
            // Arrange
            var router = new TaskRouter();
            var originalOut = Console.Out;
            using var sw = new StringWriter();
            Console.SetOut(sw);

            try
            {
                // Act
                router.RouteTask("MyTask", "Do something important");
                string output = sw.ToString();

                // Assert
                Assert.Contains("Routing task: MyTask", output);
                Assert.Contains("Task description: Do something important", output);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void RouteTask_WithEmptyStrings_DoesNotThrow()
        {
            var router = new TaskRouter();

            var exception = Record.Exception(() => router.RouteTask(string.Empty, string.Empty));
            Assert.Null(exception);
        }

        [Fact]
        public void RouteTask_WithNullValues_DoesNotThrow()
        {
            var router = new TaskRouter();

            var exception = Record.Exception(() => router.RouteTask(null!, null!));
            Assert.Null(exception);
        }
    }
}
