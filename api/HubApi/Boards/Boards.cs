using HubApi.Model.Entity;
using HubApi.Model.Static;

namespace HubApi.Boards;

public class Board
{
    // TODO: Should come from a central Database
    public static HardwareConfiguration SmartUroV1 { get; } = new ()
    {
        Id = 1,
        Name = "Smart Uro V1",
        Serialnumber = "ABC-123-123",
        AttachedComponents = new List<Component>()
        {
            {
                new Component()
                {
                    Id = 1,
                    Name = "RGB_DIODE_1",
                    Type = ComponentType.RgbDiode,
                    Pins = new List<Pin>()
                    {
                        { new Pin() {Id=1, Direction = PinDirection.Input, Descriptor = "r", HwPinNumber = 10 } },
                        { new Pin() {Id=2, Direction = PinDirection.Input, Descriptor = "g", HwPinNumber = 11 } },
                        { new Pin() {Id=3, Direction = PinDirection.Input, Descriptor = "b", HwPinNumber = 12 } },
                    }
                }
            }
        }
    };
}