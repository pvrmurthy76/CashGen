using System;
using System.Collections.Generic;

namespace CashGen.Models
{
    public class OrderForCreationDto
    {
        public string email { get; set; }

        public DateTime created_at { get; set; }

        public Decimal total_price { get; set; }

        public int number { get; set; }

        public string token { get; set; }

        public int order_number { get; set; }

        public string fulfillment_status { get; set; }

        public ICollection<LineItemForCreationDto> line_items { get; set; }

        public string FulfilmentMethod { get; set; }

        public int CollectionCode { get; set; }

        public string FraudRisk { get; set; }
    }
}
