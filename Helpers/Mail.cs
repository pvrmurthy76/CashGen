using PostmarkDotNet;
using System.Configuration;
using System.IO;

namespace CashGen.Helpers
{
    internal class Mail
    {
        public void SendMail(CashGen.Models.Mail msg)
        {
            foreach (string str in msg.to)
            {
                TemplatedPostmarkMessage templatedPostmarkMessage1 = new TemplatedPostmarkMessage();
                ((PostmarkMessageBase)templatedPostmarkMessage1).To = str;
                ((PostmarkMessageBase)templatedPostmarkMessage1).From = "Cash Generator<customerservices@cashgenerator.co.uk>";
                ((PostmarkMessageBase)templatedPostmarkMessage1).TrackOpens = new bool?(true);
                templatedPostmarkMessage1.TemplateId = new long?((long)msg.templateId);
                templatedPostmarkMessage1.TemplateModel = (object)msg;
                TemplatedPostmarkMessage templatedPostmarkMessage2 = templatedPostmarkMessage1;
                if (!string.IsNullOrEmpty(msg.attachment) && msg.file.Length != 0)
                    ((PostmarkMessageBase)templatedPostmarkMessage2).AddAttachment(msg.file, msg.attachment, "application/octet-stream", (string)null);
                else if (!string.IsNullOrEmpty(msg.attachment))
                {
                    byte[] numArray = File.ReadAllBytes(ConfigurationManager.AppSettings["dataPath"] + "\\" + msg.attachment);
                    ((PostmarkMessageBase)templatedPostmarkMessage2).AddAttachment(numArray, msg.attachment, "application/octet-stream", (string)null);
                }
                if (!string.IsNullOrEmpty(msg.attachment2) && msg.file2.Length != 0)
                    ((PostmarkMessageBase)templatedPostmarkMessage2).AddAttachment(msg.file2, msg.attachment2, "application/octet-stream", (string)null);
                new PostmarkClient("63723f7c-1742-445f-95b1-41c0e70c06b3", "https://api.postmarkapp.com").SendEmailWithTemplateAsync(templatedPostmarkMessage2);
            }
        }
    }
}
