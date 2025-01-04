using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShiftFlow.Application.Authetication.Queries;
using ShiftFlow.Contracts.Authentication;

namespace ShiftFlow.API.Controllers;
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;

    public AuthenticationController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        LoginQuery authQuery = new LoginQuery(request.UserName, request.Password);

        Task<ErrorOr<Domain.AuthenticationResult>> authResult = _mediator.Send(authQuery);

        return authResult.Match(
                result => Ok(authResult),
                Problem);
    }
}

