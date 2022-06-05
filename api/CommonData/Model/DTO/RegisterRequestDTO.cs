using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CommonData.Model.DTO
{
    /// <summary>
    /// Data Transfer Object for the register endpoint.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class RegisterRequestDTO
    {
        /// <summary>
        /// The desired name of the registrant.
        /// </summary>
        [Required]
        public string? Name { get; set; }
        
        /// <summary>
        /// The desired e-mail of the registrant.
        /// </summary>
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        
        /// <summary>
        /// The desired password of the registrant.
        /// </summary>
        [Required]
        public string? Password { get; set; }
    }
}