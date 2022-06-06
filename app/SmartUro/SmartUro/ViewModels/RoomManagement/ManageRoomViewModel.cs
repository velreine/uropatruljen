using CommonData.Model.Entity;

namespace SmartUro.ViewModels.RoomManagement
{
    public class ManageRoomViewModel : BaseViewModel
    {

        private Room _room;

        public Room Room {
            get => _room;
            set => OnPropertyChanged(ref _room, value);
        }

        public ManageRoomViewModel()
        {
            
        }

    }
}