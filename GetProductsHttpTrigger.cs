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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CashGen
{
    public class GetProductsHttpTrigger
    {
        private readonly ICashGenRepository _cashGenRepository;
        private readonly CashGenContext _context;
        private readonly IMapper _mapper;

        public GetProductsHttpTrigger(
          ICashGenRepository cashGenRepository,
          CashGenContext context,
          IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
            this._cashGenRepository = cashGenRepository;
        }

        [FunctionName("GetProducts")]
        public IActionResult GetProducts([HttpTrigger] HttpRequest req, ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
           
            Guid storeId = new Guid(req.Query["store"]);
            int num = 1;
            int count1 = 30;
            if (!string.IsNullOrEmpty(req.Query["page"]))
                num = Convert.ToInt32(req.Query["page"]);
            if (!string.IsNullOrEmpty(req.Query["size"]))
                count1 = Convert.ToInt32(req.Query["size"]);
            int count2 = (num - 1) * count1;
            string _keyword = "";
            string _level1 = "";
            if (!string.IsNullOrEmpty(req.Query["keyword"]))
            {
               _keyword = req.Query["keyword"];
            }
            if (!string.IsNullOrEmpty(req.Query["l1"]))
            {
                _level1 =req.Query["l1"];
            }
            string _level2 = "";
            if (!string.IsNullOrEmpty(req.Query["l2"]))
            {
                _level1 = req.Query["l2"];
            }
            string _level3 = "";
            if (!string.IsNullOrEmpty(req.Query["l3"]))
            {
                _level3 = req.Query["l3"];
            }
            IEnumerable<Product> source = this._context.Products.Where<Product>(product => (product.StoreId == storeId && product.IsDeleted != true) ||
                                                                                           (!String.IsNullOrEmpty(_keyword) && product.Title.ToLower().Contains(_keyword.ToLower())) ||
                                                                                           (!String.IsNullOrEmpty(_level1) && product.CatLevel1.ToLower().Contains(_level1.ToLower())) ||
                                                                                           (!String.IsNullOrEmpty(_level2) && product.CatLevel2.ToLower().Contains(_level2.ToLower())) ||
                                                                                           (!String.IsNullOrEmpty(_level3) && product.CatLevel3.ToLower().Contains(_level3.ToLower())));
            IEnumerable<Product> products = source.Skip<Product>(count2).Take<Product>(count1);
            return (IActionResult) new OkObjectResult((object) new GetProductsResponse()
            {
                count = source.Count<Product>(),
                results = this._mapper.Map<IEnumerable<ProductListDto>>((object)products).ToList<ProductListDto>()
            });
        }
    }
}
