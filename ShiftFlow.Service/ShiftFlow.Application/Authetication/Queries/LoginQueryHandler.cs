using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ShiftFlow.Application.Common.Interfaces.Authentication;
using ShiftFlow.Application.Common.Interfaces.Repositories;
using ShiftFlow.Domain;

namespace ShiftFlow.Application.Authetication.Queries;

public class LoginQueryHandler
    : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginQueryHandler(IUsersRepository usersRepository,
        IPasswordHasher<User> passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query,
        CancellationToken cancellationToken)
    {
        var applicationUser = await _usersRepository.GetByUserNameAsync(query.UserName);

        if (applicationUser == null)
        {
            return Error.Unauthorized("User not found");
        }

        var user = new User(applicationUser);

        if (user is null ||
             user.IsCorrectPasswordHash(query.Password, _passwordHasher) == PasswordVerificationResult.Failed)
        {
            return Error.Unauthorized("Invalid credentials.");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        ErrorOr<AuthenticationResult> response = user is null
            ? Error.Unauthorized("User not found")
            : new AuthenticationResult(user, token);

        return response;
    }
}