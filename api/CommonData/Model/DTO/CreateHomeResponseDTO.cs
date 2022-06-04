using CommonData.Model.Entity;

namespace CommonData.Model.DTO
{
    /// <summary>
    /// Represents a DTO object for the response of a newly created home.
    /// </summary>
    public sealed class CreateHomeResponseDTO
    {
        
        public int Id { get; }
        
        public string Name { get; }

        public CreateHomeResponseDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}