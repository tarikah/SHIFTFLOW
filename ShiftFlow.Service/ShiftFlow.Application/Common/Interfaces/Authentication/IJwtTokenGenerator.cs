using ShiftFlow.Domain;
using System.Security.Claims;

namespace ShiftFlow.Application.Common.Interfaces.Authentication;
public interface IJwtTokenGenerator
{
    AuthenticationToken GenerateToken(User user);

    ClaimsPrincipal GetPrincipalFromToken(string accessToken);
}

