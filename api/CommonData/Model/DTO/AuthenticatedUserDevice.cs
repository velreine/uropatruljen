using CommonData.Model.Entity;

namespace CommonData.Model.DTO
{
    public class AuthenticatedUserDevice
    {
        public int Id { get; }
        
        public string SerialNumber { get; }
        
        public string Name { get; }
        
        public int HomeId { get; }
        
        public int HardwareLayoutId { get; }
        
        public int? RoomId { get; }

        public AuthenticatedUserDevice(int id, string serialNumber, string name, int homeId, int hardwareLayoutId, int? roomId)
        {
            Id = id;
            SerialNumber = serialNumber;
            Name = name;
            HomeId = homeId;
            HardwareLayoutId = hardwareLayoutId;
            RoomId = roomId;
        }
    }
}