using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Contact.Api.Filters
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute" />
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is NotImplementedException)
            {
                context.Result = new ObjectResult(HttpStatusCode.NotImplemented);
            }
            else
            {
                context.Result = new ObjectResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}
