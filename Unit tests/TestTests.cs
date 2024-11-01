namespace Testing_tests
{
    public class TestFixture
    {
        // Very complex initialization
        public int Value { get; } = 15;

        private static bool IsInitialized = false;

        public TestFixture() {
            if(!IsInitialized) {
                IsInitialized = true;
                Value = 5;
            }
        }
    }

    public class TestTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        public TestTests(TestFixture fixture) {
            _fixture = fixture;
        }

        [Fact]
        public void MultiplyTwoNumbers() {
            //Arrange
            int a = 5;
            double b = 4.5;

            //Act
            var c = a * b;

            //Assert
            Assert.Equal(22.5, c, 0.001);
        }

        public static IEnumerable<object[]> TestModData() {
            yield return new object[] { 9, 5, 4 };
            yield return new object[] { 32, 4, 0 };
            yield return new object[] { 16, 3, 1 };
        }

        [Theory, MemberData(nameof(TestModData))]
        public void CalculateModOfAnInt(int a, int b, int expected) {
            int c = a % b;
            Assert.Equal(expected, c);
        }

        [Fact]
        public void TestFixtureAdd2() {
            int result = _fixture.Value + 2;
            Assert.Equal(7, result);
        }

        [Fact]
        public void TestFixtureAdd3() {
            int result = _fixture.Value + 3;
            Assert.Equal(8, result);
        }
    }
}