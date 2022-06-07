using System.ComponentModel.DataAnnotations;

namespace CommonData.Model.DTO
{
    public class UpdateRoomRequestDTO
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        public UpdateRoomRequestDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}