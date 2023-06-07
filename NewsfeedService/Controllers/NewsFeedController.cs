using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsFeedService.Data;
using NewsFeedService.Dtos;
using NewsFeedService.Models;

namespace NewsFeedService.Controllers
{
    [Route("api/nf/[controller]")]
    [ApiController]
    [Authorize]
    public class NewsFeedController : ControllerBase
    {
        private readonly INewsFeedRepo _repository;
        private readonly IMapper _mapper;

        public NewsFeedController(INewsFeedRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet("{id}", Name = "GetNewsfeedByUser")]
        public ActionResult<NewsFeed> GetNewsfeedByUser(string id)
        {
            var tweetItem = _repository.GetNewsFeedByUser(id);
            if (tweetItem != null)
            {
                return Ok(tweetItem);
            }
            return NotFound();
        }
        [HttpPost]
        public ActionResult<NewsFeed> CreateNewsFeed([FromForm] string username)
        {
            Console.WriteLine($"--> Hit CreateNewsFeed");
            _repository.CreateNewsFeed(username);
            _repository.SaveChanges();
            var newsfeed = _repository.GetNewsFeedByUser(username);

            return Ok(newsfeed);
        }
        [HttpPost("follow")] // id = userID
        public ActionResult<string> Follow(string username)
        {
            Console.WriteLine($"--> Follow");
            _repository.CreateNewsFeed(username);
            _repository.SaveChanges();

            return Ok();
        }
        [HttpGet("allnewsfeed")]
        public ActionResult<IEnumerable<NewsFeed>> GetNewsFeed()
        {
            Console.WriteLine("--> Getting Newsfeeds from NewsFeedService");
            var nfItems = _repository.GetNewsFeed();
            return Ok(nfItems);
        }
        [HttpGet("socialgraph")]
        public ActionResult<IEnumerable<Follower>> GetsocialGraph()
        {
            var scItems = _repository.GetSocialGraph();
            return Ok(scItems);
        }
    }
}