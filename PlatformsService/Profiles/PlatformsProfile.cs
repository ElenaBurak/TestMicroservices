using AutoMapper;
using PlatformsService.Dtos;
using PlatformsService.Models;

namespace PlatformsService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            //source -> target(platform model)
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
        }
    }
}