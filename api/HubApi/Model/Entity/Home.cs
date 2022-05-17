using HubApi.Model.Entity.Contracts;

namespace HubApi.Model.Entity;

public class Home : AbstractEntity
{
    public string Name { get; set; }

    public ICollection<Person> Residents { get; set; }
    
}