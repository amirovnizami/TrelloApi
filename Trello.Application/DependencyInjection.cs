using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Trello.Application.Abstract;
using Trello.Application.AutoMapper;
using Trello.Application.Concrete;
using Trello.Application.Security;
using Trello.DAL.SqlServer.Abstract;
using Trello.DAL.SqlServer.Implementation.EFEntityFramework;

namespace Trello.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // var mapperConfig = new MapperConfiguration(mc =>
        // {
        //     mc.AddProfile(new MappingProfile());
        // });
        //
        // IMapper mapper = mapperConfig.CreateMapper();
        // services.AddSingleton(mapper);

        services.AddScoped<IUserDal,EFUserDal>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITaskDal, EFTaskDal>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }
}