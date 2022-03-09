using System;
using System.ComponentModel.DataAnnotations;


namespace CashGen.Entities
{
    public class Note
    {
        [Key]
        public Guid Id { get; set; }

        public Guid LinkedId { get; set; }

        public Guid UserId { get; set; }

        public DateTime NoteTime { get; set; }

        public string NoteText { get; set; }
    }
}
