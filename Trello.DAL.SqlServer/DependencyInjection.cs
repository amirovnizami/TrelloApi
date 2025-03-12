using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Trello.DAL.SqlServer.Context;

namespace Trello.DAL.SqlServer;

public static class DependencyInjection
{
    public static IServiceCollection AddSqlServerServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TrelloDbContext>(opt => opt.UseSqlServer(connectionString));
        return services;
    }
}