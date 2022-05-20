using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity {

public class Person : AbstractEntity
{
    public string Name { get; set; }

    public Home Home { get; set; }
}

}