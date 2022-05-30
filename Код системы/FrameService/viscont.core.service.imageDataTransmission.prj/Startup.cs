using Mallenom.Imaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Viscont.Core.Framework.ImageDataTransmission;
using Viscont.Core.Service.ImageDataTransmission.Data;
using Viscont.Core.Service.ImageDataTransmission.Data.ImageData;
using Viscont.Core.Service.ImageDataTransmission.Hubs;

namespace Viscont.Core.Service.ImageDataTransmission;

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

		services.AddSignalR();

		services.AddControllers();

		services.AddTransient<IImageDataAllocator, ImageDataAllocator>();
		services.AddTransient<IImageDataReader, ImageDataReader>();

		services.AddSingleton<IImageRepository, ImageRepository>();

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
			endpoints.MapHub<NotificationHub>("/NewImage");
        });
    }
}
