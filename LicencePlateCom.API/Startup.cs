using System.Diagnostics.CodeAnalysis;
using LicencePlateCom.API.Business;
using LicencePlateCom.API.Database;
using LicencePlateCom.API.Database.Adapters;
using LicencePlateCom.API.Database.Entities;
using LicencePlateCom.API.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LicencePlateCom.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.TryAddSingleton<IMongoContext<Message>, MongoContext<Message>>();
            services.TryAddSingleton<IMessageService, MessageService>();
            services.Configure<Settings>(c =>
            {
                c.ConnectionString = Configuration.GetConnectionString("LicencePlateCom");
                c.DatabaseName = Configuration.GetSection("Database")["Name"];
            });
            services.AddTransient<IMongoClient>(provider =>
            {
                var settings = provider.GetService<IOptions<Settings>>();
                return new MongoClient(settings.Value.ConnectionString);
            });
            services.AddTransient(provider =>
            {
                var settings = provider.GetService<IOptions<Settings>>();
                var mongoClient = provider.GetService<IMongoClient>();
                return mongoClient.GetDatabase(settings.Value.DatabaseName);
            });
            services.AddTransient(provider =>
            {
                var database = provider.GetService<IMongoDatabase>();
                return new MongoCollectionAdapter<Message>(database.GetCollection<Message>(new Message().Collection));
            });
        }

        [SuppressMessage("ReSharper",
            "UnusedMember.Global",
            Justification =
                "This method gets called by the runtime. Use this method to configure the HTTP request pipeline.")]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}