using System.Device.Gpio;
using CommonData.Model.Entity;
using CommonData.Model.Static;

namespace HubApi.Manager;

public class ComponentManager
{

    private List<Component> components = new List<Component>()
    {
        new()
        {
            Id = 1,
            Type = ComponentType.RgbDiode,
            Pins = new List<Pin>
            {
                new() { Component = null, Direction = PinDirection.Input, Descriptor = "r_input", HwPinNumber = 2 },
                new() { Component = null, Direction = PinDirection.Input, Descriptor = "g_input", HwPinNumber = 3 },
                new() { Component = null, Direction = PinDirection.Input, Descriptor = "b_input", HwPinNumber = 4 },
            }
        }
    };

    public ComponentManager()
    {
        
    }


    public void TurnOnRgb(int componentId, RgbComponentState rgb)
    {
        var component = components.Find(c => c.Id == componentId);
        
        // GPIO 17 which is physical pin 11
        using (GpioController controller = new GpioController(PinNumberingScheme.Logical))
        {
            foreach (var pin in component.Pins)
            {
                if (!controller.IsPinOpen(pin.HwPinNumber))
                {
                    controller.OpenPin(pin.HwPinNumber, PinMode.Output);

                    var rgbValue = pin.Descriptor switch
                    {
                        "r_input" => rgb.RValue,
                        "g_input" => rgb.GValue,
                        "b_input" => rgb.BValue,
                        _ => throw new Exception("wtf pin?")
                    };
                    
                    controller.Write(pin.HwPinNumber, rgbValue/255);
                }
            }
        }
    }
    
}