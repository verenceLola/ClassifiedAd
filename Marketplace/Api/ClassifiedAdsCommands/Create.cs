using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace Marketplace.Api.ClassifiedAdsCommands
{
    [Route("/ad")]
    public class Create : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post(Contracts.ClassifiedAds.V1.Create request) => await Task.FromResult<IActionResult>(Ok());
    }
}
