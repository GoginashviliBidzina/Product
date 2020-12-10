using Microsoft.EntityFrameworkCore;
using Product.Domain.CategoryAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Product.Infrastructure.DataBase.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
            => builder.ToTable(nameof(Category));
    }
}
