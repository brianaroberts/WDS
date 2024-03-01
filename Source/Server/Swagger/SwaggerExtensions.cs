using WDS.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace WDS.Swagger
{
    public static class SwaggerExtension
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("SwaggerDoc", new OpenApiInfo { Version = "v1", Title = "Web Service Testing" });
                
                c.AddSecurityDefinition(HttpHeaderNameConstants.ApiKey, new OpenApiSecurityScheme
                { 
                    Description = "Api key needed to access the endpoints. X-Api-Key: My_API_Key",
                    In = ParameterLocation.Header,
                    Name = HttpHeaderNameConstants.ApiKey,
                    Type = SecuritySchemeType.ApiKey,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = HttpHeaderNameConstants.ApiKey,
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = HttpHeaderNameConstants.ApiKey },
                        },
                        new string[] { }
                    }
                }); 
            });
        }

        public static void UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("SwaggerDoc/Swagger.json", "JOS.ApiKeyAuthentication");
                //c.SwaggerEndpoint(SettingsConfigHelper.AppSetting("AppKeys", "SwaggerEndpoint") + "Swagger/SwaggerDoc/Swagger.json", "API Testing");
            });
        }
    }
}
