using PortfolioApi.ExternalServices.CMS;
using PortfolioApi.ExternalServices.Persistence;
using PortfolioApi.ExternalServices.Persistence.Cache;
using PortfolioApi.ExternalServices.Persistence.Postgresql;

namespace PortfolioApi;

public static class ProgramExtensionsForExternalServices
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddExternalServicesPersistence()
        {
            services.AddScoped<IQueryUser, QueryUser>();
            services.AddScoped<IQueryProfile, QueryProfile>();
            services.AddScoped<IQueryProfileNavi, QueryProfileNavi>();
            services.AddScoped<IQueryProfileHero, QueryProfileHero>();
            services.AddScoped<IQueryProfileCard, QueryProfileCard>();
            services.AddScoped<IQueryProfileSocial, QueryProfileSocial>();
            services.AddScoped<IQueryProfilePlus, QueryProfilePlus>();
            services.AddScoped<IQueryProfileWork, QueryProfileWork>();

            // profile cms
            services.AddKeyedScoped<IQueryProfileCms, QueryProfileCms>("db");
            services.AddScoped<IQueryProfileCms, CacheProfileCms>();

            return services;
        }
    }

    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddCMS()
        {
            builder.Services.AddHttpClient("cms_sender", x =>
            {
                x.BaseAddress = new Uri(builder.Configuration["CMS:Url"]!);
                x.Timeout = TimeSpan.FromSeconds(60);
            });

            builder.Services.AddScoped<ICmsInvoker, MainCmsInvoker>();

            return builder;
        }
    }
}