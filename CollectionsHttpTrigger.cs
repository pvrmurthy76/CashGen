using AutoMapper;
using CashGen.DBContexts;
using CashGen.Models;
using CashGen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CashGen
{
    public class CollectionsHttpTrigger
    {
        private readonly ICashGenRepository _cashGenRepository;
        private readonly CashGenContext _context;
        private readonly IMapper _mapper;

        public CollectionsHttpTrigger(
          ICashGenRepository cashGenRepository,
          CashGenContext context,
          IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
            this._cashGenRepository = cashGenRepository;
        }

        [FunctionName("GetCollections")]
        public IActionResult GetCollections([HttpTrigger] HttpRequest req, Guid id, ILogger log) => (IActionResult)new OkObjectResult((object)this._mapper.Map<IEnumerable<CollectionListDto>>((object)this._cashGenRepository.GetCollections(id)));
    }
}
