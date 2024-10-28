using Balta.Domain.AccountContext.ValueObjects;

namespace Balta.Domain.Test
{
    public static class PasswordTestData
    {
        public static IEnumerable<object[]> ValidPasswords =>
            new List<object[]> {
                new object[] { Password.ShouldGenerate(45) },
            };

        public static IEnumerable<object[]> PasswordLenIsGreaterThanMaxChars =>
            new List<object[]> {
                    new object[] { Password.ShouldGenerate(50) },
                    new object[] { Password.ShouldGenerate(100) },
            };

        public static IEnumerable<object[]> PasswordLenIsLessThanMinimumChars =>
            new List<object[]> {
                            new object[] { Password.ShouldGenerate(7) },
                            new object[] { Password.ShouldGenerate(5) },
            };
    }
}
