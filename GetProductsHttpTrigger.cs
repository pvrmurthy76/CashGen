using AutoMapper;
using CashGen.DBContexts;
using CashGen.Entities;
using CashGen.Models;
using CashGen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CashGen
{
    public class GetProductsHttpTrigger
    {
        private readonly ICashGenRepository _cashGenRepository;
        private readonly CashGenContext _context;
        private readonly IMapper _mapper;

        public GetProductsHttpTrigger(
          ICashGenRepository cashGenRepository,
          CashGenContext context,
          IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
            this._cashGenRepository = cashGenRepository;
        }

        [FunctionName("GetProducts")]
        public IActionResult GetProducts([HttpTrigger] HttpRequest req, ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0 cDisplayClass40 = new GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass40.store = new Guid(StringValues.op_Implicit(req.Query["store"]));
            int num = 1;
            int count1 = 30;
            if (!string.IsNullOrEmpty(StringValues.op_Implicit(req.Query["page"])))
                num = Convert.ToInt32(StringValues.op_Implicit(req.Query["page"]));
            if (!string.IsNullOrEmpty(StringValues.op_Implicit(req.Query["size"])))
                count1 = Convert.ToInt32(StringValues.op_Implicit(req.Query["size"]));
            int count2 = (num - 1) * count1;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass40._keyword = "";
            if (!string.IsNullOrEmpty(StringValues.op_Implicit(req.Query["keyword"])))
            {
                // ISSUE: reference to a compiler-generated field
                cDisplayClass40._keyword = StringValues.op_Implicit(req.Query["keyword"]);
            }
            // ISSUE: reference to a compiler-generated field
            cDisplayClass40._level1 = "";
            if (!string.IsNullOrEmpty(StringValues.op_Implicit(req.Query["l1"])))
            {
                // ISSUE: reference to a compiler-generated field
                cDisplayClass40._level1 = StringValues.op_Implicit(req.Query["l1"]);
            }
            // ISSUE: reference to a compiler-generated field
            cDisplayClass40._level2 = "";
            if (!string.IsNullOrEmpty(StringValues.op_Implicit(req.Query["l2"])))
            {
                // ISSUE: reference to a compiler-generated field
                cDisplayClass40._level2 = StringValues.op_Implicit(req.Query["l2"]);
            }
            // ISSUE: reference to a compiler-generated field
            cDisplayClass40._level3 = "";
            if (!string.IsNullOrEmpty(StringValues.op_Implicit(req.Query["l3"])))
            {
                // ISSUE: reference to a compiler-generated field
                cDisplayClass40._level3 = StringValues.op_Implicit(req.Query["l3"]);
            }
            ParameterExpression parameterExpression;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: method reference
            IEnumerable<Product> source = (IEnumerable<Product>)((IQueryable<Product>)this._context.Products).Where<Product>(Expression.Lambda<Func<Product, bool>>((Expression)Expression.AndAlso((Expression)Expression.AndAlso((Expression)Expression.AndAlso((Expression)Expression.AndAlso(c.StoreId == cDisplayClass40.store && c.IsDeleted != true, (Expression)Expression.OrElse((Expression)Expression.OrElse(cDisplayClass40._keyword == "", (Expression)Expression.Call((Expression)Expression.Call(c.Title, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(string.ToLower)), Array.Empty<Expression>()), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(string.Contains)), new Expression[1]
            {
        (Expression) Expression.Call((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0._keyword))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>())
          })), (Expression) Expression.Call((Expression) Expression.Call((Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Product.get_Barcode))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>()), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.Contains)), new Expression[1]
      {
        (Expression) Expression.Call((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0._keyword))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>())
      }))), (Expression) Expression.OrElse((Expression) Expression.Equal((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0._level1))), (Expression) Expression.Constant((object) "", typeof (string))), (Expression) Expression.Equal((Expression) Expression.Call((Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Product.get_CatLevel1))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>()), (Expression) Expression.Call((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0._level1))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>())))), (Expression) Expression.OrElse((Expression) Expression.Equal((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0._level2))), (Expression) Expression.Constant((object) "", typeof (string))), (Expression) Expression.Equal((Expression) Expression.Call((Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Product.get_CatLevel2))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>()), (Expression) Expression.Call((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0._level2))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>())))), (Expression) Expression.OrElse((Expression) Expression.Equal((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0._level3))), (Expression) Expression.Constant((object) "", typeof (string))), (Expression) Expression.Equal((Expression) Expression.Call((Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Product.get_CatLevel3))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>()), (Expression) Expression.Call((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass40, typeof (GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0)), FieldInfo.GetFieldFromHandle(__fieldref (GetProductsHttpTrigger.\u003C\u003Ec__DisplayClass4_0._level3))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>())))), parameterExpression));
      IEnumerable<Product> products = source.Skip<Product>(count2).Take<Product>(count1);
      return (IActionResult) new OkObjectResult((object) new GetProductsResponse()
    {
        count = source.Count<Product>(),
        results = this._mapper.Map<IEnumerable<ProductListDto>>((object)products).ToList<ProductListDto>()
      });
    }
  }
}
