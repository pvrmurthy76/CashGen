using System;
using System.Collections.Generic;


namespace CashGen.Models
{
    public class ProductForUpdateDto
    {
        public string Barcode { get; set; }

        public string Title { get; set; }

        public Decimal Price { get; set; }

        public Guid StoreId { get; set; }

        public string Intro { get; set; }

        public string Status { get; set; }

        public string GoogleId { get; set; }

        public string Condition { get; set; }

        public string Grade { get; set; }

        public string ConditionText { get; set; }

        public string CatLevel1 { get; set; }

        public string CatLevel2 { get; set; }

        public string CatLevel3 { get; set; }

        public ICollection<ImageForCreationDto> Images { get; set; } = (ICollection<ImageForCreationDto>)new List<ImageForCreationDto>();

        public ICollection<FeatureForCreationDto> Features { get; set; } = (ICollection<FeatureForCreationDto>)new List<FeatureForCreationDto>();

        public ICollection<ProductFilterForCreationDto> Filters { get; set; } = (ICollection<ProductFilterForCreationDto>)new List<ProductFilterForCreationDto>();

        public bool OnSale { get; set; }

        public int Quantity { get; set; }

        public Decimal WasPrice { get; set; }

        public string Gtin { get; set; }

        public string FulfilmentOption { get; set; }

        public Decimal CostPrice { get; set; }
    }
}
