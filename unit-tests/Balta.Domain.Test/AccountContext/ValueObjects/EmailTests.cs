using Balta.Domain.AccountContext.ValueObjects.Exceptions;
using Balta.Domain.SharedContext.Abstractions;
using Balta.Domain.SharedContext.Extensions;
using Balta.Domain.Test.Command;
using Balta.Domain.Test.Repository;
using Moq;
using System.Runtime.CompilerServices;

namespace Balta.Domain.Test.AccountContext.ValueObjects;

public class EmailTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly FakeEmailRepository _emailRepository;

    
    private const string EMAIL_INVALID_WITHOUT_AT_SIGN = "emailinvalido.com.br";
    private const string EMAIL_INVALID_WITH_NO_DOMAIN = "email-invalido@";

    private const string EMAIL_VALID = "emailvalido@hotmail.com";
    private const string EMAIL_VALID_UPPERCASE = "EMAIL@hotmail.com";
    private const string EMAIL_VALID_DOMAIN_UPPERCASE = "email@HOTMAIL.com";
    private const string EMAIL_VALID_STARTING_BLANK_SPACES = "   emailvalido@hotmail.com";
    private const string EMAIL_VALID_ENDING_BLANK_SPACES = "emailvalido@hotmail.com   ";
    private const string EMAIL_VALID_BLANK_SPACES = "   emailvalido@hotmail.com   ";

    public EmailTests()
    {
        _emailRepository = new FakeEmailRepository();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);
    }

    [Theory]
    [MemberData(nameof(EmailTestData.ValidEmails), MemberType = typeof(EmailTestData))]
    //[InlineData(EMAIL_VALID_UPPERCASE)]
    //[InlineData(EMAIL_VALID_DOMAIN_UPPERCASE)]
    public void ShouldLowerCaseEmail(string emailAddress)
    {
        // Arrange
        var command = new CreateEmailCommand(emailAddress, _dateTimeProviderMock.Object);
        command.Validate();

        // Act
        var email = command.Email;
        string expectedLowerCaseEmail = emailAddress.ToLower();

        // Assert
        Assert.True(command.IsValid);
        Assert.Equal(expectedLowerCaseEmail, email.Address);
    }

    [Theory]
    [MemberData(nameof(EmailTestData.ValidEmailsWithBlankSpaces), MemberType = typeof(EmailTestData))]    
    //[InlineData(EMAIL_VALID_BLANK_SPACES)]
    //[InlineData(EMAIL_VALID_STARTING_BLANK_SPACES)]
    //[InlineData(EMAIL_VALID_ENDING_BLANK_SPACES)]    
    public void ShouldTrimEmail(string emailAddress)
    {
        var command = new CreateEmailCommand(emailAddress, _dateTimeProviderMock.Object);
        Assert.Equal(emailAddress.Trim(), command.Email.Address);
    }

    [Fact]
    public void ShouldFailIfEmailIsNull()
    {
        Assert.Throws<NullReferenceException>(() =>
              new CreateEmailCommand(null, _dateTimeProviderMock.Object));
    }

    [Fact]
    public void ShouldFailIfEmailIsEmpty()
    {
        Assert.Throws<InvalidEmailException>(() =>
        new CreateEmailCommand(string.Empty, _dateTimeProviderMock.Object));
    }

    [Theory]
    [MemberData(nameof(EmailTestData.InValidEmails), MemberType = typeof(EmailTestData))]
    //[InlineData(EMAIL_INVALID_WITHOUT_AT_SIGN)]
    //[InlineData(EMAIL_INVALID_WITH_NO_DOMAIN)]
    public void ShouldFailIfEmailIsInvalid(string emailAddress)
    {
        Assert.Throws<InvalidEmailException>(() =>
            new CreateEmailCommand(emailAddress, _dateTimeProviderMock.Object));
    }

    [Theory]
    [MemberData(nameof(EmailTestData.ValidEmails), MemberType = typeof(EmailTestData))]
    [MemberData(nameof(EmailTestData.ValidEmailsWithBlankSpaces), MemberType = typeof(EmailTestData))]
    //[InlineData(EMAIL_VALID)]
    //[InlineData(EMAIL_VALID_UPPERCASE)]
    //[InlineData(EMAIL_VALID_DOMAIN_UPPERCASE)]
    //[InlineData(EMAIL_VALID_STARTING_BLANK_SPACES)]
    //[InlineData(EMAIL_VALID_ENDING_BLANK_SPACES)]
    //[InlineData(EMAIL_VALID_BLANK_SPACES)]
    public void ShouldPassIfEmailIsValid(string emailAddress)
    {
        try
        {
            var command = new CreateEmailCommand(emailAddress, _dateTimeProviderMock.Object);
            command.Validate();
        }
        catch (InvalidEmailException)
        {
            Assert.True(false);
        }

        Assert.True(true);
    }

    [Theory]
    [MemberData(nameof(EmailTestData.ValidEmails), MemberType = typeof(EmailTestData))]
    //[MemberData(nameof(EmailTestData.ValidEmailsWithBlankSpaces), MemberType = typeof(EmailTestData))]

    //[InlineData(EMAIL_VALID)]
    //[InlineData(EMAIL_VALID_UPPERCASE)]
    //[InlineData(EMAIL_VALID_DOMAIN_UPPERCASE)]
    //[InlineData(EMAIL_VALID_STARTING_BLANK_SPACES)]
    //[InlineData(EMAIL_VALID_ENDING_BLANK_SPACES)]
    //[InlineData(EMAIL_VALID_BLANK_SPACES)]
    public void ShouldHashEmailAddress(string emailAddress)
    {
        var command = new CreateEmailCommand(emailAddress, _dateTimeProviderMock.Object);
        string expectedHash = emailAddress.ToLower().ToBase64();

        Assert.Equal(command.Email.Hash, expectedHash);
    }

    [Theory]
    [MemberData(nameof(EmailTestData.ValidEmails), MemberType = typeof(EmailTestData))]
    [MemberData(nameof(EmailTestData.ValidEmailsWithBlankSpaces), MemberType = typeof(EmailTestData))]

    //[InlineData(EMAIL_VALID)]
    //[InlineData(EMAIL_VALID_UPPERCASE)]
    //[InlineData(EMAIL_VALID_DOMAIN_UPPERCASE)]
    //[InlineData(EMAIL_VALID_STARTING_BLANK_SPACES)]
    //[InlineData(EMAIL_VALID_ENDING_BLANK_SPACES)]
    //[InlineData(EMAIL_VALID_BLANK_SPACES)]
    public void ShouldExplicitConvertFromString(string emailAddress)
    {
        var command = new CreateEmailCommand(emailAddress, _dateTimeProviderMock.Object);
        Assert.Equal(emailAddress, (string)command.Email);
        //Assert.Equal(emailAddress, (string)command.Email, ignoreCase: true, ignoreAllWhiteSpace: true);
    }

    [Theory]
    [MemberData(nameof(EmailTestData.ValidEmails), MemberType = typeof(EmailTestData))]
    [MemberData(nameof(EmailTestData.ValidEmailsWithBlankSpaces), MemberType = typeof(EmailTestData))]

    //[InlineData(EMAIL_VALID)]
    //[InlineData(EMAIL_VALID_UPPERCASE)]
    //[InlineData(EMAIL_VALID_DOMAIN_UPPERCASE)]
    //[InlineData(EMAIL_VALID_STARTING_BLANK_SPACES)]
    //[InlineData(EMAIL_VALID_ENDING_BLANK_SPACES)]
    //[InlineData(EMAIL_VALID_BLANK_SPACES)]
    public void ShouldExplicitConvertToString(string emailAddress)
    {
        var command = new CreateEmailCommand(emailAddress, _dateTimeProviderMock.Object);
        Assert.Equal(emailAddress, command.Email.ToString());
    }

    [Theory]
    [MemberData(nameof(EmailTestData.ValidEmails), MemberType = typeof(EmailTestData))]
    //[MemberData(nameof(EmailTestData.ValidEmailsWithBlankSpaces), MemberType = typeof(EmailTestData))]

    //[InlineData(EMAIL_VALID)]
    //[InlineData(EMAIL_VALID_UPPERCASE)]
    //[InlineData(EMAIL_VALID_DOMAIN_UPPERCASE)]
    //[InlineData(EMAIL_VALID_STARTING_BLANK_SPACES)]
    //[InlineData(EMAIL_VALID_ENDING_BLANK_SPACES)]
    //[InlineData(EMAIL_VALID_BLANK_SPACES)]
    public void ShouldReturnEmailWhenCallToStringMethod(string emailAddress)
    {
        var command = new CreateEmailCommand(emailAddress, _dateTimeProviderMock.Object);
        Assert.Equal(emailAddress, command.Email.ToString());        
    }

    [Theory]
    [MemberData(nameof(EmailTestData.ValidEmails), MemberType = typeof(EmailTestData))]
    [MemberData(nameof(EmailTestData.ValidEmailsWithBlankSpaces), MemberType = typeof(EmailTestData))]

    //[InlineData(EMAIL_VALID)]
    //[InlineData(EMAIL_VALID_UPPERCASE)]
    //[InlineData(EMAIL_VALID_DOMAIN_UPPERCASE)]
    //[InlineData(EMAIL_VALID_STARTING_BLANK_SPACES)]
    //[InlineData(EMAIL_VALID_ENDING_BLANK_SPACES)]
    //[InlineData(EMAIL_VALID_BLANK_SPACES)]
    public void ShouldReturnSuccessIfAddValidEmail(string emailAddress)
    {
        var command = new CreateEmailCommand(emailAddress, _dateTimeProviderMock.Object);
        command.Validate();

        if (command.IsValid)
            _emailRepository.AddEmail(command.Email);

        string? emailAdded = _emailRepository.Get(command.Email);
        Assert.NotNull(emailAdded);
    }


    [Theory]
    [InlineData("emailvalido@hotmail.com")]
    public void ShouldReturnFalseWhenTryAddingExistingEmail(string emailAddress)
    {
        var command = new CreateEmailCommand(emailAddress, _dateTimeProviderMock.Object);
        command.Validate();
        bool addedAgain = false;

        if (command.IsValid)
        {
            _emailRepository.AddEmail(command.Email);
            addedAgain = _emailRepository.AddEmail(command.Email);
        }

        Assert.False(addedAgain);
    }
}