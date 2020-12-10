using Template.Shared;

namespace Product.Domain.ProductAggregate.Events
{
    public class ProductDeletedEvent : DomainEvent
    {
        public ProductDeletedEvent(int id)
        {
            AggregateRootId = id;
        }
    }
}
