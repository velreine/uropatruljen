using System.Device.Gpio;
using CommonData.Model.Entity;
using CommonData.Model.Static;

namespace HubApi.Manager;

/// <summary>
/// The component manager is responsible for managing components on the board.
/// This manager class must be re-compiled to fit all device models. (Due to component state being static).
/// This is a flaw due to time constraints.
/// </summary>
public static class ComponentManager
{

    private static IEnumerable<ComponentState>? _componentStates;

    private static IEnumerable<ComponentState> ComponentStates => _componentStates ?? GetBoardComponentState();

    private static IEnumerable<ComponentState> GetBoardComponentState()
    {
        // Device is a logical domain-model for giving devices logical names,
        // It is not necessary for this self-aware-board.
        // While the layout describes the layout to external users, the board can be self-aware about it.
        Device device = null;
        HardwareLayout layout = null;
        
        #region COMPONENT_RGB_DIODE_1
        // In this region create everything necessary to represent RGB_DIODE_1
        var rgbDiode1Pin1 = new Pin(1, "r_input", 10, PinDirection.Output);
        var rgbDiode1Pin2 = new Pin(2, "g_input", 11, PinDirection.Output);
        var rgbDiode1Pin3 = new Pin(3, "b_input", 12, PinDirection.Output);

        var rgbDiode1Pins = new List<Pin>();
        rgbDiode1Pins.AddRange(new []{ rgbDiode1Pin1, rgbDiode1Pin2, rgbDiode1Pin3});
        var rgbDiode1 = new Component(1, "RGB GROUP 1", ComponentType.RgbDiode, layout, rgbDiode1Pins);

        // Now create the final state for this component.
        var rgbDiode1State = new RgbComponentState(1, false, device, rgbDiode1, 0, 0, 0);
        #endregion COMPONENT_RGB_DIODE_1


        // Create a list containing all the states.
        var allComponentStates = new List<ComponentState>
        {
            rgbDiode1State
        };

        // Update private field to prevent re-runs of this method.
        _componentStates = allComponentStates;
        
        // Return all the component states.
        return allComponentStates;
    }
    
    /// <summary>
    /// Attempts to turn on a component.
    /// </summary>
    /// <param name="componentId">The identifier of the component to turn on.</param>
    /// <exception cref="Exception"></exception>
    public static void TurnOn(int componentId)
    {
        var component = ComponentStates.FirstOrDefault(c => c.Id == componentId);

        switch (component)
        {
            case RgbComponentState rgbComponent:
            {
                foreach (var pin in rgbComponent?.Component?.Pins)
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
                break;
            }
            case ComponentState cs:
            {
                foreach (var pin in cs.Component.Pins)
                {
                    ApplyValueToPin(pin.HwPinNumber, 1);
                }

                cs.IsOn = true;
                break;
            }
        }
    }

    /// <summary>
    /// Attempts to turn off a component.
    /// </summary>
    /// <param name="componentId">The identifier of the component to turn off.</param>
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

    /// <summary>
    /// Attempts to set the color of a component.
    /// </summary>
    /// <param name="componentId">The identifier of the component to set the color on.</param>
    /// <param name="rValue">The (R)ed value. 0-255.</param>
    /// <param name="gValue">The (G)reen value. 0-255</param>
    /// <param name="bValue">The (B)lue value. 0-255</param>
    /// <exception cref="Exception"></exception>
    public static void SetColorComponent(int componentId, int rValue, int gValue, int bValue)
    {
        var component = ComponentStates.FirstOrDefault(c => c.Id == componentId) as RgbComponentState;
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
    /// Returns a boolean indicating if the component is on or off.
    /// NB. Returns false if the component cannot be found also.
    /// </summary>
    /// <returns>boolean</returns>
    public static bool ComponentIsOn(int componentId)
    {
        var component = ComponentStates.FirstOrDefault(c => c.Id == componentId);

        return component?.IsOn ?? false;
    }

    /// <summary>
    /// Physically writes a value to a pin using GPIO (General Purpose Input/Output) controller.
    /// </summary>
    /// <param name="pinNumber">The hardware pin number.</param>
    /// <param name="value">The value to write.</param>
    private static void ApplyValueToPin(int pinNumber, int value)
    {
        using var gpioController = new GpioController();
        if (!gpioController.IsPinOpen(pinNumber)) 
            gpioController.OpenPin(pinNumber, PinMode.Output);
            
        gpioController.Write(pinNumber, value);
    }
}