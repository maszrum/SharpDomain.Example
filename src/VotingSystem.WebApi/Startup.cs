using System.Diagnostics.CodeAnalysis;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using VotingSystem.AccessControl.AspNetCore;
using VotingSystem.Application.Identity;
using VotingSystem.WebApi.Jwt;
using VotingSystem.WebApi.VoterAuthentication;
using VotingSystem.WebApi.VotingSystem;

namespace VotingSystem.WebApi
{
    [SuppressMessage("ReSharper", "CA1822")]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddJwt(Configuration);
            services.AddAuthenticationService();
            
            services.AddSwaggerGen(c =>
            {
                var apiInfo = new OpenApiInfo
                {
                    Title = "VotingSystem.WebApi", 
                    Version = "v1"
                };
                c.SwaggerDoc("v1", apiInfo);
            });
        }

        public void ConfigureContainer(ContainerBuilder builder) => 
            builder.BuildVotingSystem();
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VotingSystem.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.ConfigureJwt();
            app.UseIdentity<VoterIdentity>();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}