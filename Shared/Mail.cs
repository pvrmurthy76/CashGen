using CashGen.DBContexts;
using CashGen.Entities;
using CashGen.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CashGen.Shared
{
    internal class Mail
    {
        public void AddMailNote(Guid _id, CashGenContext _context, string note, Guid sender)
        {
            _context.Notes.Add(new Note()
            {
                LinkedId = _id,
                NoteTime = DateTime.Now,
                UserId = sender,
                NoteText = note
            });
            _context.SaveChanges();
        }

        public bool OrderMail(Guid _id, CashGenContext _context, string type, Guid sender)
        {
            Order order1 = new Order();
            Order order2 = ((IQueryable<Order>)_context.Orders).Where<Order>((Expression<Func<Order, bool>>)(x => x.Id == _id)).FirstOrDefault<Order>();
            CashGen.Models.Mail msg = new CashGen.Models.Mail();
            CashGen.Helpers.Mail mail = new CashGen.Helpers.Mail();
            List<string> stringList1 = new List<string>();
            List<string> stringList2 = new List<string>();
            stringList2.Add(order2.email);
            if (order2.FulfilmentMethod == "collection")
            {
                msg.collection = true;
                msg.delivery = false;
            }
            else
            {
                msg.collection = false;
                msg.delivery = true;
            }
            List<MailOrderLine> mailOrderLineList = new List<MailOrderLine>();
            List<LineItem> lineItemList = new List<LineItem>();
            DbSet<LineItem> lineItems = _context.LineItems;
            Expression<Func<LineItem, bool>> predicate = (Expression<Func<LineItem, bool>>)(x => x.OrderId == _id);
            foreach (LineItem lineItem in ((IQueryable<LineItem>)lineItems).Where<LineItem>(predicate).ToList<LineItem>())
            {
                LineItem item = lineItem;
                MailOrderLine mailOrderLine = new MailOrderLine();
                mailOrderLine.description = item.title;
                mailOrderLine.fulfilment = item.fulfilment;
                mailOrderLine.amount = item.line_price.ToString("F2");
                Product productFromRepo = ((IQueryable<Product>)_context.Products).Where<Product>((Expression<Func<Product, bool>>)(c => c.Id == item.ProductKey)).FirstOrDefault<Product>();
                if (productFromRepo != null)
                {
                    Store store = ((IQueryable<Store>)_context.Stores).Where<Store>((Expression<Func<Store, bool>>)(c => c.Id == productFromRepo.StoreId)).FirstOrDefault<Store>();
                    if (store != null)
                    {
                        if (!stringList1.Contains(store.Email))
                            stringList1.Add(store.Email);
                        if (order2.FulfilmentMethod == "collection")
                        {
                            string str1 = "Collection from Cash Generator " + store.Title + ", " + store.Line1 + ", ";
                            if (!string.IsNullOrEmpty(store.Line2))
                                str1 = str1 + store.Line2 + ", ";
                            string str2 = str1 + store.Town + ", " + store.PostCode;
                            mailOrderLine.fulfilment = str2;
                            mailOrderLine.openHours = store.OpenHours;
                        }
                    }
                }
                mailOrderLineList.Add(mailOrderLine);
            }
            if (type == "collection")
            {
                msg.to = stringList2;
                msg.name = order2.CustomerFirstName;
                msg.orderNumber = order2.order_number;
                msg.totalCost = order2.total_price.ToString("F2");
                msg.orderLines = mailOrderLineList;
                msg.templateId = 21007684;
                msg.collectionCode = order2.CollectionCode;
                mail.SendMail(msg);
                this.AddMailNote(_id, _context, "Order ready for collection notification sent to " + order2.email, sender);
            }
            else if (type == "cancelled")
            {
                msg.to = stringList2;
                msg.name = order2.CustomerFirstName;
                msg.orderNumber = order2.order_number;
                msg.totalCost = order2.total_price.ToString("F2");
                msg.orderLines = mailOrderLineList;
                msg.templateId = 22145396;
                mail.SendMail(msg);
                this.AddMailNote(_id, _context, "Order cancelled notification sent to " + order2.email, sender);
            }
            else if (type == "collected")
            {
                msg.to = stringList2;
                msg.name = order2.CustomerFirstName;
                msg.orderNumber = order2.order_number;
                msg.totalCost = order2.total_price.ToString("F2");
                msg.orderLines = mailOrderLineList;
                msg.templateId = 21073497;
                mail.SendMail(msg);
                this.AddMailNote(_id, _context, "Order collected notification sent to " + order2.email, sender);
            }
            else if (type == "confirmation")
            {
                msg.to = stringList2;
                msg.name = order2.CustomerFirstName;
                msg.orderNumber = order2.order_number;
                msg.totalCost = order2.total_price.ToString("F2");
                msg.orderLines = mailOrderLineList;
                msg.templateId = 22142734;
                mail.SendMail(msg);
                this.AddMailNote(_id, _context, "Order confirmation sent to " + order2.email, sender);
                mail.SendMail(new CashGen.Models.Mail()
                {
                    to = stringList1,
                    orderNumber = order2.order_number,
                    totalCost = order2.total_price.ToString("F2"),
                    actionUrl = "https://webepos.cashgenerator.co.uk/orders/" + order2.Id.ToString(),
                    orderLines = mailOrderLineList,
                    templateId = 20324450
                });
                foreach (string str in stringList1)
                    this.AddMailNote(_id, _context, "New order notification sent to " + str, sender);
            }
            return true;
        }

    }
}
