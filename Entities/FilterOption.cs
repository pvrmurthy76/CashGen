using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashGen.Entities
{
    public class FilterOption
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("FilterId")]
        public Guid FilterId { get; set; }

        public string Value { get; set; }
    }
}
