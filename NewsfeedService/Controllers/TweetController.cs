using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsFeedService.Data;
using NewsFeedService.Dtos;

namespace NewsFeedService.Controllersctl
{
    [Route("api/ut/[controller]")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        private readonly INewsFeedRepo _repository;
        private readonly IMapper _mapper;

        public TweetController(INewsFeedRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<TweetReadDto>> GetTweets()
        {
            Console.WriteLine("--> Getting Tweets from NewsFeedService");
            var tweetItems = _repository.GetAllTweets();
            return Ok(_mapper.Map<IEnumerable<TweetReadDto>>(tweetItems));
        }
        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine(" --> Inbound Post #NewsFeed Service");
            return Ok("Inbound test from tweet controller");
        }
    }
}