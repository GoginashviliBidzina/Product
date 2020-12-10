using System;
using System.Linq;
using FluentValidation;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation.Attributes;
using Microsoft.EntityFrameworkCore;
using Product.Application.Infrastructure;
using Product.Domain.ProductAggregate.ReadModels;

namespace Product.Application.Queries
{
    [Validator(typeof(ProductDetailsQueryValidator))]
    public class ProductDetailsQuery : Query<ProductDetailsQueryResult>
    {
        public int Id { get; set; }

        public async override Task<QueryExecutionResult<ProductDetailsQueryResult>> ExecuteAsync()
        {
            try
            {
                var product = await _db.Set<ProductReadModel>().FirstOrDefaultAsync(prod => prod.AggregateRootId == Id);

                if (product == null)
                    return await FailAsync(ErrorCode.NotFound);

                return await OkAsync(new ProductDetailsQueryResult(product.Id,
                                                                   product.Name,
                                                                   product.Amount,
                                                                   product.PhotoWidth,
                                                                   product.PhotoHeight,
                                                                   product.PhotoUrl,
                                                                   product.CategoryNames?.Split(',').ToList(),
                                                                   product.CategoryIds?.Split(',').Select(Int32.Parse).ToList()));
            }
            catch (Exception)
            {
                return await FailAsync(ErrorCode.Exception);
            }
        }
    }

    internal class ProductDetailsQueryValidator : AbstractValidator<ProductDetailsQuery>
    {
        public ProductDetailsQueryValidator()
        {
            RuleFor(product => product.Id).GreaterThan(0);
        }
    }

    public class ProductDetailsQueryResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }

        public int PhotoWidth { get; set; }

        public int PhotoHeight { get; set; }

        public string PhotoUrl { get; set; }

        public List<string> Categories { get; set; }

        public List<int> CategoryIds { get; set; }

        public ProductDetailsQueryResult(int id,
                                         string name,
                                         int amount,
                                         int width,
                                         int height,
                                         string url,
                                         List<string> categories,
                                         List<int> categoryIds)
        {
            Id = id;
            Name = name;
            Amount = amount;
            PhotoWidth = width;
            PhotoHeight = height;
            PhotoUrl = url;
            Categories = categories;
            CategoryIds = categoryIds;
        }
    }
}
