using AutoMapper;
using UserTimelineService.Dtos;
using UserTimelineService.Models;
//using TweetService;

namespace UserTimelineService.Profiles
{
    public class UserTimelineProfile : Profile
    {
        public UserTimelineProfile()
        {
            // Source -> Target
            // CreateMap<Tweet, TweetreadDto>();
            // CreateMap<UserTimelineCreateDto, UserTimeline>();
            // CreateMap<UserTimeline, UserTimelineReadDto>();
            // CreateMap<TweetPublishedDto, Tweet>()
            //     .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id));
            // CreateMap<GrpcTweetModel, Tweet>()
            //     .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.TweetId))
            //     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            //     .ForMember(dest => dest.UserTimeline, opt => opt.Ignore());
        }
    }
}