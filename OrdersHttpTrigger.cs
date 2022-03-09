using AutoMapper;
using CashGen.API;
using CashGen.DBContexts;
using CashGen.Entities;
using CashGen.Models;
using CashGen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace CashGen
{
    public class OrdersHttpTrigger
    {
        private readonly ICashGenRepository _cashGenRepository;
        private readonly CashGenContext _context;
        private readonly IMapper _mapper;

        public OrdersHttpTrigger(
          ICashGenRepository cashGenRepository,
          CashGenContext context,
          IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
            this._cashGenRepository = cashGenRepository;
        }

        [FunctionName("GetOrders")]
        public IActionResult GetOrders([HttpTrigger] HttpRequest req, ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0 cDisplayClass40 = new OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0();
            int num = 1;
            int count1 = 30;
            if (!string.IsNullOrEmpty(StringValues.op_Implicit(req.Query["page"])))
                num = Convert.ToInt32(StringValues.op_Implicit(req.Query["page"]));
            if (!string.IsNullOrEmpty(StringValues.op_Implicit(req.Query["size"])))
                count1 = Convert.ToInt32(StringValues.op_Implicit(req.Query["size"]));
            int count2 = (num - 1) * count1;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass40.store = Guid.NewGuid();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass40.admin = false;
            if (!string.IsNullOrEmpty(StringValues.op_Implicit(req.Query["admin-view"])))
            {
                // ISSUE: reference to a compiler-generated field
                cDisplayClass40.admin = true;
            }
            else
            {
                // ISSUE: reference to a compiler-generated field
                cDisplayClass40.store = new Guid(StringValues.op_Implicit(req.Query["store"]));
            }
            // ISSUE: reference to a compiler-generated field
            cDisplayClass40._risk = false;
            if (!string.IsNullOrEmpty(StringValues.op_Implicit(req.Query["risk"])))
            {
                // ISSUE: reference to a compiler-generated field
                cDisplayClass40._risk = true;
            }
            // ISSUE: reference to a compiler-generated field
            cDisplayClass40._keyword = "";
            if (!string.IsNullOrEmpty(StringValues.op_Implicit(req.Query["keyword"])))
            {
                // ISSUE: reference to a compiler-generated field
                cDisplayClass40._keyword = StringValues.op_Implicit(req.Query["keyword"]);
            }
            // ISSUE: reference to a compiler-generated field
            cDisplayClass40._status = "";
            if (!string.IsNullOrEmpty(StringValues.op_Implicit(req.Query["status"])))
            {
                // ISSUE: reference to a compiler-generated field
                cDisplayClass40._status = StringValues.op_Implicit(req.Query["status"]);
            }
            ParameterExpression parameterExpression;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: field reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: field reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            List<Order> list = ((IQueryable<Order>)this._context.Orders).Join((IEnumerable<LineItem>)this._context.LineItems, (Expression<Func<Order, Guid>>)(ord => ord.Id), (Expression<Func<LineItem, Guid>>)(ln => ln.OrderId), (ord, ln) => new
            {
                ord = ord,
                ln = ln
            }).Join((IEnumerable<Product>)this._context.Products, data => data.ln.ProductKey, (Expression<Func<Product, Guid>>)(pd => pd.Id), (data, pd) => new
            {
        \u003C\u003Eh__TransparentIdentifier0 = data,
                pd = pd
            }).Where(Expression.Lambda < Func <\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >, bool >> ((Expression)Expression.AndAlso((Expression)Expression.AndAlso((Expression)Expression.AndAlso((Expression)Expression.AndAlso((Expression)Expression.AndAlso((Expression)Expression.AndAlso(\u003C\u003Eh__TransparentIdentifier1.pd.StoreId == cDisplayClass40.store || cDisplayClass40.admin == true, (Expression)Expression.OrElse((Expression)Expression.OrElse((Expression)Expression.OrElse((Expression)Expression.OrElse(cDisplayClass40._keyword == "", (Expression)Expression.Call((Expression)Expression.Call(\u003C\u003Eh__TransparentIdentifier1.\u003C\u003Eh__TransparentIdentifier0.ord.email, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(string.ToLower)), Array.Empty<Expression>()), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(string.Contains)), new Expression[1]
            {
        (Expression) Expression.Call((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0._keyword))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>())
          })), (Expression) Expression.Call((Expression) Expression.Call((Expression) Expression.Property((Expression) Expression.Property((Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref(\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Order.get_CustomerFirstName))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>()), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.Contains)), new Expression[1]
      {
        (Expression) Expression.Call((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0._keyword))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>())
      })), (Expression) Expression.Call((Expression) Expression.Call((Expression) Expression.Property((Expression) Expression.Property((Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref(\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Order.get_CustomerLastName))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>()), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.Contains)), new Expression[1]
      {
        (Expression) Expression.Call((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0._keyword))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>())
      })), (Expression)Expression.Call((Expression)Expression.Call((Expression)Expression.Property((Expression)Expression.Property((Expression)Expression.Property((Expression)parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref(\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Order.get_order_number))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(int.ToString)), Array.Empty<Expression>()), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(string.Contains)), new Expression[1]
{
        (Expression) Expression.Call((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0._keyword))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>())
      }))), (Expression)Expression.OrElse((Expression)Expression.OrElse((Expression)Expression.Equal((Expression)Expression.Field((Expression)Expression.Constant((object)cDisplayClass40, typeof(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0._status))), (Expression)Expression.Constant((object)"", typeof(string))), (Expression)Expression.AndAlso((Expression)Expression.Equal((Expression)Expression.Field((Expression)Expression.Constant((object)cDisplayClass40, typeof(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0._status))), (Expression)Expression.Constant((object)"pending", typeof(string))), (Expression)Expression.OrElse((Expression)Expression.Equal((Expression)Expression.Property((Expression)Expression.Property((Expression)Expression.Property((Expression)parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref(\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Order.get_fulfillment_status))), (Expression)Expression.Constant((object)"", typeof(string))), (Expression)Expression.Equal((Expression)Expression.Property((Expression)Expression.Property((Expression)Expression.Property((Expression)parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref(\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Order.get_fulfillment_status))), (Expression)Expression.Constant((object)null, typeof(string)))))), (Expression)Expression.Equal((Expression)Expression.Property((Expression)Expression.Property((Expression)Expression.Property((Expression)parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref(\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Order.get_fulfillment_status))), (Expression)Expression.Field((Expression)Expression.Constant((object)cDisplayClass40, typeof(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0._status)))))), (Expression)Expression.OrElse((Expression)Expression.AndAlso((Expression)Expression.Equal((Expression)Expression.Field((Expression)Expression.Constant((object)cDisplayClass40, typeof(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0._risk))), (Expression)Expression.Constant((object)false, typeof(bool))), (Expression)Expression.NotEqual((Expression)Expression.Property((Expression)Expression.Property((Expression)Expression.Property((Expression)parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref(\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Order.get_FraudRisk))), (Expression)Expression.Constant((object)"cancel", typeof(string)))), (Expression)Expression.AndAlso((Expression)Expression.Equal((Expression)Expression.Field((Expression)Expression.Constant((object)cDisplayClass40, typeof(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0._risk))), (Expression)Expression.Constant((object)true, typeof(bool))), (Expression)Expression.Equal((Expression)Expression.Property((Expression)Expression.Property((Expression)Expression.Property((Expression)parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref(\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Order.get_FraudRisk))), (Expression)Expression.Constant((object)"cancel", typeof(string)))))), (Expression)Expression.Equal((Expression)Expression.Property((Expression)Expression.Property((Expression)Expression.Property((Expression)parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref(\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Order.get_IsDeleted))), (Expression)Expression.Constant((object)false, typeof(bool)))), (Expression)Expression.NotEqual((Expression)Expression.Property((Expression)Expression.Property((Expression)Expression.Property((Expression)parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>.get_\u003C\u003Eh__TransparentIdentifier0), __typeref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref (\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Order.get_FraudRisk))), (Expression) Expression.Constant((object) "declined", typeof (string)))), (Expression) Expression.NotEqual((Expression) Expression.Property((Expression) Expression.Property((Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>.get_\u003C\u003Eh__TransparentIdentifier0), __typeref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref (\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Order.get_FinancialStatus))), (Expression) Expression.Constant((object) "pending", typeof (string)))), parameterExpression)).Select(data => data.\u003C\u003Eh__TransparentIdentifier0.ord).Distinct<Order>().OrderByDescending<Order, int>((Expression<Func<Order, int>>) (o => o.order_number)).ToList<Order>();
      IEnumerable<OrderListDto> source = this._mapper.Map<IEnumerable<OrderListDto>>((object) list).ToList<OrderListDto>().Skip<OrderListDto>(count2).Take<OrderListDto>(count1);
      return (IActionResult) new OkObjectResult((object) new GetOrdersResponse()
      {
        count = list.Count<Order>(),
        results = source.ToList<OrderListDto>()
      });
    }

    [FunctionName("GetOrder")]
    public IActionResult GetOrder([HttpTrigger] HttpRequest req, Guid id, ILogger log)
    {
      if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>) (object) req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
        return (IActionResult) new BadRequestResult();
      Order order = this._cashGenRepository.GetOrder(id);
      IEnumerable<LineItem> lineItems = this._cashGenRepository.GetLineItems(id);
      order.line_items = (ICollection<LineItem>) lineItems;
      OrderDto orderDto1 = new OrderDto();
      OrderDto orderDto2 = this._mapper.Map<OrderDto>((object) order);
      if (((IQueryable<Transaction>) this._context.Transactions).Where<Transaction>((Expression<Func<Transaction, bool>>) (c => c.OrderId == id)).Count<Transaction>() == 0)
        orderDto2.PendingTransactions = 1;
      else
        orderDto2.PendingTransactions = ((IQueryable<Transaction>) this._context.Transactions).Where<Transaction>((Expression<Func<Transaction, bool>>) (c => c.OrderId == id && c.isPaid == false)).Count<Transaction>();
      return (IActionResult) new OkObjectResult((object) orderDto2);
    }

    [FunctionName("OrderCollection")]
    public async Task<IActionResult> OrderCollection(
      [HttpTrigger] HttpRequest req,
      Guid id,
      ILogger log)
    {
      userAuth userAuth = new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>) (object) req.Headers["auth_token"]).FirstOrDefault<string>());
      if (!userAuth.valid)
        return (IActionResult) new BadRequestResult();
      string str = "ready";
      if (!string.IsNullOrEmpty(StringValues.op_Implicit(req.Query["status"])))
        str = StringValues.op_Implicit(req.Query["status"]);
      CashGen.Shared.Mail mail = new CashGen.Shared.Mail();
      if (str == "collected")
        mail.OrderMail(id, this._context, "collected", userAuth.auth_token);
      else
        mail.OrderMail(id, this._context, "collection", userAuth.auth_token);
      return (IActionResult) new OkObjectResult((object) true);
    }

    [FunctionName("CreateOrder")]
    public async Task<IActionResult> CreateOrder([HttpTrigger] HttpRequest req, ILogger log)
    {
      bool isNewOrder = true;
      try
      {
        Shopify api = new Shopify();
        string endAsync = await ((TextReader) new StreamReader(req.Body)).ReadToEndAsync();
        ShopifyOrder order = JsonConvert.DeserializeObject<ShopifyOrder>(endAsync);
        this._cashGenRepository.AddEventLog(new EventLog()
        {
          EventDate = DateTime.Now,
          EventType = "API Request",
          Area = nameof (CreateOrder),
          Message = endAsync
        });
        this._cashGenRepository.Save();
        if (order.financial_status.ToLower() != "pending")
        {
          Order orderEntity = new Order();
          if (((IQueryable<Order>) this._context.Orders).Where<Order>((Expression<Func<Order, bool>>) (c => c.ShopifyId == order.id)).Count<Order>() > 0)
          {
            isNewOrder = false;
            orderEntity = ((IQueryable<Order>) this._context.Orders).Where<Order>((Expression<Func<Order, bool>>) (c => c.ShopifyId == order.id)).FirstOrDefault<Order>();
          }
          else
            orderEntity.ShopifyId = order.id;
          orderEntity.email = order.email;
          orderEntity.created_at = order.created_at;
          orderEntity.updated_at = order.updated_at;
          orderEntity.total_price = order.total_price;
          orderEntity.number = order.number;
          orderEntity.token = order.token;
          orderEntity.order_number = order.order_number;
          orderEntity.currency = order.currency;
          orderEntity.telephone = order.phone;
          orderEntity.Gateway = string.IsNullOrEmpty(order.gateway) || orderEntity.Gateway == "cash" || orderEntity.CustomerLastName == "Payl8r" ? "cash" : order.gateway;
          orderEntity.FinancialStatus = order.financial_status;
          orderEntity.CustomerFirstName = order.customer.first_name;
          orderEntity.CustomerLastName = order.customer.last_name;
          if (order.shipping_address != null)
          {
            orderEntity.ShippingFirstName = order.shipping_address.first_name;
            orderEntity.ShippingLastName = order.shipping_address.last_name;
            orderEntity.ShippingLine1 = order.shipping_address.address1;
            orderEntity.ShippingLine2 = order.shipping_address.address2;
            orderEntity.ShippingTown = order.shipping_address.city;
            orderEntity.ShippingCounty = order.shipping_address.province;
            orderEntity.ShippingPostCode = order.shipping_address.zip;
          }
          int num1 = new Random().Next(1932, 9145);
          orderEntity.CollectionCode = num1;
          foreach (ShopifyOrderAttribute noteAttribute in order.note_attributes)
          {
            if (noteAttribute.name.Trim().ToLower() == "fulfilment-group")
              orderEntity.FulfilmentMethod = noteAttribute.value;
          }
          if (string.IsNullOrEmpty(orderEntity.FulfilmentMethod) && order.shipping_lines.Count > 0)
          {
            if (order.shipping_lines[0].code.ToLower() == "collection")
              orderEntity.FulfilmentMethod = "collection";
            else if (order.shipping_lines[0].code.ToLower() == "delivery")
              orderEntity.FulfilmentMethod = "delivery";
          }
          if (string.IsNullOrEmpty(orderEntity.FulfilmentMethod))
            orderEntity.FulfilmentMethod = "delivery";
          string str1 = api.GetFraudRisk(orderEntity.ShopifyId);
          if (orderEntity.Gateway == "paypal")
            str1 = "accept";
          if (str1 == "investigate" && order.total_price >= 199M)
            str1 = "cancel";
          if (orderEntity.Gateway == "buy_now_pay_later_with_klarna" && order.line_items.Count > 1)
            str1 = "cancel";
          if (orderEntity.Gateway == "cash")
          {
            str1 = "cancel";
            new Shopify().TagOrder(orderEntity.ShopifyId, "Payl8r");
          }
          if (orderEntity.FraudRisk == "accept")
            str1 = orderEntity.FraudRisk;
          orderEntity.FraudRisk = str1;
          if (!isNewOrder)
          {
            this._cashGenRepository.UpdateOrder(orderEntity);
            this._cashGenRepository.Save();
          }
          else
          {
            this._cashGenRepository.AddOrder(orderEntity);
            this._cashGenRepository.Save();
          }
          if (!isNewOrder)
          {
            if (((IQueryable<LineItem>) this._context.LineItems).Where<LineItem>((Expression<Func<LineItem, bool>>) (c => c.OrderId == orderEntity.Id)).Count<LineItem>() != 0)
              goto label_62;
          }
          this._cashGenRepository.RemoveOrderLines(orderEntity.Id);
          foreach (ShopifyOrderLine lineItem1 in order.line_items)
          {
            ShopifyOrderLine line = lineItem1;
            Product product = ((IQueryable<Product>) this._context.Products).Where<Product>((Expression<Func<Product, bool>>) (c => (long?) Convert.ToInt64(c.ShopifyId) == line.product_id)).FirstOrDefault<Product>();
            long? nullable;
            if (product != null)
            {
              nullable = line.product_id;
              if (nullable.HasValue)
                goto label_41;
            }
            product = ((IQueryable<Product>) this._context.Products).Where<Product>((Expression<Func<Product, bool>>) (c => c.Barcode == line.sku)).FirstOrDefault<Product>();
label_41:
            if (product != null)
            {
              Store store = this._cashGenRepository.GetStore(product.StoreId);
              string empty = string.Empty;
              string str2;
              if (string.IsNullOrEmpty(orderEntity.FulfilmentMethod))
                str2 = "Delivery from Cash Generator";
              else if (orderEntity.FulfilmentMethod == "delivery")
              {
                str2 = "Delivery from Cash Generator " + store.Title;
              }
              else
              {
                string str3 = "Collection from Cash Generator " + store.Title + ", " + store.Line1 + ", ";
                if (!string.IsNullOrEmpty(store.Line2))
                  str3 = str3 + store.Line2 + ", ";
                str2 = str3 + store.Town + ", " + store.PostCode;
              }
              LineItem line1 = new LineItem();
              line1.OrderId = orderEntity.Id;
              nullable = line.variant_id;
              if (nullable.HasValue)
              {
                LineItem lineItem2 = line1;
                nullable = line.variant_id;
                long num2 = nullable.Value;
                lineItem2.variant_id = num2;
              }
              else
                line1.variant_id = 0L;
              line1.title = line.title;
              line1.quantity = line.quantity;
              line1.sku = line.sku;
              nullable = line.product_id;
              if (nullable.HasValue)
              {
                LineItem lineItem3 = line1;
                nullable = line.product_id;
                long num3 = nullable.Value;
                lineItem3.product_id = num3;
              }
              else
                line1.product_id = 0L;
              line1.ProductKey = product.Id;
              line1.fulfilment = str2;
              line1.line_price = line.price;
              line1.line_id = line.id;
              this._cashGenRepository.AddOrderLine(line1);
              this._cashGenRepository.Save();
              int num4 = product.Quantity - 1;
              if (num4 < 0)
                num4 = 0;
              product.Quantity = num4;
              product.IsSold = true;
              if (num4 <= 0 && product.ShopifyId != null)
              {
                this._cashGenRepository.AddEventLog(new EventLog()
                {
                  EventDate = DateTime.Now,
                  EventType = "Automated",
                  Area = "CreateOrder / RemoveProduct",
                  Message = "Product removed from Shopify (Product Id: " + product.Id.ToString() + ", Shopify Product Id: " + product.ShopifyId.ToString() + ")"
                });
                this._cashGenRepository.Save();
                api.RemoveProduct(product.ShopifyId);
                product.ShopifyId = "";
              }
              this._cashGenRepository.UpdateProduct(product);
              this._cashGenRepository.Save();
            }
          }
label_62:
          CashGen.Models.Mail mail1 = new CashGen.Models.Mail();
          CashGen.Helpers.Mail mail2 = new CashGen.Helpers.Mail();
          if (str1 != "cancel" && orderEntity.FinancialStatus == "paid" && isNewOrder)
            new CashGen.Shared.Mail().OrderMail(orderEntity.Id, this._context, "confirmation", Guid.Empty);
          return (IActionResult) new OkObjectResult((object) this._mapper.Map<OrderDto>((object) orderEntity));
        }
        GenericAPIResponse genericApiResponse = new GenericAPIResponse()
        {
          status = "ERROR"
        };
        genericApiResponse.status = "Pending orders not accepted.";
        return (IActionResult) new OkObjectResult((object) genericApiResponse);
      }
      catch (Exception ex)
      {
        this._cashGenRepository.AddEventLog(new EventLog()
        {
          EventDate = DateTime.Now,
          EventType = "ERROR",
          Area = "CreateOrder / TryCatch",
          Message = ex.Message
        });
        this._cashGenRepository.Save();
        return (IActionResult) new BadRequestObjectResult((object) false);
      }
    }

    [FunctionName("UpdateOrder")]
    public async Task<IActionResult> UpdateOrder(
      [HttpTrigger] HttpRequest req,
      Guid id,
      ILogger log)
    {
      OrderForUpdateDto orderForUpdateDto = JsonConvert.DeserializeObject<OrderForUpdateDto>(await ((TextReader) new StreamReader(req.Body)).ReadToEndAsync());
      Order order = this._cashGenRepository.GetOrder(id);
      this._mapper.Map<OrderForUpdateDto, Order>(orderForUpdateDto, order);
      this._cashGenRepository.UpdateOrder(order);
      this._cashGenRepository.Save();
      ShopifyOrderRequest shopifyOrderRequest = new ShopifyOrderRequest();
      shopifyOrderRequest.location_id = 36254613588L;
      if (!string.IsNullOrEmpty(order.TrackingCode))
        shopifyOrderRequest.tracking_numbers = new List<string>()
        {
          order.TrackingCode
        };
      if (!string.IsNullOrEmpty(order.TrackingUrl))
        shopifyOrderRequest.tracking_urls = new List<string>()
        {
          order.TrackingUrl
        };
      if (order.fulfillment_status == "ready")
      {
        shopifyOrderRequest.shipment_status = "ready_for_pickup";
        shopifyOrderRequest.status = "open";
      }
      else if (order.fulfillment_status == "collected")
      {
        shopifyOrderRequest.shipment_status = "ready_for_pickup";
        shopifyOrderRequest.status = "success";
      }
      else if (order.fulfillment_status == "fulfilled")
      {
        shopifyOrderRequest.shipment_status = "delivered";
        shopifyOrderRequest.status = "success";
      }
      shopifyOrderRequest.notify_customer = !(order.FulfilmentMethod == "collection");
      shopifyOrderRequest.tracking_company = "Other";
      new Shopify().UpdateOrder(order.ShopifyId, new ShopifyOrderRequestWrapper()
      {
        fulfillment = shopifyOrderRequest
      });
      return (IActionResult) new OkObjectResult((object) this._mapper.Map<OrderDto>((object) order));
    }

    [FunctionName("UpdateFraud")]
    public async Task<IActionResult> UpdateFraud(
      [HttpTrigger] HttpRequest req,
      Guid id,
      ILogger log)
    {
      userAuth userAuth = new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>) (object) req.Headers["auth_token"]).FirstOrDefault<string>());
      if (!userAuth.valid)
        return (IActionResult) new BadRequestResult();
      OrderFraudUpdateDto orderFraudUpdateDto = JsonConvert.DeserializeObject<OrderFraudUpdateDto>(await ((TextReader) new StreamReader(req.Body)).ReadToEndAsync());
      Order order = this._cashGenRepository.GetOrder(id);
      NoteForCreationDto noteForCreationDto = new NoteForCreationDto();
      noteForCreationDto.UserId = userAuth.auth_token;
      noteForCreationDto.NoteTime = DateTime.Now;
      noteForCreationDto.LinkedId = id;
      noteForCreationDto.NoteText = "Fraud risk changed from " + order.FraudRisk + " to " + orderFraudUpdateDto.FraudRisk;
      this._mapper.Map<OrderFraudUpdateDto, Order>(orderFraudUpdateDto, order);
      this._cashGenRepository.UpdateOrder(order);
      this._cashGenRepository.Save();
      this._cashGenRepository.AddNote(this._mapper.Map<Note>((object) noteForCreationDto));
      this._cashGenRepository.Save();
      CashGen.Shared.Mail mail = new CashGen.Shared.Mail();
      if (orderFraudUpdateDto.FraudRisk == "accept")
        mail.OrderMail(id, this._context, "confirmation", Guid.Empty);
      else if (orderFraudUpdateDto.FraudRisk == "declined")
        mail.OrderMail(id, this._context, "cancelled", Guid.Empty);
      return (IActionResult) new OkObjectResult((object) this._mapper.Map<OrderDto>((object) order));
    }

    [FunctionName("UpdateBlock")]
    public IActionResult UpdateBlock([HttpTrigger] HttpRequest req, Guid id, ILogger log)
    {
      if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>) (object) req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
        return (IActionResult) new BadRequestResult();
      Order orderFromRepo = this._cashGenRepository.GetOrder(id);
      orderFromRepo.BlockPayment = !orderFromRepo.BlockPayment;
      List<Transaction> transactionList = new List<Transaction>();
      DbSet<Transaction> transactions = this._context.Transactions;
      Expression<Func<Transaction, bool>> predicate = (Expression<Func<Transaction, bool>>) (c => c.OrderId == orderFromRepo.Id);
      foreach (Transaction transaction1 in ((IQueryable<Transaction>) transactions).Where<Transaction>(predicate).ToList<Transaction>())
      {
        Transaction tran = transaction1;
        tran.BlockPayment = orderFromRepo.BlockPayment;
        Transaction newBalance = ((IQueryable<Transaction>) this._context.Transactions).Where<Transaction>((Expression<Func<Transaction, bool>>) (c => c.StoreId == tran.StoreId && c.TransactionDate < tran.TransactionDate && !c.BlockPayment)).OrderByDescending<Transaction, DateTime>((Expression<Func<Transaction, DateTime>>) (c => c.TransactionDate)).ThenBy<Transaction, Decimal>((Expression<Func<Transaction, Decimal>>) (c => c.Amount)).FirstOrDefault<Transaction>();
        List<Transaction> list = ((IQueryable<Transaction>) this._context.Transactions).Where<Transaction>((Expression<Func<Transaction, bool>>) (c => c.StoreId == tran.StoreId && c.TransactionDate > newBalance.TransactionDate && !c.BlockPayment)).OrderBy<Transaction, DateTime>((Expression<Func<Transaction, DateTime>>) (c => c.TransactionDate)).ToList<Transaction>();
        Decimal num = 0M;
        if (newBalance != null)
          num = newBalance.Balance;
        foreach (Transaction transaction2 in list)
        {
          num += transaction2.Amount;
          transaction2.Balance = num;
        }
      }
      this._cashGenRepository.Save();
      return (IActionResult) new OkObjectResult((object) this._mapper.Map<OrderDto>((object) orderFromRepo));
    }

    [FunctionName("OrderMail")]
    public IActionResult OrderMail([HttpTrigger] HttpRequest req, Guid id, ILogger log)
    {
      userAuth userAuth = new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>) (object) req.Headers["auth_token"]).FirstOrDefault<string>());
      if (!userAuth.valid)
        return (IActionResult) new BadRequestResult();
      string type = "confirmation";
      if (!string.IsNullOrEmpty(StringValues.op_Implicit(req.Query["type"])))
        type = StringValues.op_Implicit(req.Query["type"]);
      new CashGen.Shared.Mail().OrderMail(id, this._context, type, userAuth.auth_token);
      return (IActionResult) new OkObjectResult((object) true);
    }
  }
}
