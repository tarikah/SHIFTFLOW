using ErrorOr;
using MediatR;
using ShiftFlow.Domain;

namespace ShiftFlow.Application.Authetication.Queries;
public record LoginQuery(
    string UserName,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;
