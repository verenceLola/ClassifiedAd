using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Marketplace.Api.ApplicationServices.Interfaces;
using Serilog;
namespace Marketplace.Api.ClassifiedAdsCommands
{
    [Route("/ad")]
    public class Create : Controller
    {
        private readonly IApplicationService _applicationService;
        public Create(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        [HttpPost]
        public async Task<IActionResult> Post(Contracts.ClassifiedAds.V1.Create request) =>
            await HandleRequest(request, _applicationService.Handle);

        [Route("name")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.V1.SetTitle request) =>
            await HandleRequest(request, _applicationService.Handle);

        [Route("text")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.V1.UpdateText request) =>
             await HandleRequest(request, _applicationService.Handle);

        [Route("price")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.V1.UpdatePrice request) =>
             await HandleRequest(request, _applicationService.Handle);

        [Route("publish")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.V1.RequestToPublish request) =>
             await HandleRequest(request, _applicationService.Handle);

        private async Task<IActionResult> HandleRequest<T>(T request, Func<T, Task> handler)
        {
            try
            {
                Log.Debug("Handling HTTP request of type {type}", typeof(T).Name); ;
                await handler(request);

                return Ok();
            }
            catch (Exception e)
            {
                Log.Error("Error handling the request", e);

                return new BadRequestObjectResult(new { error = e.Message, stackTrace = e.StackTrace });
            }
        }
    }
}
