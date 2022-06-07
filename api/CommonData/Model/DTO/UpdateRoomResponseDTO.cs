namespace CommonData.Model.DTO
{
    public class UpdateRoomResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public UpdateRoomResponseDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}