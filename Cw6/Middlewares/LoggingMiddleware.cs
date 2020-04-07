using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw6.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
            if (httpContext.Request !=null)
            {
                string path = httpContext.Request.Path;
                string method = httpContext.Request.Method;
                string querystr = httpContext.Request.QueryString.ToString();
                var bodyStream = string.Empty;


                using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStream = await reader.ReadToEndAsync();
                   
                }
                using (StreamWriter outputFile = new StreamWriter(( "/Users/dianakravets/Data/IV sem/Cw6/Cw6/Middlewares/requestsLog.txt"),true))
                {
                    
                        outputFile.WriteLine("Metoda - "+method+" Sciezka - "+path+" QueryString - "+querystr+" Body - "+bodyStream);
                }
            }
            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            await _next(httpContext);
        }
    }
}