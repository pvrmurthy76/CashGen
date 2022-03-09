using System.Collections.Generic;


namespace CashGen.Models
{
    public class FilterForCreationDto
    {
        public string Label { get; set; }

        public IEnumerable<FilterCollectionForCreationDto> Collections { get; set; }

        public IEnumerable<FilterOptionForCreationDto> Options { get; set; }
    }
}
