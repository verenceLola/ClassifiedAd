using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MarketPlace.Services.ApplicationServices.Interfaces;
using Serilog;

namespace MarketPlace.Api.ClassifiedAdsCommands
{
    [Route("/ad")]
    public class ClassifiedAdApiCommands : Controller
    {
        private readonly IApplicationService _applicationService;
        private static readonly ILogger Log = Serilog.Log.ForContext<ClassifiedAdApiCommands>();
        public ClassifiedAdApiCommands(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        [HttpPost]
        public async Task<IActionResult> Post(Contracts.ClassifiedAds.Create request) =>
            await Infrastructure.RequestHandler.HandleRequest(request, _applicationService.Handle, Log);

        [Route("name")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.SetTitle request) =>
            await Infrastructure.RequestHandler.HandleRequest(request, _applicationService.Handle, Log);

        [Route("text")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.UpdateText request) =>
             await Infrastructure.RequestHandler.HandleRequest(request, _applicationService.Handle, Log);

        [Route("price")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.UpdatePrice request) =>
             await Infrastructure.RequestHandler.HandleRequest(request, _applicationService.Handle, Log);

        [Route("request to publish")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.RequestToPublish request) =>
             await Infrastructure.RequestHandler.HandleRequest(request, _applicationService.Handle, Log);

        [Route("publish")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.Publish request) =>
            await Infrastructure.RequestHandler.HandleRequest(request, _applicationService.Handle, Log);

    }
}
