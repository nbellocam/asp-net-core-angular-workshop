# Introducción a Angular 2

En este modulo vamos a ver como crear una aplicación base con Angular 2 con Typescript y Webpack. 

El tutorial que se encuentra en la página de Angular 2 utiliza system.js en vez de webpack para buscar las referencias

## Creando la estructura base

1. Crear una carpeta donde vamos a tener nuestra aplicación y abrir una terminal/consola en ese directorio

1. Inicializar el package.json donde vamos a tener las dependencias de npm ejecutando el siguiente comando y completando los campos que son requeridos con los valores por defecto (apretando **Enter**).

    ```
    npm init
    ```

    > **Nota**: El contenido del archivo debería quedar similar al siguiente.
    > 
    > ```json
    > {
    >    "name": "tour-of-heroes",
    >    "version": "1.0.0",
    >    "description": "",
    >    "main": "index.js",
    >    "scripts": {
    >       "test": "echo \"Error: no test specified\" && exit 1"
    >    },
    >    "author": "",
    >    "license": "ISC"
    > }
    >```

1. Agregar las dependencias para angular ejecutando el siguiente comando.

    ```
    npm install --save @angular/common@2.0.0-rc.5 @angular/compiler@2.0.0-rc.5 @angular/core@2.0.0-rc.5 @angular/forms@0.3.0 @angular/http@2.0.0-rc.5 @angular/platform-browser@2.0.0-rc.5 @angular/platform-browser-dynamic@2.0.0-rc.5 @angular/router@3.0.0-rc.1 bootstrap@^3.3.6 core-js@^2.4.0 reflect-metadata@0.1.3 rxjs@5.0.0-beta.6 zone.js@0.6.12
    ```

    > **Nota**: El contenido del _package.json_ debería tener ahora un nodo _dependencies_ con el siguiente contenido.
    > 
    > ```json
    > "dependencies": {
    >    "@angular/common": "^2.0.0-rc.5",
    >    "@angular/compiler": "^2.0.0-rc.5",
    >    "@angular/core": "^2.0.0-rc.5",
    >    "@angular/forms": "^0.3.0",
    >    "@angular/http": "^2.0.0-rc.5",
    >    "@angular/platform-browser": "^2.0.0-rc.5",
    >    "@angular/platform-browser-dynamic": "^2.0.0-rc.5",
    >    "@angular/router": "^3.0.0-rc.1",
    >    "bootstrap": "^3.3.7",
    >    "core-js": "^2.4.1",
    >    "reflect-metadata": "^0.1.3",
    >    "rxjs": "^5.0.0-beta.6",
    >    "zone.js": "^0.6.12"
    > }
    > ```

1. Agregar las dependencias de las herramientas que necesitamos como typescript y webpack.

    ```
    npm install --save-dev angular2-template-loader@^0.4.0 css-loader@^0.23.1 extract-text-webpack-plugin@^1.0.1 file-loader@^0.8.5 html-loader@^0.4.3 html-webpack-plugin@^2.15.0 null-loader@^0.1.1 raw-loader@^0.5.1 rimraf@^2.5.2 style-loader@^0.13.1 ts-loader@^0.8.1 typescript@^1.8.10 typings@^1.0.4 webpack@^1.13.0 webpack-dev-server@^1.14.1 webpack-merge@^0.14.0
    ```

    > **Nota**: El contenido del _package.json_ debería tener ahora un nodo _devDependencies_ con el siguiente contenido.
    > 
    > ```json
    > "devDependencies": {
    >     "angular2-template-loader": "^0.4.0",
    >     "css-loader": "^0.23.1",
    >     "extract-text-webpack-plugin": "^1.0.1",
    >     "file-loader": "^0.8.5",
    >     "html-loader": "^0.4.3",
    >     "html-webpack-plugin": "^2.15.0",
    >     "null-loader": "^0.1.1",
    >     "raw-loader": "^0.5.1",
    >     "rimraf": "^2.5.2",
    >     "style-loader": "^0.13.1",
    >     "ts-loader": "^0.8.1",
    >     "typescript": "^1.8.10",
    >     "typings": "^1.0.4",
    >     "webpack": "^1.13.0",
    >     "webpack-dev-server": "^1.14.1",
    >     "webpack-merge": "^0.14.0"
    > }
    > ```

1. Abrir el archivo con el editor de textos y agregar la sección dependencies

    ```json
    {
      "name": "angular2-webpack",
      "version": "1.0.0",
      "scripts": {
        "start": "webpack-dev-server --inline --hot --progress --port 8080",
        "build": "rimraf dist && webpack --config config/webpack.prod.js --progress --profile --bail",
        "postinstall": "typings install"
      },
      "license": "MIT",
      "dependencies": {
        "@angular/common": "2.0.0-rc.5",
        "@angular/compiler": "2.0.0-rc.5",
        "@angular/core": "2.0.0-rc.5",
        "@angular/forms": "0.3.0",
        "@angular/http": "2.0.0-rc.5",
        "@angular/platform-browser": "2.0.0-rc.5",
        "@angular/platform-browser-dynamic": "2.0.0-rc.5",
        "@angular/router": "3.0.0-rc.1",
        "bootstrap": "^3.3.6",
        "core-js": "^2.4.0",
        "reflect-metadata": "0.1.3",
        "rxjs": "5.0.0-beta.6",
        "zone.js": "0.6.12"
      },
      "devDependencies": {
        "angular2-template-loader": "^0.4.0",
        "css-loader": "^0.23.1",
        "extract-text-webpack-plugin": "^1.0.1",
        "file-loader": "^0.8.5",
        "html-loader": "^0.4.3",
        "html-webpack-plugin": "^2.15.0",
        "null-loader": "^0.1.1",
        "raw-loader": "^0.5.1",
        "rimraf": "^2.5.2",
        "style-loader": "^0.13.1",
        "ts-loader": "^0.8.1",
        "typescript": "^1.8.10",
        "typings": "^1.0.4",
        "webpack": "^1.13.0",
        "webpack-dev-server": "^1.14.1",
        "webpack-merge": "^0.14.0"
      }
    }
    ```

1. Crear el tsconfig

    ```json
    {
      "compilerOptions": {
        "target": "es5",
        "module": "commonjs",
        "moduleResolution": "node",
        "sourceMap": true,
        "emitDecoratorMetadata": true,
        "experimentalDecorators": true,
        "removeComments": false,
        "noImplicitAny": false
      },
      "exclude": [
        "node_modules"
      ]
    }
    ```

1. typings.json

    ```json
    {
      "globalDependencies": {
        "core-js": "registry:dt/core-js#0.0.0+20160725163759",
        "node": "registry:dt/node#6.0.0+20160818175514"
      }
    }
    ```

1. Crear el html

    ```html
    <!DOCTYPE html>
    <html>
      <head>
        <base href="/">
        <title>My Angular 2 app</title>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1">
      </head>
      <body>
        <my-app>Loading...</my-app>
      </body>
    </html>
    ```

1. Ejecutar

    ```
    npm install
    ```

    > **Nota**: Esto debería crear dos nuevas carpetas *node_modules* y _typings_. Si no se creó la carpeta _typings_ ejecutar el comando nuevamente.


## Configurando webpack

Crear el archivo de webpack

## Corriendo la applicación

1. En la consola/terminal, ejecutar el siguiente comando

    ```
    npm start
    ```

1. Abrir un navegador e ir a [http://localhost:8080](http://localhost:8080).
