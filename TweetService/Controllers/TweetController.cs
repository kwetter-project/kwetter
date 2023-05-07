using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TweetService.AsyncDataServices;
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
        private readonly INewsFeedDataClient _utDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public TweetController(ITweetRepo repository, IMapper mapper, INewsFeedDataClient utDataClient, IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _utDataClient = utDataClient;
            _messageBusClient = messageBusClient;
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

            // Send Sync Message
            try
            {
                await _utDataClient.SendTweetToNewsFeed(tweetReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            // Send Async Message
            try
            {
                var tweetPublishedDto = _mapper.Map<TweetPublishedDto>(tweetReadDto);
                tweetPublishedDto.Event = "Tweet_Published";
                _messageBusClient.PublishNewTweet(tweetPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetTweetById), new { Id = tweetReadDto.Id }, tweetReadDto);
        }
    }
}