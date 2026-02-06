using AutoMapper;
using SampleNo.Entity;
using SampleNo.Models;

namespace SampleNo
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<UpdateUserDto, ApplicationUser>();

            CreateMap<Story, StoryDto>();
            CreateMap<StoryDto, Story>();

            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>();

            CreateMap<Follow, FollowDto>();
            CreateMap<FollowDto, Follow>();

            CreateMap<Like, LikeDto>();
            CreateMap<LikeDto, Like>();

            CreateMap<Message, MessageDto>();
            CreateMap<MessageDto, Message>();

        }
    }
}
