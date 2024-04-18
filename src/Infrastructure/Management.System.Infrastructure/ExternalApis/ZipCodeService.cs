using Management.System.Domain.Management.ExternalApis;
using Management.System.Domain.Management.ExternalApis.Dtos;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Management.System.Application.Services;

public class ZipCodeService : IZipCodeService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ZipCodeService> _logger;
    private const string BaseUrl = "https://www.ceprapido.com/api/addresses/";

    public ZipCodeService(IHttpClientFactory httpClientFactory, ILogger<ZipCodeService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<List<ZipCodeDto>> GetAsync(string zipCode)
    {
        try
        {
            string url = $"{BaseUrl}{zipCode}";

            using var httpClient = _httpClientFactory.CreateClient();
           
            HttpResponseMessage response = await httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<List<ZipCodeDto>>(json);

            if (result == null || result.Count == 0)
            {
                throw new Exception("O JSON retornado está vazio ou não pôde ser desserializado.");
            }

            _logger.LogInformation("CEP encontrado com sucesso: {CEP}", result[0].zipCode);

            return result;

        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro ao buscar CEP.");
            throw new Exception("Erro ao buscar CEP.", ex);
        }
    }
}
