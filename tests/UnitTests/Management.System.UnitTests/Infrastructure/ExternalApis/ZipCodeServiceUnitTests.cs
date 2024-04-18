using Management.System.Application.Services;
using Management.System.Domain.Management.ExternalApis;
using Management.System.Domain.Management.ExternalApis.Dtos;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Xunit;

namespace Management.System.UnitTests.Infrastructure.ExternalApis;

public class ZipCodeServiceUnitTests
{
    private readonly IZipCodeService _sut;
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new();
    private readonly Mock<ILogger<ZipCodeService>> _loggerMock = new();

    public ZipCodeServiceUnitTests()
    {
        _sut = new ZipCodeService(_httpClientFactoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsZipCodeResponse_WhenDataIsValid()
    {
        // Arrange
        var baseUrl = "https://www.ceprapido.com/mock/";
       
        var zipCode = "12345678";
       
        var expectedResponse = new List<ZipCodeDto>
        {
              new() { zipCode = zipCode },
        };

        var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);

        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(expectedResponse)),
            });

        var httpClient = new HttpClient(httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri(baseUrl),
        };

        _httpClientFactoryMock
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        // Act
        var response = await _sut.GetAsync(zipCode);
       
        var result = response.ToString();

        // Removendo espaços em branco no ínicio
        result!.TrimStart();
  
        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.ToString(), result);
    }

    [Fact]
    public async Task GetAsync_ThrowsException_WhenDataIsInvalid()
    {
        // Arrange
        var baseUrl = "https://www.ceprapido.com/mock/";

        var zipCode = "12345678";

        var expectedResponse = new List<ZipCodeDto>();
     

        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(expectedResponse)),
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri(baseUrl),
        };

        _httpClientFactoryMock
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);


        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.GetAsync(zipCode));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal("O JSON retornado está vazio ou não pôde ser desserializado.", exception.Message);
    }
}
