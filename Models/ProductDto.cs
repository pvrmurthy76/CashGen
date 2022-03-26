using CashGen.Entities;
using System;
using System.Collections.Generic;


namespace CashGen.Models
{
    public class ProductDto
    {
        public Guid Id { get; set; }

        public string Barcode { get; set; }

        public string Title { get; set; }

        public Decimal Price { get; set; }

        public string ProductType { get; set; }

        public string Intro { get; set; }

        public string Description { get; set; }

        public string Brand { get; set; }

        public string GoogleId { get; set; }

        public string Condition { get; set; }

        public string Grade { get; set; }

        public string ConditionText { get; set; }

        public string CatLevel1 { get; set; }

        public string CatLevel2 { get; set; }

        public string CatLevel3 { get; set; }

        public Guid StoreId { get; set; }

        public IEnumerable<Image> Images { get; set; }

        public string Status { get; set; }

        public IEnumerable<Feature> Features { get; set; }

        public IEnumerable<ProductFilter> Filters { get; set; }

        public bool OnSale { get; set; }

        public string Handle { get; set; }

        public bool Uploading { get; set; }

        public int Quantity { get; set; }

        public Decimal WasPrice { get; set; }

        public string Gtin { get; set; }

        public string ShopifyId { get; set; }

        public string FulfilmentOption { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsSold { get; set; }

        public Decimal CostPrice { get; set; }
    }
}
