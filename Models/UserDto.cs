using System;


namespace CashGen.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string UserLevel { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public string CompanyNumber { get; set; }

        public bool LiveChat { get; set; }

        public bool Accounting { get; set; }
    }
}
