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
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using SerpApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CashGen
{
    public class ProductsHttpTrigger
    {
        private readonly ICashGenRepository _cashGenRepository;
        private readonly CashGenContext _context;
        private readonly IMapper _mapper;

        public ProductsHttpTrigger(
          ICashGenRepository cashGenRepository,
          CashGenContext context,
          IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
            this._cashGenRepository = cashGenRepository;
        }

        [FunctionName("CreateProduct")]
        public async Task<IActionResult> CreateProduct(
          [HttpTrigger] HttpRequest req,
          [Queue("products")] IAsyncCollector<Product> productQueue,
          [Queue("serpapi")] IAsyncCollector<Product> serpapiQueue,
          ILogger log)
        {
            userAuth userAuth = new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>());
            if (!userAuth.valid)
                return (IActionResult)new BadRequestResult();
            Product productEntity = this._mapper.Map<Product>((object)JsonConvert.DeserializeObject<ProductForCreationDto>(await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync()));
            productEntity.Uploading = true;
            productEntity.IsDeleted = false;
            productEntity.CreatedAt = DateTime.Now;
            productEntity.CreatedBy = userAuth.auth_token;
            this._cashGenRepository.AddProduct(productEntity);
            this._context.Notes.Add(new Note()
            {
                LinkedId = productEntity.Id,
                NoteTime = DateTime.Now,
                UserId = userAuth.auth_token,
                NoteText = "Product created"
            });
            this._context.SaveChanges();
            this._cashGenRepository.Save();
            await serpapiQueue.AddAsync(productEntity, new CancellationToken());
            return (IActionResult)new OkObjectResult((object)this._mapper.Map<ProductDto>((object)productEntity));
        }

        [FunctionName("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(
          [HttpTrigger] HttpRequest req,
          Guid id,
          [Queue("serpapi")] IAsyncCollector<Product> serpapiQueue,
          ILogger log)
        {
            userAuth userAuth = new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>());
            if (!userAuth.valid)
                return (IActionResult)new BadRequestResult();
            ProductForUpdateDto productForUpdateDto = JsonConvert.DeserializeObject<ProductForUpdateDto>(await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync());
            this._cashGenRepository.DeleteProductFeatures(id);
            this._cashGenRepository.DeleteProductImages(id);
            this._cashGenRepository.DeleteProductFilters(id);
            this._cashGenRepository.Save();
            Product productFromRepo = this._cashGenRepository.GetProduct(id);
            productFromRepo.Uploading = true;
            productFromRepo.IsDeleted = false;
            productFromRepo.UpdatedAt = DateTime.Now;
            this._mapper.Map<ProductForUpdateDto, Product>(productForUpdateDto, productFromRepo);
            this._cashGenRepository.UpdateProduct(productFromRepo);
            this._context.Notes.Add(new Note()
            {
                LinkedId = productFromRepo.Id,
                NoteTime = DateTime.Now,
                UserId = userAuth.auth_token,
                NoteText = "Product updated"
            });
            this._cashGenRepository.Save();
            await serpapiQueue.AddAsync(productFromRepo, new CancellationToken());
            return (IActionResult)new OkObjectResult((object)this._mapper.Map<ProductDto>((object)productFromRepo));
        }

        [FunctionName("CheckProduct")]
        public async Task<IActionResult> CheckProduct([HttpTrigger] HttpRequest req, ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            ProductCheckDto prodCheckDto = new ProductCheckDto();
            Guid productId = new Guid();
            String endAsync = await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync();
            Product product = this._mapper.Map<Product>((object)JsonConvert.DeserializeObject<ProductForCheckDto>(endAsync));
            if(product.Id != Guid.Empty)
            {
                productId = product.Id;
            }
            bool flag = true;
            string error = string.Empty;
            if(flag && string.IsNullOrEmpty(product.Barcode)){
                flag = false;
                error= "Please enter a unique Barcode.";
            }
            List<Product> products = ((IQueryable<Product>)this._context.Products).Where<Product>(prod => prod.Id == productId && prod.Barcode.Equals(product.Barcode)).ToList();
            if(flag && products.Count > 0)
            {
                flag = false;
                error = "Barcode in use. Please enter a unique Barcode.";
            }
            prodCheckDto.Valid = false;
            prodCheckDto.Message = error;
            this._cashGenRepository.AddEventLog(new EventLog()
            {
                EventDate = DateTime.Now,
                EventType = "API Request",
                Area = nameof(CheckProduct),
                Message = "Request: " + endAsync + " - Result: " + JsonConvert.SerializeObject((object)prodCheckDto)
            });
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)prodCheckDto);
           
        }

        [FunctionName("LookupResults")]
        public IActionResult LookupResults([HttpTrigger] HttpRequest req, ILogger log)
        {
            string str1 = req.Query["keyword"];
            string apiKey = "f6be2782b4529a15bd6de79e47e8466cd35d0be825bbd313b6c3dfffe25b2c4c";
            Hashtable hashtable = new Hashtable();
            hashtable.Add((object)"engine", (object)"google");
            hashtable.Add((object)"q", (object)str1);
            hashtable.Add((object)"location", (object)"London, England, United Kingdom");
            hashtable.Add((object)"google_domain", (object)"google.co.uk");
            hashtable.Add((object)"gl", (object)"uk");
            hashtable.Add((object)"hl", (object)"en");
            hashtable.Add((object)"tbm", (object)"shop");
            try
            {
                return (IActionResult)new OkObjectResult((object)(new GoogleSearch(hashtable, apiKey)).GetJson()["shopping_results"]);
            }
            catch (SerpApiSearchException ex)
            {
                return (IActionResult)new OkObjectResult((object)new List<string>());
            }
        }

        [FunctionName("LookupProduct")]
        public IActionResult LookupProduct([HttpTrigger] HttpRequest req, string id, ILogger log)
        {
            string str = "f6be2782b4529a15bd6de79e47e8466cd35d0be825bbd313b6c3dfffe25b2c4c";
            Hashtable hashtable = new Hashtable();
            hashtable.Add((object)"engine", (object)"google_product");
            hashtable.Add((object)"product_id", (object)id);
            hashtable.Add((object)"location", (object)"London, England, United Kingdom");
            hashtable.Add((object)"google_domain", (object)"google.co.uk");
            hashtable.Add((object)"gl", (object)"uk");
            hashtable.Add((object)"hl", (object)"en");
            try
            {
                return (IActionResult)new OkObjectResult((object)(new GoogleSearch(hashtable, str)).GetJson());
            }
            catch (SerpApiSearchException ex)
            {
                return (IActionResult)new OkObjectResult((object)((object)ex).ToString());
            }
        }

        [FunctionName("AttachImage")]
        public async Task<IActionResult> AttachImage([HttpTrigger] HttpRequest req, ILogger log)
        {
            ProductsHttpTrigger.AttachResponse resp = new ProductsHttpTrigger.AttachResponse();
            resp.url = "";
            if (((IReadOnlyCollection<IFormFile>)req.Form.Files).Count > 0)
            {
                string str = Guid.NewGuid().ToString().ToLower() + ".jpg";
                using (Stream stream = ((IReadOnlyList<IFormFile>)req.Form.Files)[0].OpenReadStream())
                {
                    CloudBlockBlob blockBlob = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("StorageConnectionString")).CreateCloudBlobClient().GetContainerReference("product-media").GetBlockBlobReference(str);
                    ((CloudBlob)blockBlob).Properties.ContentType = ((IReadOnlyList<IFormFile>)req.Form.Files)[0].ContentType;
                    await blockBlob.UploadFromStreamAsync(stream);
                    resp.url = ((CloudBlob)blockBlob).Uri.AbsoluteUri;
                    blockBlob = (CloudBlockBlob)null;
                }
            }
            IActionResult iactionResult = (IActionResult)new OkObjectResult((object)resp);
            resp = (ProductsHttpTrigger.AttachResponse)null;
            return iactionResult;
        }

        public class AttachResponse
        {
            public string url { get; set; }
        }
    }
}
