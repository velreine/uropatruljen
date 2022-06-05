using System;

namespace CommonData.Model.Entity {

public class RgbComponentState : ComponentState
{
    public int RValue { get; set; }
    public int GValue { get; set; }
    public int BValue { get; set; }

    [Obsolete("This constructor should only be used by Entity Framework and not in User-Land as using this constructor cannot guarantee a \"valid\" entity state.")]
    public RgbComponentState() {} 
    

    
    public RgbComponentState(bool isOn, Device device, Component component, int rValue, int gValue, int bValue) : base(isOn, device, component)
    {
        RValue = rValue;
        GValue = gValue;
        BValue = bValue;
    }
}
}