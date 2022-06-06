namespace CommonData.Model.DTO
{
    public class UpdateHomeResponseDTO
    {
        public int Id { get; }
        
        public string Name { get; }

        public UpdateHomeResponseDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}