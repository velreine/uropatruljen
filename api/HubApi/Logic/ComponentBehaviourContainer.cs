using System.Collections.Immutable;
using CommonData.Model.Static;

namespace HubApi.Logic;

/**
 * A class that holds behaviours attached to each component type.
 */
public class ComponentBehaviourContainer
{
    
    // Control state of the dictionary through explicit methods like Register...
    private readonly Dictionary<ComponentType, ICollection<ComponentBehaviour>> _componentBehaviours;
    
    /// <summary>
    /// Expose the dictionary as an immutable copy, this lets the client know that it is immutable,
    /// (e.g. hinting at the need of use of other methods).
    /// </summary>
    public ImmutableDictionary<ComponentType, ICollection<ComponentBehaviour>> ComponentBehaviours => _componentBehaviours.ToImmutableDictionary();
    
    /// <summary>
    /// The ComponentBehaviourContainer class maps the enum type ComponentType to a list of ComponentBehaviour.
    /// That the type supports.
    /// </summary>
    public ComponentBehaviourContainer()
    {
        
        // Create the default behaviour mapping for the application.
        _componentBehaviours = new Dictionary<ComponentType, ICollection<ComponentBehaviour>>()
        {
            {ComponentType.Diode, new List<ComponentBehaviour>(){ComponentBehaviour.CanBeToggled}},
            {ComponentType.RgbDiode, new List<ComponentBehaviour>(){ComponentBehaviour.CanBeToggled, ComponentBehaviour.CanBeColoured}},
            {ComponentType.Camera, new List<ComponentBehaviour>(){ComponentBehaviour.CanBeToggled}},
            {ComponentType.Microphone, new List<ComponentBehaviour>(){ComponentBehaviour.CanBeToggled}},
        };
        
    }
    
    /// <summary>
    /// Registers a new behaviour to a Component Type.
    /// </summary>
    /// <param name="behaviour"></param>
    /// <param name="type"></param>
    public void RegisterBehaviour(ComponentBehaviour behaviour, ComponentType type)
    {
        
        // If the component type cannot be found in the map, add it and its behaviour.
        if (!_componentBehaviours.TryGetValue(type, out var behaviours))
        {
            _componentBehaviours.Add(type, new List<ComponentBehaviour>(){behaviour});
            return;
        }

        // Normally, just add the behaviour to the existing behaviours.
        if (!behaviours.Contains(behaviour))
        {
            behaviours.Add(behaviour);
        }
    }
    
    
    
}