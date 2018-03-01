using AutoMapper;
using Sirius.Dtos;
using Sirius.Models;

namespace Sirius.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
