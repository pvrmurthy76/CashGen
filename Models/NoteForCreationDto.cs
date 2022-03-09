using System;


namespace CashGen.Models
{
    public class NoteForCreationDto
    {
        public Guid LinkedId { get; set; }

        public Guid UserId { get; set; }

        public DateTime NoteTime { get; set; }

        public string NoteText { get; set; }
    }
}
