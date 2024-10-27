using Balta.Domain.AccountContext.ValueObjects;
using Balta.Domain.SharedContext.Abstractions;

namespace Balta.Domain.Test.AccountContext.Services
{
    public class AccountDeniedServiceTests()
    {
        private readonly IDateTimeProvider dateTimeProvider;

        [Fact]
        public void SholdFailWhenEmailIsDenied()
        {
            Email email = Email.ShouldCreate("", dateTimeProvider);
        }
    }
}
