using System;
using System.Collections.Generic;


namespace CashGen.Models
{
    internal class AccountsForExportDto
    {
        public DateTime start { get; set; }

        public DateTime end { get; set; }

        public List<Guid> stores { get; set; }
    }
}
