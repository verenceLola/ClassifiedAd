using System.Threading.Tasks;
using Serilog;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Api.Commands
{
    [Route("/profile")]
    public class UserProfileApiCommands : Controller
    {
        private readonly Services.ApplicationServices.Interfaces.IApplicationService _applicationService;
        private readonly static ILogger Log = Serilog.Log.ForContext<UserProfileApiCommands>();
        public UserProfileApiCommands(Services.ApplicationServices.UserProfileApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        [HttpPost]
        public async Task<IActionResult> Post(Contracts.UserProfile.RegisterUser request) =>
            await Infrastructure.RequestHandler.HandleRequest(request, _applicationService.Handle, Log);
        [Route("fullname")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.UserProfile.UpdateUserFullName request) =>
            await Infrastructure.RequestHandler.HandleRequest(request, _applicationService.Handle, Log);
        [Route("displayname")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.UserProfile.UpdateUserDisplayName request) =>
            await Infrastructure.RequestHandler.HandleRequest(request, _applicationService.Handle, Log);
        [Route("photo")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.UserProfile.UpdateUserProfilePhoto request) =>
            await Infrastructure.RequestHandler.HandleRequest(request, _applicationService.Handle, Log);
    }
}
