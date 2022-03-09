using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashGen.Entities
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        public DateTime TransactionDate { get; set; }

        public Guid StoreId { get; set; }

        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public string Description { get; set; }

        public string DebitCredit { get; set; }

        public Decimal Amount { get; set; }

        public Decimal Balance { get; set; }

        public bool isPaid { get; set; }

        public int PayoutId { get; set; }

        public string url { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool isPayout { get; set; }

        public bool isManual { get; set; }

        public bool BlockPayment { get; set; }

        public long LineId { get; set; }
    }
}
