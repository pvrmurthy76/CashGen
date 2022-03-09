using System;


namespace CashGen.Models
{
    public class StoreUserForCreationDto
    {
        public Guid UserId { get; set; }

        public Guid StoreId { get; set; }
    }
}
