using System.Diagnostics.CodeAnalysis;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharpDomain.AccessControl.AspNetCore;
using SharpDomain.IoC;
using VotingSystem.Application.Identity;
using VotingSystem.IoC;
using VotingSystem.WebApi.Jwt;
using VotingSystem.WebApi.VoterAuthentication;

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

            services.AddSwagger();
        }

        public void ConfigureContainer(ContainerBuilder builder) =>
            new VotingSystemBuilder()
                .UseContainerBuilder(builder)
                .WireUpApplication()
                .InitializeIfNeed();

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