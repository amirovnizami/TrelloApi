using AutoMapper;
using BCrypt.Net;
using Microsoft.Extensions.DependencyInjection;
using Trello.Application.DTOs;
using Trello.Application.DTOs.Task;
using Trello.Domain.Entities;
using Task = Trello.Domain.Entities.Task;

namespace Trello.Application.AutoMapper;

public class MappingProfile
{
    // public MappingProfile()
    // {
    //     CreateMap<UserDto, User>()
    //         
    //     CreateMap<User, UserDto>().ReverseMap();
    // }
    public static void ConfigureMapper(IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.CreateMap<TaskUpdateDto, Task>();
            cfg.CreateMap<TaskCreateDto, Task>().ReverseMap();
            cfg.CreateMap<UserDto, LoginDto>();
            cfg.CreateMap<UserDto, RegisterDto>();
            cfg.CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId));

            cfg.CreateMap<User, UserDto>().ReverseMap();
        });
    }
}