using System.ComponentModel.DataAnnotations;


namespace CashGen.Entities
{
    public class ShopifyCollect
    {
        [Key]
        public long Id { get; set; }

        public long ShopifyId { get; set; }

        public long CollectionId { get; set; }

        public long ProductId { get; set; }
    }
}
