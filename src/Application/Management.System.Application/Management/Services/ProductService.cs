using System.Net.Http.Headers;
using Management.System.Domain.Management.Repositories;
using Microsoft.Extensions.Logging;
using Management.System.Application.Management.Mappers;
using Management.System.Domain.Management.Enums;
using Management.System.Domain.Management.Services;
using Management.System.Domain.Management.Services.Dtos;

namespace Management.System.Application.Management.Services
{
    public class ProductService : IProductService
    {
        private readonly IAuthService _authService;
        private readonly IProductRepository _productRepository;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IAuthService authService,
            IProductRepository productRepository,
            HttpClient httpClient,
            ILogger<ProductService> logger
        )
        {
            _authService = authService;
            _productRepository = productRepository;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id)
        {
            var result = await _productRepository.GetByIdAsync(id);

            if (result == null)
            {
                throw new Exception("Produto não encontrado.");
            }

            var response = result.Map();

            return response;
        }

        public async Task CreateAsync(ProductDto productDto, UserDto userDto)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", productDto.AccessToken);

                var userTypeClaim = _authService.GetClaimValueFromToken(productDto.AccessToken, "userType");

                if (string.IsNullOrEmpty(userTypeClaim))
                {
                    throw new UnauthorizedAccessException("Tipo de usuário não encontrado.");
                }

                if (userTypeClaim != EUserType.Admin.ToString())
                {
                    throw new UnauthorizedAccessException("Acesso não autorizado para usuários que não são administradores.");
                }

                var productQuantity = productDto.Quantity;

                if (productQuantity <= 0 || productQuantity != productDto.Quantity)
                {
                    throw new Exception("Produto inválido.");
                }

                var result = new ProductDto()
                {
                    ProductId = productDto.ProductId,
                    Name = productDto.Name,
                    Price = productDto.Price,
                    Quantity = productDto.Quantity,
                    AccessToken = productDto.AccessToken,
                };

                var response = result.Map();

                await _productRepository.CreateAsync(response);

                _logger.LogInformation("Produto criado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar produto.");
                throw;
            }
        }

        public async Task UpdateAsync(Guid id, ProductDto productDto)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);

                if (product == null)
                {
                    throw new Exception("Produto não encontrado.");
                }

                product.Name = productDto.Name;
                product.Price = productDto.Price;
                product.Quantity = productDto.Quantity;

                await _productRepository.UpdateAsync(id, product);

                _logger.LogInformation("Produto atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizado produto.");
                throw;
            }
        }
    }
}
