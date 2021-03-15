using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;


namespace MarketPlace.Infrastructure
{
    public static class RequestHandler
    {
        public static async Task<IActionResult> HandleRequest<T>(T request, Func<T, Task> handler, ILogger logger)
        {
            try
            {
                Log.Debug("Handling HTTP request of type {type}", typeof(T).Name);
                await handler(request);

                return new OkResult();
            }
            catch (Exception e)
            {
                Log.Error("Error handling the request", e);

                return new BadRequestObjectResult(new { error = e.Message, stackTrace = e.StackTrace });
            }
        }
        public static IActionResult HandleQuery<TModel>(Func<TModel> query, ILogger logger)
        {
            try
            {
                return new OkObjectResult(query());
            }
            catch (Exception e)
            {
                logger.Error(e, "Error handling the query");

                return new BadRequestObjectResult(new
                {
                    error = e.Message,
                    stackTrace = e.StackTrace
                });
            }
        }
    }
}
