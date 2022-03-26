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
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashGen
{
    public class ShopifyHttpTrigger
    {
        private readonly ICashGenRepository _cashGenRepository;
        private readonly CashGenContext _context;
        private readonly IMapper _mapper;

        public ShopifyHttpTrigger(
          ICashGenRepository cashGenRepository,
          CashGenContext context,
          IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
            this._cashGenRepository = cashGenRepository;
        }

        [FunctionName("Contact")]
        public async Task<IActionResult> Contact([HttpTrigger] HttpRequest req, ILogger log)
        {
            ContactDto contact = JsonConvert.DeserializeObject<ContactDto>(await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync());
            CashGen.Models.Mail mail1 = new CashGen.Models.Mail();
            CashGen.Helpers.Mail mail2 = new CashGen.Helpers.Mail();
            List<string> stringList = new List<string>();
            stringList.Add(contact.recipient);
            mail1.templateId = 21795648;
            mail1.to = stringList;
            mail1.subject = "CGUK: Contact Form Submission";
            mail1.name = contact.name;
            mail1.email = contact.email;
            mail1.telephone = contact.telephone;
            mail1.message = contact.message;
            CashGen.Models.Mail msg = mail1;
            mail2.SendMail(msg);
            Chat chat = new Chat();
            Store store1 = new Store();
            Store store2 = ((IQueryable<Store>)this._context.Stores).Where<Store>(c => c.Email == contact.recipient).FirstOrDefault<Store>();
            string str = "";
            Guid guid = Guid.Empty;
            if (store2 != null)
            {
                str = store2.Title;
                guid = store2.Id;
            }
            chat.ParentId = Guid.Empty;
            chat.FromEmail = contact.email;
            chat.FromName = contact.name;
            chat.FromTelephone = contact.telephone;
            chat.Message = contact.message;
            chat.MessageDate = DateTime.Now;
            chat.StoreId = guid;
            chat.Source = "web";
            chat.ChatType = string.IsNullOrEmpty(contact.type) ? "enquiry" : contact.type;
            chat.StoreName = str;
            this._cashGenRepository.AddChat(chat);
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)chat);
        }
    }
}
