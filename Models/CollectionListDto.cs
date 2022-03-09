using System;

namespace CashGen.Models
{
    public class CollectionListDto
    {
        public Guid Id { get; set; }

        public string Handle { get; set; }

        public string Title { get; set; }

        public string ShopifyId { get; set; }
    }
}
