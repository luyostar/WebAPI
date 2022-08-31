
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DoubanAPI.Attributes
{
    public class AllowCrossSiteJsonAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context != null)
            {
                context.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                context.HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET,PUT,POST,OPTIONS,DELETE,PATCH,HEAD");
                context.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");
              //  context.HttpContext.Response.Headers.Add("Access-Control-Allow-Credentials","true"); //允许跨域携带cookies
            }
            base.OnActionExecuted(context);
        }
    }
}
