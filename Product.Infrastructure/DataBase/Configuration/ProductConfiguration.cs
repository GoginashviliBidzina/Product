using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Product.Infrastructure.DataBase.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Domain.ProductAggregate.Product>
    {
        public void Configure(EntityTypeBuilder<Domain.ProductAggregate.Product> builder)
        {
            builder.OwnsOne(o => o.Photo);

            builder.ToTable(nameof(Domain.ProductAggregate.Product));
        }
    }
}
