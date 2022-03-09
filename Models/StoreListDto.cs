using System;


namespace CashGen.Models
{
    public class StoreListDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }

        public string ContactName { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Town { get; set; }

        public string PostCode { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public int Products { get; set; }
    }
}
