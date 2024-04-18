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
public class ProductController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController
    (
        IAuthService authService,
        IProductService productService,
        ILogger<ProductController> logger
    )
    {
        _authService = authService;
        _productService = productService;
        _logger = logger;
    }

    [HttpPost("/products")]
    [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> HandleAsync
    (
        [FromBody] ProductRequest productRequest,
        [FromHeader(Name = "x-access-token")] string accessToken
    )
    {
        try
        {
            var email = _authService.GetClaimValueFromToken(accessToken, "email");

            var result = productRequest.Map(accessToken);

            await _productService.CreateAsync(result, new UserDto()
            {
                Email = email!
            });

            return Created(string.Empty, "Produto criado com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar produto.");

            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Um ou mais erros ocorreram.",
                Detail = ex.Message
            });
        }
    }
}

