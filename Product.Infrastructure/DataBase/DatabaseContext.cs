using Microsoft.EntityFrameworkCore;
using Product.Infrastructure.DataBase.Configuration;

namespace Product.Infrastructure.DataBase
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new ProductReadModelConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
