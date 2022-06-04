using System.ComponentModel.DataAnnotations;

namespace CommonData.Model.DTO
{
    /// <summary>
    /// Represents a DTO object for creating new homes.
    /// </summary>
    public sealed class CreateHomeRequestDTO
    {
        [Required]
        [MinLength(2)]
        public string Name { get; }

        public CreateHomeRequestDTO(string name)
        {
            Name = name;
        }
    }
}