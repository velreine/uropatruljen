using System.Collections.Generic;
using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity {

public class Person : AbstractEntity
{
    public string Name { get; set; }

    public string Email { get; set; }
    
    public string HashedPassword { get; set; }

    /**
     * ManyToMany relation, inverse=Home.Residents
     */
    public ICollection<Home> ConnectedHomes { get; set; }
}

}