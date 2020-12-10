using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Product.Application.Infrastructure;
using Product.Domain.ProductAggregate.ReadModels;

namespace Product.Application.Queries
{
    public class ProductListingQuery : Query<ProductListingQueryResult>
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int CategoryId { get; set; }

        public async override Task<QueryExecutionResult<ProductListingQueryResult>> ExecuteAsync()
        {
            var products = _db.Set<ProductReadModel>().ToList();

            if (products == null)
                return await FailAsync(ErrorCode.NotFound);

            if (CategoryId > 0)
                products = products.Where(product => !string.IsNullOrWhiteSpace(product.CategoryIds) && product.CategoryIds.Split(',')
                                                                                                                           .Select(Int32.Parse)
                                                                                                                           .Contains(CategoryId))
                                                                                                                           .ToList();

            var listing = products.Skip(PageIndex * PageSize)
                                  .Take(PageSize)
                                  .Select(product => new ProductListing(product.Id,
                                                                                  product.Name,
                                                                                  product.PhotoWidth,
                                                                                  product.PhotoHeight,
                                                                                  product.PhotoUrl,
                                                                                  product.CategoryNames?.Split(',').ToList()));

            var result = new ProductListingQueryResult(listing.Count() < PageSize,
                                                       products.Count(),
                                                       listing);

            return await OkAsync(result);
        }
    }

    public class ProductListing
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int PhotoWidth { get; set; }

        public int PhotoHeight { get; set; }

        public string PhotoUrl { get; set; }

        public List<string> Categories { get; set; }

        public bool IsLastPage { get; set; }

        public ProductListing(int id,
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

    public class ProductListingQueryResult
    {
        public bool IsLastPage { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<ProductListing> ProductListing { get; set; }

        public ProductListingQueryResult(bool isLastPage,
                                         int totalCount,
                                         IEnumerable<ProductListing> productListing)
        {
            IsLastPage = isLastPage;
            TotalCount = totalCount;
            ProductListing = productListing;
        }
    }
}
