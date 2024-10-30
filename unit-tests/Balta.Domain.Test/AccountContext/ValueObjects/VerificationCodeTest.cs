using Balta.Domain.AccountContext.ValueObjects;
using Balta.Domain.AccountContext.ValueObjects.Exceptions;
using Balta.Domain.SharedContext.Abstractions;
using Moq;

namespace Balta.Domain.Test.AccountContext.ValueObjects;

public class VerificationCodeTest
{
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private VerificationCode _verificationCode;

    public VerificationCodeTest()
    {
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);
        _verificationCode = VerificationCode.ShouldCreate(_dateTimeProviderMock.Object);
    }

    [Fact]
    public void ShouldGenerateVerificationCode()
    {        
        Assert.NotNull(_verificationCode.Code);
    }

    [Fact]
    public void ShouldGenerateExpiresAtInFuture()
    {        
        Assert.True(_verificationCode.ExpiresAtUtc > DateTime.UtcNow);
    }

    [Fact]
    public void ShouldGenerateVerifiedAtAsNull()
    {        
        Assert.Null(_verificationCode.VerifiedAtUtc);
    }

    [Fact]
    public void ShouldBeInactiveWhenCreated()
    {        
        _verificationCode.ShouldVerify(_verificationCode);
        Assert.False(_verificationCode.IsActive);
    }

    [Fact]
    public void ShouldFailIfExpired()
    {
        var expiresAtField = typeof(VerificationCode).GetProperty("ExpiresAtUtc");
        expiresAtField?.SetValue(_verificationCode, DateTime.UtcNow.AddMinutes(-1));

        Assert.Throws<InvalidVerificationCodeException>(() => _verificationCode.IsExpired());
    }

    [Theory]
    [MemberData(nameof(VerificationCodeTestData.InvalidCodes), MemberType = typeof(VerificationCodeTestData))]
    public void ShouldFailIfCodeIsInvalid(string code)
    {                
        Assert.Throws<InvalidVerificationCodeException>(() => _verificationCode.ShouldVerify(code));
    }

    [Theory]
    [InlineData("test")]
    public void ShouldFailIfCodeIsLessThanSixChars(string code)
    {        
        Assert.Throws<InvalidVerificationCodeException>(() => _verificationCode.ShouldVerify(code));
    }

    [Theory]
    [InlineData("test10000")]
    public void ShouldFailIfCodeIsGreaterThanSixChars(string code)
    {        
        Assert.Throws<InvalidVerificationCodeException>(() => _verificationCode.ShouldVerify(code));
    }

    [Fact]
    public void ShouldFailIfIsNotActive()
    {        
        _verificationCode.ShouldVerify(_verificationCode);        
        Assert.Throws<InvalidVerificationCodeException>(() => _verificationCode.ShouldVerify(_verificationCode));
    }

    [Fact]
    public void ShouldFailIfIsAlreadyVerified()
    {        
        _verificationCode.ShouldVerify(_verificationCode);
        Assert.Throws<InvalidVerificationCodeException>(() => _verificationCode.ShouldVerify(_verificationCode));
    }

    [Fact]
    public void ShouldFailIfIsVerificationCodeDoesNotMatch()
    {
        string incorrectCode = "fail";                
        Assert.Throws<InvalidVerificationCodeException>(() => _verificationCode.ShouldVerify(incorrectCode));
    }

    [Fact]
    public void ShouldVerify()
    {        
        Exception? exception = Record.Exception(() => _verificationCode.ShouldVerify(_verificationCode.Code));
        Assert.Null(exception);
    }
}