using System.ComponentModel.DataAnnotations;

namespace CommonData.Model.DTO
{
    /// <summary>
    /// Represents a DTO object for creating new rooms.
    /// </summary>
    public class CreateRoomRequestDTO
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public int HomeId { get; set; }
    }
}