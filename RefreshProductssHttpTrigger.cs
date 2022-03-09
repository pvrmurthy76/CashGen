using AutoMapper;
using CashGen.DBContexts;
using CashGen.Entities;
using CashGen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace CashGen
{
    public class RefreshProductssHttpTrigger
    {
        private readonly CashGenContext _context;
        private readonly ICashGenRepository _cashGenRepository;
        private readonly IMapper _mapper;

        public RefreshProductssHttpTrigger(
          CashGenContext context,
          ICashGenRepository cashGenRepository,
          IMapper mapper)
        {
            this._context = context;
            this._cashGenRepository = cashGenRepository ?? throw new ArgumentNullException(nameof(cashGenRepository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [FunctionName("RefreshProducts")]
        public async Task<IActionResult> RefreshProducts(
          [HttpTrigger] HttpRequest req,
          [Queue("serpapi")] IAsyncCollector<Product> serpapiQueue,
          ILogger log)
        {
            LoggerExtensions.LogInformation(log, "C# HTTP trigger function processed a request.", Array.Empty<object>());
            foreach (Product product in (IEnumerable<Product>)this._context.Products)
                await serpapiQueue.AddAsync(product, new CancellationToken());
            return (IActionResult)new OkObjectResult((object)true);
        }
    }
}
