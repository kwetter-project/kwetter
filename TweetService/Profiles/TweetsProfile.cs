using AutoMapper;
using TweetService.Dtos;
using TweetService.Models;

namespace TweetService.Profiles
{
    public class TweetsProfile : Profile
    {
        public TweetsProfile()
        {
            // Source -> Target
            CreateMap<Tweet, TweetReadDto>();
            CreateMap<TweetCreateDto, Tweet>();
        }
    }
}