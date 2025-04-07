# Foro 📖

## Objetivos 📋
Desarrollar un sistema, que permita la administración general de un Foro (de cara a los administradores): Usuarios Miembros, Administradores, Pregunta, Respuesta, MeGusta, etc., como así también, permitir a los usuarios puedan navegar el foro y realizar preguntas y/o respuestas.
Utilizar Visual Studio 2022 community edition y crear una aplicación utilizando ASP.NET MVC Core (versión a definir por el docente, actualmente 8.0).

<hr />

## Enunciado 📢
La idea principal de este trabajo práctico, es que Uds. se comporten como un equipo de desarrollo.
Este documento, les acerca, un equivalente al resultado de una primera entrevista entre el cliente y alguien del equipo, el cual relevó e identificó la información aquí contenida. 
A partir de este momento, deberán comprender lo que se está requiriendo y construir dicha aplicación, 

Lo primero que deben hacer es comprender en detalle, que es lo que se espera y se busca como resultado del proyecto, para ello, deben recopilar todas las dudas que tengan entre Uds. y evacuarlas con su nexo (el docente) de cara al cliente. De esta manera, él nos ayudará a conseguir la información ya un poco más procesada. 
Es importante destacar, que este proceso no debe esperarse hacerlo 100% en clase; deben ir contemplandolas de manera independientemente, las unifican y hace una puesta comun dentro del equipo (ya sean de índole funcional o técnicas), en lugar de enviar consultas individuales, se sugiere y solicita que las envien de manera conjunta. 

Las consultas que sean realizadas por correo deben seguir el siguiente formato:

Subject: [NT1-<CURSO LETRA>-GRP-<GRUPO NUMERO>] <Proyecto XXX> | Informativo o Consulta

Body: 

1.<xxxxxxxx>

2.< xxxxxxxx>


# Ejemplo
**Subject:** [NT1-A-GRP-5] Agenda de Turnos | Consulta

**Body:**

1.La relación del paciente con Turno es 1:1 o 1:N?
2.Está bien que encaremos la validación del turno activo, con una propiedad booleana en el Turno?

<hr />

Es sumamente importante que los correos siempre tengan:
1.Subject con la referencia, para agilizar cualquier interaccion entre el docente y el grupo
2. Siempre que envien una duda o consulta, pongan en copia a todos los participantes del equipo. 

Nota: A medida que avancemos en la materia, TODAS las dudas relacionadas al proyecto deberán ser canalizadas por medio de Github, y desde alli tendremos: seguimiento y las dudas con comentarios, accesibles por todo el equipo y el avance de las mismas. 

**Crear un Issue nuevo o agregar un comentario sobre un issue en cuestion**, si se requiere asistencia, evacuar una duda o lo que fuese, siempre arrobando al docente, ejemplo: @marianolongoort y agregando las etiquetas correspondientes.


