using System;
using System.Linq;
using Product.Infrastructure.DataBase;
using Product.Domain.CategoryAggregate;
using Product.Domain.ProductAggregate.Events;
using PersonBook.Infrastructure.EvnetDispatching;
using Product.Domain.ProductAggregate.ReadModels;
using System.Collections.Generic;

namespace Product.Application.EventHandlers
{
    public class ProductEventHandlers : IHandleEvent<ProductDeletedEvent>,
                                        IHandleEvent<ProductAddedEvent>
    {
        public void Handle(ProductUpdatedEvent @event, DatabaseContext db)
        {
            var product = @event.Product;
            var categoryIds = !string.IsNullOrWhiteSpace(product.CategoryIds) ? product.CategoryIds.Split(',')
                                                                                                  .Select(Int32.Parse)
                                                                                                  .ToList() : new List<int>();
            var categories = db.Set<Category>()
                               .Where(category => categoryIds.Contains(category.Id))
                               .Distinct()
                               .ToList();

            var productReadModel = db.Set<ProductReadModel>().FirstOrDefault(x => x.AggregateRootId == @event.AggregateRootId);

            if (productReadModel != null)
            {
                productReadModel.AggregateRootId = product.Id;
                productReadModel.Name = product.Name;
                productReadModel.CreatedDate = product.CreateDate.UtcDateTime;
                productReadModel.Code = product.Code;
                productReadModel.Amount = product.Amount;
                productReadModel.PhotoHeight = product.Photo.Height;
                productReadModel.PhotoUrl = product.Photo.Url;
                productReadModel.PhotoWidth = product.Photo.Width;
                productReadModel.CategoryIds = product.CategoryIds;
                productReadModel.CategoryNames = categories?.Any() == true ? string.Join(',', categories.Select(category => category.Name).ToList()) : string.Empty;

                db.Set<ProductReadModel>().Update(productReadModel);
            }
        }

        public void Handle(ProductAddedEvent @event, DatabaseContext db)
        {
            try
            {
                var product = @event.Product;

                var categoryIds = !string.IsNullOrWhiteSpace(product.CategoryIds) ? product.CategoryIds.Split(',')
                                                                                                      .Select(Int32.Parse)
                                                                                                      .ToList() : new List<int>();

                var categories = db.Set<Category>()
                                   .Where(category => categoryIds.Contains(category.Id))
                                   .Distinct()
                                   .ToList();

                var productDetails = new ProductReadModel()
                {
                    AggregateRootId = product.Id,
                    Name = product.Name,
                    CreatedDate = product.CreateDate.UtcDateTime,
                    Code = product.Code,
                    Amount = product.Amount,
                    PhotoHeight = product.Photo.Height,
                    PhotoUrl = product.Photo.Url,
                    PhotoWidth = product.Photo.Width,
                    CategoryIds = product.CategoryIds,
                    CategoryNames = categories?.Any() == true ? string.Join(',', categories.Select(category => category.Name).ToList()) : string.Empty
                };

                db.Set<ProductReadModel>().Add(productDetails);

                @event.Message = "Product Succesfully placed...";
            }
            catch (Exception exc)
            {

                throw;
            }
        }

        public void Handle(ProductDeletedEvent @event, DatabaseContext db)
        {
            var personReadModel = db.Set<ProductReadModel>().FirstOrDefault(product => product.AggregateRootId == @event.AggregateRootId);

            if (personReadModel != null)
                db.Set<ProductReadModel>().Remove(personReadModel);
        }
    }
}
