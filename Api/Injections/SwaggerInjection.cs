using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Api.Injections
{
    public static class SwaggerInjection
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services
                .AddSwaggerDocument(document =>
                {
                    document.Title = "Obermind Backend API";
                    document.DocumentName = "v1";
                    document.ApiGroupNames = new[] { "v1" };
                    document.DocumentProcessors.Add(
                        new SecurityDefinitionAppender("JWT",
                            new OpenApiSecurityScheme
                            {
                                Type = OpenApiSecuritySchemeType.ApiKey,
                                Name = "Authorization",
                                In = OpenApiSecurityApiKeyLocation.Header,
                                Description = "Type into the box: Bearer {your JWT token}."
                            }
                        )
                    );
                    document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
                }
                );

            return services;
        }
    }
}
