using System;


namespace CashGen.Models
{
    public class UserPasswordDto
    {
        public Guid ResetToken { get; set; }

        public string Password { get; set; }
    }
}
