using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsFeedService.Data;
using NewsFeedService.Dtos;
using NewsFeedService.Models;

namespace NewsFeedService.Controllers
{
    [Route("api/ut/tweet/{tweetId}/[controller]")]
    [ApiController]
    public class NewsFeedController : ControllerBase
    {
        private readonly INewsFeedRepo _repository;
        private readonly IMapper _mapper;

        public NewsFeedController(INewsFeedRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<NewsFeedReadDto>> GetNewsFeedsForTweet(int tweetId)
        {
            Console.WriteLine($"--> Hit GetNewsFeedsForTweet: {tweetId}");
            if (!_repository.TweetExist(tweetId))
            {
                return NotFound();
            }
            var newsFeeds = _repository.GetNewsFeedsForTweet(tweetId);
            return Ok(_mapper.Map<IEnumerable<NewsFeedReadDto>>(newsFeeds));
        }
        [HttpGet("{newsFeedId}", Name = "GetNewsFeedForTweet")]
        public ActionResult<NewsFeedReadDto> GetNewsFeedForTweet(int tweetId, int newsFeedId)
        {
            Console.WriteLine($"--> Hit GetNewsFeedForTweet: {tweetId} / {newsFeedId}");
            if (!_repository.TweetExist(tweetId))
            {
                return NotFound();
            }
            var newsFeed = _repository.GetNewsFeed(tweetId, newsFeedId);
            if (newsFeed == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<NewsFeedReadDto>(newsFeed));
        }

        [HttpPost]
        public ActionResult<NewsFeedReadDto> CreateNewsFeedForTweet(int tweetId, NewsFeedCreateDto newsFeedDto)
        {
            Console.WriteLine($"--> Hit CreateNewsFeedForTweet: {tweetId}");
            if (!_repository.TweetExist(tweetId))
            {
                return NotFound();
            }
            var newsFeed = _mapper.Map<NewsFeed>(newsFeedDto);
            _repository.CreateNewsFeed(tweetId, newsFeed);
            _repository.SaveChanges();

            var newsFeedReadDto = _mapper.Map<NewsFeedReadDto>(newsFeed);

            return CreatedAtRoute(nameof(GetNewsFeedForTweet), new { tweetId = tweetId, newsfeedId = newsFeedReadDto.Id }, newsFeedReadDto);
        }
    }
}