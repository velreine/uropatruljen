using System.Device.Gpio;
using CommonData.Model.Entity;
using CommonData.Model.Static;

namespace HubApi.Manager;

/// <summary>
/// 
/// </summary>
public static class ComponentManager
{
    private static IEnumerable<ComponentState> ComponentStates = new List<ComponentState>
    {
        new RgbComponentState()
        {
            Id = 1,
            RValue = 0,
            GValue = 0,
            BValue = 0,
            Component =
                new Component
                {
                    Id = 1,
                    Name = "Rgb group 1",
                    Type = ComponentType.RgbDiode,
                    Pins = new List<Pin>
                    {
                        new()
                        {
                            Component = null, Direction = PinDirection.Output, Descriptor = "r_input", HwPinNumber = 2
                        },
                        new()
                        {
                            Component = null, Direction = PinDirection.Output, Descriptor = "g_input", HwPinNumber = 3
                        },
                        new()
                        {
                            Component = null, Direction = PinDirection.Output, Descriptor = "b_input", HwPinNumber = 4
                        },
                    }
                },
            Device = { }
        },
        new RgbComponentState()
        {
            Id = 2,
            RValue = 0,
            GValue = 0,
            BValue = 0,
            Component =
                new Component
                {
                    Id = 2,
                    Name = "Rgb group 2",
                    Type = ComponentType.RgbDiode,
                    Pins = new List<Pin>
                    {
                        new()
                        {
                            Component = null, Direction = PinDirection.Output, Descriptor = "r_input", HwPinNumber = 5
                        },
                        new()
                        {
                            Component = null, Direction = PinDirection.Output, Descriptor = "g_input", HwPinNumber = 6
                        },
                        new()
                        {
                            Component = null, Direction = PinDirection.Output, Descriptor = "b_input", HwPinNumber = 7
                        },
                    }
                },
            Device = { }
        },
        new ComponentState()
        {
            Id = 3,
            IsOn = false,
            Component = new Component
            {
                Id = 3,
                Name = "Blue diode group",
                Type = ComponentType.Diode,
                Pins = new List<Pin>
                {
                    new() { Component = null, Direction = PinDirection.Output, Descriptor = "diode", HwPinNumber = 8 }
                }
            },
            Device = { }
        }
    };

    public static void TurnOn(int componentId)
    {
        var component = ComponentStates.Where(c => c.Id == componentId);
        
        if (component is RgbComponentState rgbComponent)
        {
            foreach (var pin in rgbComponent.Component.Pins)
            {
                var value = pin.Descriptor switch
                {
                    "r_input" => rgbComponent.RValue,
                    "g_input" => rgbComponent.GValue,
                    "b_input" => rgbComponent.BValue,
                    _ => throw new Exception("pin not found")
                };
                ApplyValueToPin(pin.HwPinNumber, value);
            }

            rgbComponent.IsOn = true;
        }
        else if (component is ComponentState cs)
        {
            foreach (var pin in cs.Component.Pins)
            {
                ApplyValueToPin(pin.HwPinNumber, 1);
            }

            cs.IsOn = true;
        }
        
    }

    public static void TurnOff(int componentId)
    {
        var component = ComponentStates.FirstOrDefault(c => c.Id == componentId);
        if (component == null) return;
        foreach (var pin in component.Component.Pins)
        {
            ApplyValueToPin(pin.HwPinNumber, 0);
        }

        component.IsOn = false;
    }

    public static void SetColorComponent(int componentId, int rValue, int gValue, int bValue)
    {
        var component = ComponentStates.Where(c => c.Id == componentId) as RgbComponentState;
        if (component != null)
        {
            component.RValue = rValue;
            component.GValue = gValue;
            component.BValue = bValue;
        }

        if (component != null && component.IsOn)
        {
            foreach (var pin in component.Component.Pins)
            {
                var value = pin.Descriptor switch
                {
                    "r_input" => component.RValue,
                    "g_input" => component.GValue,
                    "b_input" => component.BValue,
                    _ => throw new Exception("pin not found")
                };
                ApplyValueToPin(pin.HwPinNumber, value);
            }
        }
    }

    /// <summary>
    /// Returns boolean indicating if the component is on or off.
    /// </summary>
    /// <returns>boolean</returns>
    public static bool ComponentIsOn(int componentId)
    {
        return ComponentStates.FirstOrDefault(c => c.Id == componentId)!.IsOn;
    }

    private static void ApplyValueToPin(int pinNumber, int value)
    {
        using var gpioController = new GpioController();
        if (!gpioController.IsPinOpen(pinNumber)) 
            gpioController.OpenPin(pinNumber, PinMode.Output);
            
        gpioController.Write(pinNumber, value);
    }
}