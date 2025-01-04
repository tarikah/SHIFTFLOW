namespace ShiftFlow.Domain
{
    public record AuthenticationResult(User User, AuthenticationToken Token);
}
