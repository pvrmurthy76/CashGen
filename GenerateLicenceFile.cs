using CashGen.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CashGen
{
    public static class GenerateLicenceFile
    {
        [FunctionName("GenerateLicenceFile")]
        public static void Run([QueueTrigger("orders", Connection = "AzureWebJobsStorage")] Product product, [Blob("products/{rand-guid}.txt")] TextWriter outputBlob, ILogger log)
        {
            outputBlob.WriteLine("Barcode: " + product.Barcode);
            outputBlob.WriteLine("Title: " + product.Title);
            outputBlob.WriteLine("Email: " + product.Email);
            outputBlob.WriteLine(string.Format("Price: {0}", (object)product.Price));
            outputBlob.WriteLine(string.Format("Submitted: {0}", (object)DateTime.UtcNow));
            byte[] hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(product.Email + "secret"));
            outputBlob.WriteLine("SecretCode: " + BitConverter.ToString(hash).Replace("-", ""));
        }
    }
}
