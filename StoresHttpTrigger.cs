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
using System.Net;
using System.Threading.Tasks;

namespace CashGen
{
    public class StoresHttpTrigger
    {
        private readonly ICashGenRepository _cashGenRepository;
        private readonly CashGenContext _context;
        private readonly IMapper _mapper;

        public StoresHttpTrigger(
          ICashGenRepository cashGenRepository,
          CashGenContext context,
          IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
            this._cashGenRepository = cashGenRepository;
        }

        [FunctionName("GetStores")]
        public IActionResult GetStores([HttpTrigger] HttpRequest req, ILogger log) => new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid ? (IActionResult)new OkObjectResult((object)this._mapper.Map<IEnumerable<StoreListDto>>((object)this._cashGenRepository.GetStores())) : (IActionResult)new BadRequestResult();

        [FunctionName("GetStore")]
        public IActionResult GetProduct([HttpTrigger] HttpRequest req, Guid id, ILogger log) => new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid ? (IActionResult)new OkObjectResult((object)this._mapper.Map<StoreDto>((object)this._cashGenRepository.GetStore(id))) : (IActionResult)new BadRequestResult();

        [FunctionName("CreateStore")]
        public async Task<IActionResult> CreateStore([HttpTrigger] HttpRequest req, ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            Store store = this._mapper.Map<Store>((object)JsonConvert.DeserializeObject<StoreForCreationDto>(await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync()));
            store.CreatedAt = DateTime.Now;
            this._cashGenRepository.AddStore(store);
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)this._mapper.Map<StoreDto>((object)store));
        }

        [FunctionName("UpdateStore")]
        public async Task<IActionResult> UpdateStore(
          [HttpTrigger] HttpRequest req,
          Guid id,
          ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            StoreForUpdateDto storeForUpdateDto = JsonConvert.DeserializeObject<StoreForUpdateDto>(await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync());
            Store store = this._cashGenRepository.GetStore(id);
            this._mapper.Map<StoreForUpdateDto, Store>(storeForUpdateDto, store);
            store.UpdatedAt = DateTime.Now;
            this._cashGenRepository.UpdateStore(store);
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)this._mapper.Map<StoreDto>((object)store));
        }

        [FunctionName("GetGeolocation")]
        public IActionResult GetGeoLocation([HttpTrigger] HttpRequest req, string location, ILogger log)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://maps.googleapis.com/maps/api/geocode/json?address=" + Uri.EscapeUriString(location) + "&key=AIzaSyDnpfj7dYGZiBttDGL6DKJsN6zwY_QLqB4");
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            string str = string.Empty;
            using (Stream responseStream = ((WebResponse)response).GetResponseStream())
            {
                using (StreamReader streamReader = new StreamReader(responseStream))
                    str = ((TextReader)streamReader).ReadToEnd();
            }
            StoresHttpTrigger.GeoData geoData = JsonConvert.DeserializeObject<StoresHttpTrigger.GeoData>(str);
            return geoData.results.Count > 0 ? (IActionResult)new OkObjectResult((object)geoData.results[0].geometry.location) : (IActionResult)new OkObjectResult((object)false);
        }

        [FunctionName("DeleteStore")]
        public IActionResult DeleteStore([HttpTrigger] HttpRequest req, Guid id, ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            this._cashGenRepository.DeleteStore(this._cashGenRepository.GetStore(id));
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)true);
        }

        public class GeoData
        {
            public List<StoresHttpTrigger.GeoDataItem> results { get; set; }
        }

        public class GeoDataItem
        {
            public StoresHttpTrigger.GeoDataGeometry geometry { get; set; }
        }

        public class GeoDataGeometry
        {
            public StoresHttpTrigger.GeoDataLocation location { get; set; }
        }

        public class GeoDataLocation
        {
            public Decimal lat { get; set; }

            public Decimal lng { get; set; }
        }
    }
}
