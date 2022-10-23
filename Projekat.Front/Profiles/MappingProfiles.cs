using AutoMapper;
using Projekat.Front.Dtos;
using Projekat.Front.Infrastructure.Persistence.Models;

namespace Projekat.Front.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Post, PostReadDto>();
            CreateMap<CreatePostDto, Post>();
        }
    }
}
