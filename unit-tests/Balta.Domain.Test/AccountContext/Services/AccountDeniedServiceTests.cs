//using Balta.Domain.AccountContext.Services.Abstractions;
//using Balta.Domain.AccountContext.ValueObjects;
//using Balta.Domain.SharedContext.Abstractions;
//using Moq;

//namespace Balta.Domain.Test.AccountContext.Services
//{
//    public class AccountDeniedServiceTests()
//    {
//        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
//        private readonly IAccountDeniedService accountDeniedService;

//        public AccountDeniedServiceTests()
//        {
//            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
//            _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);
//        }

//        [Theory]
//        [InlineData("email@hotmail.com")]
//        public void SholdFailWhenEmailIsDenied(string emailAddress)
//        {
//            Email email = Email.ShouldCreate(emailAddress, _dateTimeProviderMock.Object);
//            Assert.ThrowsAsync<NotImplementedException>(() => accountDeniedService.IsAccountDenied(emailAddress));
//        }
//    }
//}
