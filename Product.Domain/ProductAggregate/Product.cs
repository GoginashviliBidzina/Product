using Template.Shared;
using Product.Domain.ProductAggregate.Events;
using Product.Domain.ProductAggregate.ValueObjects;

namespace Product.Domain.ProductAggregate
{
    public class Product : AggregateRoot
    {
        public string Name { get; private set; }

        public int Amount { get; private set; }

        public string Code { get; private set; }

        public string CategoryIds { get; set; }

        public Photo Photo { get; private set; }

        protected Product()
        {

        }

        public Product(string name,
                       int amount,
                       string code,
                       string categoryIds,
                       Photo photo)
        {
            Name = name;
            Amount = amount;
            Code = code;
            CategoryIds = categoryIds;
            Photo = photo;

            Raise(new ProductAddedEvent(this));
        }

        public void ChangeDetails(string name,
                                  int amount,
                                  string code,
                                  string categoryIds,
                                  Photo photo)
        {
            Name = name;
            Amount = amount;
            Code = code;
            CategoryIds = categoryIds;
            Photo = photo;

            Raise(new ProductUpdatedEvent(this));
        }

        public void RaiseDeleteEvent()
           => Raise(new ProductDeletedEvent(Id));
    }
}