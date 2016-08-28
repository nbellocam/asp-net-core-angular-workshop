# Introducción a ASP.NET Core

ASP.NET Core es una nuevo framework web, open-source (GitHub) y multiplataforma pensado para crear aplicaciones web modernas, con foco en aprovechar la nube así como en solucionar algunos de los nuevos desafios como IoT y backends para mobile apps. Algo interesante a tener en cuenta es que ASP.NET Core puede correr sobre .NET Core o sobre el clásico .NET framework.
En este articulo voy a explicar como crear una API REST de cero usando .NET Core como base. Para comenzar, vamos a necesitar tener instalado .NET Core, para esto, ir al sitio oficial y seguir las instrucciones. Como sugerencia, instalar Visual Studio Code (que también es multiplataforma) junto con la extension de C#.

## Tarea 1 : Creando la app base

1. Crear una nueva carpeta para trabajar y abrir una terminal/consola en ese directorio. 

1. Luego, jecutar el siguiente comando para crear una aplicación base.

    ```
    dotnet new
    ```

1. Una vez creada la aplicación, abrir el _project.json_ y agregar la siguiente linea dentro de las dependencias, para agregar el HTTP server web.

    ```json
    "Microsoft.AspNetCore.Server.Kestrel": "1.0.0"
    ```

1. Ahora, hay que instalar localmente los paquetes, para esto ejecutamos la siguiente linea.

    ```
    dotnet restore
    ```

    > **Nota**: Al realizar esta tarea, se creará un archivo _project.lock.json_.

1. Luego de que termine de instalar todos las dependencias, vamos a crear un nuevo archivo llamado Startup.cs y agregamos el siguiente contenido (cambiar el namespace si es necesario):

    ```csharp
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    namespace TourOfHeroes
    {
      public class Startup
      {
        public void Configure(IApplicationBuilder app)
        {
          app.Run(context =>
          {
            return context.Response.WriteAsync("Hello from ASP.NET Core!");
          });
        }
      }
    }
    ```

1. Ahora es momento de actualizar el Program.cs para hacer que ejecute el web host para lo cual usamos el siguiente código.

    ```charp
    using Microsoft.AspNetCore.Hosting;

    namespace TourOfHeroes
    {
      public class Program
      {
        public static void Main(string[] args)
        {
          var host = new WebHostBuilder()
            .UseKestrel()
            .UseStartup<Startup>()
            .Build();
            
          host.Run();
        }
      }
    }
    ```

    > **Nota**: Es interesante notar que estamos corriendo una aplicación de consola que instancia el WebHostBuilder y usando una fluent API, configura y corre el Web Server. 

1. Para probar que todo ande correctamente hasta este punto, ejecutar el siguiente comando.

    ```
    dotnet run
    ```

1. Navegar en el browser a localhost:5000. Con esto deberían ver un “Hello World” en el browser.


## Tarea 2: Agregando logging

1. Abrir el project.json y agregar las siguientes dependencias.

    ```json
    "Microsoft.Extensions.Configuration.EnvironmentVariables": "1.0.0",
    "Microsoft.Extensions.Configuration.Json": "1.0.0",
    "Microsoft.Extensions.Configuration.CommandLine": "1.0.0",
    "Microsoft.Extensions.Logging": "1.0.0",
    "Microsoft.Extensions.Logging.Console": "1.0.0",
    "Microsoft.Extensions.Logging.Debug": "1.0.0"
    ```

1. Program.cs

    ```csharp
    using System.IO;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    namespace TourOfHeroes
    {
      public class Program
      {
        public static void Main(string[] args)
        {
          var config = new ConfigurationBuilder()
                    .AddCommandLine(args)
                    .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                    .Build();

          var host = new WebHostBuilder()
              .UseConfiguration(config)
              .UseKestrel()
              .UseContentRoot(Directory.GetCurrentDirectory())
              .UseStartup<Startup>()
              .Build();

          host.Run();
        }
      }
    }
    ```

