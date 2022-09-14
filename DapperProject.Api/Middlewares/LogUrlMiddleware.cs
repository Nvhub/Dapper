using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DapperProject.Api.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LogUrlMiddleware
    {
        private readonly RequestDelegate _next;

        public LogUrlMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var request = httpContext.Request;
            var response = httpContext.Response;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" Log Url : {request.Path}      {request.Method}      {request}");

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LogUrlMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogUrlMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogUrlMiddleware>();
        }
    }
}
