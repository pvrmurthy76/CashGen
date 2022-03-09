using System;


namespace CashGen.Models
{
    public class TransactionForCreationDto
    {
        public DateTime TransactionDate { get; set; }

        public Guid StoreId { get; set; }

        public string Description { get; set; }

        public string DebitCredit { get; set; }

        public Decimal Amount { get; set; }

        public Decimal Balance { get; set; }
    }
}
