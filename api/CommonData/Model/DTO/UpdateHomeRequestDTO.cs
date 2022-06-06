using System.ComponentModel.DataAnnotations;

namespace CommonData.Model.DTO
{
    public class UpdateHomeRequestDTO
    {
        [Required]
        public int Id { get; }
        
        [Required]
        [MinLength(2)]
        public string Name { get; }
        

        public UpdateHomeRequestDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}