using System.Collections.Generic;
using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity {

public class Person : AbstractEntity
{
    public string Name { get; set; }

    /**
     * ManyToMany relation, inverse=Home.Residents
     */
    public ICollection<Home> ConnectedHomes { get; set; }
}

}