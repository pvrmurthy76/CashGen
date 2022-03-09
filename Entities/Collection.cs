using System;
using System.ComponentModel.DataAnnotations;


namespace CashGen.Entities
{
    public class Collection
    {
        [Key]
        public Guid Id { get; set; }

        public string Handle { get; set; }

        public string Title { get; set; }

        public string ShopifyId { get; set; }

        public Guid ParentId { get; set; }
    }
}
