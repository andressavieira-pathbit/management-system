using System.Diagnostics.CodeAnalysis;

namespace Management.System.Api.Management.Requests;

[ExcludeFromCodeCoverage]
public class SignUpRequest
{
    public UserRequest User { get; set; } = default!;
    public CustomerRequest Customer { get; set; } = default!;
}
