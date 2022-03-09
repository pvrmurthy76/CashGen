using System;


namespace CashGen.Models
{
    public class StoreDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }

        public string ContactName { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Town { get; set; }

        public string PostCode { get; set; }

        public string ShopifyId { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public bool Cheque { get; set; }

        public bool Gold { get; set; }

        public bool MoneyTransfer { get; set; }

        public bool Travel { get; set; }

        public bool Vaping { get; set; }

        public string OpenHours { get; set; }

        public bool LiveChat { get; set; }

        public bool ProPlan { get; set; }

        public string GroupName { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string SortCode { get; set; }

        public string AccountsEmail { get; set; }
    }
}
