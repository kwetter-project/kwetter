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
    [Authorize]
    public class LikeController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly ILikeRepo _repository;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public LikeController(ILikeRepo repository, IMapper mapper, IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }
        [HttpGet]
        public ActionResult<IEnumerable<LikeReadDto>> GetLikes()
        {
            Console.WriteLine("--> Getting Likes...");
            var likeItem = _repository.GetAllLikes();
            return Ok(_mapper.Map<IEnumerable<LikeReadDto>>(likeItem));
        }
        [HttpGet("{id}", Name = "GetLikeById")]
        public ActionResult<LikeReadDto> GetLikeById(int id)
        {
            var likeItem = _repository.GetLikeById(id);
            if (likeItem != null)
            {
                return Ok(_mapper.Map<LikeReadDto>(likeItem));
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<ActionResult<LikeReadDto>> CreateLike(LikeCreateDto likeCreateDto)
        {
            var likeModel = _mapper.Map<Like>(likeCreateDto);
            _repository.CreateLike(likeModel);
            _repository.SaveChanges();
            var likeReadDto = _mapper.Map<TweetReadDto>(likeModel);

            // Send Async Message
            try
            {
                var likePublishedDto = _mapper.Map<LikePublishedDto>(likeReadDto);
                likePublishedDto.Event = "Add Like";
                _messageBusClient.PublishNewLike(likePublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetLikeById), new { Id = likeReadDto.Id }, likeReadDto);
        }
        [HttpDelete]
        public async Task<ActionResult<string>> DeleteLike(int likeId)
        {
            _repository.DeleteLike(likeId);
            _repository.SaveChanges();

            // Send Async Message
            try
            {
                var likeDeletedDto = new LikeDeletedDto { TweetId = likeId, Event = "like_Deleted" };

                _messageBusClient.PublishDeleteLike(likeDeletedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return Ok($"tweet id = {likeId} deleted");
        }
    }

}