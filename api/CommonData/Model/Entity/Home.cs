using System.Collections.Generic;
using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity {

public class Home : AbstractEntity
{
    public string Name { get; set; }

    public ICollection<Person> Residents { get; set; }
    
}

}