using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace EEQuoteGenerator.Filters
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
         
            string UserId = context.HttpContext.Session.GetString("UserId");
            if(UserId == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "User",
                    action = "Login"
                }));
            }

            base.OnActionExecuting(context);
        }

    }
}