### Proceso de ejecución en alto nivel ☑️
 - Crear un nuevo proyecto en [visual studio](https://visualstudio.microsoft.com/en/vs/) utilizando la template de MVC (Model-View-Controller).
 - Crear todos los modelos definidos y/o detectados por ustedes, dentro de la carpeta Models, cada uno en un archivo separado (Modelos anemicos, modelos sin responsabilidades).
 - En el proyecto trataremos de reducir al mínimo las herencias sobre los modelos anémicos.  Ej. la clase Persona, tendrá especializaciones como ser Empleado, Cliente, Alumno, Profesional, etc. según corresponda al proyecto.
 - Sobre dichos modelos, definir y aplica las restricciones necesarias y solicitadas para cada una de las entidades. [DataAnnotations](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-8.0).
 - Agregar las propiedades navegacionales que pudisen faltar, para las relaciones entre las entidades (modelos) nueva que pudieramos generar o encontrar.
 - Agregar las propiedades relacionales, en el modelo donde se quiere alojar la relacion (entidad dependiente). La entidad fuerte solo tendrá una propiedad Navegacional, mientras que la entidad debíl tendrá la propiedad relacional.
 - Crear una carpeta Data en la raíz del proyecto, y crear dentro al menos una clase que representará el contexto de la base de datos (DbContext - los datos a almacenar) para nuestra aplicacion. 
 - Agregar los paquetes necesarios para Incorporar Entity Framework e Identitiy en nuestros proyectos.
 - Crear el DbContext utilizando en esta primera estapa con base de datos en memoria (con fines de testing inicial, introduccion y fine tunning de las relaciones entre modelos). [DbContext](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext?view=efcore-8.0), [Database In-Memory](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=vs).
 - Agregar las propiedades del tipo DbSet para cada una de las entidades que queremos persistir en el DbContext. Estas propiedades, serán colecciones de tipos que deseamos trabajar en la base de datos. En nuestro caso, serán las Tablas en la base de datos.
 - Agregar Identity a nuestro proyecto, y al menos definir IdentityUser como clase base de Persona en nuestro poryecto. Esto nos facilitará la inclusion de funcionalidades como Iniciar y cerrar sesion, agregado de entidades de soporte para esto Usuario y Roles que nos serviran para aplicar un control de acceso basado en roles (RBAC) basico. 
 - Por medio de Scaffolding, crear en esta instancia todos los CRUD (Create-Read-Update-Delete)/ABM (Altas-Bajas-Modificaiciones) de las entidades a persistir. Luego verificaremos cuales mantenemos, cuales removemos, y cuales adecuaremos para darle forma a nuestra WebApp.
 - Antes de continuar es importante realizar algun tipo de pre-carga de la base de datos. No solo es requisito del proyecto, sino que les ahorrara mucho tiempo en las pruebas y adecuaciones de los ABM.
 - Testear en detalle los ABM generados, y detectar todas las modificaciones requeridas para nuestros ABM e interfaces de usuario faltantes para resolver funcionalidades requeridas. (siempre tener presente el checklist de evaluacion final, que les dara el rumbo para esto).
 - Cambiar el dabatabase service provider de Database In Memory a SQL. Para aquellos casos que algunos alumnos utilicen MAC, tendran dos opciones para avanzar (adecuar el proyecto, para utilizar SQLLite o usar un docker con SQL Server instalado alli).
 - Aplicar las adecuaciones y validaciones necesarias en los controladores.  
 - Si el proyecto lo requiere, generar el proceso de auto-registración. Es importante aclarar que este proceso debe ser adecuado según las necesidades de cada proyecto, sus entidades y requerimientos al momento de auto-registrar; no seguir explicitamente un registro tan simple como email y password. 
 - A estas alturas, ya se han topado con varios inconvenientes en los procesos de adecuacion de las vistas y por consiguiente es una buena idea que generen ViewModels para desbloquear esas problematicas que nos estan trayendo los Modelos anemicos utilizados hasta el momento.
 - En el caso de ser requerido en el enunciado, un administrador podrá realizar todas tareas que impliquen interacción del lado del negocio (ABM "Alta-Baja-Modificación" de las entidades del sistema y configuraciones en caso de ser necesarias).
 - El <Usuario Cliente o equivalente> sólo podrá tomar acción en el sistema, en base al rol que que se le ha asignado al momento de auto-registrarse o creado por otro medio o entidad.
 - Realizar todos los ajustes necesarios en los modelos y/o código desde la perspectiva de funcionalidad.
 - Realizar los ajustes requeridos desde la perspectiva de permisos y validaciones.
 - Realizar los ajustes y mejoras en referencia a la presentación de la aplicaión (cuestiones visuales).
 
<hr />

Nota: Para la pre-carga de datos, las cuentas creadas por este proceso, deben cumplir las siguientes reglas de manera EXCLUYENTE:
 1. La contraseña por defecto para todas las cuentas pre-cargadas será: Password1!
 2. El UserName y el Email deben seguir la siguiente regla:  <classname>+<rolname si corresponde diferenciar>+<indice>@ort.edu.ar Ej.: cliente1@ort.edu.ar, empleado1@ort.edu.ar, empleadorrhh1@ort.edu.ar

<hr />

## Entidades 📄

- Persona
- Empleado
- Miembro
- Categoria
- Entrada
- Pregunta
- Respuesta
- Reaccion


## `⚠️Importante: Todas las entidades deben tener su identificador único. Id⚠️`

`
Las propiedades descriptas a continuación, son las mínimas que deben tener las entidades. Uds. pueden proponer agregar las que consideren necesarias. Siempre validar primero con el docente.
De la misma manera Uds. deben definir los tipos de datos asociados a cada una de ellas, como así también las restricciones.
`

**Persona**
```
- UserName
- Nombre
- Apellido
- Telefonos
- Direccion
- FechaAlta
- Email
```

**Empleado**
```
- UserName
- Nombre
- Apellido
- Telefonos
- Direccion
- FechaAlta
- Email
- Legajo
```

**Miembro**
```
- UserName
- Nombre
- Apellido
- Telefonos
- Direccion
- FechaAlta
- Email
- Entradas (Creadas)
- Preguntas (Realizadas)
- Respuestas (Realizadas)
- Reacciones (Realizadas)
- Habilitaciones (Acceso a entradas privadas)
```

**Categoria**
```
- Nombre
- Entradas
```

**Entrada**
```
- Fecha (fecha y hora)
- Titulo
- Texto
- Privada (flag)
- Categoria
- Miembro (Creador)
- Preguntas
- Habilitaciones (Miembros habilitados)
```

**Pregunta**
```
- Fecha (fecha y hora)
- Texto
- Miembro
- Activa (flag)
- Entrada (sobre la que se pregunta)
- Respuestas
```

**Respuesta**
```
- Fecha (fecha y hora)
- Texto
- Miembro
- Pregunta (sobre la que responde)
- Reacciones (colección de Likes, "MeGusta")
```

**Reaccion**
```
- Fecha (fecha y hora)
- Texto
- Miembro (que reacciona)
- MeGusta (flag)
- Respuesta (sobre la que se reacciona)
```

**NOTA:** aquí un link para refrescar el uso de los [Data annotations](https://www.c-sharpcorner.com/UploadFile/af66b7/data-annotations-for-mvc/).

<hr />

## Características y Funcionalidades ⌨️
`⚠️Todas las entidades, deben tener implementado su correspondiente ABM, a menos que sea implícito el no tener que soportar alguna de estas acciones.⚠️`
 
 **NOTA:** En el EP3, se deberán presentar las ABM de todoas las entidades, independientemente de que luego sean modificadas o eliminadas. El fin academico de esto, es que tomen contacto con formas de manejar los datos con los usuarios desde una interfaz gráfica de usuario y les sea más facil en el siguiente entregable comprender que deben modificar o adecuar.

## Generalidades 🏠
- Los Miembros pueden auto registrarse.
- Deberá contar con información institucional (inventada) relacionada al proyecto.
- Contenido anónimo que debe estar disponible (sin iniciar sesión):
	- La auto registración desde el sitio, es exclusiva para los miembros. Por lo cual, se le asignará dicho rol, pero podrán definir su password.
	- Un miembro puede navegar las entradas y ver las preguntas, respuestas y reacciones sin iniciar sesión, pero no puede interactuar.
        - En caso de querer interactuar, forzará a iniciar sesión, recordando la acción solicitada.
	- Las entradas privadas, deben visualizarse como tal y no pueden ser accedidas, a menos que se inicie sesión y tenga los permisos necesarios.		
- Los empleados, deben ser agregados por otro empleado.
	- Al momento, del alta del empleado, se le definirá un username y la password será definida por el sistema.
    - También se les asignará a estas cuentas el rol de Empleado.
- El foro, mostrará en la página principal:
    - Un listado de las ultimas 5 entradas cargadas más recientemente.
    - Un top 5, de Entradas con más preguntas y respuestas.
    - Un top 3, de los miembros con más entradas cargadas en el último mes. 
- Se debe ofrecer también, navegación de entradas por categorías.

⚠️Todo esto de manera anónima (sin iniciar sesión).⚠️

**Administrador**
- Un administrador, solo puede crear nuevas categorías. Es un empleado.
- Sacar un listado de cantidad de Entradas por categorías. El listado de categorías debe tener un contador de entradas.
- Los administradores del Foro, deben ser agregados por otro Administrador.
	- Al momento, del alta del Administradores, se le definirá un username y la password será definida por el sistema.
    - También se les asignará a estas cuentas el rol de Administrador.
- Pueden realizar todo tipo de acciones en el sistema, pero no pueden eliminar preguntas, respuestas ni reacciones. Solo pueden desactivar preguntas en caso de ser inapropiada. 
Nota: No se consideran respuestas inapropiadas, solo preguntas inapropiadas.

**Miembro**
- Puede auto registrarse.
- La auto registración desde el sitio, es exclusiva para los usuarios miembros. Por lo cual, se le asignará dicho rol.
- Los miembros pueden navegar por el foro.
- No pueden editar entradas, datos u otra información de otros miembros.
- Pueden crear Entradas.
    - Pueden desactivar una pregunta en cualquier momento. Si está inactiva, no se dejará de ver, solo impedirá que carguen nuevas respuestas otros miembros.
    - No se puede cargar una respuesta de una pregunta del mismo miembro. Esta acción, debe estar deshabilitada.

```
Ejemplo:
Título = Implementando autenticación y autorización en ASP.NET Core
Texto  = Estoy trabajando en una aplicación web usando ASP.NET Core, y necesito implementar un sistema de autenticación y autorización. Ya configuré Identity, pero me cuesta entender cómo usar roles y políticas de manera eficiente. Además, me interesa integrar autenticación externa, como Google o Facebook.
Pregunta = ¿Cuál es la mejor manera de administrar roles personalizados y manejar autorizaciones basadas en políticas en ASP.NET Core?
Respuesta = Para implementar autenticación y autorización en ASP.NET Core, puedes usar Identity para manejar usuarios y roles, y aplicar políticas con Authorize. Configura la autenticación externa mediante proveedores como Google o Facebook en  Startup.cs usando  AddAuthentication. Es simple y flexible para aplicaciones modernas. 
```
    
- Puede crear nuevas categorías.
    - Antes de crearla, se le propondrá un listado de categorías ya existentes en orden alfabético para evitar duplicados.
      - Si no encuentra, puede crear una nueva y asignarla a la entrada de manera automática. No hacer repetir cargas en el formulario al miembro. Pensar como lograr esto de manera sencilla.
- A cualquier respuesta, un miembro (que no es el autor de la respuesta), puede poner reaccionar con Like (MeGusta), Dislike (NoMeGusta) o resetearlo (Quita la reacción a dicha respuesta) haciendo clic en su reacción.
- En todo momento, puede ver un listado de Mis Entradas, Mis Preguntas, Mis Respuestas y Mis Reacciones. 
    - En orden Descendiente por fecha de creación. 
    - Si hace clic, irá a la entrada correspondiente.
- Como miembro creador de una entrada privada, puede ver un listado de miembros que quieren ser habilitados, y habilitarlos uno por uno o eliminar la solicitud. 
    - Esta eliminación no dará notificación alguna al miembro solicitante.
    - El miembro solicitante, solo verá que puede nuevamente realizar una solicitud de acceso.
    - Si ya realizó una solicitud, no podrá realizar otra hasta que el creador de la entrada, la habilite o la elimine.
    - Si el creador, quiere, puede eliminar todas las habilitaciones de una entrada privada con una simple acción de "Revocar todos los accesos".

**Entrada**
- Al generar una entrada por un miembro, quedarán los datos básicos asignados, como ser fecha, el miembro que la creó, etc.
    - La categoría puede ser una existente o una nueva que quiera crear en el momento.
- La entrada, creará junto con está la primera pregunta, que también, será este miembro que la generó.
    - Las entradas, listarán las preguntas en orden cronológico ascendente.
    - Estas preguntas, mostrarán al costado la cantidad de respuestas que recibieron.
- La entrada puede ser privada, en tal caso, se listará en el foro, con su título, pero solo miembros habilitados, podrán acceder al detalle con las preguntas y respuestas para interactuar.
    - El creador de la entrada, no necesita ser habilitado explícitamente.
    - Los miembros no habilitados pueden solicitar que se los habilite.
    - Un miembro autor de la entrada, podrá ver un listado de miembros que quieren ser habilitados, y habilitarlos uno por uno.
- Al acceder a una entrada, se deberá mostrar las preguntas, en orden descendente por cantidad de likes recibidos.

**Pregunta**
- Mientras que una pregunta esté activa, otros miembros, podrán dar respuestas a las preguntas.
- La entrada, puede tener más preguntas del mismo miembro, como así también, recibir más preguntas de otros miembros.
- Se visualizará las respuestas en orden cronológico ascendente, al acceder a cada pregunta.
    - La respuesta con más likes, se deberá destacar visualmente. Ejemplo, en un recuadro verde. 
    - La respuesta con más dislikes, se deberá destacar visualmente. Ejemplo, en un recuadro rojo.

**Respuesta**
- Las respuestas serán cargadas por miembros, que no sean los creadores de la pregunta a la cual quieren responder.
    - Un miembro, solo puede responder una vez a una pregunta.
    - Un miembro, solo puede responder a una pregunta, si esta está activa.    
- Podrán recibir reacciones.
- Se visualizará la cantidad de likes y dislikes, en cada respuesta.
- Las respuestas, se visualizarán en orden cronológico ascendente.
 
**Reacciones**
- Las reacciones, acerca de las respuestas, no pueden ser realizadas por los mismos autores de las respuestas. 
- La reacción a una respuesta será validándola con las 3 posibilidades mencionadas antes (Like, Dislike, Reset).    
- Al quitar (resetear) la reacción, no se desea guardar registro previo de la misma.
    - Un miembro, solo puede quitar las reacciones que uno mismo ha reaccionado.
- Se visualizará la cantidad de likes y dislikes, en cada respuesta.
- No se mostrará quien reaccionó, solo la cantidad de likes y dislikes.

**Aplicación General**
- Los miembros no pueden eliminar las entradas.
- No se puede repetir Categoria.Nombre.
- Los accesos a las funcionalidades y/o capacidades, debe estar basada en los roles que tenga cada individuo.

