using System.Collections.Generic;
using System.Linq;
using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity {

public class Person : AbstractEntity
{
    public string Name { get; set; }

    public string Email { get; set; }
    
    public string HashedPassword { get; set; }

    
    private ICollection<Home> _homes = new List<Home>();
    
    /**
     * ManyToMany relation, inverse=Home.Residents
     */
    public ICollection<Home> ConnectedHomes { 
        get => _homes;
        set => _homes = value;
    }
    
    public Person AddHome(Home home)
    {
        
        // If the list already contains this Home do nothing.
        if (ConnectedHomes.Any(h => h.Id == home.Id)) return this;
        
        // Otherwise add the home.
        ConnectedHomes.Add(home);
        // And also populate the inverse side.
        home.AddPerson(this);

        return this;
    }

    public Person RemoveHome(Home home)
    {
        
        // If the list does not contain the home, do nothing.
        if (ConnectedHomes.All(h => h.Id != home.Id)) return this;
        
        // Remove the home.
        ConnectedHomes.Remove(home);
            
        // And also update the inverse side.
        home.RemovePerson(this);

        return this;
    }
    
}

}