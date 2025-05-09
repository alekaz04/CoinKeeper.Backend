using CoinKeeper.Authentication;
using CoinKeeper.Authentication.Domain;
using CoinKeeper.Authentication.Mapper;
using CoinKeeper.Authentication.Validators;
using CoinKeeper.Common;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CoinKeeper.Extensions.DependencyInjection;

public static class AuthServiceCollectionExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

        if (jwtOptions is null)
        {
            throw new CommonErrorException("jwtOptions is null");
        }

        services.AddSingleton(jwtOptions);

        services.AddHttpContextAccessor();

        services.AddValidatorsFromAssembly(typeof(RequestUserDtoValidator).Assembly);
        services.AddSingleton<PasswordHashService>();

        services.AddScoped<AuthUserService>();
        services.AddScoped<ICurrentUser, CurrentUserService>();
        services.AddScoped<JsonWebTokenService>();
        services.AddScoped<UserService>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey))
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                    }

                    return Task.CompletedTask;
                }
            };

        });

        services.AddAutoMapper(typeof(UserMapperProfile));
        return services;
    }
}
