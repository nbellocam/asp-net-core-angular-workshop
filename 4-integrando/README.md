# Consumiendo la API de ASP.NET Core desde Angular 2

Por mas que hoy en día se suelen hacer desarrollos por separado, creando APIs por un lado y el front end por el otro, normalmente estas dos partes se unen.

Es momento de empezar a integrar los modulos anteriores, haciendo que la aplicación de Angular 2 consuma la API creada con ASP.NET Core. Aparte de esto, la aplicación creada en ASP.NET Core puede servir la aplicación cliente.

En este modulo veremos una primer versión de esta integración, realizando todo a mano.

## Tarea 1: Integrando las soluciones

1. Crear una nueva carpeta donde trabajar.

1. Abrir la carpeta _begin_ que se encuentra dentro de la carpeta _src_ y copiar el contenido de la carpeta _server_ a la carpeta de trabajo.

    > **Nota**: El contenido de esta carpeta es el mismo que el que se genera cuando se termina el modulo de **ASP.NET Core**.

1. Ahora repetir la operación con el contenido de la carpeta _client_

    > **Nota**: El contenido de esta carpeta es el mismo que el que se genera cuando se termina el modulo de **Angular 2**.

1. Luego de tener todo el contenido junto, hay que actualizar ambas aplicaciones para que el server sirva los archivos del cliente. El primer paso será agregar el soporte para archivos estaticos en el server. Para eso, agregar la siguiente dependencia en el _project.json_.

    ```json
    "Microsoft.AspNetCore.StaticFiles": "1.0.0",
    ```

1. Ahora, agregar al método _Configure_ de la clase _Startup_ la llamada `app.UseStaticFiles();`.

    ```csharp
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      app.UseStaticFiles();
      
      app.UseMvc();
    }
    ```

1. Luego, crear la carpeta _wwwroot_ que será la que tenga los archivos estaticos.

1. Con esto el servidor ya está actualizado y queda el cliente. Para esto, actualizar el nodo output del archivo _webpack.prod.js_.

    ```js
    output: {
      path: helpers.root('wwwroot'),
      filename: '[name].js',
      publicPath: '/'
    },
    ```

1. Por último, mover el archivo _index.html_ a la carpeta _wwwroot_.

1. Generar los archivos del cliente, ejecutando en la terminal/consola `npm run build`.

    ![Generando los archivos del cliente](./images/npm-run-build.png "Generando los archivos del cliente")
    
    _Generando los archivos del cliente_

1. Ahora, ejecutar la aplicación con `dotnet run`.

1. Navegar a [http://localhost:5000/index.html](http://localhost:5000/index.html) y comprobar que funcione.

> **Nota**: Este proceso es muy manual y propenso a errores. Aparte de esto, notar que si se actualiza la página o si no se pone explicitamente el archivo _index.html_, entonces el sitio quedará en blanco.
>
> En las próximas tareas se harán cambios para mejorar estos temas.

## Tarea 2: Actualizando el manejo de rutas

En la tarea anterior se unieron ambas aplicaciones de forma manual. Ahora se arreglaran las rutas para que ante cualquier request se devuelva el cliente hecho en Angular 2, dado que las rutas se resuelven de este lado.

1. Primero, crear un nuevo archivo llamado _HomeController.cs_ dentro de la carpeta _Controllers_.

1. Agregar el siguiente contenido al archivo recién creado.

    ```csharp
    using Microsoft.AspNetCore.Mvc;

    namespace TourOfHeroes.Controllers
    {
      public class HomeController : Controller
      {
        public IActionResult Index()
        {
          return View();
        }
      }
    }
    ```

    > **Nota**: Este controlador y la action Index serán utilizados por defecto para devolver el cliente de Angular 2.

1. Ahora, crear la carpeta _Views_ y dentro de esta otra carpeta llamada _Home_.

1. Mover el archivo _index.html_ de la carpeta wwwroot a la nueva carpeta _Home_ y renombrarlo a _Index.cshtml_.

    > **Nota**: Con esto se convirtió el archivo original en la vista de la acción _Index_ del controlador _Home_.

1. Por último, actualizar la llamada al método _UseMvc_ en el método _Configure_ de la clase _Startup_ con el siguiente.

    ```csharp
    app.UseMvc(routes =>
    {
      routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");

      routes.MapRoute(
        name: "spa-fallback",
        template: "{*url}",
        defaults: new { controller = "Home", action = "Index" });
    });
    ```

    > **Nota**: Esta configuración hace que ante cualquier request que no se pueda resolver se devuelva el resultado de la action _Index_ del controller _Home_.

1. Ahora, ejecutar nuevamente la aplicación con `dotnet run`.

    > **Nota**: En esta oportunidad no modificamos el cliente, con lo cual no hace falta volver a generar el paquete con `npm run build` como en el caso anterior.

1. Navegar a [http://localhost:5000/](http://localhost:5000/) y comprobar que funcione.

    > **Nota**: En esta oportunidad, podemos probar de actualizar el sitio y deberíamos volver a la misma página en la que estabamos, aunque se perderá el estado.

## Tarea 3: Consumiendo la API

1. // TODO  Actualizar el cliente.



1. Como se modificó el cliente, tenemos que ejecutar `npm run build` para generar los archivos nuevamente.

1. Ahora, ejecutar nuevamente la aplicación con `dotnet run`.

1. Navegar a [http://localhost:5000/](http://localhost:5000/) y comprobar que funcione.

    > **Nota**: En esta oportunidad, podemos probar de actualizar el sitio y deberíamos volver a la misma página en la que estabamos y no se perderá el estado.
