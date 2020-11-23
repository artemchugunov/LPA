using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LPA.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContextPool<MyContext>
            //(
            //    dbContextOptionsBuilder =>
            //    {
            //        dbContextOptionsBuilder.UseNpgsql("Host=ec2-46-137-156-205.eu-west-1.compute.amazonaws.com;Port=5432;Database=;User Id=;Password=;SSL MODE=Require;Trust Server Certificate=true",
            //            optionsSqlServer => { optionsSqlServer.MigrationsAssembly("EF"); });
            //    }
            //);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var context = new MyContext())
            {
                context.Database.Migrate();
            }
        }
    }
}
