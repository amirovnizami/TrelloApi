// using System.Text;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
//
// namespace Trello.WebUi.Security
// {
//     public static class AuthenticationDependency
//     {
//         public static IServiceCollection AddAuthenticationDependency(this IServiceCollection services, IConfiguration configuration)
//         {
//             services.AddAuthentication(options =>
//                 {
//                     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//                 })
//                 .AddJwtBearer(cfg =>
//                 {
//                     cfg.RequireHttpsMetadata = false;
//                     cfg.SaveToken = true;
//                     cfg.TokenValidationParameters = new TokenValidationParameters()
//                     {
//                         ValidIssuer = configuration["JWT:ValidIssuer"], 
//                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!)),
//                         ValidAudience = configuration["JWT:ValidAudience"], 
//                         ValidateIssuer = true, 
//                         ValidateIssuerSigningKey = true, 
//                         ValidateAudience = true,
//                         ClockSkew = TimeSpan.Zero  
//                     };
//                     cfg.IncludeErrorDetails = true;
//                 });
//
//             return services;
//         }
//     }
// }