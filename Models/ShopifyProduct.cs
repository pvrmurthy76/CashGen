
using System.Collections.Generic;


namespace CashGen.Models
{
    internal class ShopifyProduct
    {
        public long id { get; set; }

        public string handle { get; set; }

        public List<ShopifyProductVariant> variants { get; set; }
    }
}
