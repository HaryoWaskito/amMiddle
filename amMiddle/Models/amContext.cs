using Microsoft.EntityFrameworkCore;

namespace amMiddle.Models
{
    public class amContext : DbContext
    {
        public amContext(DbContextOptions<amContext> options) : base(options)
        {


        }

        public DbSet<amModel> amModels { get; set; }
    }
}
