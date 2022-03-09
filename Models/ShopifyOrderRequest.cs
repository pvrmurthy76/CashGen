using Newtonsoft.Json;
using System.Collections.Generic;


namespace CashGen.Models
{
    internal class ShopifyOrderRequest
    {
        public long location_id { get; set; }

        public bool notify_customer { get; set; }

        [JsonProperty]
        public List<string> tracking_numbers { get; set; }

        [JsonProperty]
        public List<string> tracking_urls { get; set; }

        public string tracking_company { get; set; }

        [JsonProperty]
        public string shipment_status { get; set; }

        [JsonProperty]
        public string status { get; set; }
    }
}
