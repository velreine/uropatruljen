using CommonData.Model.Entity;

namespace CommonData.Model.DTO
{
    public class AuthenticatedUserRoom
    {
        public int Id { get; }
        
        public string Name { get; }

        public int HomeId { get; }

        public AuthenticatedUserRoom(int id, string name, int homeId)
        {
            Id = id;
            Name = name;
            HomeId = homeId;
        }
    }
}