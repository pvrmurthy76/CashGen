using System.Collections.Generic;

namespace CashGen.Models
{
    internal class Mail
    {
        public List<string> to { get; set; }

        public string subject { get; set; }

        public string name { get; set; }

        public string body { get; set; }

        public string email { get; set; }

        public string telephone { get; set; }

        public string message { get; set; }

        public string password { get; set; }

        public int templateId { get; set; }

        public int orderNumber { get; set; }

        public string totalCost { get; set; }

        public string actionUrl { get; set; }

        public List<MailOrderLine> orderLines { get; set; }

        public string resetToken { get; set; }

        public int collectionCode { get; set; }

        public bool collection { get; set; }

        public bool delivery { get; set; }

        public string periodFrom { get; set; }

        public string periodTo { get; set; }

        public string attachment { get; set; }

        public byte[] file { get; set; }

        public string attachment2 { get; set; }

        public byte[] file2 { get; set; }
    }
}
