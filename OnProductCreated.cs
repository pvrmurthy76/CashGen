using AutoMapper;
using CashGen.API;
using CashGen.DBContexts;
using CashGen.Entities;
using CashGen.Helpers;
using CashGen.Models;
using CashGen.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace CashGen
{
    public class OnProductCreated
    {
        private readonly ICashGenRepository _cashGenRepository;
        private readonly IMapper _mapper;
        private readonly CashGenContext _context;

        public OnProductCreated(
          ICashGenRepository cashGenRepository,
          CashGenContext context,
          IMapper mapper)
        {
            this._cashGenRepository = cashGenRepository ?? throw new ArgumentNullException(nameof(cashGenRepository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [FunctionName("OnProductCreated")]
        public void Run([QueueTrigger("serpapi", Connection = "AzureWebJobsStorage")] Product product, ILogger log)
        {
            Shopify shopify = new Shopify();
            Product productFromRepo = this._cashGenRepository.GetProduct(product.Id);
            if (productFromRepo == null)
                return;
            if (productFromRepo.OnSale)
            {
                ShopifyProductWrapperRequest product1 = new ShopifyProductWrapperRequest();
                ShopifyProductRequest shopifyProductRequest1 = new ShopifyProductRequest();
                ShopifyProductVariantRequest productVariantRequest = new ShopifyProductVariantRequest();
                List<ShopifyProductVariantRequest> productVariantRequestList = new List<ShopifyProductVariantRequest>();
                shopifyProductRequest1.title = productFromRepo.Title;
                shopifyProductRequest1.sku = productFromRepo.Barcode;
                shopifyProductRequest1.body_html = "";
                IEnumerable<Feature> features = this._cashGenRepository.GetFeatures(product.Id);
                if (!string.IsNullOrEmpty(product.ShopifyId))
                {
                    DbSet<ShopifyCollect> shopifyCollects1 = this._context.ShopifyCollects;
                    IEnumerable<ShopifyCollect> shopifyCollects = shopifyCollects1.Where<ShopifyCollect>(shopify => shopify.ProductId.Equals(product.Id));
                    if (shopifyCollects.Count() > 0)
                    {
                        foreach (ShopifyCollect shopifyCollect in shopifyCollects)
                            shopify.RemoveProductFromCollection(shopifyCollect.ShopifyId);
                    }
                }
                Store store1 = new Store();
                DbSet<Store> stores = this._context.Stores;
                Store store = stores.Where<Store>(store1 => store1.Id.Equals(product.StoreId)).FirstOrDefault<Store>();
                /*ParameterExpression parameterExpression = Expression.Parameter(typeof(Store), "c");
                Expression<Func<Store, bool>> predicate = Expression.Lambda<Func<Store, bool>>((Expression)Expression.Equal((Expression)Expression.Property((Expression)parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Store.get_Id))), (Expression)Expression.Property((Expression)Expression.Field((Expression)Expression.Constant((object)cDisplayClass40, typeof(OnProductCreated.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref(OnProductCreated.\u003C\u003Ec__DisplayClass4_0.productFromRepo))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Product.get_StoreId))), false, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Guid.op_Equality))), new ParameterExpression[1]
                {
                     parameterExpression
                });
                Store store2 = ((IQueryable<Store>)stores).Where<Store>(predicate).FirstOrDefault<Store>();*/
                shopifyProductRequest1.body_html += "<h2 class='listing-title'>Item Location</h2>";
                if (productFromRepo.FulfilmentOption == "collection")
                    shopifyProductRequest1.body_html += "<p class='listing-location'>This item is available for <strong>Collection Only</strong> from:<br />";
                else if (productFromRepo.FulfilmentOption == "delivery")
                    shopifyProductRequest1.body_html += "<p class='listing-location'>This item is available for <strong>Delivery Only</strong> from:<br />";
                else
                    shopifyProductRequest1.body_html += "<p class='listing-location'>This item is available for <strong>Collection or Delivery</strong> from:<br />";
                ShopifyProductRequest shopifyProductRequest2 = shopifyProductRequest1;
                shopifyProductRequest2.body_html = shopifyProductRequest2.body_html + "<strong>Cash Generator " + store.Title + "</strong>, " + store.Line1 + ", ";
                if (!string.IsNullOrEmpty(store.Line2))
                {
                    ShopifyProductRequest shopifyProductRequest3 = shopifyProductRequest1;
                    shopifyProductRequest3.body_html = shopifyProductRequest3.body_html + store.Line2 + ", ";
                }
                ShopifyProductRequest shopifyProductRequest4 = shopifyProductRequest1;
                shopifyProductRequest4.body_html = shopifyProductRequest4.body_html + store.Town + ", " + store.PostCode + "<br />";
                ShopifyProductRequest shopifyProductRequest5 = shopifyProductRequest1;
                shopifyProductRequest5.body_html = shopifyProductRequest5.body_html + "Email: <a href='mailto:" + store.Email + "'>" + store.Email + "</a><br />";
                ShopifyProductRequest shopifyProductRequest6 = shopifyProductRequest1;
                shopifyProductRequest6.body_html = shopifyProductRequest6.body_html + "Telephone: <a href='tel:" + store.Telephone + "'>" + store.Telephone + "</a></p>";
                shopifyProductRequest1.body_html += "<h2 class='listing-title'>Item Condition</h2>";
                if (productFromRepo.Condition == "new")
                {
                    if (productFromRepo.ConditionText != "")
                    {
                        ShopifyProductRequest shopifyProductRequest7 = shopifyProductRequest1;
                        shopifyProductRequest7.body_html = shopifyProductRequest7.body_html + "<p class='condition-box'><strong>Condition: New</strong><br />" + productFromRepo.ConditionText + "</p>";
                    }
                    else
                        shopifyProductRequest1.body_html += "<p class='condition-box'><strong>Condition: New</strong></p>";
                }
                if (productFromRepo.Condition == "other")
                {
                    if (productFromRepo.ConditionText != "")
                    {
                        ShopifyProductRequest shopifyProductRequest8 = shopifyProductRequest1;
                        shopifyProductRequest8.body_html = shopifyProductRequest8.body_html + "<p class='condition-box'><strong>Condition: New Other</strong><br />" + productFromRepo.ConditionText + "</p>";
                    }
                    else
                        shopifyProductRequest1.body_html += "<p class='condition-box'><strong>Condition: New Other</strong></p>";
                }
                else if (productFromRepo.Condition == "refurbished")
                {
                    if (productFromRepo.ConditionText != "")
                    {
                        ShopifyProductRequest shopifyProductRequest9 = shopifyProductRequest1;
                        shopifyProductRequest9.body_html = shopifyProductRequest9.body_html + "<p class='condition-box'><strong>Condition: Refurbished: Grade " + productFromRepo.Grade + "</strong><br />" + productFromRepo.ConditionText + "</p>";
                    }
                    else
                    {
                        ShopifyProductRequest shopifyProductRequest10 = shopifyProductRequest1;
                        shopifyProductRequest10.body_html = shopifyProductRequest10.body_html + "<p class='condition-box'><strong>Condition: Refurbished: Grade " + productFromRepo.Grade + "</strong></p>";
                    }
                }
                else if (productFromRepo.Condition == "used")
                {
                    if (productFromRepo.ConditionText != "")
                    {
                        ShopifyProductRequest shopifyProductRequest11 = shopifyProductRequest1;
                        shopifyProductRequest11.body_html = shopifyProductRequest11.body_html + "<p class='condition-box'><strong>Condition: Pre-Owned: Grade " + productFromRepo.Grade + "</strong><br />" + productFromRepo.ConditionText + "</p>";
                    }
                    else
                    {
                        ShopifyProductRequest shopifyProductRequest12 = shopifyProductRequest1;
                        shopifyProductRequest12.body_html = shopifyProductRequest12.body_html + "<p class='condition-box'><strong>Condition: Pre-Owned: Grade " + productFromRepo.Grade + "</strong></p>";
                    }
                }
                shopifyProductRequest1.body_html += "<h2 class='listing-title'>Item Description</h2>";
                ShopifyProductRequest shopifyProductRequest13 = shopifyProductRequest1;
                shopifyProductRequest13.body_html = shopifyProductRequest13.body_html + "<p>" + productFromRepo.Intro + "</p>";
                if (features.Count<Feature>() > 0)
                {
                    shopifyProductRequest1.body_html += "<ul>";
                    foreach (Feature feature in features)
                    {
                        ShopifyProductRequest shopifyProductRequest14 = shopifyProductRequest1;
                        shopifyProductRequest14.body_html = shopifyProductRequest14.body_html + "<li>" + feature.Value + "</li>";
                    }
                    shopifyProductRequest1.body_html += "</ul>";
                }
                Store store3 = this._cashGenRepository.GetStore(product.StoreId);
                shopifyProductRequest1.vendor = "Cash Generator " + store3.Title.Trim();
                productVariantRequest.taxable = false;
                productVariantRequest.compare_at_price = !(productFromRepo.WasPrice > 0M) ? new Decimal?() : new Decimal?(Convert.ToDecimal(productFromRepo.WasPrice));
                productVariantRequest.sku = productFromRepo.Barcode;
                productVariantRequest.barcode = productFromRepo.Barcode;
                productVariantRequest.inventory_quantity = productFromRepo.Quantity;
                productVariantRequest.inventory_management = "shopify";
                productVariantRequest.inventory_policy = "deny";
                productVariantRequest.price = Convert.ToDecimal(productFromRepo.Price);
                productVariantRequestList.Add(productVariantRequest);
                shopifyProductRequest1.variants = productVariantRequestList;
                string empty1 = string.Empty;
                foreach (ProductFilter productFilter in this._cashGenRepository.GetProductFilters(product.Id))
                {
                    if (!string.IsNullOrEmpty(empty1))
                        empty1 += ", ";
                    empty1 += productFilter.Value;
                }
                if (!string.IsNullOrEmpty(empty1))
                    empty1 += ", ";
                string str1 = !(Convert.ToDecimal(productFromRepo.Price) > 5000M) ? (!(Convert.ToDecimal(productFromRepo.Price) > 2000M) ? (!(Convert.ToDecimal(productFromRepo.Price) > 1000M) ? (!(Convert.ToDecimal(productFromRepo.Price) > 500M) ? (!(Convert.ToDecimal(productFromRepo.Price) > 250M) ? (!(Convert.ToDecimal(productFromRepo.Price) > 200M) ? (!(Convert.ToDecimal(productFromRepo.Price) > 150M) ? (!(Convert.ToDecimal(productFromRepo.Price) > 100M) ? (!(Convert.ToDecimal(productFromRepo.Price) > 50M) ? (!(Convert.ToDecimal(productFromRepo.Price) > 25M) ? (!(Convert.ToDecimal(productFromRepo.Price) > 10M) ? empty1 + "£0>£10" : empty1 + "£10>£25") : empty1 + "£25>£50") : empty1 + "£50>£100") : empty1 + "£100>£150") : empty1 + "£150>£200") : empty1 + "£200>£250") : empty1 + "£250>£500") : empty1 + "£500>£1000") : empty1 + "£1000>£2000") : empty1 + "£2000>£5000") : empty1 + ">£5000";
                if (productFromRepo.Condition == "new")
                    str1 += ", New";
                else if (productFromRepo.Condition == "other")
                    str1 += ", New Other";
                else if (productFromRepo.Condition == "refurbished")
                    str1 = str1 + ", Refurbished: Grade " + productFromRepo.Grade;
                else if (productFromRepo.Condition == "used")
                    str1 = str1 + ", Pre-Owned: Grade " + productFromRepo.Grade;
                Store store4 = this._cashGenRepository.GetStore(productFromRepo.StoreId);
                string str2;
                if (!string.IsNullOrEmpty(store4.Line2))
                    str2 = "Cash Generator " + store4.Title + " - " + store4.Line1.Replace(",", "") + " - " + store4.Line2.Replace(",", "") + " - " + store4.Town + " - " + store4.PostCode;
                else
                    str2 = "Cash Generator " + store4.Title + " - " + store4.Line1.Replace(",", "") + " - " + store4.Town + " - " + store4.PostCode;
                if (!string.IsNullOrEmpty(productFromRepo.FulfilmentOption))
                {
                    string str3 = str1 + ", " + productFromRepo.FulfilmentOption;
                    str1 = !(productFromRepo.FulfilmentOption == "collection") ? (!(productFromRepo.FulfilmentOption == "delivery") ? str3 + ", Fulfilment:Delivery or Collection from " + str2 : str3 + ", Fulfilment:Delivery only") : str3 + ", Fulfilment:Collection only from " + str2;
                }
                shopifyProductRequest1.tags = str1;
                List<ShopifyProductImageRequest> productImageRequestList = new List<ShopifyProductImageRequest>();
                IEnumerable<Image> images = this._cashGenRepository.GetImages(product.Id);
                if (images.Count<Image>() == 0)
                {
                    ShopifyProductImageRequest productImageRequest = new ShopifyProductImageRequest();
                    productImageRequest.src = "https://cashgen.blob.core.windows.net/product-media/placeholder.png";
                    productImageRequestList.Add(productImageRequest);
                }
                else
                {
                    int num = 0;
                    foreach (Image image in images)
                    {
                        if (!string.IsNullOrEmpty(image.Src))
                        {
                            ShopifyProductImageRequest productImageRequest = new ShopifyProductImageRequest();
                            string jpg = new ConvertImage().ConvertWebpToJpg(image.Src);
                            productImageRequest.src = jpg;
                            productImageRequestList.Add(productImageRequest);
                            if (num <= 2)
                                ++num;
                            else
                                break;
                        }
                    }
                }
                shopifyProductRequest1.images = productImageRequestList;
                product1.product = shopifyProductRequest1;
                ShopifyProduct shopifyProduct1 = new ShopifyProduct();
                string empty2 = string.Empty;
                if (productFromRepo.Quantity > 0)
                {
                    string shopifyProductId;
                    if (string.IsNullOrEmpty(productFromRepo.ShopifyId))
                    {
                        ShopifyProduct product2 = shopify.CreateProduct(product1);
                        shopifyProductId = product2.id.ToString();
                        productFromRepo.Handle = product2.handle;
                        productFromRepo.ShopifyId = product2.id.ToString();
                        productFromRepo.InventoryItemId = product2.variants[0].inventory_item_id;
                        productFromRepo.Uploading = false;
                        this._cashGenRepository.UpdateProduct(productFromRepo);
                        this._cashGenRepository.Save();
                    }
                    else
                    {
                        ShopifyProduct shopifyProduct2 = shopify.UpdateProduct(productFromRepo.ShopifyId, product1);
                        shopifyProductId = shopifyProduct2.id.ToString();
                        productFromRepo.Handle = shopifyProduct2.handle;
                        productFromRepo.InventoryItemId = shopifyProduct2.variants[0].inventory_item_id;
                        productFromRepo.Uploading = false;
                        this._cashGenRepository.UpdateProduct(productFromRepo);
                        this._cashGenRepository.Save();
                    }
                    foreach (Collection productCollection in this._cashGenRepository.GetProductCollections(product.Id))
                    {
                        ShopifyCollectResponseWrapper collectResponseWrapper = new ShopifyCollectResponseWrapper();
                        ShopifyCollectResponseWrapper collection = shopify.AddProductToCollection(shopifyProductId, productCollection.ShopifyId);
                        ShopifyCollect shopifyCollect = new ShopifyCollect();
                        shopifyCollect.ShopifyId = collection.collect.id;
                        shopifyCollect.CollectionId = collection.collect.collection_id;
                        shopifyCollect.ProductId = collection.collect.product_id;
                        this._context.ShopifyCollects.Add(shopifyCollect);
                        this._cashGenRepository.Save();
                    }
                }
                else
                {
                    if (productFromRepo.ShopifyId != "" && productFromRepo.ShopifyId != null)
                    {
                        shopify.RemoveProduct(productFromRepo.ShopifyId);
                        productFromRepo.ShopifyId = "";
                    }
                    productFromRepo.OnSale = false;
                    productFromRepo.Uploading = false;
                    this._cashGenRepository.UpdateProduct(productFromRepo);
                    this._cashGenRepository.Save();
                }
            }
            else
            {
                if (!(productFromRepo.ShopifyId != "") || productFromRepo.ShopifyId == null)
                    return;
                shopify.RemoveProduct(productFromRepo.ShopifyId);
                productFromRepo.ShopifyId = "";
                productFromRepo.Uploading = false;
                this._cashGenRepository.UpdateProduct(productFromRepo);
                this._cashGenRepository.Save();
            }

        }
    }

}
 