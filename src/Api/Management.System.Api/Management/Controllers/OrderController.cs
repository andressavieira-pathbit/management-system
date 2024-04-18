using Management.System.Api.Management.Mappers;
using Management.System.Api.Management.Requests;
using Management.System.Domain.Management.Services;
using Management.System.Domain.Management.Services.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Management.System.Api.Management.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "Management")]
[Produces("application/json")]
[Consumes("application/json")]
[Route("api/management")]
public class OrderController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;

    public OrderController
    (
        IAuthService authService,
        IOrderService orderService,
        ILogger<OrderController> logger
    )
    {
        _authService = authService;
        _orderService = orderService;
        _logger = logger;
    }

    [HttpPost("/orders")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> HandleAsync
    (
        [FromBody] OrderRequest orderRequest,
        [FromHeader(Name = "x-access-token")] string accessToken
    )
    {
        try
        {
            var email = _authService.GetClaimValueFromToken(accessToken, "email");

            var result = orderRequest.Map(accessToken);

            await _orderService.CreateAsync(result, new CustomerDto()
            {
                Email = email,
            });

            return Created(string.Empty, "Ordem criada com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar ordem.");

            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Um ou mais erros ocorreram.",
                Detail = ex.Message
            });
        }
    }
}
