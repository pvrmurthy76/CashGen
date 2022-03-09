using AutoMapper;
using CashGen.Entities;
using CashGen.Models;

namespace CashGen.API.Profiles
{
    public class NotesProfile : Profile
    {
        public NotesProfile()
        {
            this.CreateMap<Note, NoteDto>();
            this.CreateMap<NoteForCreationDto, Note>();
        }
    }
}
