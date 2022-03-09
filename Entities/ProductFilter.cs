using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashGen.Entities
{
    public class ProductFilter
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("ProductId")]
        public Guid ProductId { get; set; }

        public string Value { get; set; }
    }
}
