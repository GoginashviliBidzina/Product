using System;

namespace Product.Domain.ProductAggregate.ReadModels
{
    public class ProductReadModel
    {
        public int Id { get; set; }

        public int AggregateRootId { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }

        public string Code { get; set; }

        public string CategoryIds { get; set; }

        public string CategoryNames { get; set; }

        public DateTime CreatedDate { get; set; }

        public int PhotoWidth { get; set; }

        public int PhotoHeight { get; set; }

        public string PhotoUrl { get; set; }
    }
}
