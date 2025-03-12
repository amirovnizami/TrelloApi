using Microsoft.OpenApi.Models;

namespace Trello.WebUi.Security;

public static class AddSwaggerGenService
{
    public static IServiceCollection AddSwaggerService(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Enter JWT Bearer token"
            });
            c.OperationFilter<AuthorizeCheckOperationFilter>();
        });
        return services;
    }

}