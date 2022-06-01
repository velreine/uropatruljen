using System.Collections.Generic;
using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity {

public class Home : AbstractEntity
{
    public string Name { get; set; }

    /**
     * ManyToMany relation, inverse=Person.ConnectedHomes
     */
    public ICollection<Person> Residents { get; set; } = new List<Person>();

}

}