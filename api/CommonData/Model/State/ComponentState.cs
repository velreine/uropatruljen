using CommonData.Model.Entity;

namespace CommonData.Model.State {

public abstract class ComponentState
{

    public bool IsOn { get; set; }
    
    protected Component Component { get; }
    
    public ComponentState(Component component)
    {
        this.Component = component;
    }
}
}