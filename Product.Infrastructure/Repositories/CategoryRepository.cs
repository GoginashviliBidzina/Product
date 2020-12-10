using Product.Infrastructure.Shared;
using Product.Infrastructure.DataBase;
using Product.Domain.CategoryAggregate.Repository;

namespace Product.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<DatabaseContext, Domain.CategoryAggregate.Category>, ICategoryRepository
    {
        DatabaseContext _context;

        public CategoryRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
