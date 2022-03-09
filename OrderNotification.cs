using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using System;

namespace CashGen
{
    public static class OrderNotification
    {
        [FunctionName("OrderNotification")]
        public static void Run([QueueTrigger("orders", Connection = "AzureWebJobsStorage")] string myQueueItem, [Microsoft.Azure.WebJobs.SendGrid(ApiKey = "SendGridAPIKey")] out SendGridMessage message, ILogger log)
        {
            message = new SendGridMessage();
            message.From = new EmailAddress(Environment.GetEnvironmentVariable("EmailSender"), (string)null);
            message.AddTo("craig@craigmankelow.com", (string)null);
            message.TemplateId = "d-a523d63fe77f4d2d9e115798c3750799";
        }
    }
}
