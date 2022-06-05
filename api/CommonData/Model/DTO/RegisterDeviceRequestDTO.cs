using System.Diagnostics.CodeAnalysis;

namespace CommonData.Model.DTO
{
    /// <summary>
    /// A Data Transfer Object used for the Register Device endpoint.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class RegisterDeviceRequestDTO
    {
        /// <summary>
        /// The model number of the device to register.
        /// </summary>
        public string? ModelNumber { get; set; }
        
        /// <summary>
        /// The serial number of the device to register.
        /// </summary>
        public string? SerialNumber { get; set; }
        
        /// <summary>
        /// The home this device should be registered to. 
        /// </summary>
        public int RegisterToHomeId { get; set; }
    }
}