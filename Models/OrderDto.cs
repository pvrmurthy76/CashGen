using CashGen.Entities;
using System;
using System.Collections.Generic;

namespace CashGen.Models
{
    public class OrderDto
    {
        public Guid Id { get; set; }

        public long ShopifyId { get; set; }

        public string email { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public Decimal total_price { get; set; }

        public int number { get; set; }

        public string token { get; set; }

        public int order_number { get; set; }

        public string currency { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public string ShippingFirstName { get; set; }

        public string ShippingLastName { get; set; }

        public string ShippingLine1 { get; set; }

        public string ShippingLine2 { get; set; }

        public string ShippingTown { get; set; }

        public string ShippingCounty { get; set; }

        public string ShippingPostCode { get; set; }

        public string fulfillment_status { get; set; }

        public IEnumerable<LineItem> line_items { get; set; }

        public string TrackingCode { get; set; }

        public string TrackingUrl { get; set; }

        public string FulfilmentMethod { get; set; }

        public int CollectionCode { get; set; }

        public string FraudRisk { get; set; }

        public bool BlockPayment { get; set; }

        public int PendingTransactions { get; set; }

        public string FinancialStatus { get; set; }

        public string telephone { get; set; }
    }
}
