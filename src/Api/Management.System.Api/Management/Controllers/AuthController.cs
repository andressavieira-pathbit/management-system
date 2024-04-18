using Management.System.Api.Management.Requests;
using Management.System.Domain.Management.Services;
using Microsoft.AspNetCore.Mvc;

namespace Management.System.Api.Management.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "Management")]
[Produces("application/json")]
[Consumes("application/json")]
[Route("api/management")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController
    (
        IUserService userService,
        IAuthService authService,
        ILogger<AuthController> logger
    )
    {
        _userService = userService;
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("/login")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> HandleAsync
    (
        [FromBody] LoginRequest loginRequest
    )
    {
        try
        {
            var user = await _userService.GetByEmailAsync(loginRequest.Email);

            var token = await _authService.CreateAsync(user.Email, loginRequest.Password);

            return Created(string.Empty, new { Token = token });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar token.");

            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Um ou mais erros ocorreram.",
                Detail = ex.Message
            });
        }
    }
}
