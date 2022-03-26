
using AutoMapper;
using CashGen.DBContexts;
using CashGen.Entities;
using CashGen.Models;
using CashGen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashGen
{
    public class ChatHttpTrigger
    {
        private readonly ICashGenRepository _cashGenRepository;
        private readonly CashGenContext _context;
        private readonly IMapper _mapper;

        public ChatHttpTrigger(
          ICashGenRepository cashGenRepository,
          CashGenContext context,
          IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
            this._cashGenRepository = cashGenRepository;
        }

        [FunctionName("GetChats")]
        public IActionResult GetChats([HttpTrigger] HttpRequest req, ILogger log)
        {
            int num1 = 1;
            int num2 = 30;
            if (!string.IsNullOrEmpty(req.Query["page"]))
                num1 = Convert.ToInt32(req.Query["page"]);
            if (!string.IsNullOrEmpty(req.Query["size"]))
                num2 = Convert.ToInt32(req.Query["size"]);
            Guid id = Guid.NewGuid();
            bool admin = false;
            if (!string.IsNullOrEmpty(req.Query["admin-view"]))
                admin = true;
            else
                id = new Guid(req.Query["store"]);
            return new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid ? (IActionResult)new OkObjectResult((object)this._mapper.Map<IEnumerable<ChatListDto>>((object)this._cashGenRepository.GetChats(id, admin))) : (IActionResult)new BadRequestResult();
        }

        [FunctionName("GetChat")]
        public IActionResult GetChat([HttpTrigger] HttpRequest req, Guid id, ILogger log) => (IActionResult)new OkObjectResult((object)this._mapper.Map<IEnumerable<ChatDto>>((object)this._cashGenRepository.GetChat(id).ToList<Chat>()));

        [FunctionName("CreateChat")]
        public async Task<IActionResult> CreateChat([HttpTrigger] HttpRequest req, ILogger log)
        {
            ChatForCreationDto chat = JsonConvert.DeserializeObject<ChatForCreationDto>(await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync());
            Chat chat1 = this._mapper.Map<Chat>((object)chat);
            chat1.MessageDate = DateTime.Now;
            Store store = ((IQueryable<Store>)this._context.Stores).Where<Store>((c => c.Id == chat.StoreId)).FirstOrDefault<Store>();
            if (store != null)
                chat1.StoreName = store.Title;
            this._cashGenRepository.AddChat(chat1);
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)true);
        }
    }
}
