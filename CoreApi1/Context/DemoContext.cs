using Microsoft.EntityFrameworkCore;

namespace CoreApi1.Context
{
    public class DemoContext:DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DemoContext(DbContextOptions<DemoContext> option) : base(option)
        {

        }
    }
}
