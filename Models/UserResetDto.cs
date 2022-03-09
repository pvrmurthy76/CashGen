using System;

namespace CashGen.Models
{
    public class UserResetDto
    {
        public string Email { get; set; }

        public Guid ResetToken { get; set; }
    }
}
