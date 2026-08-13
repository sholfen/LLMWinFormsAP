using SearchEngineManager;

namespace LLMWinFormsAP.Tests
{
    public class UtilitiesTests
    {
        [Theory]
        [InlineData(typeof(byte), true)]
        [InlineData(typeof(sbyte), true)]
        [InlineData(typeof(ushort), true)]
        [InlineData(typeof(uint), true)]
        [InlineData(typeof(ulong), true)]
        [InlineData(typeof(short), true)]
        [InlineData(typeof(int), true)]
        [InlineData(typeof(long), true)]
        [InlineData(typeof(decimal), true)]
        [InlineData(typeof(double), true)]
        [InlineData(typeof(float), true)]
        public void IsNumericType_NumericTypes_ReturnsTrue(Type type, bool expected)
        {
            // Act
            bool result = type.IsNumericType();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(bool))]
        [InlineData(typeof(char))]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(object))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(TimeSpan))]
        public void IsNumericType_NonNumericTypes_ReturnsFalse(Type type)
        {
            // Act
            bool result = type.IsNumericType();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNumericType_NullableInt_ReturnsFalse()
        {
            // Nullable<int> 的 Type 是 Nullable`1，不在 TypeCode 判斷範圍內
            Type type = typeof(int?);

            bool result = type.IsNumericType();

            // Nullable<T> 的 TypeCode 為 Object，所以應回傳 false
            Assert.False(result);
        }

        [Fact]
        public void IsNumericType_Enum_ReturnsFalse()
        {
            // Enum 的 TypeCode 取決於底層型別，但 Type.GetTypeCode 對 enum 會返回底層型別
            Type type = typeof(DayOfWeek);

            bool result = type.IsNumericType();

            // DayOfWeek 底層是 int，所以 GetTypeCode 會回傳 Int32 → true
            Assert.True(result);
        }
    }
}
