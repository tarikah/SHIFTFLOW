using Microsoft.AspNetCore.Identity;
using ShiftFlow.Entities;
using System.Text.RegularExpressions;

namespace ShiftFlow.Domain;
public partial class User
{
    private readonly string _passwordHash;

    public User(
        string id,
        string? userName,
        string passwordHash,
        string? email)
    {
        Id = id;
        UserName = userName;
        Email = email;
        _passwordHash = passwordHash;
    }

    public User(ApplicationUser identityUser)
    {
        Id = identityUser.Id;
        UserName = identityUser.UserName;
        Email = identityUser.Email;
        _passwordHash = identityUser.PasswordHash!;
    }

    public string Id { get; init; }

    public string? UserName { get; init; }
    public string? Email { get; init; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public PasswordVerificationResult IsCorrectPasswordHash(string password,
        IPasswordHasher<User> passwordHasher)
    {
        return passwordHasher.VerifyHashedPassword(this, _passwordHash, password);
    }

    // https://stackoverflow.com/questions/19605150/regex-for-password-must-contain-at-least-eight-characters-at-least-one-number-a
    [GeneratedRegex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        RegexOptions.Compiled)]
    private static partial Regex StrongPasswordRegex();

}
