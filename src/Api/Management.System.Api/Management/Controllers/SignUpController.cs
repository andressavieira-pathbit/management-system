using Management.System.Api.Management.Mappers;
using Management.System.Api.Management.Requests;
using Management.System.Domain.Management.Services;
using Management.System.Domain.Management.Services.Dtos;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc;

namespace Management.System.Api.Management.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "Management")]
[Produces("application/json")]
[Consumes("application/json")]
[Route("api/management")]
public class SignUpController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<SignUpController> _logger;

    public SignUpController
    (
        IUserService userService,
        ILogger<SignUpController> logger
    )
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost]
    [Route("/signup")]
    [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> HandleAsync
    (
        [FromBody] SignUpRequest signUpRequest
    )
    {
        try
        {
            var user = signUpRequest.User.Map();

            var customer = signUpRequest.Customer.Map();

            customer = new CustomerDto()
            {
                Name = signUpRequest.Customer.Name,
                UserId = user.UserId
            };

            await _userService.CreateAsync(user, customer);

            return Created(string.Empty, "Cadastro realizado com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar informações do usuário e do cliente.");

            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Um ou mais erros ocorreram.",
                Detail = ex.Message
            });
        }
    }
}
