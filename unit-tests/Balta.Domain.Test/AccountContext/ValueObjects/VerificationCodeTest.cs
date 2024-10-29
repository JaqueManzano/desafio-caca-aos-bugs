using Balta.Domain.AccountContext.ValueObjects;
using Balta.Domain.AccountContext.ValueObjects.Exceptions;
using Balta.Domain.SharedContext.Abstractions;
using Moq;

namespace Balta.Domain.Test.AccountContext.ValueObjects;

public class VerificationCodeTest
{
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

    public VerificationCodeTest()
    {
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);
    }

    [Fact]
    public void ShouldGenerateVerificationCode()
    {
        VerificationCode verificationCode = VerificationCode.ShouldCreate(_dateTimeProviderMock.Object);
        Assert.NotNull(verificationCode.Code);
    }

    [Fact]
    public void ShouldGenerateExpiresAtInFuture()
    {
        VerificationCode verificationCode = VerificationCode.ShouldCreate(_dateTimeProviderMock.Object);
        Assert.True(verificationCode.ExpiresAtUtc > DateTime.UtcNow);
    }

    [Fact]
    public void ShouldGenerateVerifiedAtAsNull()
    {
        VerificationCode verificationCode = VerificationCode.ShouldCreate(_dateTimeProviderMock.Object);
        Assert.Null(verificationCode.VerifiedAtUtc);
    }

    [Fact]
    public void ShouldBeInactiveWhenCreated()
    {
        VerificationCode verificationCode = VerificationCode.ShouldCreate(_dateTimeProviderMock.Object);
        verificationCode.ShouldVerify(verificationCode);
        Assert.False(verificationCode.IsActive);
    }

    [Fact]
    public void ShouldFailIfExpired()
    {
        VerificationCode verificationCode = VerificationCode.ShouldCreate(_dateTimeProviderMock.Object);

        var expiresAtField = typeof(VerificationCode).GetProperty("ExpiresAtUtc");
        expiresAtField.SetValue(verificationCode, DateTime.UtcNow.AddMinutes(-1));

        Assert.Throws<InvalidVerificationCodeException>(() => verificationCode.IsExpired());
    }

    [Fact]
    public void ShouldFailIfCodeIsInvalid() => Assert.Fail();

    [Fact]
    public void ShouldFailIfCodeIsLessThanSixChars() => Assert.Fail();

    [Fact]
    public void ShouldFailIfCodeIsGreaterThanSixChars() => Assert.Fail();

    [Fact]
    public void ShouldFailIfIsNotActive() => Assert.Fail();

    [Fact]
    public void ShouldFailIfIsAlreadyVerified()
    {
        VerificationCode verificationCode = VerificationCode.ShouldCreate(_dateTimeProviderMock.Object);
        verificationCode.ShouldVerify(verificationCode);
        Assert.Throws<InvalidVerificationCodeException>(() => verificationCode.ShouldVerify(verificationCode));
    }
    [Fact]
    public void ShouldFailIfIsVerificationCodeDoesNotMatch() => Assert.Fail();

    [Fact]
    public void ShouldVerify()
    {
        VerificationCode verificationCode = VerificationCode.ShouldCreate(_dateTimeProviderMock.Object);
        Exception? exception = Record.Exception(() => verificationCode.ShouldVerify(verificationCode.Code));
        Assert.Null(exception);
    }
}