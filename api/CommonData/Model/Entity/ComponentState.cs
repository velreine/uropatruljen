using CommonData.Model.Entity;
using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity {

public abstract class ComponentState : AbstractEntity
{

    public bool IsOn { get; set; }
    
    /** The device this saved state belongs to. **/
    public Device Device { get; set; }
    
    /** The component this saved state belongs to. **/
    public Component Component { get; set; }
    
    public ComponentState()
    {
        
    }
}
}