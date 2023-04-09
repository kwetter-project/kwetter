using Microsoft.AspNetCore.Mvc;

namespace UserTimelineService.Controllers
{
    [Route("api/ut/[controller]")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        public TweetController() { }
        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine(" --> Inbound Post #UserTimeline Service");
            return Ok("Inbound test from tweet controller");
        }
    }
}