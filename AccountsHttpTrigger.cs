using AutoMapper;
using CashGen.DBContexts;
using CashGen.Entities;
using CashGen.Models;
using CashGen.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CashGen
{
    public class AccountsHttpTrigger
    {
        // Fields
        private readonly ICashGenRepository _cashGenRepository;
        private readonly CashGenContext _context;
        private readonly IMapper _mapper;

        // Methods
        public AccountsHttpTrigger(ICashGenRepository cashGenRepository, CashGenContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
            this._cashGenRepository = cashGenRepository;
        }

        [FunctionName("CreateTransaction")]
        public  Task<IActionResult> CreateTransaction(
            [HttpTrigger(0, new string[] { "post"}, Route = "accounts")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("DeleteTransaction")]
        public IActionResult DeleteTransaction(
           [HttpTrigger(0, new string[] { "delete" }, Route = "accounts/{id}")] HttpRequest req,int id,
           ILogger log)
        {
        }

        [FunctionName("ExportAccounts")]
        public  Task<IActionResult> ExportAccounts(
            [HttpTrigger(0, new string[] { "post" }, Route = "accounts/export")] HttpRequest req,
            ILogger log)
        {
        }

        [FunctionName("GetAccount")]
        public IActionResult GetAccount(
            [HttpTrigger(0, new string[] { "get" }, Route = "accounts/{id}")] HttpRequest req,Guid id,
            ILogger log)
        {
        }

        [FunctionName("GetAccounts")]
        public IActionResult GetAccounts(
           [HttpTrigger(0, new string[] { "get" }, Route = "accounts")] HttpRequest req, 
           ILogger log)
        {
        }
    }
}
