# Introducción a ASP.NET Core

ASP.NET Core es una nuevo framework web, open-source (GitHub) y multiplataforma pensado para crear aplicaciones web modernas, con foco en aprovechar la nube así como en solucionar algunos de los nuevos desafios como IoT y backends para mobile apps. Algo interesante a tener en cuenta es que ASP.NET Core puede correr sobre .NET Core o sobre el clásico .NET framework.
En este articulo voy a explicar como crear una API REST de cero usando .NET Core como base. Para comenzar, vamos a necesitar tener instalado .NET Core, para esto, ir al sitio oficial y seguir las instrucciones. Como sugerencia, instalar Visual Studio Code (que también es multiplataforma) junto con la extension de C#.

### Tarea: Creando la app base
Una vez instalado .NET Core, abrir una terminal y crear una nueva carpeta para trabajar. Luego, dentro de la carpeta recién creada, ejecutar el siguiente comando para crear una aplicación base.

```
dotnet new
```

Una vez creada la aplicación, abrir el project.json y agregar la siguiente linea dentro de las dependencias, para agregar el HTTP server web.

```json
"Microsoft.AspNetCore.Server.Kestrel": "1.0.0"
```

Ahora, hay que instalar localmente los paquetes, para esto ejecutamos la siguiente linea. Esto sería similar a utilizar npm install en node.js.

```
dotnet restore
```

Luego de que termine de instalar todos las dependencias, vamos a crear un nuevo archivo llamado Startup.cs y agregamos el siguiente contenido (cambiar el namespace si es necesario):

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace aspnetcoreapp
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

Ahora es momento de actualizar el Program.cs para hacer que ejecute el web host para lo cual usamos el siguiente código.

```charp
using Microsoft.AspNetCore.Hosting;

namespace aspnetcoreapp
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

Es interesante notar que estamos corriendo una aplicación de C# común que instancia el WebHostBuilder y usando una fluent API se configura y se corre el Web Server. Para probar que todo ande correctamente hasta este punto ejecutar el siguiente comando y navegar en el browser a localhost:5000. Con esto deberían ver un “Hello World” en el browser.

```
dotnet run
```

### Tarea: Creando la API REST

Lo que antes existia por separado como Web API ahora es todo parte de MVC, con lo cual lo primero que tenemos que hacer es agregar la dependencia de MVC y volver a ejecutar dotnet restore. Para agregar la dependencia, abrir el project.json y agregar la siguiente linea dentro de las dependencias, debajo de la que agregamos anteriormente.

```json
"Microsoft.AspNetCore.Mvc": "1.0.0"
```

Luego de ejecutar dotnet restore, el siguiente paso es actualizar el Startup.cs para registrar MVC en el pipeline de procesamiento de los requests. Para esto vamos a reemplazar el contenido del archivo con el siguiente:

```charp
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace aspnetcoreapp
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

Por último, vamos a crear un primer controller. Para esto vamos a crear una nueva carpeta llamada Controller y vamos a agregar un archivo dentro de ella denominado ValuesController.cs con el siguiente contenido:

```charp
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcoreapp.Controllers
{
  [Route("api/[controller]")]
  public class ValuesController : Controller
  {
    [HttpGet]
    public IEnumerable<string> Get()
    {
      return new string[] { "value1", "value2" };
    }

    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }

    [HttpPost]
    public void Post([FromBody]string value)
    {
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
```

Ahora no queda otra cosa que probar la aplicación ejecutando nuevamente dotnet run y luego navegar al endpoint de la API (en este caso http://localhost:5000/api/values). Luego de navegar podrán ver en el browser el siguiente resultado:

```json
["value1","value2"]
```

## Conclusiones

Crear REST APIs con ASP.NET Core es muy simple y lo mejor es que ahora podemos trabajar desde cualquier plataforma (Windows, Mac o Linux) sin ningún problema. Lo interesante también es que se pueden crear aplicaciones muy simples usando una idea mas similar a la de node.js y sin necesidad de configurar miles de cosas.