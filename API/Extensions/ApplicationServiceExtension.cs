using API.Data;
using API.Entities;
using API.Helper;
using API.Interfaces;
using API.Mapper;
using API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        /// <summary>
        ///  Any Repository created here we need to make an dependency Injection in the Extension method
        /// </summary>
        /// <param name="services"></param>
        /// <param name="_configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration _configuration)
        {

            var emailConfig = _configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.Configure<CloudinarySettings>(_configuration.GetSection("CloudinarySettings"));
            //For Automapper we need to add below configuration
            services.AddAutoMapper(typeof(MapperProfiles));
            services.AddHttpContextAccessor();

            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IGenericRepository, GenericRepository>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            return services;
        }
    }
}