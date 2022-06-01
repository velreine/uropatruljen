using System.Diagnostics.CodeAnalysis;

namespace HubApi.Settings;

/**
 * Should be bound from appsettings.json
 */
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class MqttAppSettings
{
    /// <summary>
    /// This is the server endpoint that our Hub Api should attempt to connect to.
    /// </summary>
    public string? Endpoint { get; set; }
}