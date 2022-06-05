using System;
using CommonData.Model.Entity.Contracts;
using CommonData.Model.Static;

namespace CommonData.Model.Entity
{
    /**
 * A pin is a logical representation of a pin attached to a component.
 * For a simple diode, this means it has an input pin (voltage input) and an output pin (ground).
 * 
 */
    public class Pin : AbstractEntity
    {
        // A descriptor that is unique for all pins in the attached/related component..
        // For an RGB diode for example this descriptor could be "red_input", "green_input", "blue_input" etc.
        public string Descriptor { get; set; }

        // This is the Pin Number according to the embedded device, e.g. Arduino.
        // As can be examined here: https://www.arduino.cc/en/Tutorial/BuiltInExamples/Blink
        // The Arduino IDE uses a "board descriptor file" that maps these constants like "LED_BUILTIN" to numbers.
        // Notice that on most boards LED_BUILTIN maps to D13 (DIGITAL 13),
        // how-ever on our primary used board MKR1000 LED_BUILTIN maps to D6 (DIGITAL 6)
        // Since we do not have this board descriptor file, this number must be the the physical number of the constant.
        // E.g. LED_BUILTIN => D6 => 14
        public int HwPinNumber { get; set; }

        // Choose between input or output.
        public PinDirection Direction { get; set; }

        // ManyToOne => Component.
        public Component? Component { get; set; }

        [Obsolete("This constructor should only be used by Entity Framework and not in User-Land as using this constructor cannot guarantee a \"valid\" entity state.")]
        public Pin() {}
        
        public Pin(string descriptor, int hwPinNumber, PinDirection direction)
        {
            Descriptor = descriptor;
            HwPinNumber = hwPinNumber;
            Direction = direction;
        }
        
        public Pin(string descriptor, int hwPinNumber, PinDirection direction, Component component)
        {
            Descriptor = descriptor;
            HwPinNumber = hwPinNumber;
            Direction = direction;
            Component = component;
        }
    }
}