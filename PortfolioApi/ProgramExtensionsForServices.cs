using PortfolioApi.Services;

namespace PortfolioApi;

public static class ProgramExtensionsForServices
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDefaultServices()
        {
            services.AddScoped<IUserService, DefaultUserService>();
            services.AddScoped<IProfileNaviService, DefaultProfileNaviService>();
            services.AddScoped<IProfileHeroService, DefaultProfileHeroService>();
            services.AddScoped<IProfileCardService, DefaultProfileCardService>();
            services.AddScoped<IProfilePlusService, DefaultProfilePlusService>();
            services.AddScoped<IProfileWorkService, DefaultProfileWorkService>();
            services.AddScoped<IProfileLearning, DefaultProfileLearning>();
            services.AddScoped<IProfileArticle, DefaultProfileArticle>();

            return services;
        }
    }
}