using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsFeedService.Data;
using NewsFeedService.Dtos;
using NewsFeedService.Models;

namespace NewsFeedService.Controllers
{
    [Route("api/[controller]/nf")]
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
        [HttpPost] // id = userID
        public ActionResult<string> CreateNewsFeedForTweet(string username)
        {
            Console.WriteLine($"--> Hit CreateNewsFeedForTweet");
            _repository.CreateNewsFeed(username);
            _repository.SaveChanges();

            return Ok(
                "newsfeed created"
            );
        }
    }
}