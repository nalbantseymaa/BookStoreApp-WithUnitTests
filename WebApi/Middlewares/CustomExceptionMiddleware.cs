using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Services;

namespace WebApi.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILoggerService _loggerService;
        public CustomExceptionMiddleware(RequestDelegate next, ILoggerService loggerService)
        {
            _next = next;
            _loggerService = loggerService;
        }

        //USE
        public async Task Invoke(HttpContext context)
        {
            var watch = Stopwatch.StartNew();
            try
            {

                //Request loglama
                string message = "[Request] HTTP " + context.Request.Method + " - " + context.Request.Path;
                // Console.WriteLine(message);
                _loggerService.Write(message);
                await _next(context);   //bir sonraki middleware çağrıldı
                watch.Stop();

                //Response loglama
                message = "[Response] HTTP " + context.Request.Method + " - " + context.Request.Path + " responded " + context.Response.StatusCode + " in " + watch.ElapsedMilliseconds + " ms";
                // Console.WriteLine(message);
                _loggerService.Write(message);

            }
            catch (Exception ex)
            {
                watch.Stop();
                await HandleException(context, ex, watch);

            }

        }
        //context.Response objesini ezerek exception durumlarında dönülecek hata mesajını belirledik
        private Task HandleException(HttpContext context, Exception ex, Stopwatch watch)
        {
            context.Response.ContentType = "application/json";
            //exception objesini json olarak döndürüyoruz 
            //message objesini jsona çevirmek için serialize etmeiliyiz-Newtonsoft.Json package

            //Eğer hata varsa contex'in responsunu burada set edebiliriz
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            string message = "[Error] HTTP " + context.Request.Method + " - " + context.Response.StatusCode + " Error Mesage" + ex.Message + " in " + watch.Elapsed.TotalMilliseconds + " ms";

            // Console.WriteLine(message);
            _loggerService.Write(message);



            var result = JsonConvert.SerializeObject(new { error = ex.Message }, Formatting.None);

            return context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomExceptionMiddle(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}