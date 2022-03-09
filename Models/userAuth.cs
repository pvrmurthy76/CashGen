using System;


namespace CashGen.Models
{
    public class userAuth
    {
        public bool valid { get; set; }

        public Guid auth_token { get; set; }
    }
}
