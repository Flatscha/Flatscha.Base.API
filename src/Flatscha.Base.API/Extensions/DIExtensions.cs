using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Flatscha.Base.API.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection AddBaseApi(this IServiceCollection services, string title, string version = "v1")
        {
            services.AddEndpointsApiExplorer();

            services.AddExceptionHandler<APIExceptionHandler>();
            services.AddSwagger(title, version);

            return services;
        }

        private static IServiceCollection AddSwagger(this IServiceCollection services, string title, string version = "v1")
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = version });
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = $"JWT",
                    Name = "Authorization",
                    BearerFormat = JwtConstants.TokenType,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = JwtBearerDefaults.AuthenticationScheme,
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        } });
            });
            return services;
        }

        public static IApplicationBuilder UseBaseApi(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(o => { });

            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
