using System.Diagnostics.CodeAnalysis;

namespace CloudApi.Settings;

/// <summary>
/// Should be bound from appsettings.json.
/// Represents settings for Database on the Cloud Api.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class DatabaseSettings
{
    /// <summary>
    /// This is the connection string that the CloudApi will use to connect to the database context.
    /// </summary>
    public string? ConnectionString { get; set; }
}