using Balta.Domain.AccountContext.ValueObjects;
using Balta.Domain.AccountContext.ValueObjects.Exceptions;

namespace Balta.Domain.Test.AccountContext.ValueObjects;

public class PasswordTests
{

    [Fact]
    public void ShouldFailIfPasswordIsNull()
    {
        Assert.Throws<InvalidPasswordException>(() => Password.ShouldCreate(null));
    }


    [Fact]
    public void ShouldFailIfPasswordIsEmpty()
    {
        Assert.Throws<InvalidPasswordException>(() => Password.ShouldCreate(string.Empty));
    }

    [Fact]
    public void ShouldFailIfPasswordIsWhiteSpace()
    {
        Assert.Throws<InvalidPasswordException>(() => Password.ShouldCreate(""));
    }

    [Theory]
    [MemberData(nameof(PasswordTestData.PasswordLenIsLessThanMinimumChars), MemberType = typeof(PasswordTestData))]
    public void ShouldFailIfPasswordLenIsLessThanMinimumChars(string plainText)
    {
        Assert.Throws<InvalidPasswordException>(() => Password.ShouldCreate(plainText));
    }

    [Theory]
    [MemberData(nameof(PasswordTestData.PasswordLenIsGreaterThanMaxChars), MemberType = typeof(PasswordTestData))]
    public void ShouldFailIfPasswordLenIsGreaterThanMaxChars(string plainText)
    {
        Assert.Throws<InvalidPasswordException>(() => Password.ShouldCreate(plainText));
    }

    [Theory]
    [MemberData(nameof(PasswordTestData.ValidPasswords), MemberType = typeof(PasswordTestData))]
    public void ShouldHashPassword(string plainText)
    {
        Password password = Password.ShouldCreate(plainText);
        Assert.NotNull(password.Hash);
    }

    [Theory]
    [MemberData(nameof(PasswordTestData.ValidPasswords), MemberType = typeof(PasswordTestData))]
    public void ShouldVerifyPasswordHash(string plainText)
    {
        Password password = Password.ShouldCreate(plainText);
        Assert.True(Password.ShouldMatch(password.Hash, plainText));
    }

    [Fact]
    public void ShouldGenerateStrongPassword()
    {        
        Password password = Password.ShouldGenerate(upperCase:false);
        Assert.True(Password.IsStrongPassword(password));
    }

    [Theory]
    [MemberData(nameof(PasswordTestData.ValidPasswords), MemberType = typeof(PasswordTestData))]
    public void ShouldImplicitConvertToString(string plainText) 
    {
        Password password = Password.ShouldCreate(plainText);
        string implicitConv = password;
        Assert.Equal(implicitConv, password.Hash);
    }

    [Theory]
    [MemberData(nameof(PasswordTestData.ValidPasswords), MemberType = typeof(PasswordTestData))]
    public void ShouldReturnHashAsStringWhenCallToStringMethod(string plainText)
    {
        Password password = Password.ShouldCreate(plainText);
        Assert.Equal(password.Hash, password.ToString());
    }

    [Theory]
    [MemberData(nameof(PasswordTestData.ValidPasswords), MemberType = typeof(PasswordTestData))]
    public void ShouldMarkPasswordAsExpired(string plainText)
    {        
        Password password = Password.ShouldCreate(plainText);
        password.SetNewExpiresAtUtc(DateTime.Now.AddMinutes(-1));
        Assert.True(DateTime.UtcNow >= password.ExpiresAtUtc);
    }

    [Theory]
    [MemberData(nameof(PasswordTestData.ValidPasswords), MemberType = typeof(PasswordTestData))]
    public void ShouldFailIfPasswordIsExpired(string plainText)
    {
        Password password = Password.ShouldCreate(plainText);
        password.SetNewExpiresAtUtc(DateTime.Now.AddMinutes(-1));
        Assert.True(password.IsPasswordExpired());
    }

    [Theory]
    [MemberData(nameof(PasswordTestData.ValidPasswords), MemberType = typeof(PasswordTestData))]
    public void ShouldMarkPasswordAsMustChange(string plainText) 
    {
        Password password = Password.ShouldCreate(plainText);
        password.SetMustChange();
        Assert.True(password.MustChange);
    }

    [Theory]
    [MemberData(nameof(PasswordTestData.ValidPasswords), MemberType = typeof(PasswordTestData))]
    public void ShouldFailIfPasswordIsMarkedAsMustChange(string plainText) 
    {
        Password password = Password.ShouldCreate(plainText);
        password.SetMustChange();
        Assert.True(password.MustChange);
    }
}