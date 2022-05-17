using HubApi.Model.Entity.Contracts;

namespace HubApi.Model.Entity;

public class Person : AbstractEntity
{
    public string Name { get; set; }

    public Home Home { get; set; }
}