using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity {

public class Person : AbstractEntity
{
    public string Name { get; set; }

    public string Email { get; set; }
    
    public string? HashedPassword { get; set; }
    
    private readonly ICollection<Home> _homes = new List<Home>();
    
    /**
     * ManyToMany relation, inverse=Home.Residents
     */
    public IReadOnlyCollection<Home> Homes => (IReadOnlyCollection<Home>)_homes;

    [Obsolete("This constructor should only be used by Entity Framework and not in User-Land as using this constructor cannot guarantee a \"valid\" entity state.")]
    public Person() {}
    
    public Person(string name, string email)
    {
        Name = name;
        Email = email;
    }
    
    public Person(string name, string email, string hashedPassword, ICollection<Home> homes)
    {
        Name = name;
        Email = email;
        HashedPassword = hashedPassword;
        _homes = homes;
    }
    
    public Person AddHome(Home home)
    {
        
        // If the list already contains this Home do nothing.
        if (_homes.Any(h => h.Id == home.Id)) return this;
        
        // Otherwise add the home.
        _homes.Add(home);
        // And also populate the inverse side.
        home.AddPerson(this);

        return this;
    }

    public Person RemoveHome(Home home)
    {
        
        // If the list does not contain the home, do nothing.
        if (_homes.All(h => h.Id != home.Id)) return this;
        
        // Remove the home.
        _homes.Remove(home);
            
        // And also update the inverse side.
        home.RemovePerson(this);

        return this;
    }
    
}

}