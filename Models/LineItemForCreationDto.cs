using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashGen.Models
{
    public class LineItemForCreationDto
    {
        public long variant_id { get; set; }

        public string title { get; set; }

        public int quantity { get; set; }

        public string sku { get; set; }

        public long product_id { get; set; }
    }
}
