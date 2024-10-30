namespace Balta.Domain.Test
{
    public static class EmailTestData
    {
        private const string EMAIL_INVALID_WITHOUT_AT_SIGN = "emailinvalido.com.br";
        private const string EMAIL_INVALID_WITHOUT_DOMAIN = "email-invalido@";

        private const string EMAIL_VALID_HOTMAIL = "emailvalido@hotmail.com";
        private const string EMAIL_VALID_GMAIL = "emailvalido@gmail.com";
        private const string EMAIL_VALID_UPPERCASE = "EMAIL@hotmail.com";
        private const string EMAIL_VALID_DOMAIN_UPPERCASE = "email@HOTMAIL.com";
        private const string EMAIL_VALID_STARTING_BLANK_SPACES = "   emailvalido@hotmail.com";
        private const string EMAIL_VALID_ENDING_BLANK_SPACES = "emailvalido@hotmail.com   ";
        private const string EMAIL_VALID_BLANK_SPACES = "   emailvalido@hotmail.com   ";

        public static IEnumerable<object[]> ValidEmails =>
            new List<object[]> {
                new object[] { EMAIL_VALID_GMAIL },
                new object[] { EMAIL_VALID_HOTMAIL }
            };

        public static IEnumerable<object[]> ValidEmailsWithBlankSpaces =>
            new List<object[]> {
                new object[] { EMAIL_VALID_STARTING_BLANK_SPACES },
                new object[] { EMAIL_VALID_ENDING_BLANK_SPACES },
                new object[] { EMAIL_VALID_BLANK_SPACES },
        };

        public static IEnumerable<object[]> InvalidEmails =>
            new List<object[]> {
                new object[] { EMAIL_INVALID_WITHOUT_AT_SIGN },
                new object[] { EMAIL_INVALID_WITHOUT_DOMAIN },

            };
    }
}