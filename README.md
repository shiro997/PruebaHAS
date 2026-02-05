PruebaHAS Mateo Blanco

en esta prueba práctica se encuentra un microservicio que expone un API Rest para el manejo de datos vía CRUD, dedicado a una entidad de las siguientes:

-Usuario

-Categoria

-Producto

-Cliente

-Tarea

la entidad seleccionada es Usuario, dicha API Rest tiene expuestos los siguientes endpoints:

-/api/v1/User (GET)

-/api/v1/User (POST)

-/api/v1/User (PUT)

-/api/v1/User/id={id} (GET)

-/api/v1/User/id={id} (DELETE)

-/api/v1/User/Login (POST)

el login es un extra que permite controlar los accesos y autorizaciones al componente de dashboard el cual controla los procecos de crud a excepción de crear un nuevo usuario
la creación de nuevos usuarios se puede hacer tanto en el inicio del aplicativo web como en el dashboard, un usuario no puede editar sus propios datos pero si los de otros usuarios
un usuario no puede eliminar sus datos pero si los de otros usuarios, en el dashboard se encuentra una tabla que contiene el listado de todos los usuarios con los siguientes campos:

- NombreUsuario (User Name)

-Email (Email)

para ejecutar la prueba se requiere que el proyecto hecho con angular 15 use el puerto 3400 ya que en la política de cors establecida en el backend se especifica dicho puerto para ejecución en local, al momento de crear un usuario nuevo justo antes de crear el objeto de la capa de datos se realiza el proceso de hash de la contraseña para garantizar la creación de contraseñas seguras, las contraseñas deben tener mínimo 8 caracteres, almenos una letra mayúscula y al menos una letra minúscula, la contraseña no acepta caracteres especiales, la contraseña por defecto en todos los usuarios es HAS2026sql, puede cambiarse en el dashboard iniciando sesión con cualquier usuario

la tabla de usuarios en la base de datos posee los siguientes campos:

-IdUsuario de tipo entero, campo de identidad iniciado en 1 y auto incrementado en 1, también es la llave primaria

-NombreUsuario de tipo texto (nvarchar) con longitud máxima, sin embargo el script puede ser editado a gusto del evaluador

-Email de tipo texto (nvarchar) con longitud máxima, sin embargo el script puede ser editado a gusto del evaluador

-UsrPassword de tipo texto (nvarchar) con longitud máxima, está destinado a almacenar los hash de contraseña

en las validaciones de los formularios el frontend mediante los validadores de los formularios reactivos de angular se encarga de la verificación de los datos de entrada y que los campos relevantes a la creación, actualización y login de usuarios lleven los datos requeridos, con los patrones requeridos y en el caso de las contraseñas la longitud mínima requerida. A tener en cuenta: El backend está hecho usando .Net 10 con lenguaje C#, el frontend está realizado en Angular v15 usando typescript para el manejo del flujo de trabajo, se recomienda cambiar en el appsettings.json la cadena de conexión de la base de datos, no se utilizó ningún ORM (Dapper o EF Core) para el manejo de la capa de datos y comunicación con la base de datos.

pasos para la configuración y ejecución:
paso uno ejecutar los scripts en el siguiente orden:
1. Database-Creation.sql
2. datos base usuarios.sql
3. en el archivo appsettings json cambiar el conection string relevante al servidor de base de datos del evaluador y la base de datos PruebaHas
4. ejecutar el backend en modo Http y escuchando en el puerto 5417
5. abrir la carpeta /PruebaHas/Frontend/PruebaHass
6. en una terminal abierta en el directorio del paso anterior ejecutar comando npm i
7. en la terminal ejecutar comando ng s --port 3400
