using Template.Shared;

namespace Product.Domain.ProductAggregate.Events
{
    public class ProductAddedEvent : DomainEvent
    {
        public string Message { get; set; }

        public Product Product { get; private set; }

        public ProductAddedEvent(Product product)
        {
            Product = product;
        }
    }
}
