using API.Extensions;
using API.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
    public class Startup
    {
        public IConfiguration _configuration { get; }
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            StaticConfig = configuration;
        }

        public static IConfiguration StaticConfig { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();
            // code has moved in the extenstion file where all the dependency injection has been moved.
            services.AddApplicationServices(_configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            //Always keep the exception middleware at the top
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();
            // CORS TO BE INCLUDED BETWEEN ROUTING AND AUTH
            // Always include CORS policy in same place between routing and the Authorization
            app.UseCors(policy =>
             policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            /// this allow credential used as it required for SIGNAL R
             .AllowCredentials()
            .WithOrigins("https://localhost:4200"
            //,"https://localhost:5001"
            ));

            // After Routing the Authentication after that Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            ///for serving the static file in the wwwroot with index.html
            ///
            app.UseDefaultFiles();
            // To say wwwroot folder content servered as static file
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //Here we mention the fall back controller for the angular deployment
                // Name of the action  and the controller name
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}