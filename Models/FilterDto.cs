using System;
using System.Collections.Generic;


namespace CashGen.Models
{
    public class FilterDto
    {
        public Guid Id { get; set; }

        public string Label { get; set; }

        public IEnumerable<FilterCollectionDto> Collections { get; set; }

        public IEnumerable<FilterOptionDto> Options { get; set; }
    }
}
