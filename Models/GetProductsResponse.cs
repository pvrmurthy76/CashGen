using System.Collections.Generic;


namespace CashGen.Models
{
    public class GetProductsResponse
    {
        public int count { get; set; }

        public List<ProductListDto> results { get; set; }
    }


}
