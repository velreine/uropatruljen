using System.ComponentModel.DataAnnotations;

namespace CommonData.Model.DTO
{
    /// <summary>
    /// Represents a DTO object for the response of a newly created room.
    /// </summary>
    public class CreateRoomResponseDTO
    {
        [Required]
        public int Id { get; }
        
        [Required]
        public string Name { get; }
        
        [Required]
        public int HomeId { get;  }

        public CreateRoomResponseDTO(int id, string name, int homeId)
        {
            Id = id;
            Name = name;
            HomeId = homeId;
        }
    }
}