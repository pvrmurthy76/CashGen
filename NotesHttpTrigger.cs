using AutoMapper;
using CashGen.DBContexts;
using CashGen.Entities;
using CashGen.Models;
using CashGen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CashGen
{
    public class NotesHttpTrigger
    {
        private readonly ICashGenRepository _cashGenRepository;
        private readonly CashGenContext _context;
        private readonly IMapper _mapper;

        public NotesHttpTrigger(
          ICashGenRepository cashGenRepository,
          CashGenContext context,
          IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
            this._cashGenRepository = cashGenRepository;
        }

        [FunctionName("GetNotes")]
        public IActionResult GetNotes([HttpTrigger] HttpRequest req, Guid id, ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            IEnumerable<NoteDto> noteDtos = this._mapper.Map<IEnumerable<NoteDto>>((object)this._cashGenRepository.GetNotes(id));
            foreach (NoteDto noteDto in noteDtos)
            {
                if (noteDto.UserId != Guid.Empty)
                {
                    User user = this._cashGenRepository.GetUser(noteDto.UserId);
                    if (user != null)
                    {
                        noteDto.FirstName = user.FirstName;
                        noteDto.LastName = user.LastName;
                    }
                    else
                    {
                        noteDto.FirstName = "System";
                        noteDto.LastName = "";
                    }
                }
                else
                {
                    noteDto.FirstName = "System";
                    noteDto.LastName = "";
                }
            }
            return (IActionResult)new OkObjectResult((object)noteDtos);
        }

        [FunctionName("CreateNote")]
        public async Task<IActionResult> CreateNote([HttpTrigger] HttpRequest req, ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            this._cashGenRepository.AddNote(this._mapper.Map<Note>((object)JsonConvert.DeserializeObject<NoteForCreationDto>(await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync())));
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)true);
        }
    }
}
