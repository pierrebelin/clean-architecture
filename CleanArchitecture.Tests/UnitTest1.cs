using FluentAssertions;

namespace CleanArchitecture.Tests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(2, 2)]
        public void MyTest(int a, int b)
        {
            Assert.Equal(a, b);
        }
    }
}