
using System.Collections.Generic;


namespace CashGen.Models
{
    internal class ShopifyProductRequest
    {
        public string title { get; set; }

        public string body_html { get; set; }

        public string vendor { get; set; }

        public string product_type { get; set; }

        public string tags { get; set; }

        public List<ShopifyProductVariantRequest> variants { get; set; }

        public string sku { get; set; }

        public List<ShopifyProductImageRequest> images { get; set; }
    }
}
