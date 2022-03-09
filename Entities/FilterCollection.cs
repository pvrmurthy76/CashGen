using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashGen.Entities
{
    public class FilterCollection
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("FilterId")]
        public Guid FilterId { get; set; }

        [ForeignKey("CollectionId")]
        public Guid CollectionId { get; set; }
    }
}
