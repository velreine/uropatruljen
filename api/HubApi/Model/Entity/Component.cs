using HubApi.Model.Entity.Contracts;
using HubApi.Model.Static;

namespace HubApi.Model.Entity;

/**
 * A component represents a physical hardware component that belongs to a hardware model/configuration.
 */
public class Component : AbstractEntity
{
 public ComponentType Type { get; set; }
 
 // OneToMany Pins.
 public ICollection<Pin> Pins { get; set; }
}