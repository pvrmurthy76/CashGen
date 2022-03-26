using AutoMapper;
using CashGen.DBContexts;
using CashGen.Entities;
using CashGen.Models;
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
    public class HttpTrigger
    {
        private readonly ICashGenRepository _cashGenRepository;
        private readonly CashGenContext _context;
        private readonly IMapper _mapper;

        public HttpTrigger(
          ICashGenRepository cashGenRepository,
          CashGenContext context,
          IMapper mapper)
        {
            this._context = context;
            this._cashGenRepository = cashGenRepository ?? throw new ArgumentNullException(nameof(cashGenRepository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [FunctionName("GetProduct")]
        public IActionResult GetProduct([HttpTrigger] HttpRequest req, Guid id, ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            log.LogInformation("C# HTTP trigger function processed a request.", Array.Empty<object>());
            Product product = this._cashGenRepository.GetProduct(id);
            IEnumerable<Image> images = this._cashGenRepository.GetImages(id);
            product.Images = (ICollection<Image>)images;
            IEnumerable<Feature> features = this._cashGenRepository.GetFeatures(id);
            product.Features = (ICollection<Feature>)features;
            IEnumerable<ProductFilter> productFilters = this._cashGenRepository.GetProductFilters(id);
            product.Filters = (ICollection<ProductFilter>)productFilters;
            return (IActionResult)new OkObjectResult((object)this._mapper.Map<ProductDto>((object)product));
        }
    }
}
