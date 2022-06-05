using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CommonData.Model.DTO
{
    /// <summary>
    /// Data Transfer Object for the login endpoint.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class LoginRequestDTO
    {
        /// <summary>
        /// The e-mail of the person to sign in.
        /// </summary>
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        
        /// <summary>
        /// The password of the person to sign in.
        /// </summary>
        [Required]
        public string? Password { get; set; }
    }
}