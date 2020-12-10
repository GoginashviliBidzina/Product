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
        public void Handle(ProductAddedEvent @event, DatabaseContext db)
        {
            var product = @event.Product;
            var categoryIds = !string.IsNullOrWhiteSpace(product.CategoryIds) ? product.CategoryIds.Split(',')
                                                                                                  .Select(Int32.Parse)
                                                                                                  .ToList() : new List<int>();

            var categories = db.Set<Category>()
                               .Where(category => categoryIds.Contains(category.Id))
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

        public void Handle(ProductDeletedEvent @event, DatabaseContext db)
        {
            var personReadModel = db.Set<ProductReadModel>().FirstOrDefault(product => product.AggregateRootId == @event.AggregateRootId);

            if (personReadModel != null)
                db.Set<ProductReadModel>().Remove(personReadModel);
        }
    }
}
