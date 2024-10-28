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
    public void ShouldGenerateStrongPassword() => Assert.Fail();

    [Fact]
    public void ShouldImplicitConvertToString() => Assert.Fail();

    [Fact]
    public void ShouldReturnHashAsStringWhenCallToStringMethod() => Assert.Fail();

    [Fact]
    public void ShouldMarkPasswordAsExpired() => Assert.Fail();

    [Fact]
    public void ShouldFailIfPasswordIsExpired() => Assert.Fail();

    [Fact]
    public void ShouldMarkPasswordAsMustChange() => Assert.Fail();

    [Fact]
    public void ShouldFailIfPasswordIsMarkedAsMustChange() => Assert.Fail();
}