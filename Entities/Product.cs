using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashGen.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("StoreId")]
        public Store Store { get; set; }

        public Guid StoreId { get; set; }

        public string Barcode { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

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

        public ICollection<Image> Images { get; set; } = (ICollection<Image>)new List<Image>();

        public string Status { get; set; }

        public ICollection<Feature> Features { get; set; } = (ICollection<Feature>)new List<Feature>();

        public string ShopifyId { get; set; }

        public ICollection<ProductFilter> Filters { get; set; } = (ICollection<ProductFilter>)new List<ProductFilter>();

        public bool OnSale { get; set; }

        public string Handle { get; set; }

        public bool Uploading { get; set; }

        public int Quantity { get; set; }

        public Decimal WasPrice { get; set; }

        public string Gtin { get; set; }

        public long InventoryItemId { get; set; }

        public string FulfilmentOption { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Guid CreatedBy { get; set; }

        public bool IsSold { get; set; }

        public Decimal CostPrice { get; set; }
    }
}
