using Balta.Domain.SharedContext.Extensions;

namespace Balta.Domain.Test.SharedContext.Extensions;

public class StringExtensionsTests
{
    private const string VALUE_TO_CONVERT = "Value To Convert";

    [Fact]
    public void ShouldGenerateBase64FromString()
    {
        var result = StringExtensions.ToBase64(VALUE_TO_CONVERT);
        string expectedBase64 = VALUE_TO_CONVERT.ToBase64();
        Assert.Equal(expectedBase64, result);

    }
}