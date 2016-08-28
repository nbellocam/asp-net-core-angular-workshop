using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TourOfHeroes
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();
        
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();
      
      var logger = loggerFactory.CreateLogger("Catchall Endpoint");
      app.Run(context =>
      {
        logger.LogInformation("New request to: {path}", context.Request.Path);

        return context.Response.WriteAsync("Hello from ASP.NET Core!");
      });
    }
  }
}