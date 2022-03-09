using CashGen.Models;
using Newtonsoft.Json;
using System;

namespace CashGen.API
{
    internal class Shopify
    {
        public ShopifyProduct CreateProduct(ShopifyProductWrapperRequest product)
        {
            string str1 = "https://cash-generator-uk.myshopify.com/admin/api/2020-07/products.json";
            CashGen.Shared.API api = new CashGen.Shared.API();
            string str2 = JsonConvert.SerializeObject((object)product);
            Console.WriteLine("####");
            Console.WriteLine(str1);
            Console.WriteLine(str2);
            Console.WriteLine("####");
            string url = str1;
            string json = str2;
            string str3 = api.APIRequest(url, "POST", json);
            Console.WriteLine("Shopify Response: ", (object)str3);
            return JsonConvert.DeserializeObject<ShopifyProductWrapper>(str3).product;
        }

        public ShopifyProduct UpdateProduct(
          string shopifyId,
          ShopifyProductWrapperRequest product)
        {
            string str1 = "https://cash-generator-uk.myshopify.com/admin/api/2020-07/products/" + shopifyId + ".json";
            CashGen.Shared.API api = new CashGen.Shared.API();
            string str2 = JsonConvert.SerializeObject((object)product);
            Console.WriteLine("####");
            Console.WriteLine(str1);
            Console.WriteLine(str2);
            Console.WriteLine("####");
            string url = str1;
            string json = str2;
            return JsonConvert.DeserializeObject<ShopifyProductWrapper>(api.APIRequest(url, "PUT", json)).product;
        }

        public ShopifyCollectResponseWrapper AddProductToCollection(
          string shopifyProductId,
          string shopifyCollectionId)
        {
            CashGen.Shared.API api = new CashGen.Shared.API();
            string str1 = "https://cash-generator-uk.myshopify.com/admin/api/2020-07/collects.json";
            string str2 = JsonConvert.SerializeObject((object)new ShopifyCollectWrapperRequest()
            {
                collect = new ShopifyCollectRequest()
                {
                    product_id = Convert.ToInt64(shopifyProductId),
                    collection_id = Convert.ToInt64(shopifyCollectionId)
                }
            });
            Console.WriteLine(str1);
            Console.WriteLine(str2);
            string url = str1;
            string json = str2;
            return JsonConvert.DeserializeObject<ShopifyCollectResponseWrapper>(api.APIRequest(url, "POST", json));
        }

        public void RemoveProduct(string productId) => new CashGen.Shared.API().APIRequest("https://cash-generator-uk.myshopify.com/admin/api/2020-07/products/" + productId + ".json", "DELETE");

        public void TagOrder(long orderId, string tags)
        {
            string json = JsonConvert.SerializeObject((object)new ShopifyOrderTags()
            {
                order = new ShopifyOrderTag()
                {
                    id = orderId,
                    tags = tags
                }
            });
            new CashGen.Shared.API().APIRequest("https://cash-generator-uk.myshopify.com/admin/api/2020-07/orders/" + orderId.ToString() + ".json", "PUT", json);
        }

        public void RemoveProductFromCollection(long collectId) => new CashGen.Shared.API().APIRequest("https://cash-generator-uk.myshopify.com/admin/api/2020-07/collects/" + collectId.ToString() + ".json", "DELETE");

        public void UpdateOrder(long orderId, ShopifyOrderRequestWrapper order)
        {
            string str1 = "https://cash-generator-uk.myshopify.com/admin/api/2020-07/orders/" + orderId.ToString() + "/fulfillments.json";
            CashGen.Shared.API api = new CashGen.Shared.API();
            string str2 = JsonConvert.SerializeObject((object)order);
            string url = str1;
            string json = str2;
            api.APIRequest(url, "POST", json);
        }

        public string GetFraudRisk(long orderId)
        {
            ShopifyOrderRisks shopifyOrderRisks = JsonConvert.DeserializeObject<ShopifyOrderRisks>(new CashGen.Shared.API().APIRequest("https://cash-generator-uk.myshopify.com/admin/api/2020-07/orders/" + orderId.ToString() + "/risks.json", json: ((string)null)));
            string str = "";
            if (shopifyOrderRisks.risks.Count <= 0)
                return str.ToLower().Trim();
            foreach (ShopifyOrderRisk risk in shopifyOrderRisks.risks)
            {
                if (risk.recommendation.ToLower() == "cancel")
                    str = risk.recommendation;
                else if (risk.recommendation.ToLower() == "investigate" && str.ToLower() != "cancel")
                    str = risk.recommendation;
                else if (string.IsNullOrEmpty(str))
                    str = risk.recommendation;
            }
            return str.ToLower().Trim();
        }
    }
}
