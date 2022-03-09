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
    public class FiltersHttpTrigger
    {
        private readonly ICashGenRepository _cashGenRepository;
        private readonly CashGenContext _context;
        private readonly IMapper _mapper;

        public FiltersHttpTrigger(
          ICashGenRepository cashGenRepository,
          CashGenContext context,
          IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
            this._cashGenRepository = cashGenRepository;
        }

        [FunctionName("GetFilters")]
        public IActionResult GetFilters([HttpTrigger] HttpRequest req, ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            IEnumerable<Filter> filters = this._cashGenRepository.GetFilters();
            foreach (Filter filter in filters)
            {
                IEnumerable<FilterOption> filterOptions = this._cashGenRepository.GetFilterOptions(filter.Id);
                filter.Options = (ICollection<FilterOption>)filterOptions;
                IEnumerable<FilterCollection> filterCollections = this._cashGenRepository.GetFilterCollections(filter.Id);
                filter.Collections = (ICollection<FilterCollection>)filterCollections;
            }
            return (IActionResult)new OkObjectResult((object)this._mapper.Map<IEnumerable<FilterDto>>((object)filters));
        }

        [FunctionName("GetFilter")]
        public IActionResult GetFilter([HttpTrigger] HttpRequest req, Guid id, ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            Filter filter = this._cashGenRepository.GetFilter(id);
            IEnumerable<FilterOption> filterOptions = this._cashGenRepository.GetFilterOptions(id);
            filter.Options = (ICollection<FilterOption>)filterOptions;
            IEnumerable<FilterCollection> filterCollections = this._cashGenRepository.GetFilterCollections(id);
            filter.Collections = (ICollection<FilterCollection>)filterCollections;
            IEnumerable<FilterCollectionDto> filterCollectionDtos = this._mapper.Map<IEnumerable<FilterCollectionDto>>((object)filterCollections);
            List<FilterCollectionDto> filterCollectionDtoList = new List<FilterCollectionDto>();
            foreach (FilterCollectionDto filterCollectionDto in filterCollectionDtos)
            {
                Collection collection = this._cashGenRepository.GetCollection(filterCollectionDto.CollectionId);
                filterCollectionDto.Title = collection.Title;
                filterCollectionDtoList.Add(filterCollectionDto);
            }
            FilterDto filterDto = this._mapper.Map<FilterDto>((object)filter);
            filterDto.Collections = (IEnumerable<FilterCollectionDto>)filterCollectionDtoList;
            return (IActionResult)new OkObjectResult((object)filterDto);
        }

        [FunctionName("GetCollectionFilters")]
        public IActionResult GetCollectionFilters([HttpTrigger] HttpRequest req, Guid id, ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            IEnumerable<Filter> collectionFilters = this._cashGenRepository.GetCollectionFilters(id);
            foreach (Filter filter in collectionFilters)
            {
                IEnumerable<FilterOption> filterOptions = this._cashGenRepository.GetFilterOptions(filter.Id);
                filter.Options = (ICollection<FilterOption>)filterOptions;
                IEnumerable<FilterCollection> filterCollections = this._cashGenRepository.GetFilterCollections(filter.Id);
                filter.Collections = (ICollection<FilterCollection>)filterCollections;
            }
            return (IActionResult)new OkObjectResult((object)this._mapper.Map<IEnumerable<FilterDto>>((object)collectionFilters));
        }

        [FunctionName("CreateFilter")]
        public async Task<IActionResult> CreateFilter([HttpTrigger] HttpRequest req, ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            Filter filter = this._mapper.Map<Filter>((object)JsonConvert.DeserializeObject<FilterForCreationDto>(await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync()));
            this._cashGenRepository.AddFilter(filter);
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)this._mapper.Map<FilterDto>((object)filter));
        }

        [FunctionName("UpdateFilter")]
        public async Task<IActionResult> UpdateFilter(
          [HttpTrigger] HttpRequest req,
          Guid id,
          ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            FilterForUpdateDto filterForUpdateDto = JsonConvert.DeserializeObject<FilterForUpdateDto>(await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync());
            this._cashGenRepository.DeleteFilterOptions(id);
            this._cashGenRepository.DeleteFilterCollections(id);
            this._cashGenRepository.Save();
            Filter filter = this._cashGenRepository.GetFilter(id);
            this._mapper.Map<FilterForUpdateDto, Filter>(filterForUpdateDto, filter);
            this._cashGenRepository.UpdateFilter(filter);
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)this._mapper.Map<FilterDto>((object)filter));
        }
    }
}
