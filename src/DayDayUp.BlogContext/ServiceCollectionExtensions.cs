using System;
using DayDayUp.BlogContext.Commands;
using DayDayUp.BlogContext.Commands.Posts;
using DayDayUp.BlogContext.Mapings;
using DayDayUp.BlogContext.Queries;
using DayDayUp.BlogContext.Repositories;
using DayDayUp.BlogContext.Services;
using DayDayUp.BlogContext.ValueObject;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DayDayUp.BlogContext
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlogModule(this IServiceCollection services, IConfiguration configuration)
        {
            MapsterConfig.Init();
            services.Configure<Secrets>(configuration.GetSection("Secrets"));
            services.AddDbContextPool<Repositories.BlogDbContext>(opt =>
            {
                opt.UseSqlite(configuration.GetConnectionString("Blog"));
            });
            services.AddSingleton<ITextConversionService, TextConversion>();
            services.AddHttpClient("markdown", x => x.BaseAddress = new Uri("http://markdown_api:3000"));
            
            // Repositories
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITagRepository, TagRepository>();

            // Commands
            services.AddMediatR(typeof(CreatePostCommandHandler));
            services.AddScoped<IPostDomainService, PostDomainService>();

            services.AddScoped<IPostQueries, PostQueries>();
            services.AddScoped<ICategoryQueries, CategoryQueries>();
            services.AddScoped<ITagQueries, TagQueries>();

            return services;
        }
    }
}