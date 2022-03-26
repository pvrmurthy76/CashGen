using AutoMapper;
using CashGen.API;
using CashGen.Entities;
using CashGen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CashGen
{
    public class HttpTriggerDeleteProduct
    {
        private readonly ICashGenRepository _cashGenRepository;
        private readonly IMapper _mapper;

        public HttpTriggerDeleteProduct(ICashGenRepository cashGenRepository, IMapper mapper)
        {
            this._cashGenRepository = cashGenRepository ?? throw new ArgumentNullException(nameof(cashGenRepository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [FunctionName("DeleteProduct")]
        public IActionResult DeleteProduct([HttpTrigger] HttpRequest req, Guid id, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.", Array.Empty<object>());
            string str = "";
            if (!string.IsNullOrEmpty(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()))
                str = ((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>();
            Product product = this._cashGenRepository.GetProduct(id);
            product.IsDeleted = true;
            this._cashGenRepository.UpdateProduct(product);
            this._cashGenRepository.Save();
            this._cashGenRepository.AddEventLog(new EventLog()
            {
                EventDate = DateTime.Now,
                Area = nameof(DeleteProduct),
                EventType = "User Action",
                Message = "Product deleted by user (User Id: " + str + ", Product Id: " + product.Id.ToString() + ", Shopify Product Id: " + product.ShopifyId.ToString() + ")"
            });
            this._cashGenRepository.Save();
            if (!string.IsNullOrEmpty(product.ShopifyId))
                new Shopify().RemoveProduct(product.ShopifyId);
            return (IActionResult)new OkObjectResult((object)true);
        }
    }
}
