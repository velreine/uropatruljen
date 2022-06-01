using System.Diagnostics.CodeAnalysis;

namespace HubApi.Settings;

/**
 * Should be bound from appsettings.json
 */
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class HardwareSettings
{
 /// <summary>
 /// Represents the smart device Serial Number, this should ideally be "burned" into the hardware or similar.
 /// This should be unique per physical product created.
 /// </summary>
 public string? SerialNumber { get; set; }
 /// <summary>
 /// The model number is a unique string that identifies which type of device this is.
 /// Multiple physical created products can share the same model number, but not serial number.
 /// </summary>
 public string? ModelNumber { get; set; }
}