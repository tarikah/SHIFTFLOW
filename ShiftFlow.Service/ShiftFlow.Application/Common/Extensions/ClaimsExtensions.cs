using System.Security.Claims;

namespace ShiftFlow.Application.Common.Extensions;
public static class ClaimsExtensions
{
    public static List<Claim> AddIfNotNull(this List<Claim> claims, string type, string? value)
    {
        if (value is not null)
        {
            claims.Add(new Claim(type: type, value: value!));
        }

        return claims;
    }
}

