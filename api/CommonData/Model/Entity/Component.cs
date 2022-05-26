using System.Collections.Generic;
using CommonData.Model.Entity.Contracts;
using CommonData.Model.Static;

namespace CommonData.Model.Entity {

/**
 * A component represents a physical hardware component that belongs to a hardware model/configuration.
 */
public class Component : AbstractEntity
{
 public string Name { get; set; }
 
 public ComponentType Type { get; set; }
 
 // OneToMany Pins. TODO: måske?
 public ICollection<Pin> Pins { get; set; }
}

}