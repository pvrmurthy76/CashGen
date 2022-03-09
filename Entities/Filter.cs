using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CashGen.Entities
{
    public class Filter
    {
        [Key]
        public Guid Id { get; set; }

        public string Label { get; set; }

        public ICollection<FilterOption> Options { get; set; } = (ICollection<FilterOption>)new List<FilterOption>();

        public ICollection<FilterCollection> Collections { get; set; } = (ICollection<FilterCollection>)new List<FilterCollection>();
    }
}
