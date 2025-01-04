namespace ShiftFlow.Domain;
public record AuthenticationToken(
    string Token,
    string RefreshToken,
    long RefreshTokenExpiryTicks);

