# Introducción a ASP.NET Core

ASP.NET Core es una nuevo framework web, open-source ([GitHub](https://github.com/aspnet)) y multiplataforma pensado para crear aplicaciones web modernas, con foco en aprovechar la nube así como en solucionar algunos de los nuevos desafios como IoT (Internet of Things) y backends para mobile apps. 

Algo interesante a tener en cuenta es que ASP.NET Core puede correr sobre .NET Core o sobre el clásico .NET framework.

En este modulo se verá como crear una API REST de cero usando .NET Core como base.

## Tarea 1 : Creando la app base

1. Crear una nueva carpeta para trabajar y abrir una terminal/consola en ese directorio. 

1. Luego, ejecutar el siguiente comando para crear una aplicación base.

    ```
    dotnet new
    ```

1. Una vez creada la aplicación, abrir el _project.json_ y agregar la siguiente linea dentro de las dependencias, para agregar el HTTP server web.

    ```json
    "Microsoft.AspNetCore.Server.Kestrel": "1.0.0"
    ```

    > **Nota**: El contenido del _project.json_ en este punto debería quedar similar al siguiente:
    > 
    > ```json    
    > {
    >   "version": "1.0.0-*",
    >   "buildOptions": {
    >     "debugType": "portable",
    >     "emitEntryPoint": true
    >   },
    >   "dependencies": {
    >     "Microsoft.AspNetCore.Server.Kestrel": "1.0.0"
    >   },
    >   "frameworks": {
    >     "netcoreapp1.0": {
    >       "dependencies": {
    >         "Microsoft.NETCore.App": {
    >           "type": "platform",
    >           "version": "1.0.0"
    >         }
    >       },
    >       "imports": "dnxcore50"
    >     }
    >   }
    > }
    > ```


1. Ahora, hay que instalar localmente los paquetes, para esto ejecutamos la siguiente linea.

    ```
    dotnet restore
    ```

    > **Nota**: Al realizar esta tarea, se creará un nuevo archivo _project.lock.json_.

1. Luego de que termine de instalar todos las dependencias, vamos a crear un nuevo archivo llamado _Startup.cs_ y agregamos el siguiente contenido.

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

    > **Nota**: Cambiar el namespace si es necesario/preferido. Tener en cuenta en cambiarlo en todos los archivos que se crearán a continuación.

1. Ahora es momento de actualizar el _Program.cs_ para hacer que ejecute el web host. Para esto, usamos el siguiente código.

    ```csharp
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

    ![Corriendo la aplicación](./images/first-run.png "Corriendo la aplicación")

    _Corriendo la aplicación_

1. Navegar en el browser a [http://localhost:5000](http://localhost:5000). Con esto se mostrará un “Hello World” en el browser.

    ![Mensaje de Hello World](./images/first-run-browser.png "Mensaje de Hello World")

    _Mensaje de Hello World_

## Tarea 2: Agregando logging y configuraciones

A la hora de crear aplicaciones, resulta muy útil poder saber que pasa y si hay errores. Aparte de esto, el tipo de información que nos interesa cambia según el momento y el entorno donde nos encontramos. 

Para esto en los siguientes pasos se configurará logging, como también el manejo de configuraciones desde diferentes fuentes.

1. Abrir el _project.json_ y agregar las siguientes dependencias.

    ```json
    "Microsoft.Extensions.Configuration.EnvironmentVariables": "1.0.0",
    "Microsoft.Extensions.Configuration.Json": "1.0.0",
    "Microsoft.Extensions.Configuration.CommandLine": "1.0.0",
    "Microsoft.Extensions.Logging": "1.0.0",
    "Microsoft.Extensions.Logging.Console": "1.0.0",
    "Microsoft.Extensions.Logging.Debug": "1.0.0"
    ```

1. Ejecutar en la terminal `dotnet restore` para actualizar las dependencias y el _project.lock.json_.

1. Ahora, abrir el _Program.cs_ y actualizar el contenido para pasarle las configuraciones al WebHostBuilder.

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

1. Abrir el _Startup.cs_ y actualizar el contenido

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

1. Agregar el archivo _appsettings.json_

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

1. Para comprobar que todo esté bien, ejecutar nuevamente la aplicación.

    ```
    dotnet run
    ```

    ![Corriendo la aplicación con logging](./images/second-run.png "Corriendo la aplicación con logging")

    _Corriendo la aplicación con logging_

1. Navegar en el browser a [http://localhost:5000](http://localhost:5000), que mostrará el mismo mensaje, pero esta vez en la terminal tendremos mas información.

    ![Los mensajes en la terminal](./images/terminal-logging.png "Los mensajes en la terminal")

    _Los mensajes en la terminal_

## Tarea 3: Creando la API REST

A la hora de crear una API REST, vamos a usar MVC. En el pasado, existía Web API como algo separado de MVC, pero en la nueva versión se unificó.

1. Para agregar la dependencia, abrir el project.json y agregar la siguiente linea dentro de las dependencias, debajo de la que agregamos anteriormente.

    ```json
    "Microsoft.AspNetCore.Mvc": "1.0.0"
    ```

1. Ejecutar en la terminal `dotnet restore` para actualizar las dependencias y el _project.lock.json_.

1. Luego, actualizar el _Startup.cs_ para registrar MVC en el pipeline de procesamiento de los requests. Para esto, primero, agregar el siguiente `using`.

    ```csharp
    using Microsoft.Extensions.DependencyInjection;
    ```

1. Ahora, agregar el método `ConfigureServices` con la siguiente implementación.

    ```csharp
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
    }
    ```

1. Por último, actualizar el método `Configure` con la siguiente implementación.

    ```csharp
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();
      
      app.UseMvc();
    }
    ```

1. Ahora, crear una carpeta _Models_ y dentro de ella crear un archivo con el nombre _Hero.cs_.

1. Agregar el siguiente código al archivo recién creado, que define la clase Hero con un id y un nombre.

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

1. Por último, se necesita el controller que implementará la API. Para esto, crear una nueva carpeta _Controllers_ y agregar un archivo llamado _HeroController.cs_.

1. En el archivo recién creado, agregar la estructura del controller.

    ```csharp
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using TourOfHeroes.Models;

    namespace TourOfHeroes.Controllers
    {
      [Route("api/[controller]")]
      public class HeroController : Controller
      {
      }
    }
    ```

1. Ahora, agregar las siguientes variables estáticas que se usarán para simular la base de datos.

    ```csharp
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
    }
    ```

1. Agregar las acciones para obtener los datos.

    ```csharp
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
    ```

1. Luego, agregar las siguientes acciones para agregar nuevos heroes y modificarlos.

    ```csharp
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
    ```

1. Por último, agregar la acción correspondiente para borrar heroes.

    ```csharp
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      var dbHero = heroes.FirstOrDefault(h => h.Id == id);
      heroes.Remove(dbHero);
    }
    ```

1. Ahora no queda otra cosa que probar la aplicación ejecutando el siguiente comando.

    ```
    dotnet run
    ```

    ![Corriendo la aplicación con la API](./images/last-run.png "Corriendo la aplicación con la API")

    _Corriendo la aplicación con la API_

1. Navegar en el browser al endpoint de la API [http://localhost:5000/api/hero](http://localhost:5000/api/hero), que mostrará el listado de heroes en formato json.

    ![Heroes como json](./images/heroes-json.png "Heroes como json")

    _Heroes como json_

    > **Nota**: También se pueden probar las otras acciones si se cuenta con un cliente como [Postman](https://www.getpostman.com).

## Conclusiones

Crear REST APIs con ASP.NET Core es muy simple. Aparte de esto, se puede crear y correr desde cualquier plataforma (Windows, Mac o Linux) sin ningún problema.