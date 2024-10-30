using Balta.Domain.AccountContext.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balta.Domain.Test
{
    public static class VerificationCodeTestData
    {
        public static IEnumerable<object[]> InvalidCodes =>
            new List<object[]> {
                new object[] { null },
                new object[] { string.Empty },
                new object[] { new string(' ', 6) },
                new object[] { "test1" },
                new object[] { "test1000" },
            };


    }
}
