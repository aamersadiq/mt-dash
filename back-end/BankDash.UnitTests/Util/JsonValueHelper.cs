using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BankDash.UnitTests.Util
{
    public class JsonValueHelper
    {
        public static string? GetValue(object value, string property)
        {
            return value?.GetType()?.GetProperty(property)?.GetValue(value, null) as string;
        }

    }
}
