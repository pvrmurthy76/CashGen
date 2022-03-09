using System;
using System.IO;
using System.Net;

namespace CashGen.Shared
{
    public class API
    {
        // Methods
        public string APIRequest(string url, string method = "GET", string json = "")
        {
            string str = string.Empty;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Credentials = (ICredentials)new NetworkCredential("bd0727ddf54ffcf5f087dc4ba66a3879", "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");
                httpWebRequest.Method = method;
                if (method == "POST" || method == "PUT")
                {
                    using (StreamWriter streamWriter = new StreamWriter(((WebRequest)httpWebRequest).GetRequestStream()))
                    {
                        ((TextWriter)streamWriter).Write(json);
                        ((TextWriter)streamWriter).Flush();
                        ((TextWriter)streamWriter).Close();
                    }
                }
                using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
                    str = ((TextReader)streamReader).ReadToEnd();
                Console.WriteLine(url);
                Console.WriteLine(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## API ERROR ##");
                Console.WriteLine(url);
                Console.WriteLine(ex.Message);
            }
            return str;
        }

    }
}
