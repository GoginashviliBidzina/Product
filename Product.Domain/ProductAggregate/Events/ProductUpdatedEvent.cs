using Template.Shared;

namespace Product.Domain.ProductAggregate.Events
{
    public class ProductUpdatedEvent : DomainEvent
    {
        public Product Product;

        public ProductUpdatedEvent(Product product)
        {
            Product = product;
        }
    }
}
