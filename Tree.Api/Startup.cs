using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tree.Database;
using Tree.Repository;
using Tree.Service;
using Tree.Service.Interfaces;

namespace Tree
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TreeContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), 
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var nodeService = serviceScope.ServiceProvider.GetRequiredService<INodeService>();
                nodeService.AddRootAsync();
            }
                app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterServices();
            builder.RegisterRepositories();
        }
    }
}
