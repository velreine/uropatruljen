using CommonData.Model.Entity;

namespace CommonData.Model.State {

public class RgbComponentState : ComponentState
{
    public int RValue { get; set; }
    public int GValue { get; set; }
    public int BValue { get; set; }
    
    public RgbComponentState(Component component) : base(component)
    {
    }
    
    
}

}