using CloudApi.Data;
using CommonData.Model.Entity;

namespace CloudApi.Repository;

public class HomeRepository
{
    private readonly UroContext _dbContext;


    public HomeRepository(UroContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public IEnumerable<Home> GetUserHomes(int userId)
    {
        var homes = _dbContext.Homes.Where(home => home.Residents.Any(p => p.Id == userId));

        return homes;
    }

    public Home? Find(int id)
    {
        return _dbContext.Homes.Find(id);
    }


}