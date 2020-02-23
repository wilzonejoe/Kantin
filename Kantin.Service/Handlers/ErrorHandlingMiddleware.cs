using Kantin.Data.Exceptions;
using Kantin.Data.Exceptions.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Kantin.Service.Exceptions
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            object innerError = null;

            switch (exception)
            {
                case ItemNotFoundException itemNotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                case UnauthorizedAccessException unauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    break;
                case BadRequestException badRequestException:
                    code = HttpStatusCode.BadRequest;

                    if (badRequestException.Type == BadRequestType.Validation)
                        innerError = badRequestException.ValidationResults;

                    break;
            }

            var error = new { Error = innerError ?? exception.Message };
            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
