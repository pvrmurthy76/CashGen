using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace CashGen
{
    public static class EmailLicenceFile
    {
        [FunctionName("EmailLicenceFile")]
        public static void Run([BlobTrigger("orders/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob, string name, ILogger log) => LoggerExtensions.LogInformation(log, string.Format("C# Blob trigger function Processed blob\n Name:{0} \n Size: {1} Bytes", (object)name, (object)myBlob.Length), Array.Empty<object>());
    }
}
