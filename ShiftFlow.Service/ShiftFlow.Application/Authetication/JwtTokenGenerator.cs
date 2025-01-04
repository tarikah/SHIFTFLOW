using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShiftFlow.Application.Common.Extensions;
using ShiftFlow.Application.Common.Interfaces.Authentication;
using ShiftFlow.Application.Common.Settings;
using ShiftFlow.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShiftFlow.Application.Authetication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
    {
        var environmentJwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
        if (!string.IsNullOrEmpty(environmentJwtSecret))
        {
            jwtOptions.Value.Secret = environmentJwtSecret;
        }

        _jwtSettings = jwtOptions.Value;
    }

    public AuthenticationToken GenerateToken(User user)
    {
        SigningCredentials credentials = GetSigningCredentials();

        List<Claim> claims = SetUserClaims(user);

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
            signingCredentials: credentials
        );

        var refreshToken = GenerateRefreshToken();

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshTokenExpiryDateTicks =
            DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenValidityInDays).Ticks;

        return new AuthenticationToken(
            accessToken,
            refreshToken,
            RefreshTokenExpiryTicks: refreshTokenExpiryDateTicks);
    }

    public ClaimsPrincipal GetPrincipalFromToken(string accessToken)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            };

            var principal =
                tokenHandler.ValidateToken(accessToken, tokenValidationParameters,
                    out var validatedToken);

            return IsJwtWithValidSecurityAlgorithm(validatedToken)
                ? principal
                : throw new SecurityTokenException("Invalid token algorithm.");
        }
        catch (Exception ex)
        {
            throw new SecurityTokenValidationException("Token validation failed.", ex);
        }
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        return credentials;
    }

    private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
               jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                   StringComparison.InvariantCultureIgnoreCase);
    }

    private static List<Claim> SetUserClaims(User user)
    {
        var claims = new List<Claim>();
        // claims.AddIfNotNull(JwtRegisteredClaimNames.Name, user.FirstName);
        // claims.AddIfNotNull(JwtRegisteredClaimNames.FamilyName, user.LastName);
        claims.AddIfNotNull(JwtRegisteredClaimNames.Email, user.Email);
        claims.AddIfNotNull("id", user.Id);
        return claims;
    }
}

