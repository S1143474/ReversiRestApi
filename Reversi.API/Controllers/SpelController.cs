using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reversi.API.Application.Spellen.Queries.LoadAllReversi;

namespace Reversi.API.Controllers
{
    public class SpelController : BaseController
    {
        [HttpGet("index")]
        public ActionResult<string> Index()
        {
            return "Hoi";
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetReversiListWithWaitingSpelers(CancellationToken token)
        {
            var result = await Mediator.Send(new LoadAllReversiWithWaitingSpelersQuery(), token);
            return result;
        }
    }
}
