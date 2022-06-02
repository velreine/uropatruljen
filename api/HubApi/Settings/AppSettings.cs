using System.Diagnostics.CodeAnalysis;

namespace HubApi.Settings;

/**
 * Should be bound from appsettings.json
 */
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class AppSettings
{
    /// <summary>
    /// Hardware settings for the Hub Api.
    /// </summary>
    public HardwareSettings? Hardware { get; set; }
    /// <summary>
    /// Mqtt settings for the Hub Api.
    /// </summary>
    public MqttAppSettings? Mqtt { get; set; }
}