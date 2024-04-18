using Management.System.Domain.Management.ExternalApis.Dtos;

namespace Management.System.Domain.Management.ExternalApis;

public interface IZipCodeService
{
    Task<List<ZipCodeDto>> GetAsync(string zipCode);
}
