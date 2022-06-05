using System;

namespace CommonData.Model.Entity.Contracts {

public abstract class AbstractEntity : IEntity
{
    public int? Id { get; set; }

    protected AbstractEntity(int? id)
    {
        Id = id;
    }

    [Obsolete("This constructor should only be used by Entity Framework and not in User-Land as using this constructor cannot guarantee a \"valid\" entity state.")]
    public AbstractEntity()
    {
        
    }
}

}