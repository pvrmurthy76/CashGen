using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashGen.Models
{
    internal class AccountListDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public Decimal Balance { get; set; }

        public string GroupName { get; set; }

        public Decimal OpeningBalance { get; set; }

        public Decimal ClosingBalance { get; set; }
    }

}
