using AutoMapper;
using CashGen.Entities;
using CashGen.Models;
using CashGen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CashGen
{
    public class CreateProductHttpTrigger
    {
        private readonly ICashGenRepository _cashGenRepository;
        private readonly IMapper _mapper;

        public CreateProductHttpTrigger(ICashGenRepository cashGenRepository, IMapper mapper)
        {
            this._cashGenRepository = cashGenRepository ?? throw new ArgumentNullException(nameof(cashGenRepository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [FunctionName("CreateProductOld")]
        public async Task<IActionResult> CreateProduct(
          [HttpTrigger] HttpRequest req,
          [Queue("products")] IAsyncCollector<Product> productQueue,
          [Queue("serpapi")] IAsyncCollector<Product> serpapiQueue,
          ILogger log)
        {
            log.LogInformation( "C# HTTP trigger function processed a request.", Array.Empty<object>());
            ProductForCreationDto productForCreationDto = JsonConvert.DeserializeObject<ProductForCreationDto>(await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync());
            productForCreationDto.Status = "DRAFT";
            Product productEntity = this._mapper.Map<Product>((object)productForCreationDto);
            this._cashGenRepository.AddProduct(productEntity);
            this._cashGenRepository.Save();
            await serpapiQueue.AddAsync(productEntity, new CancellationToken());
            IActionResult product = (IActionResult)new OkObjectResult((object)this._mapper.Map<ProductDto>((object)productEntity));
            productEntity = (Product)null;
            return product;
        }
    }
}
