using Balta.Domain.AccountContext.ValueObjects.Exceptions;
using Balta.Domain.SharedContext.Abstractions;
using Balta.Domain.SharedContext.Extensions;
using Balta.Domain.Test.Command;
using Balta.Domain.Test.Repository;
using Moq;
using System.Net.Mail;

namespace Balta.Domain.Test.AccountContext.ValueObjects;

public class EmailTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly FakeEmailRepository _emailRepository;

    public EmailTests()
    {
        _emailRepository = new FakeEmailRepository();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);
    }

    [Theory]
    [InlineData("TESTE@hotmail.com")]
    [InlineData("teste@Hotmail.com")]
    [InlineData("teste@HOTMAIL.com")]
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
    [InlineData("  emailvalido@hotmail.com  ")]
    [InlineData("   emailvalido@hotmail.com  ")]
    [InlineData("  email_valido@hotmail.com   ")]
    public void ShouldTrimEmail(string emailAddress)
    {
        var command = new CreateEmailCommand(emailAddress, _dateTimeProviderMock.Object);
        Assert.Equal(emailAddress.Trim(), command.Email.Address);
    }

    [Fact]
    public void ShouldFailIfEmailIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
              new CreateEmailCommand(null, _dateTimeProviderMock.Object));
    }

    [Fact]
    public void ShouldFailIfEmailIsEmpty()
    {
        Assert.Throws<InvalidEmailException>(() =>
        new CreateEmailCommand(string.Empty, _dateTimeProviderMock.Object));
    }

    [Theory]
    [InlineData("emailinvalido.com.br")]
    [InlineData("email-invalido.com.br")]
    [InlineData("email-invalid@")]
    public void ShouldFailIfEmailIsInvalid(string emailAddress)
    {
        Assert.Throws<InvalidEmailException>(() =>
            new CreateEmailCommand(emailAddress, _dateTimeProviderMock.Object));
    }

    [Theory]
    [InlineData("emailvalido@hotmail.com")]
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
    [InlineData("emailvalido@hotmail.com")]
    [InlineData("emailValido@HOTMAIL.com")]
    [InlineData("email_Valido@HOTMAIL.com")]
    public void ShouldHashEmailAddress(string emailAddress)
    {
        var command = new CreateEmailCommand(emailAddress, _dateTimeProviderMock.Object);
        string expectedHash = emailAddress.ToLower().ToBase64();

        Assert.Equal(command.Email.Hash, expectedHash);
    }

    [Theory]
    [InlineData("emailvalido@hotmail.com")]
    public void ShouldExplicitConvertFromString(string emailAddress)
    {
        var command = new CreateEmailCommand(emailAddress, _dateTimeProviderMock.Object);
        Assert.Equal(emailAddress, (string)command.Email);
    }

    [Theory]
    [InlineData("emailvalido@hotmail.com")]
    public void ShouldExplicitConvertToString(string emailAddress)
    {
        var command = new CreateEmailCommand(emailAddress, _dateTimeProviderMock.Object);
        Assert.Equal(emailAddress, command.Email.ToString());
    }

    [Theory]
    [InlineData("emailvalido@hotmail.com")]
    public void ShouldReturnEmailWhenCallToStringMethod(string emailAddress)
    {
        var command = new CreateEmailCommand(emailAddress, _dateTimeProviderMock.Object);
        Assert.Equal(emailAddress, command.Email.ToString());
    }

    [Theory]
    [InlineData("emailvalido@hotmail.com")]
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