1. Abrir el startup.json

    ```csharp
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
    ```

1. Agregar el archivo appsettings.json

    ```json
    {
      "Logging": {
        "IncludeScopes": false,
        "LogLevel": {
          "Default": "Debug",
          "System": "Information",
          "Microsoft": "Information"
        }
      }
    }
    ``` 

## Tarea 3: Creando la API REST

1. Lo que antes existia por separado como Web API ahora es todo parte de MVC, con lo cual lo primero que tenemos que hacer es agregar la dependencia de MVC y volver a ejecutar dotnet restore. Para agregar la dependencia, abrir el project.json y agregar la siguiente linea dentro de las dependencias, debajo de la que agregamos anteriormente.

    ```json
    "Microsoft.AspNetCore.Mvc": "1.0.0"
    ```

1. Luego de ejecutar dotnet restore, el siguiente paso es actualizar el Startup.cs para registrar MVC en el pipeline de procesamiento de los requests. Para esto vamos a reemplazar el contenido del archivo con el siguiente:

    ```charp
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    namespace TourOfHeroes
    {
      public class Startup
      {
        public void ConfigureServices(IServiceCollection services)
        {
          services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
          app.UseMvc();
        }
      }
    }
    ```

1. Modelo

    ```csharp
    namespace TourOfHeroes.Models
    {
      public class Hero
      {
        public int Id { get; set; }

        public string Name { get; set; }
      }
    }
    ```

1. Por último, vamos a crear un primer controller. Para esto vamos a crear una nueva carpeta llamada Controller y vamos a agregar un archivo dentro de ella denominado ValuesController.cs con el siguiente contenido:

    ```charp
    using System.Collections.Generic;
    using System.Linq;
    using aspnetcoreapp.Models;
    using Microsoft.AspNetCore.Mvc;

    namespace TourOfHeroes.Controllers
    {
      [Route("api/[controller]")]
      public class HeroController : Controller
      {
        private static List<Hero> heroes = new List<Hero> {
          new Hero() { Id= 11, Name= "Mr. Nice"},
          new Hero() { Id= 12, Name= "Narco"},
          new Hero() { Id= 13, Name= "Bombasto"},
          new Hero() { Id= 14, Name= "Celeritas"},
          new Hero() { Id= 15, Name= "Magneta"},
          new Hero() { Id= 16, Name= "RubberMan"},
          new Hero() { Id= 17, Name= "Dynama"},
          new Hero() { Id= 18, Name= "Dr IQ"},
          new Hero() { Id= 19, Name= "Magma"},
          new Hero() { Id= 20, Name= "Tornado"}
        };

        private static int nextId = 21;

        [HttpGet]
        public IEnumerable<Hero> Get()
        {
          return heroes;
        }

        [HttpGet("{id}")]
        public Hero Get(int id)
        {
          return heroes.FirstOrDefault(h => h.Id == id);
        }

        [HttpPost]
        public Hero Post([FromBody]Hero hero)
        {
          hero.Id = nextId++;
          heroes.Add(hero);
          return hero;
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Hero hero)
        {
          var dbHero = heroes.FirstOrDefault(h => h.Id == hero.Id);
          dbHero.Name = hero.Name;
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
          var dbHero = heroes.FirstOrDefault(h => h.Id == id);
          heroes.Remove(dbHero);
        }
      }
    }
    ```

1. Ahora no queda otra cosa que probar la aplicación ejecutando nuevamente dotnet run y luego navegar al endpoint de la API (en este caso http://localhost:5000/api/values). Luego de navegar podrán ver en el browser el siguiente resultado:

    ```json
    ["value1","value2"]
    ```

## Conclusiones

Crear REST APIs con ASP.NET Core es muy simple y lo mejor es que ahora podemos trabajar desde cualquier plataforma (Windows, Mac o Linux) sin ningún problema. Lo interesante también es que se pueden crear aplicaciones muy simples usando una idea mas similar a la de node.js y sin necesidad de configurar miles de cosas.