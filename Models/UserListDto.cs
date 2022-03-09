using System;


namespace CashGen.Models
{
    public class UserListDto
    {
        public Guid Id { get; set; }

        public string UserLevel { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public string CompanyNumber { get; set; }
    }
}
