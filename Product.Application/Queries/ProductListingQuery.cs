using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Product.Application.Infrastructure;
using Product.Domain.ProductAggregate.ReadModels;

namespace Product.Application.Queries
{
    public class ProductListingQuery : Query<IEnumerable<ProductListingQueryResult>>
    {
        public int CategoryId { get; set; }

        public async override Task<QueryExecutionResult<IEnumerable<ProductListingQueryResult>>> ExecuteAsync()
        {
            var products = _db.Set<ProductReadModel>().ToList();

            if (products == null)
                return await FailAsync(ErrorCode.NotFound);

            if (CategoryId > 0)
                products = products.Where(product => product.CategoryIds.Split(',')
                                                                        .Select(Int32.Parse)
                                                                        .Contains(CategoryId))
                                                                        .ToList();

            var result = products.Select(product => new ProductListingQueryResult(product.Id,
                                                                                  product.Name,
                                                                                  product.PhotoWidth,
                                                                                  product.PhotoHeight,
                                                                                  product.PhotoUrl,
                                                                                  product.CategoryNames?.Split(',').ToList()));

            return await OkAsync(result);
        }
    }

    public class ProductListingQueryResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int PhotoWidth { get; set; }

        public int PhotoHeight { get; set; }

        public string PhotoUrl { get; set; }

        public List<string> Categories { get; set; }

        public ProductListingQueryResult(int id,
                                         string name,
                                         int width,
                                         int height,
                                         string url,
                                         List<string> categories)
        {
            Id = id;
            Name = name;
            PhotoWidth = width;
            PhotoHeight = height;
            PhotoUrl = url;
            Categories = categories;
        }
    }
}
