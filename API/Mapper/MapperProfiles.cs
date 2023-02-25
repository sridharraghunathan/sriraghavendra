using API.DTO;
using API.Entities;
using AutoMapper;
using Google.Apis.Auth;

namespace API.Mapper
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            // THESE are important method created for the mapping of the data from source to target or revers
            CreateMap<AradhanaDetails, Contact>();
            CreateMap<LatestEventsDTO, LatestEvents>();
            CreateMap<PhotoDto, Photo>();
            CreateMap<UserClarificationDTO, UserClarification>().ReverseMap();
            CreateMap<GoogleJsonWebSignature.Payload, UserDetails>();
        }
    }
}