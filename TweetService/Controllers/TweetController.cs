using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TweetService.Data;
using TweetService.Dtos;
using TweetService.Models;
using TweetService.SyncDataServices.Http;

namespace TweetService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly ITweetRepo _repository;
        private readonly IMapper _mapper;
        private readonly IUserTimelineDataClient _utDataClient;

        public TweetController(ITweetRepo repository, IMapper mapper, IUserTimelineDataClient utDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _utDataClient = utDataClient;
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
        public async Task<ActionResult<TweetReadDto>> CreateTweet(TweetCreateDto tweetCreateDto)
        {
            var tweetModel = _mapper.Map<Tweet>(tweetCreateDto);
            _repository.CreateTweet(tweetModel);
            _repository.SaveChanges();
            var tweetReadDto = _mapper.Map<TweetReadDto>(tweetModel);
            try
            {
                await _utDataClient.SendTweetToUserTimeline(tweetReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }
            return CreatedAtRoute(nameof(GetTweetById), new { Id = tweetReadDto.Id }, tweetReadDto);
        }
    }
}