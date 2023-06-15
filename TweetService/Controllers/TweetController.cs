using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetService.AsyncDataServices;
using TweetService.Data;
using TweetService.Dtos;
using TweetService.Models;

namespace TweetService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class TweetController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly ITweetRepo _repository;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public TweetController(ITweetRepo repository, IMapper mapper, IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTweet(int id)
        {

            _repository.DeleteTweet(id);
            _repository.SaveChanges();



            try
            {
                TweetDeletedDto tweetDeletedDto = new TweetDeletedDto { Id = id, Event = "Tweet_Deleted" };
                tweetDeletedDto.Event = "Tweet_Deleted";
                _messageBusClient.PublishDeleteTweet(tweetDeletedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return NoContent(); // Return 204 No Content on successful deletion
        }

    }
}