using HubApi.Model.Entity;

namespace HubApi.Model.State;

public abstract class ComponentState
{

    public bool IsOn { get; set; }
    
    protected Component Component { get; init; }
    
    public ComponentState(Component component)
    {
        this.Component = component;
    }
}