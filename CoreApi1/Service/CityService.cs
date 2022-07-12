using CoreApi1.Context;
using CoreApi1.IService;

namespace CoreApi1.Service
{
    public class CityService : BaseService<City>, ICityService
    {
        private readonly DemoContext _DbContext;
        public CityService(DemoContext db) : base(db)
        {
            this._DbContext = db;
        }
    }
}
