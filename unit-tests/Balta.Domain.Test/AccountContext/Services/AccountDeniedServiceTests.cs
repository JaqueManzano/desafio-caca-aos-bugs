using Balta.Domain.AccountContext.Services.Abstractions;
using Balta.Domain.AccountContext.ValueObjects;
using Balta.Domain.SharedContext.Abstractions;

namespace Balta.Domain.Test.AccountContext.Services
{
    public class AccountDeniedServiceTests()
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IAccountDeniedService accountDeniedService;

        //public AccountDeniedServiceTests()
        //{
                
        //}

        [Theory]
        [InlineData("email@hotmail.com")]
        public void SholdFailWhenEmailIsDenied(string emailAddress)
        {
            Email email = Email.ShouldCreate(emailAddress, dateTimeProvider);
            Assert.ThrowsAsync<NotImplementedException>(() => accountDeniedService.IsAccountDenied(emailAddress));
        }
    }
}
