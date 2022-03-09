using Cloudmersive.APIClient.NETCore.DocumentAndDataConvert.Api;
using Cloudmersive.APIClient.NETCore.DocumentAndDataConvert.Client;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace CashGen.Helpers
{
    internal class ConvertImage
    {
        public string ConvertWebpToJpg(string url)
        {
            Configuration.Default.AddApiKey("Apikey", "df3af9e6-df18-4994-a8b5-701b342ddd13");
            string str = Guid.NewGuid().ToString().ToLower() + ".jpg";
            using (Stream responseStream = WebRequest.Create(url).GetResponse().GetResponseStream())
            {
                ConvertImageApi convertImageApi = new ConvertImageApi((Configuration)null);
                try
                {
                    byte[] numArray = convertImageApi.ConvertImageImageFormatConvert("WEBP", "JPG", responseStream);
                    CloudBlockBlob blockBlobReference = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("StorageConnectionString")).CreateCloudBlobClient().GetContainerReference("product-media").GetBlockBlobReference(str);
                    ((CloudBlob)blockBlobReference).Properties.ContentType = "image/jpg";
                    blockBlobReference.UploadFromByteArrayAsync(numArray, 0, numArray.Length);
                    Console.WriteLine("Absolute Uri: " + ((CloudBlob)blockBlobReference).Uri.AbsoluteUri);
                    Thread.Sleep(5000);
                    return ((CloudBlob)blockBlobReference).Uri.AbsoluteUri;
                }
                catch (Exception ex)
                {
                    CashGen.Models.Mail mail1 = new CashGen.Models.Mail();
                    Mail mail2 = new Mail();
                    List<string> stringList = new List<string>();
                    stringList.Add("craig@craigmankelow.com");
                    mail1.templateId = 20324451;
                    mail1.to = stringList;
                    mail1.subject = "CGUK: Image Conversion Error";
                    mail1.name = "Craig";
                    mail1.body = "Exception when calling ConvertImageApi.ConvertImageImageFormatConvert: " + ex.Message;
                    CashGen.Models.Mail msg = mail1;
                    mail2.SendMail(msg);
                    return string.Empty;
                }
            }
        }
    }
}
