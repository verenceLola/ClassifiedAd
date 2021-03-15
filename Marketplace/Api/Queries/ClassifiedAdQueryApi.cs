using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Serilog;


namespace MarketPlace.Api.Queries
{
    [Route("/ad")]
    public class ClassifiedAdQueryApi : Controller
    {
        private readonly IEnumerable<ReadModels.ClassifiedAds.ClassfiedAdDetails> _items;
        private static ILogger _log = Log.ForContext<ClassifiedAdQueryApi>();
        public ClassifiedAdQueryApi(IEnumerable<ReadModels.ClassifiedAds.ClassfiedAdDetails> items)
        {
            _items = items;
        }
        // [HttpGet]
        // [Route("list")]
        // public Task<IActionResult> Get(QueryModels.GetPublishedClassifiedAds request)
        //     => Infrastructure.RequestHandler.HandleQuery(() => _items.Query(request), _log);
        // [HttpGet]
        // [Route("myads")]
        // public Task<IActionResult> Get(QueryModels.GetOwnersClassifiedAds request)
        //     => Infrastructure.RequestHandler.HandleQuery(() => _items.Query(request), _log);
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult Get(QueryModels.GetPublicClassifiedAd request)
             => Infrastructure.RequestHandler.HandleQuery(() => _items.Query(request), _log);
    }
}
