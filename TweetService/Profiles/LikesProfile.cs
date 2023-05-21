using AutoMapper;
using TweetService.Dtos;
using TweetService.Models;

namespace TweetService.Profiles
{
    public class LikesProfile : Profile
    {
        public LikesProfile()
        {
            // Source -> Target
            CreateMap<Like, LikeReadDto>();
            CreateMap<LikeCreateDto, Like>();
            CreateMap<LikeReadDto, LikePublishedDto>();
        }
    }
}