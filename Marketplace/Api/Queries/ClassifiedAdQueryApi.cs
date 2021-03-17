using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Serilog;
using Raven.Client.Documents.Session;


namespace MarketPlace.Api.Queries
{
    [Route("/ad")]
    public class ClassifiedAdQueryApi : Controller
    {
        private readonly IAsyncDocumentSession _session;
        private static ILogger _log = Log.ForContext<ClassifiedAdQueryApi>();
        public ClassifiedAdQueryApi(IAsyncDocumentSession session)
        {
            _session = session;
        }
        // [HttpGet]
        // [Route("list")]
        // public Task<IActionResult> Get(QueryModels.GetPublishedClassifiedAds request)
        //     => Infrastructure.RequestHandler.HandleQuery(() => _session.Query(request), _log);
        // [HttpGet]
        // [Route("myads")]
        // public Task<IActionResult> Get(QueryModels.GetOwnersClassifiedAds request)
        //     => Infrastructure.RequestHandler.HandleQuery(() => _session.Query<QueryModels.GetOwnersClassifiedAds>(request), _log);
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public Task<IActionResult> Get(QueryModels.GetPublicClassifiedAd request)
             => Infrastructure.RequestHandler.HandleQuery(() => _session.Query<QueryModels.GetPublicClassifiedAd>(request), _log);
    }
}
