using System.Diagnostics.CodeAnalysis;

namespace Management.System.Domain.Management.ExternalApis.Dtos;

[ExcludeFromCodeCoverage]
public class ZipCodeDto
{
    public string zipCode { get; set; } = default!;
    public string addressName { get; set; } = default!;
    public string districtName { get; set; } = default!;
    public string cityName { get; set; } = default!;
    public string stateName { get; set; } = default!;
    public string countryName { get; set; } = default!;
}
