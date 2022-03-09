﻿using System;


namespace CashGen.Models
{
    internal class ChatListDto
    {
        public Guid Id { get; set; }

        public DateTime MessageDate { get; set; }

        public Guid ParentId { get; set; }

        public Guid StoreId { get; set; }

        public string FromName { get; set; }

        public string FromEmail { get; set; }

        public string FromTelephone { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public string ChatType { get; set; }

        public bool isClosed { get; set; }

        public string StoreName { get; set; }
    }
}
