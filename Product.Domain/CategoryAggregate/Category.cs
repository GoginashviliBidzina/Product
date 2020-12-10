using Template.Shared;

namespace Product.Domain.CategoryAggregate
{
    public class Category : AggregateRoot
    {
        public string Name { get; private set; }

        public Category(string name)
        {
            Name = name;
        }
    }
}
