# Configurando el ambiente

Antes de arrancar, se necesitan tener las herramientas instaladas.

## .NET Core

A la hora de ejecutar ASP.NET Core podemos elegir usarlo con el clásico framework de .NET o con .NET Core. En este workshop se usará el segundo, dado que permite ejecutar las aplicaciones en cualquier plataforma.

1. Navegar a [https://get.asp.net/](https://get.asp.net/).

1. Una vez en el sitio, seleccionar la opción **Download .NET Core**.

    ![Descargando .NET Core](./images/download-net-core.png "Descargando .NET Core")

    _Descargando .NET Core_

1. La plataforma en la cual se está ejecutando debería aparecer seleccionada dentro de las posibilidades, caso contrario seleccionarla y seguir los pasos para instalar.

    > **Nota 1**: En el caso de Windows, instalar el **.NET Core SDK for Windows** que permite desarrollar sin Visual Studio.

    > **Nota 2**: No es necesario hacer los pasos una vez instalado (los que de la sección llamada _Initialize some code_), dado que estos pasos se realizaran como parte de uno de los módulos.

    ![Siguiendo los pasos para instalar .NET Core](./images/install-net-core.png "Siguiendo los pasos para instalar .NET Core")

    _Siguiendo los pasos para instalar .NET Core_

## Visual Studio Code

Para editar el código, se va a aprovechar Visual Studio Code, en especial porque permite trabajar en todas las plataformas al igual que .NET Core. Igualmente, se puede usar su editor de texto preferido.

1. Navegar a [https://code.visualstudio.com](https://code.visualstudio.com) y descargar la versión correspondiente para tu plataforma.

    ![Sitio de Visual Studio Code](./images/vs-code.png "Sitio de Visual Studio Code")

    _Sitio de Visual Studio Code_

1. Una vez descargado, seguir los pasos de la instalación.

1. Aparte de Visual Studio Code, se recomienda instalar la [extensión de C#](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp).

## Node.js & npm

A la hora de crear aplicaciones con Angular 2, se necesita tener instalado node.js y npm. El primero sirve para poder correr las herramientas necesarias. El segundo, se necesita dado que es la forma en la se distribuye los paquetes.

> **Nota**: Verificar que tenes instalado al menos la versión 4.x.x de node.js y la versión 3.x.x de npm corriendo `node -v` y `npm -v` en la terminal/consola.

1. Navegar al sitio de descargas de node.js [https://nodejs.org/es/download/](https://nodejs.org/es/download/).

1. Seleccionar la versión LTS (Long term support) y la plataforma correspondiente.

1. Una vez descargado, seguir los pasos de la instalación.
