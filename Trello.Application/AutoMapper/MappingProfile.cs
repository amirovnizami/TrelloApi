using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Trello.Application.DTOs;
using Trello.Common.GlobalResponses.Generics;
using Trello.Domain.Entities;

namespace Trello.Application.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
      
    }
}