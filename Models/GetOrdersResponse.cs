using System.Collections.Generic;


namespace CashGen.Models
{
    public class GetOrdersResponse
    {
        public int count { get; set; }

        public List<OrderListDto> results { get; set; }
    }
}
