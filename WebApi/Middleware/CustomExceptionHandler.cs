using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TestProjectLegioSoft.Middleware
{
    public class CustomExceptionHandler
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandler(RequestDelegate next) =>
            _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            switch (exception)
            {
                case ArgumentOutOfRangeException:
                    code = HttpStatusCode.NotFound;
                    break;
                case ArgumentException:
                    code = HttpStatusCode.UnsupportedMediaType;
                    break;
                case TaskCanceledException:
                    code = HttpStatusCode.BadRequest;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            
            string result = JsonSerializer.Serialize(new { errorMsg = exception.Message });
            
            return context.Response.WriteAsync(result);
        }
    }
}