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
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            OnProductCreated.\u003C\u003Ec__DisplayClass4_0 cDisplayClass40 = new OnProductCreated.\u003C\u003Ec__DisplayClass4_0();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass40.product = product;
            Shopify shopify = new Shopify();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cDisplayClass40.productFromRepo = this._cashGenRepository.GetProduct(cDisplayClass40.product.Id);
            // ISSUE: reference to a compiler-generated field
            if (cDisplayClass40.productFromRepo == null)
                return;
            // ISSUE: reference to a compiler-generated field
            if (cDisplayClass40.productFromRepo.OnSale)
            {
                ShopifyProductWrapperRequest product1 = new ShopifyProductWrapperRequest();
                ShopifyProductRequest shopifyProductRequest1 = new ShopifyProductRequest();
                ShopifyProductVariantRequest productVariantRequest = new ShopifyProductVariantRequest();
                List<ShopifyProductVariantRequest> productVariantRequestList = new List<ShopifyProductVariantRequest>();
                // ISSUE: reference to a compiler-generated field
                shopifyProductRequest1.title = cDisplayClass40.productFromRepo.Title;
                // ISSUE: reference to a compiler-generated field
                shopifyProductRequest1.sku = cDisplayClass40.productFromRepo.Barcode;
                shopifyProductRequest1.body_html = "";
                // ISSUE: reference to a compiler-generated field
                IEnumerable<Feature> features = this._cashGenRepository.GetFeatures(cDisplayClass40.product.Id);
                // ISSUE: reference to a compiler-generated field
                if (!string.IsNullOrEmpty(cDisplayClass40.product.ShopifyId))
                {
                    ParameterExpression parameterExpression1;
                    // ISSUE: method reference
                    // ISSUE: field reference
                    // ISSUE: method reference
                    if (((IQueryable<ShopifyCollect>)this._context.ShopifyCollects).Where<ShopifyCollect>(Expression.Lambda<Func<ShopifyCollect, bool>>((Expression)Expression.Equal((Expression)Expression.Call(c.ProductId, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(long.ToString)), Array.Empty<Expression>()), (Expression)Expression.Property((Expression)Expression.Field((Expression)Expression.Constant((object)cDisplayClass40, typeof(OnProductCreated.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref(OnProductCreated.\u003C\u003Ec__DisplayClass4_0.product))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Product.get_ShopifyId)))), parameterExpression1)).Count<ShopifyCollect>() > 0)
          {
                        DbSet<ShopifyCollect> shopifyCollects = this._context.ShopifyCollects;
                        ParameterExpression parameterExpression2;
                        // ISSUE: method reference
                        // ISSUE: field reference
                        // ISSUE: method reference
                        Expression<Func<ShopifyCollect, bool>> predicate = Expression.Lambda<Func<ShopifyCollect, bool>>((Expression)Expression.Equal((Expression)Expression.Call(c.ProductId, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(long.ToString)), Array.Empty<Expression>()), (Expression)Expression.Property((Expression)Expression.Field((Expression)Expression.Constant((object)cDisplayClass40, typeof(OnProductCreated.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref(OnProductCreated.\u003C\u003Ec__DisplayClass4_0.product))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Product.get_ShopifyId)))), parameterExpression2);
                        foreach (ShopifyCollect shopifyCollect in ((IQueryable<ShopifyCollect>)shopifyCollects).Where<ShopifyCollect>(predicate).ToList<ShopifyCollect>())
                            shopify.RemoveProductFromCollection(shopifyCollect.ShopifyId);
                    }
                }
                Store store1 = new Store();
                // ISSUE: reference to a compiler-generated field
                Store store2 = ((IQueryable<Store>)this._context.Stores).Where<Store>((Expression<Func<Store, bool>>)(c => c.Id == cDisplayClass40.productFromRepo.StoreId)).FirstOrDefault<Store>();
                shopifyProductRequest1.body_html += "<h2 class='listing-title'>Item Location</h2>";
                // ISSUE: reference to a compiler-generated field
                if (cDisplayClass40.productFromRepo.FulfilmentOption == "collection")
                {
                    shopifyProductRequest1.body_html += "<p class='listing-location'>This item is available for <strong>Collection Only</strong> from:<br />";
                }
                else
                {
                    // ISSUE: reference to a compiler-generated field
                    if (cDisplayClass40.productFromRepo.FulfilmentOption == "delivery")
                        shopifyProductRequest1.body_html += "<p class='listing-location'>This item is available for <strong>Delivery Only</strong> from:<br />";
                    else
                        shopifyProductRequest1.body_html += "<p class='listing-location'>This item is available for <strong>Collection or Delivery</strong> from:<br />";
                }
                ShopifyProductRequest shopifyProductRequest2 = shopifyProductRequest1;
                shopifyProductRequest2.body_html = shopifyProductRequest2.body_html + "<strong>Cash Generator " + store2.Title + "</strong>, " + store2.Line1 + ", ";
                if (!string.IsNullOrEmpty(store2.Line2))
                {
                    ShopifyProductRequest shopifyProductRequest3 = shopifyProductRequest1;
                    shopifyProductRequest3.body_html = shopifyProductRequest3.body_html + store2.Line2 + ", ";
                }
                ShopifyProductRequest shopifyProductRequest4 = shopifyProductRequest1;
                shopifyProductRequest4.body_html = shopifyProductRequest4.body_html + store2.Town + ", " + store2.PostCode + "<br />";
                ShopifyProductRequest shopifyProductRequest5 = shopifyProductRequest1;
                shopifyProductRequest5.body_html = shopifyProductRequest5.body_html + "Email: <a href='mailto:" + store2.Email + "'>" + store2.Email + "</a><br />";
                ShopifyProductRequest shopifyProductRequest6 = shopifyProductRequest1;
                shopifyProductRequest6.body_html = shopifyProductRequest6.body_html + "Telephone: <a href='tel:" + store2.Telephone + "'>" + store2.Telephone + "</a></p>";
                shopifyProductRequest1.body_html += "<h2 class='listing-title'>Item Condition</h2>";
                // ISSUE: reference to a compiler-generated field
                if (cDisplayClass40.productFromRepo.Condition == "new")
                {
                    // ISSUE: reference to a compiler-generated field
                    if (cDisplayClass40.productFromRepo.ConditionText != "")
                    {
                        ShopifyProductRequest shopifyProductRequest7 = shopifyProductRequest1;
                        // ISSUE: reference to a compiler-generated field
                        shopifyProductRequest7.body_html = shopifyProductRequest7.body_html + "<p class='condition-box'><strong>Condition: New</strong><br />" + cDisplayClass40.productFromRepo.ConditionText + "</p>";
                    }
                    else
                        shopifyProductRequest1.body_html += "<p class='condition-box'><strong>Condition: New</strong></p>";
                }
                // ISSUE: reference to a compiler-generated field
                if (cDisplayClass40.productFromRepo.Condition == "other")
                {
                    // ISSUE: reference to a compiler-generated field
                    if (cDisplayClass40.productFromRepo.ConditionText != "")
                    {
                        ShopifyProductRequest shopifyProductRequest8 = shopifyProductRequest1;
                        // ISSUE: reference to a compiler-generated field
                        shopifyProductRequest8.body_html = shopifyProductRequest8.body_html + "<p class='condition-box'><strong>Condition: New Other</strong><br />" + cDisplayClass40.productFromRepo.ConditionText + "</p>";
                    }
                    else
                        shopifyProductRequest1.body_html += "<p class='condition-box'><strong>Condition: New Other</strong></p>";
                }
                else
                {
                    // ISSUE: reference to a compiler-generated field
                    if (cDisplayClass40.productFromRepo.Condition == "refurbished")
                    {
                        // ISSUE: reference to a compiler-generated field
                        if (cDisplayClass40.productFromRepo.ConditionText != "")
                        {
                            ShopifyProductRequest shopifyProductRequest9 = shopifyProductRequest1;
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            shopifyProductRequest9.body_html = shopifyProductRequest9.body_html + "<p class='condition-box'><strong>Condition: Refurbished: Grade " + cDisplayClass40.productFromRepo.Grade + "</strong><br />" + cDisplayClass40.productFromRepo.ConditionText + "</p>";
                        }
                        else
                        {
                            ShopifyProductRequest shopifyProductRequest10 = shopifyProductRequest1;
                            // ISSUE: reference to a compiler-generated field
                            shopifyProductRequest10.body_html = shopifyProductRequest10.body_html + "<p class='condition-box'><strong>Condition: Refurbished: Grade " + cDisplayClass40.productFromRepo.Grade + "</strong></p>";
                        }
                    }
                    else
                    {
                        // ISSUE: reference to a compiler-generated field
                        if (cDisplayClass40.productFromRepo.Condition == "used")
                        {
                            // ISSUE: reference to a compiler-generated field
                            if (cDisplayClass40.productFromRepo.ConditionText != "")
                            {
                                ShopifyProductRequest shopifyProductRequest11 = shopifyProductRequest1;
                                // ISSUE: reference to a compiler-generated field
                                // ISSUE: reference to a compiler-generated field
                                shopifyProductRequest11.body_html = shopifyProductRequest11.body_html + "<p class='condition-box'><strong>Condition: Pre-Owned: Grade " + cDisplayClass40.productFromRepo.Grade + "</strong><br />" + cDisplayClass40.productFromRepo.ConditionText + "</p>";
                            }
                            else
                            {
                                ShopifyProductRequest shopifyProductRequest12 = shopifyProductRequest1;
                                // ISSUE: reference to a compiler-generated field
                                shopifyProductRequest12.body_html = shopifyProductRequest12.body_html + "<p class='condition-box'><strong>Condition: Pre-Owned: Grade " + cDisplayClass40.productFromRepo.Grade + "</strong></p>";
                            }
                        }
                    }
                }
                shopifyProductRequest1.body_html += "<h2 class='listing-title'>Item Description</h2>";
                ShopifyProductRequest shopifyProductRequest13 = shopifyProductRequest1;
                // ISSUE: reference to a compiler-generated field
                shopifyProductRequest13.body_html = shopifyProductRequest13.body_html + "<p>" + cDisplayClass40.productFromRepo.Intro + "</p>";
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
                // ISSUE: reference to a compiler-generated field
                Store store3 = this._cashGenRepository.GetStore(cDisplayClass40.product.StoreId);
                shopifyProductRequest1.vendor = "Cash Generator " + store3.Title.Trim();
                productVariantRequest.taxable = false;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                productVariantRequest.compare_at_price = !(cDisplayClass40.productFromRepo.WasPrice > 0M) ? new Decimal?() : new Decimal?(Convert.ToDecimal(cDisplayClass40.productFromRepo.WasPrice));
                // ISSUE: reference to a compiler-generated field
                productVariantRequest.sku = cDisplayClass40.productFromRepo.Barcode;
                // ISSUE: reference to a compiler-generated field
                productVariantRequest.barcode = cDisplayClass40.productFromRepo.Barcode;
                // ISSUE: reference to a compiler-generated field
                productVariantRequest.inventory_quantity = cDisplayClass40.productFromRepo.Quantity;
                productVariantRequest.inventory_management = "shopify";
                productVariantRequest.inventory_policy = "deny";
                // ISSUE: reference to a compiler-generated field
                productVariantRequest.price = Convert.ToDecimal(cDisplayClass40.productFromRepo.Price);
                productVariantRequestList.Add(productVariantRequest);
                shopifyProductRequest1.variants = productVariantRequestList;
                string empty1 = string.Empty;
                // ISSUE: reference to a compiler-generated field
                foreach (ProductFilter productFilter in this._cashGenRepository.GetProductFilters(cDisplayClass40.product.Id))
                {
                    if (!string.IsNullOrEmpty(empty1))
                        empty1 += ", ";
                    empty1 += productFilter.Value;
                }
                if (!string.IsNullOrEmpty(empty1))
                    empty1 += ", ";
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                string str1 = !(Convert.ToDecimal(cDisplayClass40.productFromRepo.Price) > 5000M) ? (!(Convert.ToDecimal(cDisplayClass40.productFromRepo.Price) > 2000M) ? (!(Convert.ToDecimal(cDisplayClass40.productFromRepo.Price) > 1000M) ? (!(Convert.ToDecimal(cDisplayClass40.productFromRepo.Price) > 500M) ? (!(Convert.ToDecimal(cDisplayClass40.productFromRepo.Price) > 250M) ? (!(Convert.ToDecimal(cDisplayClass40.productFromRepo.Price) > 200M) ? (!(Convert.ToDecimal(cDisplayClass40.productFromRepo.Price) > 150M) ? (!(Convert.ToDecimal(cDisplayClass40.productFromRepo.Price) > 100M) ? (!(Convert.ToDecimal(cDisplayClass40.productFromRepo.Price) > 50M) ? (!(Convert.ToDecimal(cDisplayClass40.productFromRepo.Price) > 25M) ? (!(Convert.ToDecimal(cDisplayClass40.productFromRepo.Price) > 10M) ? empty1 + "£0>£10" : empty1 + "£10>£25") : empty1 + "£25>£50") : empty1 + "£50>£100") : empty1 + "£100>£150") : empty1 + "£150>£200") : empty1 + "£200>£250") : empty1 + "£250>£500") : empty1 + "£500>£1000") : empty1 + "£1000>£2000") : empty1 + "£2000>£5000") : empty1 + ">£5000";
                // ISSUE: reference to a compiler-generated field
                if (cDisplayClass40.productFromRepo.Condition == "new")
                {
                    str1 += ", New";
                }
                else
                {
                    // ISSUE: reference to a compiler-generated field
                    if (cDisplayClass40.productFromRepo.Condition == "other")
                    {
                        str1 += ", New Other";
                    }
                    else
                    {
                        // ISSUE: reference to a compiler-generated field
                        if (cDisplayClass40.productFromRepo.Condition == "refurbished")
                        {
                            // ISSUE: reference to a compiler-generated field
                            str1 = str1 + ", Refurbished: Grade " + cDisplayClass40.productFromRepo.Grade;
                        }
                        else
                        {
                            // ISSUE: reference to a compiler-generated field
                            if (cDisplayClass40.productFromRepo.Condition == "used")
                            {
                                // ISSUE: reference to a compiler-generated field
                                str1 = str1 + ", Pre-Owned: Grade " + cDisplayClass40.productFromRepo.Grade;
                            }
                        }
                    }
                }
                // ISSUE: reference to a compiler-generated field
                Store store4 = this._cashGenRepository.GetStore(cDisplayClass40.productFromRepo.StoreId);
                string str2;
                if (!string.IsNullOrEmpty(store4.Line2))
                    str2 = "Cash Generator " + store4.Title + " - " + store4.Line1.Replace(",", "") + " - " + store4.Line2.Replace(",", "") + " - " + store4.Town + " - " + store4.PostCode;
                else
                    str2 = "Cash Generator " + store4.Title + " - " + store4.Line1.Replace(",", "") + " - " + store4.Town + " - " + store4.PostCode;
                // ISSUE: reference to a compiler-generated field
                if (!string.IsNullOrEmpty(cDisplayClass40.productFromRepo.FulfilmentOption))
                {
                    // ISSUE: reference to a compiler-generated field
                    string str3 = str1 + ", " + cDisplayClass40.productFromRepo.FulfilmentOption;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    str1 = !(cDisplayClass40.productFromRepo.FulfilmentOption == "collection") ? (!(cDisplayClass40.productFromRepo.FulfilmentOption == "delivery") ? str3 + ", Fulfilment:Delivery or Collection from " + str2 : str3 + ", Fulfilment:Delivery only") : str3 + ", Fulfilment:Collection only from " + str2;
                }
                shopifyProductRequest1.tags = str1;
                List<ShopifyProductImageRequest> productImageRequestList = new List<ShopifyProductImageRequest>();
                // ISSUE: reference to a compiler-generated field
                IEnumerable<Image> images = this._cashGenRepository.GetImages(cDisplayClass40.product.Id);
                if (images.Count<Image>() == 0)
                {
                    productImageRequestList.Add(new ShopifyProductImageRequest()
                    {
                        src = "https://cashgen.blob.core.windows.net/product-media/placeholder.png"
                    });
                }
                else
                {
                    int num = 0;
                    foreach (Image image in images)
                    {
                        if (!string.IsNullOrEmpty(image.Src))
                        {
                            productImageRequestList.Add(new ShopifyProductImageRequest()
                            {
                                src = new ConvertImage().ConvertWebpToJpg(image.Src)
                            });
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
                // ISSUE: reference to a compiler-generated field
                if (cDisplayClass40.productFromRepo.Quantity > 0)
                {
                    string shopifyProductId;
                    // ISSUE: reference to a compiler-generated field
                    if (string.IsNullOrEmpty(cDisplayClass40.productFromRepo.ShopifyId))
                    {
                        ShopifyProduct product2 = shopify.CreateProduct(product1);
                        shopifyProductId = product2.id.ToString();
                        // ISSUE: reference to a compiler-generated field
                        cDisplayClass40.productFromRepo.Handle = product2.handle;
                        // ISSUE: reference to a compiler-generated field
                        cDisplayClass40.productFromRepo.ShopifyId = product2.id.ToString();
                        // ISSUE: reference to a compiler-generated field
                        cDisplayClass40.productFromRepo.InventoryItemId = product2.variants[0].inventory_item_id;
                        // ISSUE: reference to a compiler-generated field
                        cDisplayClass40.productFromRepo.Uploading = false;
                        // ISSUE: reference to a compiler-generated field
                        this._cashGenRepository.UpdateProduct(cDisplayClass40.productFromRepo);
                        this._cashGenRepository.Save();
                    }
                    else
                    {
                        // ISSUE: reference to a compiler-generated field
                        ShopifyProduct shopifyProduct2 = shopify.UpdateProduct(cDisplayClass40.productFromRepo.ShopifyId, product1);
                        shopifyProductId = shopifyProduct2.id.ToString();
                        // ISSUE: reference to a compiler-generated field
                        cDisplayClass40.productFromRepo.Handle = shopifyProduct2.handle;
                        // ISSUE: reference to a compiler-generated field
                        cDisplayClass40.productFromRepo.InventoryItemId = shopifyProduct2.variants[0].inventory_item_id;
                        // ISSUE: reference to a compiler-generated field
                        cDisplayClass40.productFromRepo.Uploading = false;
                        // ISSUE: reference to a compiler-generated field
                        this._cashGenRepository.UpdateProduct(cDisplayClass40.productFromRepo);
                        this._cashGenRepository.Save();
                    }
                    // ISSUE: reference to a compiler-generated field
                    foreach (Collection productCollection in this._cashGenRepository.GetProductCollections(cDisplayClass40.product.Id))
                    {
                        ShopifyCollectResponseWrapper collectResponseWrapper = new ShopifyCollectResponseWrapper();
                        ShopifyCollectResponseWrapper collection = shopify.AddProductToCollection(shopifyProductId, productCollection.ShopifyId);
                        this._context.ShopifyCollects.Add(new ShopifyCollect()
                        {
                            ShopifyId = collection.collect.id,
                            CollectionId = collection.collect.collection_id,
                            ProductId = collection.collect.product_id
                        });
                        this._cashGenRepository.Save();
                    }
                }
                else
                {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (cDisplayClass40.productFromRepo.ShopifyId != "" && cDisplayClass40.productFromRepo.ShopifyId != null)
                    {
                        // ISSUE: reference to a compiler-generated field
                        shopify.RemoveProduct(cDisplayClass40.productFromRepo.ShopifyId);
                        // ISSUE: reference to a compiler-generated field
                        cDisplayClass40.productFromRepo.ShopifyId = "";
                    }
                    // ISSUE: reference to a compiler-generated field
                    cDisplayClass40.productFromRepo.OnSale = false;
                    // ISSUE: reference to a compiler-generated field
                    cDisplayClass40.productFromRepo.Uploading = false;
                    // ISSUE: reference to a compiler-generated field
                    this._cashGenRepository.UpdateProduct(cDisplayClass40.productFromRepo);
                    this._cashGenRepository.Save();
                }
            }
            else
            {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (!(cDisplayClass40.productFromRepo.ShopifyId != "") || cDisplayClass40.productFromRepo.ShopifyId == null)
                    return;
                // ISSUE: reference to a compiler-generated field
                shopify.RemoveProduct(cDisplayClass40.productFromRepo.ShopifyId);
                // ISSUE: reference to a compiler-generated field
                cDisplayClass40.productFromRepo.ShopifyId = "";
                // ISSUE: reference to a compiler-generated field
                cDisplayClass40.productFromRepo.Uploading = false;
                // ISSUE: reference to a compiler-generated field
                this._cashGenRepository.UpdateProduct(cDisplayClass40.productFromRepo);
                this._cashGenRepository.Save();
            }
        }
    }
}
