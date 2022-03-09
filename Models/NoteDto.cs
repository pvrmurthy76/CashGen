using System;

namespace CashGen.Models
{
    public class NoteDto
    {
        public DateTime NoteTime { get; set; }

        public string NoteText { get; set; }

        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
