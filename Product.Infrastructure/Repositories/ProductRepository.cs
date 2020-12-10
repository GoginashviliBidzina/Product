using Product.Infrastructure.Shared;
using Product.Infrastructure.DataBase;
using Product.Domain.ProductAggregate.Repository;

namespace Product.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<DatabaseContext, Domain.ProductAggregate.Product>, IProductRepository
    {
        DatabaseContext _context;

        public ProductRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
