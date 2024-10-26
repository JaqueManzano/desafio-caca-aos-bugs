using Balta.Domain.Test.Command;
using Balta.Domain.Test.Repository;

namespace Balta.Domain.Test.AccountContext.ValueObjects;

public class EmailTests
{
    private FakeEmailRepository _emailRepository;

    public EmailTests()
    {
        _emailRepository = new FakeEmailRepository();
    }

    [Theory]
    [InlineData("teste@hotmail.com", true)]
    [InlineData("teste@Hotmail.com", false)]
    [InlineData("TESTE@hotmail.com", false)]
    public void ShouldLowerCaseEmail(string param, bool isTrue)
    {
        var command = new CreateEmailCommand(param);
        command.Validate();

        bool isLowerCase = command.email == command.email.ToLower();
        Assert.Equal(isLowerCase, isTrue);
    }

    [Fact]
    public void ShouldTrimEmail() => Assert.Fail();

    [Fact]
    public void ShouldFailIfEmailIsNull()
    {
        var command = new CreateEmailCommand(null);
        command.Validate();

        Assert.False(command.IsValid);
    }

    [Fact]
    public void ShouldFailIfEmailIsEmpty()
    {
        var command = new CreateEmailCommand(string.Empty);
        command.Validate();

        Assert.False(command.IsValid);
    }

    [Fact]
    public void ShouldFailIfEmailIsInvalid() => Assert.Fail();

    [Fact]
    public void ShouldPassIfEmailIsValid() => Assert.Fail();

    [Fact]
    public void ShouldHashEmailAddress() => Assert.Fail();

    [Fact]
    public void ShouldExplicitConvertFromString() => Assert.Fail();

    [Fact]
    public void ShouldExplicitConvertToString() => Assert.Fail();

    [Theory]
    [InlineData("teste@hotmail.com")]
    public void ShouldReturnEmailWhenCallToStringMethod(string email)
    {
        var command = new CreateEmailCommand(email);
        Assert.Equal(email, command.ToString());
    }

    [Fact]
    public void ShouldReturnSuccessIfAddValidEmail()
    {
        var command = new CreateEmailCommand("jaque@hotmail.com");
        command.Validate();

        if (command.IsValid)
            _emailRepository.AddEmail(command.email);

        string? emailAdded = _emailRepository.Get(command.email);
        Assert.NotNull(emailAdded);
    }

    [Fact]
    public void ShouldReturnFalseWhenTryAddingExistingEmail()
    {
        var command = new CreateEmailCommand("jaque@hotmail.com");
        command.Validate();
        bool addedAgain = false;

        if (command.IsValid)
        {
            _emailRepository.AddEmail(command.email);
            addedAgain = _emailRepository.AddEmail(command.email);
        }

        Assert.False(addedAgain);
    }
}