using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TweetService.Data;
using TweetService.Dtos;
using TweetService.Models;

namespace TweetService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly ITweetRepo _repository;
        private readonly IMapper _mapper;

        public TweetController(ITweetRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<TweetReadDto>> GetTweets()
        {
            Console.WriteLine("--> Getting Tweets...");
            var tweetItem = _repository.GetAllTweets();
            return Ok(_mapper.Map<IEnumerable<TweetReadDto>>(tweetItem));
        }
        [HttpGet("{id}", Name = "GetTweetById")]
        public ActionResult<TweetReadDto> GetTweetById(int id)
        {
            var tweetItem = _repository.GetTweetById(id);
            if (tweetItem != null)
            {
                return Ok(_mapper.Map<TweetReadDto>(tweetItem));
            }
            return NotFound();
        }
        [HttpPost]
        public ActionResult<TweetReadDto> CreateTweet(TweetCreateDto tweetCreateDto)
        {
            var tweetModel = _mapper.Map<Tweet>(tweetCreateDto);
            _repository.CreateTweet(tweetModel);
            _repository.SaveChanges();
            var tweetReadDto = _mapper.Map<TweetReadDto>(tweetModel);
            return CreatedAtRoute(nameof(GetTweetById), new { Id = tweetReadDto.Id }, tweetReadDto);
        }
    }
}