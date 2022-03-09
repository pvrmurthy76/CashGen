using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashGen.Entities
{
    public class Image
    {
        [Key]
        public Guid Id { get; set; }

        public string Base64 { get; set; }

        [ForeignKey("ProductId")]
        public Guid ProductId { get; set; }

        public string Src { get; set; }
    }
}
