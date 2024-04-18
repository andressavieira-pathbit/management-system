using Management.System.Application.Management.Mappers;
using Management.System.Domain.Management.Enums;
using Management.System.Domain.Management.ExternalApis;
using Management.System.Domain.Management.Services;
using Management.System.Domain.Management.Services.Dtos;
using Management.System.Domain.Management.Repositories;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace Management.System.Application.Management.Services;

public class OrderService : IOrderService
{
    private readonly IAuthService _authService;
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;
    private readonly IZipCodeService _zipCodeService;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly HttpClient _httpClient;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IAuthService authService,
        ICustomerService customerService,
        IProductService productService,
        IZipCodeService zipCodeService,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        HttpClient httpClient,
        ILogger<OrderService> logger)

    {
        _authService = authService;
        _customerService = customerService;
        _productService = productService;
        _zipCodeService = zipCodeService;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task CreateAsync(OrderDto orderDto, CustomerDto customerDto)
    {
        using var transaction = _unitOfWork.BeginTransaction();

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", orderDto.AccessToken);

            var customerIdClaim = _authService.GetClaimValueFromToken(orderDto.AccessToken, "nameid");

            if (string.IsNullOrEmpty(customerIdClaim))
            {
                throw new UnauthorizedAccessException("ID do usuário não encontrado.");
            }

            var userTypeClaim = _authService.GetClaimValueFromToken(orderDto.AccessToken, "userType");

            if (string.IsNullOrEmpty(userTypeClaim))
            {
                throw new UnauthorizedAccessException("Tipo de usuário não encontrado.");
            }

            if (userTypeClaim != EUserType.Client.ToString())
            {
                throw new UnauthorizedAccessException("Acesso não autorizado para usuários que não são clientes.");
            }

            var address = await _zipCodeService.GetAsync(orderDto.ZipCode);

            if (address == null)
            {
                throw new Exception("Endereço não encontrado para o CEP fornecido.");
            }

            var productId = orderDto.ProductId;

            var product = await _productService.GetByIdAsync(productId);

            if (product == null)
            {
                throw new Exception("Produto informado não encontrado.");
            }

            if (product.Quantity < orderDto.Quantity)
            {
                throw new Exception("Quantidade disponível do produto insuficiente para atender à ordem.");
            }

            var id = new Guid(customerIdClaim);

            var customerId = await _customerService.GetByIdAsync(id);

            var userIdClaim = _authService.GetClaimValueFromToken(orderDto.AccessToken, "userId");

            var userId = new Guid(userIdClaim);

            var customer = new CustomerDto()
            {
                CustomerId = customerId!.CustomerId,
                Name = customerId.Name,
                Email = customerId.Email,
                UserId = userId
            };

            var order = new OrderDto()
            {
                OrderId = orderDto.OrderId,
                Status = orderDto.Status,
                Quantity = orderDto.Quantity,
                Price = product.Price,
                ZipCode = address?.FirstOrDefault()?.zipCode ?? string.Empty,
                DeliveryAddress = address?.FirstOrDefault()?.addressName ?? string.Empty,
                NumberAddress = orderDto.NumberAddress,
                CustomerId = customer.CustomerId,
                ProductId = product.ProductId,
            };

            var response = order.Map();

            await _orderRepository.CreateAsync(response);

            product.Quantity -= orderDto.Quantity;

            await _productService.UpdateAsync(productId, product);

            transaction.Commit();

            _logger.LogInformation("Ordem criada com sucesso.");
        }
        catch (Exception ex)
        {
            transaction.Rollback();

            _logger.LogError(ex, "Erro ao criar ordem.");
            throw;
        }
    }
}
