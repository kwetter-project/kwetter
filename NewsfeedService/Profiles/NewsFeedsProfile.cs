using AutoMapper;
using NewsFeedService.Dtos;
using NewsFeedService.Models;
namespace NewsFeedService.Profiles
{
    public class NewsFeedsProfile : Profile
    {
        public NewsFeedsProfile()
        {
            //Source -> Target
            CreateMap<Tweet, TweetReadDto>();
            CreateMap<NewsFeedCreateDto, NewsFeed>();
            CreateMap<NewsFeed, NewsFeedReadDto>();
            CreateMap<Tweet, TweetDto>();
            CreateMap<TweetPublishedDto, Tweet>();
            // CreateMap<TweetPublishedDto, Tweet>()
            //     .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id));
        }
    }
}