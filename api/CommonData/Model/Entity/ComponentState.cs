using CommonData.Model.Entity;
using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity {

/**
 * Represents the lowest demeanor for persisted component state.
 * This is because ALL components should be able to be Turned On and Off.
 * Therefore all other types of component state should derive from this base class.
 *
 *
 * TODO: Abstract keyword is commented out because of Entity Framework.
 */
public  class ComponentState : AbstractEntity
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