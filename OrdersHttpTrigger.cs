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
            int num = 1;
            int count1 = 30;
            if (!string.IsNullOrEmpty(req.Query["page"]))
                num = Convert.ToInt32(req.Query["page"]);
            if (!string.IsNullOrEmpty(req.Query["size"]))
                count1 = Convert.ToInt32(req.Query["size"]);
            int count2 = (num - 1) * count1;
            Guid store = Guid.NewGuid();
            Boolean admin = false;
            if (!string.IsNullOrEmpty(req.Query["admin-view"]))
                admin = true;
            else
                store = new Guid(req.Query["store"]);
            Boolean _risk = false;
            if (!string.IsNullOrEmpty(req.Query["risk"]))
               _risk = true;
            string _keyword = "";
            if (!string.IsNullOrEmpty(req.Query["keyword"]))
                _keyword = req.Query["keyword"];
            string _status = "";
            if (!string.IsNullOrEmpty(req.Query["status"]))
                _status = req.Query["status"];
            DbSet<Order> orders = this._context.Orders;
            DbSet<LineItem> lineItems = this._context.LineItems;
            /* ParameterExpression parameterExpression1 = Expression.Parameter(typeof(Order), "ord");
            Expression<Func<Order, Guid>> outerKeySelector1 = Expression.Lambda<Func<Order, Guid>>((Expression)Expression.Property((Expression)parameterExpression1, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Order.get_Id))), new ParameterExpression[1]
            {
                 parameterExpression1
            });
            ParameterExpression parameterExpression2 = Expression.Parameter(typeof(LineItem), "ln");
            Expression<Func<LineItem, Guid>> innerKeySelector1 = Expression.Lambda<Func<LineItem, Guid>>((Expression)Expression.Property((Expression)parameterExpression2, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(LineItem.get_OrderId))), new ParameterExpression[1]
            {
                parameterExpression2
            });
            ParameterExpression parameterExpression3 = Expression.Parameter(typeof(Order), "ord");
            ParameterExpression parameterExpression4 = Expression.Parameter(typeof(LineItem), "ln");
            Expression < Func < Order, LineItem, f__AnonymousType0 < Order, LineItem >>> resultSelector1 = Expression.Lambda < Func < Order, LineItem, f__AnonymousType0 < Order, LineItem >>> ((Expression)Expression.New((ConstructorInfo)MethodBase.GetMethodFromHandle(__methodref(f__AnonymousType0<Order, LineItem>.\u002Ector), __typeref(f__AnonymousType0<Order, LineItem>)), (IEnumerable<Expression>)new Expression[2]
            {
                (Expression) parameterExpression3,
                (Expression) parameterExpression4
            }, new MemberInfo[2]
            {
                (MemberInfo) MethodBase.GetMethodFromHandle(__methodref (f__AnonymousType0<Order, LineItem>.get_ord), __typeref (f__AnonymousType0<Order, LineItem>)),
                (MemberInfo) MethodBase.GetMethodFromHandle(__methodref (f__AnonymousType0<Order, LineItem>.get_ln), __typeref (f__AnonymousType0<Order, LineItem>))
            }), new ParameterExpression[2]
            {
                parameterExpression3,
                parameterExpression4
             });
            IQueryable <f__AnonymousType0 < Order, LineItem >> outer = ((IQueryable<Order>)orders).Join((IEnumerable<LineItem>)lineItems, outerKeySelector1, innerKeySelector1, resultSelector1);
            DbSet<Product> products = this._context.Products;
            ParameterExpression parameterExpression5 = Expression.Parameter(typeof(f__AnonymousType0<Order, LineItem>), "<>h__TransparentIdentifier0");
            Expression < Func <f__AnonymousType0<Order, LineItem>, Guid >> outerKeySelector2 = Expression.Lambda < Func <f__AnonymousType0<Order, LineItem>, Guid >> ((Expression)Expression.Property((Expression)Expression.Property((Expression)parameterExpression5, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(f__AnonymousType0<Order, LineItem>.get_ln), __typeref(f__AnonymousType0<Order, LineItem>))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(LineItem.get_ProductKey))), new ParameterExpression[1]
            {
                parameterExpression5
            });
            ParameterExpression parameterExpression6 = Expression.Parameter(typeof(Product), "pd");
            Expression<Func<Product, Guid>> innerKeySelector2 = Expression.Lambda<Func<Product, Guid>>((Expression)Expression.Property((Expression)parameterExpression6, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Product.get_Id))), new ParameterExpression[1]
            {
              parameterExpression6
            });
            ParameterExpression parameterExpression7 = Expression.Parameter(typeof(f__AnonymousType0<Order, LineItem>), "<>h__TransparentIdentifier0");
            ParameterExpression parameterExpression8 = Expression.Parameter(typeof(Product), "pd");
            Expression < Func <f__AnonymousType0<Order, LineItem>, Product, f__AnonymousType1 <f__AnonymousType0<Order, LineItem>, Product >>> resultSelector2 = Expression.Lambda < Func <f__AnonymousType0<Order, LineItem>, Product, f__AnonymousType1 <f__AnonymousType0<Order, LineItem>, Product >>> ((Expression)Expression.New((ConstructorInfo)MethodBase.GetMethodFromHandle(__methodref(f__AnonymousType1 <f__AnonymousType0<Order, LineItem>, Product >..ctor), __typeref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >)), (IEnumerable<Expression>)new Expression[2]
            {
                (Expression) parameterExpression7,
                (Expression) parameterExpression8
            }, new MemberInfo[2]
            {
                (MemberInfo) MethodBase.GetMethodFromHandle(__methodref (f__AnonymousType1<f__AnonymousType0<Order, LineItem>, Product>.get_h__TransparentIdentifier0), __typeref (f__AnonymousType1<f__AnonymousType0<Order, LineItem>, Product>)),
                (MemberInfo) MethodBase.GetMethodFromHandle(__methodref (f__AnonymousType1<f__AnonymousType0<Order, LineItem>, Product>.get_pd), __typeref (f__AnonymousType1<f__AnonymousType0<Order, LineItem>, Product>))
            }), new ParameterExpression[2]
            {
                parameterExpression7,
                parameterExpression8
            });
            IQueryable <f__AnonymousType1 <f__AnonymousType0<Order, LineItem>, Product >> source1 = outer.Join((IEnumerable<Product>)products, outerKeySelector2, innerKeySelector2, resultSelector2);
            ParameterExpression parameterExpression9 = Expression.Parameter(typeof(f__AnonymousType1 <f__AnonymousType0<Order, LineItem>, Product >), "<>h__TransparentIdentifier1");
            Expression < Func <f__AnonymousType1 <f__AnonymousType0<Order, LineItem>, Product >, bool>> predicate = Expression.Lambda < Func <f__AnonymousType1 <f__AnonymousType0<Order, LineItem>, Product >, bool>> ((Expression)Expression.AndAlso((Expression)Expression.AndAlso((Expression)Expression.AndAlso((Expression)Expression.AndAlso((Expression)Expression.AndAlso((Expression)Expression.AndAlso((Expression)Expression.OrElse((Expression)Expression.Equal((Expression)Expression.Property((Expression)Expression.Property((Expression)parameterExpression9, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >.get_pd), __typeref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Product.get_StoreId))), (Expression)Expression.Field((Expression)Expression.Constant((object)cDisplayClass40, typeof(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0.store))), false, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Guid.op_Equality))), (Expression)Expression.Equal((Expression)Expression.Field((Expression)Expression.Constant((object)cDisplayClass40, typeof(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0.admin))), (Expression)Expression.Constant((object)true, typeof(bool)))), (Expression)Expression.OrElse((Expression)Expression.OrElse((Expression)Expression.OrElse((Expression)Expression.OrElse((Expression)Expression.Equal((Expression)Expression.Field((Expression)Expression.Constant((object)cDisplayClass40, typeof(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0._keyword))), (Expression)Expression.Constant((object)"", typeof(string))), (Expression)Expression.Call((Expression)Expression.Call((Expression)Expression.Property((Expression)Expression.Property((Expression)Expression.Property((Expression)parameterExpression9, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref(\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Order.get_email))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(string.ToLower)), Array.Empty<Expression>()), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(string.Contains)), new Expression[1]
            {
                (Expression) Expression.Call((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0._keyword))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>())
            })), (Expression) Expression.Call((Expression) Expression.Call((Expression) Expression.Property((Expression) Expression.Property((Expression) Expression.Property((Expression) parameterExpression9, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref(\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Order.get_CustomerFirstName))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>()), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.Contains)), new Expression[1]
            {
             (Expression) Expression.Call((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (OrdersHttpTrigger.c__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (OrdersHttpTrigger.c__DisplayClass4_0._keyword))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>())
            })), (Expression) Expression.Call((Expression) Expression.Call((Expression) Expression.Property((Expression) Expression.Property((Expression) Expression.Property((Expression) parameterExpression9, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref(\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Order.get_CustomerLastName))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>()), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.Contains)), new Expression[1]
            {
                (Expression) Expression.Call((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (OrdersHttpTrigger.c__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (OrdersHttpTrigger.c__DisplayClass4_0._keyword))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>())
            })), (Expression)Expression.Call((Expression)Expression.Call((Expression)Expression.Property((Expression)Expression.Property((Expression)Expression.Property((Expression)parameterExpression9, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref(\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Order.get_order_number))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(int.ToString)), Array.Empty<Expression>()), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(string.Contains)), new Expression[1]
            {
                (Expression) Expression.Call((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (OrdersHttpTrigger.c__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (OrdersHttpTrigger.c__DisplayClass4_0._keyword))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>())
            }))), (Expression)Expression.OrElse((Expression)Expression.OrElse((Expression)Expression.Equal((Expression)Expression.Field((Expression)Expression.Constant((object)cDisplayClass40, typeof(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0._status))), (Expression)Expression.Constant((object)"", typeof(string))), (Expression)Expression.AndAlso((Expression)Expression.Equal((Expression)Expression.Field((Expression)Expression.Constant((object)cDisplayClass40, typeof(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0._status))), (Expression)Expression.Constant((object)"pending", typeof(string))), (Expression)Expression.OrElse((Expression)Expression.Equal((Expression)Expression.Property((Expression)Expression.Property((Expression)Expression.Property((Expression)parameterExpression9, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref(\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Order.get_fulfillment_status))), (Expression)Expression.Constant((object)"", typeof(string))), (Expression)Expression.Equal((Expression)Expression.Property((Expression)Expression.Property((Expression)Expression.Property((Expression)parameterExpression9, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref(\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Order.get_fulfillment_status))), (Expression)Expression.Constant((object)null, typeof(string)))))), (Expression)Expression.Equal((Expression)Expression.Property((Expression)Expression.Property((Expression)Expression.Property((Expression)parameterExpression9, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref(\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Order.get_fulfillment_status))), (Expression)Expression.Field((Expression)Expression.Constant((object)cDisplayClass40, typeof(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0._status)))))), (Expression)Expression.OrElse((Expression)Expression.AndAlso((Expression)Expression.Equal((Expression)Expression.Field((Expression)Expression.Constant((object)cDisplayClass40, typeof(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref(OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0._risk))), (Expression)Expression.Constant((object)false, typeof(bool))), (Expression)Expression.NotEqual((Expression)Expression.Property((Expression)Expression.Property((Expression)Expression.Property((Expression)parameterExpression9, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product >.get_\u003C\u003Eh__TransparentIdentifier0), __typeref(\u003C\u003Ef__AnonymousType1 <\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref (\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Order.get_FraudRisk))), (Expression) Expression.Constant((object) "cancel", typeof (string)))), (Expression) Expression.AndAlso((Expression) Expression.Equal((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (OrdersHttpTrigger.\u003C\u003Ec__DisplayClass4_0._risk))), (Expression) Expression.Constant((object) true, typeof (bool))), (Expression) Expression.Equal((Expression) Expression.Property((Expression) Expression.Property((Expression) Expression.Property((Expression) parameterExpression9, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>.get_\u003C\u003Eh__TransparentIdentifier0), __typeref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref (\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Order.get_FraudRisk))), (Expression) Expression.Constant((object) "cancel", typeof (string)))))), (Expression) Expression.Equal((Expression) Expression.Property((Expression) Expression.Property((Expression) Expression.Property((Expression) parameterExpression9, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>.get_\u003C\u003Eh__TransparentIdentifier0), __typeref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref (\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Order.get_IsDeleted))), (Expression) Expression.Constant((object) false, typeof (bool)))), (Expression) Expression.NotEqual((Expression) Expression.Property((Expression) Expression.Property((Expression) Expression.Property((Expression) parameterExpression9, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>.get_\u003C\u003Eh__TransparentIdentifier0), __typeref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref (\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Order.get_FraudRisk))), (Expression) Expression.Constant((object) "declined", typeof (string)))), (Expression) Expression.NotEqual((Expression) Expression.Property((Expression) Expression.Property((Expression) Expression.Property((Expression) parameterExpression9, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>.get_\u003C\u003Eh__TransparentIdentifier0), __typeref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref (\u003C\u003Ef__AnonymousType0<Order, LineItem>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Order.get_FinancialStatus))), (Expression) Expression.Constant((object) "pending", typeof (string)))), new ParameterExpression[1]
            {
                 parameterExpression9
            });
            IQueryable<f__AnonymousType1<f__AnonymousType0<Order, LineItem>, Product>> source2 = source1.Where(predicate);
            ParameterExpression parameterExpression10 = Expression.Parameter(typeof (f__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>), "<>h__TransparentIdentifier1");
            Expression<Func<f__AnonymousType1<f__AnonymousType0<Order, LineItem>, Product>, Order>> selector = Expression.Lambda<Func<f__AnonymousType1<f__AnonymousType0<Order, LineItem>, Product>, Order>>((Expression) Expression.Property((Expression) Expression.Property((Expression) parameterExpression10, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>.get_\u003C\u003Eh__TransparentIdentifier0), __typeref (\u003C\u003Ef__AnonymousType1<\u003C\u003Ef__AnonymousType0<Order, LineItem>, Product>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType0<Order, LineItem>.get_ord), __typeref (\u003C\u003Ef__AnonymousType0<Order, LineItem>))), new ParameterExpression[1]
            {
                parameterExpression10
            });
            IQueryable<Order> source3 = source2.Select(selector).Distinct<Order>();
            ParameterExpression parameterExpression11 = Expression.Parameter(typeof (Order), "o");
            // ISSUE: method reference
            Expression<Func<Order, int>> keySelector = Expression.Lambda<Func<Order, int>>((Expression) Expression.Property((Expression) parameterExpression11, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Order.get_order_number))), new ParameterExpression[1]
            {
            parameterExpression11
            });
           */
            List<Order> list = source3.OrderByDescending<Order, int>(keySelector).ToList<Order>();
            IEnumerable<OrderListDto> source4 = this._mapper.Map<IEnumerable<OrderListDto>>((object) list).ToList<OrderListDto>().Skip<OrderListDto>(count2).Take<OrderListDto>(count1);
            GetOrdersResponse getOrdersResponse = new GetOrdersResponse();
            getOrdersResponse.count = list.Count<Order>();
            getOrdersResponse.results = source4.ToList<OrderListDto>();
            return (IActionResult) new OkObjectResult((object) getOrdersResponse);
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
          if (!string.IsNullOrEmpty(req.Query["status"]))
            str = req.Query["status"];
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
