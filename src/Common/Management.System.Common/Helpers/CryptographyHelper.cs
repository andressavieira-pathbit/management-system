using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace Management.System.Common.Helpers;

[ExcludeFromCodeCoverage]
public static class CryptographyHelper
{
    public static string CreateHash(this string password)
    {
        var hash = SHA256.Create();
        var encodedValue = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
        var stringBuilder = new StringBuilder();

        foreach (var value in encodedValue)
        {
            stringBuilder.Append(value.ToString("x2"));
        }

        return stringBuilder.ToString();
    }
}
