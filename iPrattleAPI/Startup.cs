using iPrattle.Services;
using iPrattleAPI.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;


namespace iPrattleAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(Configuration)
                        .CreateLogger();
            services.AddDbContext<PrattleDbContext>(c =>
            {
                try
                {
                    var connString = Configuration.GetConnectionString("DbConnection");
                    Log.Information($"ConnString: {connString}");
                    c.UseSqlServer(connString);
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex.Message);
                }
            });

            services.AddControllers().AddNewtonsoftJson();
            services.AddScoped<IUserSvc, UserSvc>();
            services.AddScoped<ICommunicationSvc, CommunicationSvc>();
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                       .WithOrigins("http://localhost:4400")
                       .AllowCredentials();
            }));
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PrattleDbContext dbContext)
        {
            Log.Information($"[Startup]Inside Configure");

            dbContext.Database.Migrate();

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHub<PrattleHub>("/prattlehub");
            });
        }
    }
}
