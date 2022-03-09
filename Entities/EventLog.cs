using System;
using System.ComponentModel.DataAnnotations;


namespace CashGen.Entities
{
    public class EventLog
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime EventDate { get; set; }

        public string Area { get; set; }

        public string EventType { get; set; }

        public string Message { get; set; }
    }
}
