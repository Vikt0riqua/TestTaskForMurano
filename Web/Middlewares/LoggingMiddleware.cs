using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Web.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;

        public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this._next = next;
            this._loggerFactory = loggerFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var logger = this._loggerFactory.CreateLogger<LoggingMiddleware>();
            using (logger.BeginScope<LoggingMiddleware>(this))
            {
                try
                {
                    await this._next.Invoke(context);
                }
                catch (Exception e)
                {
                    logger.LogError(e,e.Message);
                }
            }
        }
    }
}