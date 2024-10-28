using Balta.Domain.AccountContext.ValueObjects;
using Balta.Domain.AccountContext.ValueObjects.Exceptions;
using Balta.Domain.SharedContext.Abstractions;
using Balta.Domain.SharedContext.Extensions;
using Balta.Domain.Test.Repository;
using Moq;

namespace Balta.Domain.Test.AccountContext.ValueObjects;

public class EmailTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly FakeEmailRepository _emailRepository;
    public Email email = null;
    public EmailTests()
    {
        _emailRepository = new FakeEmailRepository();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);
    }

    [Theory]
    [MemberData(nameof(EmailTestData.ValidEmails), MemberType = typeof(EmailTestData))]
    public void ShouldLowerCaseEmail(string emailAddress)
    {
        // Arrange
        email = Email.ShouldCreate(emailAddress, _dateTimeProviderMock.Object);
        string expectedLowerCaseEmail = emailAddress.ToLower();

        // Assert
        Assert.Equal(expectedLowerCaseEmail, email.Address);
    }

    [Theory]
    [MemberData(nameof(EmailTestData.ValidEmailsWithBlankSpaces), MemberType = typeof(EmailTestData))]
    public void ShouldTrimEmail(string emailAddress)
    {
        email = Email.ShouldCreate(emailAddress, _dateTimeProviderMock.Object);
        Assert.Equal(emailAddress.Trim(), email.Address);
    }

    [Fact]
    public void ShouldFailIfEmailIsNull()
    {
        Assert.Throws<NullReferenceException>(() =>
            Email.ShouldCreate(null, _dateTimeProviderMock.Object));
    }

    [Fact]
    public void ShouldFailIfEmailIsEmpty()
    {
        Assert.Throws<InvalidEmailException>(() =>
            Email.ShouldCreate(string.Empty, _dateTimeProviderMock.Object));
    }

    [Theory]
    [MemberData(nameof(EmailTestData.InvalidEmails), MemberType = typeof(EmailTestData))]

    public void ShouldFailIfEmailIsInvalid(string emailAddress)
    {
        Assert.Throws<InvalidEmailException>(() =>
            Email.ShouldCreate(emailAddress, _dateTimeProviderMock.Object));
    }

    [Theory]
    [MemberData(nameof(EmailTestData.ValidEmails), MemberType = typeof(EmailTestData))]
    public void ShouldPassIfEmailIsValid(string emailAddress)
    {
        Exception? exception = Record.Exception(() => Email.ShouldCreate(emailAddress, _dateTimeProviderMock.Object));
        Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(EmailTestData.ValidEmails), MemberType = typeof(EmailTestData))]
    public void ShouldHashEmailAddress(string emailAddress)
    {
        email = Email.ShouldCreate(emailAddress, _dateTimeProviderMock.Object);
        string expectedHash = emailAddress.ToLower().ToBase64();

        Assert.Equal(email.Hash, expectedHash);
    }

    [Theory]
    [MemberData(nameof(EmailTestData.ValidEmails), MemberType = typeof(EmailTestData))]
    public void ShouldExplicitConvertFromString(string emailAddress)
    {
        email = Email.ShouldCreate(emailAddress, _dateTimeProviderMock.Object);
        Assert.Equal(emailAddress, (string)email);
    }

    [Theory]
    [MemberData(nameof(EmailTestData.ValidEmails), MemberType = typeof(EmailTestData))]
    public void ShouldExplicitConvertToString(string emailAddress)
    {
        email = Email.ShouldCreate(emailAddress, _dateTimeProviderMock.Object);
        Assert.Equal(emailAddress, email.ToString());
    }

    [Theory]
    [MemberData(nameof(EmailTestData.ValidEmails), MemberType = typeof(EmailTestData))]
    public void ShouldReturnEmailWhenCallToStringMethod(string emailAddress)
    {
        email = Email.ShouldCreate(emailAddress, _dateTimeProviderMock.Object);
        Assert.Equal(emailAddress, email.ToString());
    }

    [Theory]
    [MemberData(nameof(EmailTestData.ValidEmails), MemberType = typeof(EmailTestData))]
    [MemberData(nameof(EmailTestData.ValidEmailsWithBlankSpaces), MemberType = typeof(EmailTestData))]
    public void ShouldReturnSuccessIfAddValidEmail(string emailAddress)
    {
        email = Email.ShouldCreate(emailAddress, _dateTimeProviderMock.Object);
        _emailRepository.AddEmail(email.Address);

        string? emailAdded = _emailRepository.Get(email.Address);
        Assert.NotNull(emailAdded);
    }


    [Theory]
    [InlineData("emailvalido@hotmail.com")]
    public void ShouldReturnFalseWhenTryAddingExistingEmail(string emailAddress)
    {
        email = Email.ShouldCreate(emailAddress, _dateTimeProviderMock.Object);

        bool addedAgain = false;

        _emailRepository.AddEmail(email.Address);
        addedAgain = _emailRepository.AddEmail(email.Address);

        Assert.False(addedAgain);
    }
}