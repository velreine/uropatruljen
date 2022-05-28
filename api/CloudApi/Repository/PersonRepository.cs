using CloudApi.Controllers;
using CloudApi.Data;
using CommonData.Model.Entity;

namespace CloudApi.Repository;

public class PersonRepository
{
    private readonly UroContext _dbContext;

    public PersonRepository(UroContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public Person? Find(int id)
    {
        return _dbContext.Persons.Find(id);
    } 
}