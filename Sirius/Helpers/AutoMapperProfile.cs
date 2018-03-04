using AutoMapper;
using Sirius.Dtos;
using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Sirius.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Шаблон перевода списка объектов User в UserDto, при этом даты переводятся из DateTime в сокращенное строкое значение
            CreateMap<User, UserDto>()
                .ForMember(x => x.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)));

            CreateMap<UserDto, User>()
                .ForMember(x => x.StartDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.StartDate)));
        }
    }
}
