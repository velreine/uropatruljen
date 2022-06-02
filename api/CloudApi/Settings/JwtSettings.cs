using System.Diagnostics.CodeAnalysis;

namespace CloudApi.Settings;

/// <summary>
/// Should be bound from appsettings.json.
/// Represents settings for Json Web Tokens on the Cloud Api.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class JwtSettings
{
    /// <summary>
    /// This is the issuer that should be written in the token (e.g honest-company.com)
    /// (The client should validate that the server that it got the token from indeed matches the issuer field).
    /// </summary>
    public string? Issuer { get; init; }
    /// <summary>
    /// Represents a secret key that the Cloud Api will use to cryptographically sign the tokens,
    /// if this is leaked to the client, an attacker can forge tokens on behalf of the api.
    /// </summary>
    public string? Key { get; init; }
}