using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsFeedService.Data;
using NewsFeedService.Dtos;

namespace NewsFeedService.Controllersctl
{
    [Route("api/nf/[controller]")]
    [ApiController]
    [Authorize]
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
        public ActionResult<IEnumerable<TweetDto>> GetTweets()
        {
            Console.WriteLine("--> Getting Tweets from NewsFeedService");
            var tweetItems = _repository.GetAllTweets();
            return Ok(_mapper.Map<IEnumerable<TweetDto>>(tweetItems));
        }
    }
}