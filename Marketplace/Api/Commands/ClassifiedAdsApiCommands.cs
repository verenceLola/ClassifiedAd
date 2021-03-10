using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Marketplace.Api.ApplicationServices.Interfaces;
using Serilog;

namespace Marketplace.Api.ClassifiedAdsCommands
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
        public async Task<IActionResult> Post(Contracts.ClassifiedAds.V1.Create request) =>
            await Infrastructure.RequestHandler.HandleRequest(request, _applicationService.Handle, Log);

        [Route("name")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.V1.SetTitle request) =>
            await Infrastructure.RequestHandler.HandleRequest(request, _applicationService.Handle, Log);

        [Route("text")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.V1.UpdateText request) =>
             await Infrastructure.RequestHandler.HandleRequest(request, _applicationService.Handle, Log);

        [Route("price")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.V1.UpdatePrice request) =>
             await Infrastructure.RequestHandler.HandleRequest(request, _applicationService.Handle, Log);

        [Route("publish")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.V1.RequestToPublish request) =>
             await Infrastructure.RequestHandler.HandleRequest(request, _applicationService.Handle, Log);

    }
}
