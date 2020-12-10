using Microsoft.EntityFrameworkCore;
using Product.Domain.ProductAggregate.ReadModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Product.Infrastructure.DataBase.Configuration
{
    public class ProductReadModelConfiguration : IEntityTypeConfiguration<ProductReadModel>
    {
        public void Configure(EntityTypeBuilder<ProductReadModel> builder)
            => builder.ToTable(nameof(ProductReadModel));
    }
}
